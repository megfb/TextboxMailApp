namespace TextboxMailApp.Application.Features.Users
{
    public class UsersDto
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string EmailAddress { get; set; } = default!;
        public string EmailPassword { get; set; } = default!;
        public string ServerName { get; set; } = default!;
        public int Port { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
