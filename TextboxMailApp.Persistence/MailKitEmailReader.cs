using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence
{
    public class MailKitEmailReader : IEmailReader
    {
        //Sayfalandırma ile mailleri çekiyor
        public async Task<List<EmailMessagesDto>> GetEmailsByPageAsync(User user, uint? minExistingUid = null)
        {
            using var client = new ImapClient();

            await client.ConnectAsync(user.ServerName, user.Port, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(user.EmailAddress, user.EmailPassword);
            const int pageSize = 100;

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);

            if (inbox.Count == 0)
                return new List<EmailMessagesDto>();

            // Eğer DB’de hiç kayıt yoksa en son 100 mail çekilecek
            uint endUid;
            if (minExistingUid == null || minExistingUid <= 1)
            {
                endUid = inbox.UidNext.Value.Id - 1;
            }
            else
            {
                // DB’de en küçük UID varsa, onun bir öncesinden başlıyoruz
                endUid = minExistingUid.Value - 1;
            }

            if (endUid < 1)
                return new List<EmailMessagesDto>();

            // Başlangıç UID’sini hesapla
            var startUid = endUid >= pageSize ? endUid - (uint)pageSize + 1 : 1;

            var uidRange = new UniqueIdRange(new UniqueId(startUid), new UniqueId(endUid));

            var summaries = inbox.Fetch(uidRange,
                MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure);

            var result = new List<EmailMessagesDto>();
            foreach (var summary in summaries)
            {
                var message = await inbox.GetMessageAsync(summary.UniqueId);
                result.Add(MapToDto(summary, message, user.Id));
            }

            await client.DisconnectAsync(true);

            return result.OrderByDescending(x => x.Uid).ToList();
        }

        //yeni gelen bir mail varsa o getiriliyor
        public async Task<List<EmailMessagesDto>> GetEmailsAfterUidAsync(uint lastMaxUid, User user)
        {
            var client = new ImapClient();

            await client.ConnectAsync(user.ServerName, user.Port, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(user.EmailAddress, user.EmailPassword);
            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);

            // Eğer mailbox boşsa veya UidNext null ise yeni mail yok
            if (inbox.Count == 0 || !inbox.UidNext.HasValue || inbox.UidNext.Value.Id <= lastMaxUid)
                return new List<EmailMessagesDto>();

            // UID aralığını belirle
            var startUidValue = lastMaxUid + 1;
            var endUidValue = inbox.UidNext.Value.Id - 1;

            if (startUidValue > endUidValue)
                return new List<EmailMessagesDto>(); // Yeni mail yok

            // UID aralığını listeye çevir
            var uidRange = new List<UniqueId>();
            for (uint uid = startUidValue; uid <= endUidValue; uid++)
            {
                uidRange.Add(new UniqueId(uid));
            }

            // Fetch et
            var summaries = inbox.Fetch(uidRange,
                MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure);

            var result = new List<EmailMessagesDto>();
            foreach (var summary in summaries)
            {
                var message = await inbox.GetMessageAsync(summary.UniqueId);
                result.Add(MapToDto(summary, message, user.Id));
            }

            await client.DisconnectAsync(true);

            // UID sırasına göre döndür
            return result.OrderByDescending(e => e.Uid).ToList();
        }

        //dto ya çevriliyor
        private EmailMessagesDto MapToDto(IMessageSummary summary, MimeMessage message, string id)
        {
            var bodyText = message.TextBody ?? string.Empty;

            return new EmailMessagesDto
            {
                Uid = summary.UniqueId.Id,
                FromName = message.From.Mailboxes.FirstOrDefault()!.Name,
                FromAddress = message.From.Mailboxes.FirstOrDefault()!.Address,
                Subject = message.Subject,
                Snippet = bodyText.Length > 100 ? bodyText.Substring(0, 100) : bodyText,
                Date = message.Date.DateTime,
                To = string.Join(",", message.To.Mailboxes.Select(x => x.Address)),
                Cc = string.Join(",", message.Cc.Mailboxes.Select(x => x.Address)),
                Body = bodyText,
                UserId = id,

            };
        }
    }
}

