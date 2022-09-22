using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCore.Tools.Migration.Common.Interface;

namespace NetCore.Tools.Migration.Common;

public abstract class TestDataSeed : ITestDataSeed
{
    public abstract IEnumerable<Type> Dependencies { get; }

    public abstract Task SeedAsync();
}