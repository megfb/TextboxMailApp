using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextboxMailApp.Domain.Entities.Common;

namespace TextboxMailApp.Domain.Entities
{
    public class EmailMessage:Entity
    {
        public string FromName { get; set; } = default!;
        public string FromAddress { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Snippet { get; set; } = default!;
        public DateTime Date { get; set; }
        public string To { get; set; } = default!;
        public string? Cc { get; set; }
        public string Body { get; set; } = default!;
    }
}
