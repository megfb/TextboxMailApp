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
    public class MailKitEmailReader : IEmailReader
    {
        //private readonly string _imapServer = "imap.gmail.com";
        //private readonly int _port = 993;
        //private readonly string _username = "me733017@gmail.com";
        //private readonly string _password = "krhocsllqjkjhegk";
        //private readonly string _imapServer;
        //private readonly int _port;
        //private readonly string _username;
        //private readonly string _password;

        //public MailKitEmailReader(string imapServer, int port, string username, string password)
        //{
        //    _imapServer = imapServer;
        //    _port = port;
        //    _username = username;
        //    _password = password;
        //}
        //private async Task<ImapClient> ConnectAsync()
        //{
        //    var client = new ImapClient();
        //    try
        //    {
        //        await client.ConnectAsync(_imapServer, _port, SecureSocketOptions.SslOnConnect);
        //        await client.AuthenticateAsync(_username, _password);

        //        return client;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Bağlantı hatası: {ex.Message}");
        //        throw;
        //    }
        //}
        private string ExtractPlainText(string htmlBody)
        {
            if (string.IsNullOrWhiteSpace(htmlBody))
                return string.Empty;

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);

            return doc.DocumentNode.InnerText;
        }
        private EmailMessagesDto MapToDto(IMessageSummary summary, MimeMessage message,string id)
        {
            var bodyText = message.TextBody;

            if (string.IsNullOrEmpty(bodyText) && !string.IsNullOrEmpty(message.HtmlBody))
            {
                bodyText = ExtractPlainText(message.HtmlBody);
            }

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
                UserId = id
            };
        }
        public async Task<List<EmailMessagesDto>> GetEmailsByPageAsync(int page, int pageSize, User user)
        {
            var client = new ImapClient();

            await client.ConnectAsync(user.ServerName, user.Port, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(user.EmailAddress, user.EmailPassword);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);

            if (inbox.Count == 0)
                return new List<EmailMessagesDto>();

            // Tüm UID'leri al
            var uids = inbox.Search(MailKit.Search.SearchQuery.All);

            // UID'leri büyükten küçüğe sırala (en son gelen en başta)
            var orderedUids = uids.OrderByDescending(u => u.Id).ToList();

            // Sayfaya göre aralık seç
            var skip = (page - 1) * pageSize;
            var take = pageSize;

            var pageUids = orderedUids.Skip(skip).Take(take).ToList();
            if (!pageUids.Any())
                return new List<EmailMessagesDto>();

            // UID'lere göre fetch et
            var summaries = inbox.Fetch(pageUids,
                MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure);

            var result = new List<EmailMessagesDto>();
            foreach (var summary in summaries)
            {
                var message = await inbox.GetMessageAsync(summary.UniqueId);
                result.Add(MapToDto(summary, message,user.Id));
            }

            await client.DisconnectAsync(true);

            // Uid göre azalan sırada döndür
            return result.OrderByDescending(x => x.Uid).ToList();
        }

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
