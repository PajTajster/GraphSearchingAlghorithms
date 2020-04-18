using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingAlghorithms.Alghorithms.AlghorithmResult
{
    public class BFSResult : AlghorithmCalculationResult
    {
        public int CityAmount { get; private set; }
        public int PathLength { get; private set; }
        public List<int> Path { get; private set; }

        public BFSResult(int cityAmount, int pathLength, List<int> path)
        {
            CityAmount = cityAmount;
            PathLength = pathLength;
            Path = path;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(CityAmount.ToString() + ' ' + PathLength.ToString() + newLine);

            for (int i = Path.Count - 1; i >= 0; --i)
            {
                stringBuilder.Append(Path[i].ToString() + ' ');
            }

            return stringBuilder.ToString();
        }
    }
}
