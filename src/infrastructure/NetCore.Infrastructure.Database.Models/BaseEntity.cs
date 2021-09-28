using System;

namespace NetCore.Infrastructure.Database.Models
{
    public class BaseEntity
    {
        public virtual Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedById { get; set; }
    }
}
