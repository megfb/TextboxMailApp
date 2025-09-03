using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence.EmailMessages
{
    public class EmailMessagesConfiguration : IEntityTypeConfiguration<EmailMessage>
    {
        public void Configure(EntityTypeBuilder<EmailMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Body).IsRequired();
            builder.Property(x => x.FromAddress).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Subject).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Snippet).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Cc).IsRequired(false);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired(false);
            builder.Property(x => x.FromName).HasMaxLength(256).IsRequired();
            builder.Property(x => x.To).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Date).IsRequired();
        }
    }
}
