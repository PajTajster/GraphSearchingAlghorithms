using PathfindingAlghorithms.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PathfindingAlghorithms.FileLoader
{
    public class FileLoader
    {
        private int cityAmount = 0;
        private int connectionsAmount = 0;
        private Dictionary<int, List<ConnectedCityWithDistance>> incidenceList = new Dictionary<int, List<ConnectedCityWithDistance>>();
        private Dictionary<int, CityGeoCooridnates> citiesCoordinates = new Dictionary<int, CityGeoCooridnates>();
        private int startCity = 0;
        private int endCity = 0;

        private bool isFirstLineRead = false;

        private bool loadCoordinates = false;
        private int coordinatesCurrentCityBeingRead = 1;

        public FileLoaderResult LoadFile(string filePath, bool loadGeoCoordinates)
        {
            loadCoordinates = loadGeoCoordinates;
            var isParsingCorrect = true;

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            try
            {
                foreach (var fileLineData in File.ReadLines(filePath))
                {
                    isParsingCorrect = ParseData(fileLineData);
                    if (!isParsingCorrect)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                isParsingCorrect = false;
            }

            if (loadCoordinates && citiesCoordinates.Count != cityAmount)
            {
                isParsingCorrect = false;
            }

            stopwatch.Stop();

            return GetLoadingResult(isParsingCorrect, stopwatch.ElapsedMilliseconds);
        }

        private FileLoaderResult GetLoadingResult(bool isLoadingSuccess, long timeTaken)
        {
            if (isLoadingSuccess)
            {
                return new FileLoaderResult(cityAmount, connectionsAmount, incidenceList, citiesCoordinates, startCity, endCity, timeTaken);
            }
            else
            {
                return new FileLoaderResult(isLoadingSuccess);
            }
        }

        private bool ParseData(string data)
        {
            var splitData = data.Split(' ');

            if (splitData.Length == 2)
            {
                if (!isFirstLineRead)
                {
                    if (!int.TryParse(splitData[0], out cityAmount)
                        || !int.TryParse(splitData[1], out connectionsAmount))
                    {
                        return false;
                    }
                    isFirstLineRead = true;
                }
                else if (loadCoordinates && coordinatesCurrentCityBeingRead <= cityAmount)
                {
                    if (!int.TryParse(splitData[0], out var cityLatitude)
                        || !int.TryParse(splitData[1], out var cityLongtitude))
                    {
                        return false;
                    }
                    citiesCoordinates.Add(coordinatesCurrentCityBeingRead++, new CityGeoCooridnates(cityLatitude, cityLongtitude));
                }
                else
                {
                    if (!int.TryParse(splitData[0], out startCity)
                        || !int.TryParse(splitData[1], out endCity))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return ParseConnection(splitData);
            }

            return true;
        }

        private bool ParseConnection(string[] splitData)
        {
            if (!int.TryParse(splitData[0], out var firstCity)
                || !int.TryParse(splitData[1], out var secondCity)
                || !int.TryParse(splitData[2], out var distance))
            {
                return false;
            }

            List<ConnectedCityWithDistance> connections = null;

            // First City
            if (incidenceList.ContainsKey(firstCity))
            {
                connections = incidenceList[firstCity];
            }
            else
            {
                connections = new List<ConnectedCityWithDistance>();
            }

            connections.Add(new ConnectedCityWithDistance() { ConnectedCity = secondCity, Distance = distance });

            incidenceList[firstCity] = connections;


            // Second City
            if (incidenceList.ContainsKey(secondCity))
            {
                connections = incidenceList[secondCity];
            }
            else
            {
                connections = new List<ConnectedCityWithDistance>();
            }

            connections.Add(new ConnectedCityWithDistance() { ConnectedCity = firstCity, Distance = distance });

            incidenceList[secondCity] = connections;

            return true;
        }
    }
}
