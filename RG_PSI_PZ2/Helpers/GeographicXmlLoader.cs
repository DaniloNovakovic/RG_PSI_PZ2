﻿using RG_PSI_PZ2.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace RG_PSI_PZ2.Helpers
{
    public class GeographicXmlLoader
    {
        private readonly XmlDocument _doc;
        private readonly int _zoneUtm = 34;

        public GeographicXmlLoader(string path = "Geographic.xml", int zoneUtm = 34)
        {
            _doc = new XmlDocument();
            _zoneUtm = zoneUtm;

            _doc.Load(path);
        }

        public IEnumerable<NodeEntity> GetNodeEntities(string xpath = "/NetworkModel/Nodes/NodeEntity")
        {
            return GetEntities<NodeEntity>(xpath);
        }

        public IEnumerable<SubstationEntity> GetSubstationEntities(string xpath = "/NetworkModel/Substations/SubstationEntity")
        {
            return GetEntities<SubstationEntity>(xpath);
        }

        private IEnumerable<T> GetEntities<T>(string xpath) where T : PowerEntity, new()
        {
            var entityList = new List<T>();

            foreach (XmlNode item in _doc.DocumentElement.SelectNodes(xpath))
            {
                long id = long.Parse(item.SelectSingleNode("Id").InnerText);
                string name = item.SelectSingleNode("Name").InnerText;

                double utmX = double.Parse(item.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                double utmY = double.Parse(item.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);

                CoordinateConversion.ToLatLon(utmX, utmY, _zoneUtm, out double x, out double y);

                entityList.Add(new T() { Id = id, Name = name, X = x, Y = y });
            }

            return entityList;
        }
    }
}