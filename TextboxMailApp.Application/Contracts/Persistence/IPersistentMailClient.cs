using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Contracts.Persistence
{
    public interface IPersistentMailClient
    {
        Task<ImapClient> GetConnectedClientAsync(User user);

    }
}
