using FlatGalaxy.FileHandling.Parsers;
using FlatGalaxy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatGalaxy.Parsers
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// Strategy pattern http file parser. 
    /// Made for the csv files.
    /// 
    /// </summary>
    public class CSVParserStrategy : AFileParserStrategy, IFileParserStrategy
    {

        private readonly Dictionary<string, int> _headingIndexes;

        public CSVParserStrategy()
        {
            _headingIndexes = new Dictionary<string, int>();
        }

        public Galaxy Parse(List<string> fileLines)
        {
            base.Init();
            _headingIndexes.Clear();

            // Remove headers from lines and keep if needed
            string[] headers = fileLines[0].Split(';');

            for (int i = 0; i < headers.Length; i++)
                _headingIndexes.Add(headers[i].ToLower(), i);

            fileLines.RemoveAt(0);

            // Return galaxy containing celestialBodies
            return new Galaxy(ParseLines(fileLines));
        }

        #region Parsers
        private List<CelestialBody> ParseLines(List<string> lines)
        {
            // Parse celestialbodies
            for (int i = 0; i < lines.Count(); i++)
            {
                string[] values = lines[i].Split(";");

                switch (values[_headingIndexes["type"]].ToLower())
                {
                    case "planet":
                        _planets.Add(i, ParsePlanetNode(values));
                        break;
                    case "asteroid":
                        _asteroids.Add(i, ParseAsteroidNode(values));
                        break;
                }
            }

            // Set planet neighbours
            for (int i = 0; i < lines.Count(); i++)
            {
                string[] values = lines[i].Split(";");
                string type = values[1];

                if (type.ToLower().Equals("planet"))
                {
                    string[] neighbours = values[6].Split(',');
                    foreach (string neighbour in neighbours)
                    {
                        _planets[i].AddNeighbour(_planets.Values.FirstOrDefault(cb => cb.Name == neighbour));
                    }
                }
            };

            List<CelestialBody> celestialBodies = new List<CelestialBody>();
            celestialBodies.AddRange(_planets.Values);
            celestialBodies.AddRange(_asteroids.Values);

            return celestialBodies;
        }

        private Planet ParsePlanetNode(string[] values) => new Planet(
            name: values[_headingIndexes["name"]],
            xPosition: Convert.ToDouble(values[_headingIndexes["x"]]),
            yPosition: Convert.ToDouble(values[_headingIndexes["y"]]),
            radius: Convert.ToInt32(values[_headingIndexes["radius"]]),
            xVelocity: Convert.ToDouble(values[_headingIndexes["vx"]]),
            yVelocity: Convert.ToDouble(values[_headingIndexes["vy"]]),
            colour: SelectColour(values[_headingIndexes["color"]]),
            state: SelectState(values[_headingIndexes["oncollision"]])
        );

        private Asteroid ParseAsteroidNode(string[] values) => new Asteroid(
            xPosition: Convert.ToDouble(values[_headingIndexes["x"]]),
            yPosition: Convert.ToDouble(values[_headingIndexes["y"]]),
            radius: Convert.ToInt32(values[_headingIndexes["radius"]]),
            xVelocity: Convert.ToDouble(values[_headingIndexes["vx"]]),
            yVelocity: Convert.ToDouble(values[_headingIndexes["vy"]]),
            state: SelectState(values[_headingIndexes["oncollision"]])
        );
        #endregion Parsers
    }
}
