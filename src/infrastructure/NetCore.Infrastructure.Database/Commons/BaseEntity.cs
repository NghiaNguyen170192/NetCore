using System;

namespace NetCore.Infrastructure.Database.Commons;

public abstract class BaseEntity
{
    public virtual Guid Id { get; set; }
}
