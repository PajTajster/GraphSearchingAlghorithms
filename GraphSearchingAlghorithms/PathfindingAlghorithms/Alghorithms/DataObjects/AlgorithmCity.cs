using System.Collections.Generic;

namespace PathfindingAlghorithms.Alghorithms.DataObjects
{
    public class AlgorithmCity
    {
        public AlgorithmCity(int city, int distance)
        {
            City = city;
            Distance = distance;
        }

        public int City { get; set; }
        public int Distance { get; set; }
    }
    public class AlgorithmCityComparer : IComparer<AlgorithmCity>
    {
        public int Compare(AlgorithmCity x, AlgorithmCity y)
        {
            var compare = Comparer<int>.Default.Compare(x.Distance, y.Distance);

            if (compare == 0)
            {
                compare = Comparer<int>.Default.Compare(x.City, y.City);
            }

            return compare;
        }
    }
}
