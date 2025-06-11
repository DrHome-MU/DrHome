using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dr_Home.Data.Configuration
{
    public class MessageConfigurations : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.Property(x => x.SenderName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.SenderPhoneNumber)
               .HasMaxLength(11)
               .IsRequired();

            builder.Property(x => x.SenderEmail)
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(x => x.Content)
               .IsRequired();
        }
    }
}
