#nullable disable

using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Domain.Entities;

public class Journal : Entity, IAggregateRoot
{
    public static Journal Create()
    {
        return new Journal();
    }
}
