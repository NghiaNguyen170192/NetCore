using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.Donation.Domain.Entities;

namespace NetCore.Donation.Infrastructure.Database.EntityConfigurations;

public class JournalConfiguration : EntityTypeConfiguration<Journal>
{
    public override void Configure(EntityTypeBuilder<Journal> builder)
    {
        builder.HasIndex(journal => journal.TransactionId);

        builder
            .HasOne(journal => journal.Transaction)
            .WithMany()
            .HasForeignKey(journal => journal.TransactionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
