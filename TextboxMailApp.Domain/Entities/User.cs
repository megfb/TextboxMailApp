using TextboxMailApp.Domain.Entities.Common;

namespace TextboxMailApp.Domain.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string EmailAddress { get; set; } = default!;
        public string EmailPassword { get; set; } = default!;
        public string ServerName { get; set; } = default!;
        public int Port { get; set; }

    }
}
