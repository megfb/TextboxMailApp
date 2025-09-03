using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Contracts.Persistence
{
    public interface IEmailMessageRepository:IGenericRepository<EmailMessage>
    {

    }
}
