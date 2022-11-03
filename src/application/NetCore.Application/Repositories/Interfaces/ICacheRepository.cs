using NetCore.Infrastructure.Database.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Application.Repositories.Interfaces;

public interface ICacheRepository<TEntity> where TEntity : BaseEntity
{
}
