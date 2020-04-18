namespace PathfindingAlghorithms.Common
{
    public class CityGeoCooridnates
    {
        public CityGeoCooridnates(int latitude, int longtitude)
        {
            Latitude = latitude;
            Longtitude = longtitude;
        }

        public int Latitude { get; set; }
        public int Longtitude { get; set; }
    }
}
