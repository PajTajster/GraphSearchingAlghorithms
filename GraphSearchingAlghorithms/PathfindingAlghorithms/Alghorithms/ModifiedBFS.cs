using PathfindingAlghorithms.Alghorithms.AlghorithmResult;
using PathfindingAlghorithms.Alghorithms.DataObjects;
using PathfindingAlghorithms.Alghorithms.Interface;
using PathfindingAlghorithms.FileLoader;
using System.Collections.Generic;

namespace PathfindingAlghorithms.Alghorithms
{
    public class ModifiedBFS : IAlghorithm
    {
        AlghorithmCalculationResult IAlghorithm.CalculateRoute(FileLoaderResult fileLoaderResult)
        {
            var arraysLength = fileLoaderResult.CityAmount + 1;

            var distanceArray = new Dictionary<int, BFSDistanceVertex>();
            var predecessorsArray = new int[arraysLength];

            var searchedCitiesQueue = new Queue<int>(arraysLength);

            var startCity = fileLoaderResult.StartCity;
            var endCity = fileLoaderResult.EndCity;

            for (int i = 1; i <= arraysLength; ++i)
            {
                distanceArray.Add(i, new BFSDistanceVertex(int.MaxValue, int.MaxValue));
            }

            distanceArray[startCity] = new BFSDistanceVertex(0, 0);

            searchedCitiesQueue.Enqueue(startCity);

            while (searchedCitiesQueue.Count != 0)
            {
                var currentlySearchedCity = searchedCitiesQueue.Dequeue();
                foreach (var neighbour in fileLoaderResult.IncidenceList[currentlySearchedCity])
                {
                    var newPossibleDistance = distanceArray[currentlySearchedCity].DistanceInCities + 1;

                    var currentDistanceToNeighbour = distanceArray[neighbour.ConnectedCity].DistanceInCities;
                    if (currentDistanceToNeighbour >= newPossibleDistance)
                    {
                        var newRealPossibleDistance = distanceArray[currentlySearchedCity].RealDistance + neighbour.Distance;
                        var currentRealDistance = distanceArray[neighbour.ConnectedCity].RealDistance;

                        if (currentRealDistance > newRealPossibleDistance)
                        {
                            searchedCitiesQueue.Enqueue(neighbour.ConnectedCity);

                            distanceArray[neighbour.ConnectedCity].DistanceInCities = newPossibleDistance;
                            distanceArray[neighbour.ConnectedCity].RealDistance = newRealPossibleDistance;

                            predecessorsArray[neighbour.ConnectedCity] = currentlySearchedCity;
                        }
                    }
                }
            }

            var path = new List<int>();

            path.Add(endCity);
            var pathReadCurrentCity = predecessorsArray[endCity];
            path.Add(pathReadCurrentCity);

            do
            {
                pathReadCurrentCity = predecessorsArray[pathReadCurrentCity];

                path.Add(pathReadCurrentCity);

            } while (pathReadCurrentCity != startCity);

            return new BFSResult(path.Count - 2, distanceArray[endCity].RealDistance, path);
        }
    }
}
