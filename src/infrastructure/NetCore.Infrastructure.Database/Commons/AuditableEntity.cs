using System;

namespace NetCore.Infrastructure.Database.Commons;
public class AuditableEntity : BaseEntity
{
    public DateTime CreatedOn { get; set; }

    public Guid CreatedById { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid ModifiedById { get; set; }
}
