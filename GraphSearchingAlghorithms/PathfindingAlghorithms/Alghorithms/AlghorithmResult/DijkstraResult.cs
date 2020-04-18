using System.Collections.Generic;
using System.Text;

namespace PathfindingAlghorithms.Alghorithms.AlghorithmResult
{
    public class DijkstraResult : AlghorithmCalculationResult
    {
        public int PathLength { get; private set; }
        public List<int> Path { get; private set; }

        public DijkstraResult(int pathLength, List<int> path)
        {
            PathLength = pathLength;
            Path = path;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(PathLength.ToString() + newLine);

            for (int i = Path.Count - 1; i >= 0; --i)
            {
                stringBuilder.Append(Path[i].ToString() + ' ');
            }

            return stringBuilder.ToString();
        }
    }
}
