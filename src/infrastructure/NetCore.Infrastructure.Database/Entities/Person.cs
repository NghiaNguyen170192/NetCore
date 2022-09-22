using System;
using NetCore.Infrastructure.Database.Commons;

namespace NetCore.Infrastructure.Database.Entities;

public class Person : AuditableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public string Website { get; set; }
}
