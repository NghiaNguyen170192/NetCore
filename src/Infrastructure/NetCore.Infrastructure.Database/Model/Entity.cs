using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.Infrastructure.Database.Model
{
    public class Entity
    {
        public virtual int Id { get; set; }

        //public DateTime CreatedOn { get; set; }
        
        //public Guid CreatedById { get; set; }

        //public DateTime ModifiedOn { get; set; }
        
        //public Guid ModifiedById { get; set; }
    }
}
