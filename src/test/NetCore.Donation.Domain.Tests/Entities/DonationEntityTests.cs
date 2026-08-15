using NetCore.Donation.Domain.Entities;
using NetCore.Donation.Domain.Enums;

namespace NetCore.Donation.Domain.Tests.Entities;

[TestClass]
public class DonationEntityTests
{
    [TestMethod]
    public void ContactCreate_WithValidDetails_CreatesActiveContact()
    {
        var countryId = Guid.NewGuid();

        var contact = Contact.Create(
            "Ada",
            "Lovelace",
            new DateOnly(1815, 12, 10),
            "1 Computing Lane",
            "ada@example.com",
            "+61 400 000 000",
            countryId);

        Assert.AreEqual("Ada", contact.FirstName);
        Assert.AreEqual(countryId, contact.CountryId);
        Assert.IsTrue(contact.IsActive);
    }

    [TestMethod]
    public void ContactCreate_WithEmptyCountryId_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => Contact.Create(
            "Ada",
            "Lovelace",
            new DateOnly(1815, 12, 10),
            "1 Computing Lane",
            "ada@example.com",
            "+61 400 000 000",
            Guid.Empty));
    }

    [TestMethod]
    public void PaymentScheduleCreate_WithNonPositiveAmount_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PaymentSchedule.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            0,
            DateOnly.FromDateTime(DateTime.UtcNow),
            RecurringInterval.Monthly));
    }

    [TestMethod]
    public void TransactionCreate_WithReceivedDateBeforeBookDate_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Transaction.Create(
            25,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            PaymentType.CreditCard,
            new DateOnly(2026, 8, 15),
            new DateOnly(2026, 8, 14)));
    }

    [TestMethod]
    public void ReceiptCreate_WithoutTransaction_ReservesNullableLink()
    {
        var receipt = Receipt.Create(Guid.NewGuid());

        Assert.IsNull(receipt.TransactionId);
    }
}