using PathfindingAlghorithms.Alghorithms.AlghorithmResult;
using PathfindingAlghorithms.FileLoader;

namespace PathfindingAlghorithms.Alghorithms.Interface
{
    public interface IAlghorithm
    {
        AlghorithmCalculationResult CalculateRoute(FileLoaderResult fileLoaderResult);
    }
}
