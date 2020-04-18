using PathfindingAlghorithms.Alghorithms.AlghorithmResult;
using PathfindingAlghorithms.Alghorithms.DataObjects;
using PathfindingAlghorithms.Alghorithms.Interface;
using PathfindingAlghorithms.Common;
using PathfindingAlghorithms.FileLoader;
using System;
using System.Collections.Generic;

namespace PathfindingAlghorithms.Alghorithms
{
    public class AStar : IAlghorithm
    {
        public AlghorithmCalculationResult CalculateRoute(FileLoaderResult fileLoaderResult)
        {
            var incidenceList = fileLoaderResult.IncidenceList;

            var startCity = fileLoaderResult.StartCity;
            var endCity = fileLoaderResult.EndCity;

            if (fileLoaderResult.CitiesCoordinates == null || fileLoaderResult.CitiesCoordinates.Count != fileLoaderResult.CityAmount)
            {
                return new AStarResult(0, new List<int>());
            }

            var heuristicValuation = GetHeuristicsValuation(endCity, fileLoaderResult.CitiesCoordinates);

            var arraysLength = fileLoaderResult.CityAmount + 1;

            var estimatedDistanceArray = new int[arraysLength];
            var distancesArray = new int[arraysLength];

            for (int i = 0; i < arraysLength; ++i)
            {
                estimatedDistanceArray[i] = distancesArray[i] = int.MaxValue;
            }

            var predecessorsArray = new int[arraysLength];
            var visitedArray = new bool[arraysLength];
            var isAlreadyAddedInCityDistances = new bool[arraysLength];

            distancesArray[startCity] = 0;
            estimatedDistanceArray[startCity] = heuristicValuation[startCity];

            var processedCollection = new SortedSet<AlgorithmCity>(new AlgorithmCityComparer());
            processedCollection.Add(new AlgorithmCity(startCity, estimatedDistanceArray[startCity]));

            while (processedCollection.Count != 0)
            {
                var currentCity = processedCollection.Min;
                processedCollection.Remove(currentCity);

                visitedArray[currentCity.City] = true;

                if (currentCity.City == endCity)
                {
                    break;
                }

                foreach (var neighbour in incidenceList[currentCity.City])
                {
                    var newPossibleDistance = distancesArray[currentCity.City] + neighbour.Distance;

                    if (newPossibleDistance < distancesArray[neighbour.ConnectedCity])
                    {
                        processedCollection.Remove(new AlgorithmCity(neighbour.ConnectedCity, estimatedDistanceArray[neighbour.ConnectedCity]));
                        isAlreadyAddedInCityDistances[neighbour.ConnectedCity] = false;

                        distancesArray[neighbour.ConnectedCity] = newPossibleDistance;
                        estimatedDistanceArray[neighbour.ConnectedCity] = newPossibleDistance + heuristicValuation[neighbour.ConnectedCity];
                        predecessorsArray[neighbour.ConnectedCity] = currentCity.City;

                        if (visitedArray[neighbour.ConnectedCity] == false && isAlreadyAddedInCityDistances[neighbour.ConnectedCity] == false)
                        {
                            processedCollection.Add(new AlgorithmCity(neighbour.ConnectedCity, estimatedDistanceArray[neighbour.ConnectedCity]));
                            isAlreadyAddedInCityDistances[neighbour.ConnectedCity] = true;
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

            return new AStarResult(distancesArray[endCity], path);
        }

        private int GetDistanceToTheCity(CityGeoCooridnates from, CityGeoCooridnates to)
        {
            return (int)Math.Round(Math.Sqrt(Math.Pow((to.Latitude - from.Latitude), 2) + Math.Pow((to.Longtitude - from.Longtitude), 2)));
        }

        private int[] GetHeuristicsValuation(int cityToValuate, Dictionary<int, CityGeoCooridnates> coordinatesDict)
        {
            var arrayLength = coordinatesDict.Count + 1;
            var result = new int[arrayLength];

            var cityToCalculateDistanceTo = coordinatesDict[cityToValuate];

            for (int i = 1; i < arrayLength; ++i)
            {
                result[i] = GetDistanceToTheCity(coordinatesDict[i], cityToCalculateDistanceTo);
            }

            return result;
        }
    }
}
