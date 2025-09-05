using HtmlAgilityPack;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence
{
    public class MailKitEmailReader(IPersistentMailClient persistentMailClient) : IEmailReader
    {
        //Sayfalandırma ile mailleri çekiyor
        public async Task<List<EmailMessagesDto>> GetEmailsByPageAsync(int page, int pageSize, User user)
        {
            var client = await persistentMailClient.GetConnectedClientAsync(user);
            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);

            if (inbox.Count == 0)
                return new List<EmailMessagesDto>();

            // Sayfalama için başlangıç ve bitiş indekslerini hesapla
            int skip = (page - 1) * pageSize;
            int take = Math.Min(pageSize, inbox.Count - skip); // Alınacak UID sayısını sınırla

            if (skip >= inbox.Count || take <= 0)
                return new List<EmailMessagesDto>();

            // UID'leri tümüyle çekmek yerine, sadece gerekli aralığı al
            var startIndex = Math.Max(0, inbox.Count - skip - take); // En yeni mesajdan başla
            var endIndex = Math.Min(inbox.Count - 1, inbox.Count - skip - 1); // Bitiş indeksi

            // UID'leri toplu olarak almak yerine, doğrudan mesaj özetlerini fetch et
            var summaries = inbox.Fetch(startIndex, endIndex, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure);

            var result = new List<EmailMessagesDto>();
            foreach (var summary in summaries)
            {
                var message = await inbox.GetMessageAsync(summary.UniqueId);
                result.Add(MapToDto(summary, message, user.Id));
            }

            await client.DisconnectAsync(true);

            // Uid göre azalan sırada döndür
            return result.OrderByDescending(x => x.Uid).ToList();
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
            };
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
                result.Add(MapToDto(summary, message,user.Id));
            }

            await client.DisconnectAsync(true);

            // UID sırasına göre döndür
            return result.OrderByDescending(e => e.Uid).ToList();
        }



    }
}
