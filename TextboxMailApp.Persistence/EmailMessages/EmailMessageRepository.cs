using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence.EmailMessages
{
    public class EmailMessageRepository(AppDbContext appDbContext):GenericRepository<EmailMessage>(appDbContext), IEmailMessageRepository
    {
    }
}
