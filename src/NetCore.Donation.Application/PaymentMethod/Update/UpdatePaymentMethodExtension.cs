namespace NetCore.Donation.Application.PaymentMethod.Update;

public static class UpdatePaymentMethodExtension
{
    public static void UpdateEntity(
        this UpdatePaymentMethodCommand request,
        Domain.Entities.PaymentMethod paymentMethod)
    {
        paymentMethod.UpdateDisplayName(request.DisplayName);
    }
}