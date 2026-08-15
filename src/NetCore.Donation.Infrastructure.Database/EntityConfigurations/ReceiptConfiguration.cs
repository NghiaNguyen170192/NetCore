using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.Donation.Domain.Entities;

namespace NetCore.Donation.Infrastructure.Database.EntityConfigurations;

public class ReceiptConfiguration : EntityTypeConfiguration<Receipt>
{
    public override void Configure(EntityTypeBuilder<Receipt> builder)
    {
        builder.HasIndex(receipt => receipt.ContactId);
        builder.HasIndex(receipt => receipt.TransactionId);

        builder
            .HasOne(receipt => receipt.Contact)
            .WithMany()
            .HasForeignKey(receipt => receipt.ContactId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(receipt => receipt.Transaction)
            .WithMany()
            .HasForeignKey(receipt => receipt.TransactionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}