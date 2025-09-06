namespace TextboxMailApp.Application.Features.EmailMessages
{
    public class EmailMessagesDto
    {
        public string Id { get; set; }
        public uint Uid { get; set; }
        public string FromName { get; set; } = default!;
        public string FromAddress { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Snippet { get; set; } = default!;
        public DateTime Date { get; set; }
        public string To { get; set; } = default!;
        public string? Cc { get; set; }
        public string Body { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
