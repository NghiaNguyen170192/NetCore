using System.Threading.Tasks;

namespace NetCore.Tools.Migration.Common.Interface;

public interface IDataSeedRunner<T>
    where T : IDataSeed
{
    Task RunSeedsAsync();
}
