using TextboxMailApp.Domain.Entities.Common;

namespace TextboxMailApp.Domain.Entities
{
    public class EmailMessage : Entity
    {
        public uint Uid { get; set; }
        public string FromName { get; set; } = default!;
        public string FromAddress { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Snippet { get; set; } = default!;
        public DateTime Date { get; set; }
        public string To { get; set; } = default!;
        public string? Cc { get; set; }
        public string Body { get; set; } = default!;
        public string UserId { get; set; } = default!; // FK
        public User? User { get; set; }
    }
}
