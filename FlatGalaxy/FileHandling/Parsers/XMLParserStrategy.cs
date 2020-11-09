using FlatGalaxy.FileHandling.Parsers;
using FlatGalaxy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace FlatGalaxy.Parsers
{
    /// <summary>
    /// 
    /// ## Concern
    /// 
    /// Strategy pattern http file parser. 
    /// Made for xml files.
    /// 
    /// </summary>
    public class XMLParserStrategy : AFileParserStrategy, IFileParserStrategy
    {
        public Galaxy Parse(List<string> fileLines)
        {
            base.Init();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(string.Join(string.Empty, fileLines));

            return new Galaxy(ParseNodes(xmlDoc.SelectSingleNode("galaxy").ChildNodes));
        }

        # region Node parsers
        private List<CelestialBody> ParseNodes(XmlNodeList nodes)
        {
            // Parse celestialbodies
            for (int i = 0; i < nodes.Count; i++)
            {
                switch (nodes[i].Name)
                {
                    case "planet":
                        _planets.Add(i, ParsePlanetNode(nodes[i]));
                        break;
                    case "asteroid":
                        _asteroids.Add(i, ParseAsteroidNode(nodes[i]));
                        break;
                }
            }

            // Set planet neighbours
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name.Equals("planet"))
                {
                    foreach (XmlNode node in nodes[i].SelectSingleNode("neighbours").SelectNodes("planet"))
                    {
                        _planets[i].AddNeighbour(_planets.Values.FirstOrDefault(cb => cb.Name == node.InnerText));
                    }
                }
            }

            List<CelestialBody> celestialBodies = new List<CelestialBody>();
            celestialBodies.AddRange(_planets.Values);
            celestialBodies.AddRange(_asteroids.Values);

            return celestialBodies;
        }

        private Planet ParsePlanetNode(XmlNode node) => new Planet(
            name: node.SelectSingleNode("name").InnerText,
            xPosition: Convert.ToDouble(node.SelectSingleNode("position/x").InnerText),
            yPosition: Convert.ToDouble(node.SelectSingleNode("position/y").InnerText),
            radius: Convert.ToInt32(node.SelectSingleNode("position/radius").InnerText),
            xVelocity: Convert.ToDouble(node.SelectSingleNode("speed/x").InnerText),
            yVelocity: Convert.ToDouble(node.SelectSingleNode("speed/y").InnerText),
            colour: SelectColour(node.SelectSingleNode("color").InnerText),
            state: SelectState(node.SelectSingleNode("oncollision").InnerText)
        );

        private Asteroid ParseAsteroidNode(XmlNode node) => new Asteroid(
            xPosition: Convert.ToDouble(node.SelectSingleNode("position/x").InnerText),
            yPosition: Convert.ToDouble(node.SelectSingleNode("position/y").InnerText),
            radius: Convert.ToInt32(node.SelectSingleNode("position/radius").InnerText),
            xVelocity: Convert.ToDouble(node.SelectSingleNode("speed/x").InnerText),
            yVelocity: Convert.ToDouble(node.SelectSingleNode("speed/y").InnerText),
            state: SelectState(node.SelectSingleNode("oncollision").InnerText)
        );
        #endregion Node parsers
    }
}
