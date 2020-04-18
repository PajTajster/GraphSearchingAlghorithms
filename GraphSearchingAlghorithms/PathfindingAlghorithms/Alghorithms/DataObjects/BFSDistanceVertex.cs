namespace PathfindingAlghorithms.Alghorithms.DataObjects
{
    public class BFSDistanceVertex
    {
        public int DistanceInCities { get; set; }
        public int RealDistance { get; set; }

        public BFSDistanceVertex(int cityDistance, int realDistance)
        {
            DistanceInCities = cityDistance;
            RealDistance = realDistance;
        }
    }
}
