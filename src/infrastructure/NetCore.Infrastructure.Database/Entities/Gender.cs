using NetCore.Infrastructure.Database.Commons;

namespace NetCore.Infrastructure.Database.Entities;

public class Gender : AuditableEntity
{
    public string Name { get; set; }
}
