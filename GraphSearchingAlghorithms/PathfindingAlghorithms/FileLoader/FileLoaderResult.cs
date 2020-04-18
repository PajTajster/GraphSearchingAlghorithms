using PathfindingAlghorithms.Common;
using System.Collections.Generic;

namespace PathfindingAlghorithms.FileLoader
{
    public class FileLoaderResult
    {
        public bool IsLoadingSuccess { get; private set; }
        public int CityAmount { get; private set; }
        public int ConnectionsAmount { get; private set; }
        public Dictionary<int, List<ConnectedCityWithDistance>> IncidenceList { get; private set; }
        public Dictionary<int, CityGeoCooridnates> CitiesCoordinates { get; private set; }
        public int StartCity { get; private set; }
        public int EndCity { get; private set; }

        // Statistics
        public long LoadTime { get; private set; }

        public FileLoaderResult(int cityAmount, int connectionsAmount,
            Dictionary<int, List<ConnectedCityWithDistance>> incidenceList,
            Dictionary<int, CityGeoCooridnates> citiesCoordinates,
            int startCity, int endCity, long loadTime)
        {
            CityAmount = cityAmount;
            ConnectionsAmount = connectionsAmount;
            IncidenceList = incidenceList;
            CitiesCoordinates = citiesCoordinates;
            StartCity = startCity;
            EndCity = endCity;
            LoadTime = loadTime;
            IsLoadingSuccess = true;
        }
        public FileLoaderResult(bool isLoadingSuccess)
        {
            IsLoadingSuccess = isLoadingSuccess;
        }
    }
}
