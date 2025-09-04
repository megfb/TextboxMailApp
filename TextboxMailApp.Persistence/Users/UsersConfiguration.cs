using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence.Users
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.EmailAddress).HasMaxLength(256).IsRequired();
            builder.Property(x => x.EmailPassword).IsRequired();
            builder.Property(x => x.ServerName).IsRequired();
            builder.Property(x => x.Port).IsRequired();
        }
    }
}
