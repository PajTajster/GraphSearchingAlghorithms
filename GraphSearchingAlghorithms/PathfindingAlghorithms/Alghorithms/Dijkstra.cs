using PathfindingAlghorithms.Alghorithms.AlghorithmResult;
using PathfindingAlghorithms.Alghorithms.DataObjects;
using PathfindingAlghorithms.Alghorithms.Interface;
using PathfindingAlghorithms.FileLoader;
using System.Collections.Generic;

namespace PathfindingAlghorithms.Alghorithms
{
    public class Dijkstra : IAlghorithm
    {
        public AlghorithmCalculationResult CalculateRoute(FileLoaderResult fileLoaderResult)
        {
            var incidenceList = fileLoaderResult.IncidenceList;

            var arraysLength = fileLoaderResult.CityAmount + 1;

            var visitedArray = new bool[arraysLength];
            var distancesArray = new int[arraysLength];
            var predecessorsArray = new int[arraysLength];
            var isAlreadyAddedInCityDistances = new bool[arraysLength];

            for (int i = 1; i < arraysLength; ++i)
            {
                distancesArray[i] = int.MaxValue;
            }

            var cityDistances = new SortedSet<AlgorithmCity>(new AlgorithmCityComparer());

            var startCity = fileLoaderResult.StartCity;
            var endCity = fileLoaderResult.EndCity;

            cityDistances.Add(new AlgorithmCity(startCity, 0));
            distancesArray[startCity] = 0;

            while (cityDistances.Count != 0)
            {
                var currentCity = cityDistances.Min;
                cityDistances.Remove(currentCity);
                isAlreadyAddedInCityDistances[currentCity.City] = false;

                visitedArray[currentCity.City] = true;

                foreach (var neighbour in incidenceList[currentCity.City])
                {
                    var possibleNewDistance = currentCity.Distance + neighbour.Distance;
                    if (possibleNewDistance < distancesArray[neighbour.ConnectedCity])
                    {
                        cityDistances.Remove(new AlgorithmCity(neighbour.ConnectedCity, distancesArray[neighbour.ConnectedCity]));
                        isAlreadyAddedInCityDistances[neighbour.ConnectedCity] = false;

                        distancesArray[neighbour.ConnectedCity] = possibleNewDistance;
                        predecessorsArray[neighbour.ConnectedCity] = currentCity.City;
                    }

                    if (visitedArray[neighbour.ConnectedCity] == false && isAlreadyAddedInCityDistances[neighbour.ConnectedCity] == false)
                    {
                        cityDistances.Add(new AlgorithmCity(neighbour.ConnectedCity, distancesArray[neighbour.ConnectedCity]));
                        isAlreadyAddedInCityDistances[currentCity.City] = true;
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

            return new DijkstraResult(distancesArray[endCity], path);
        }
    }
}
