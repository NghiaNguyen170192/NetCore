using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Migration.Common.Interface;

public interface IDataSeed
{
    IEnumerable<Type> Dependencies { get; }

    Task SeedAsync();
}
