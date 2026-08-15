using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.Donation.Domain.Entities;

namespace NetCore.Donation.Infrastructure.Database.EntityConfigurations;

public class TransactionConfiguration : EntityTypeConfiguration<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(transaction => transaction.Amount).HasPrecision(18, 2);
        builder.Property(transaction => transaction.PaymentType).HasConversion<string>().HasMaxLength(20);
        builder.HasIndex(transaction => transaction.PaymentScheduleId);
        builder.HasIndex(transaction => transaction.ContactId);
        builder.HasIndex(transaction => transaction.PaymentMethodId);

        builder
            .HasOne(transaction => transaction.PaymentSchedule)
            .WithMany()
            .HasForeignKey(transaction => transaction.PaymentScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(transaction => transaction.Contact)
            .WithMany()
            .HasForeignKey(transaction => transaction.ContactId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(transaction => transaction.PaymentMethod)
            .WithMany()
            .HasForeignKey(transaction => transaction.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}