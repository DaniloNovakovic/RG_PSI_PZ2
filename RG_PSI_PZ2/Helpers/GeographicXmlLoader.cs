using RG_PSI_PZ2.Model;
using System;
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

        public IEnumerable<SwitchEntity> GetSwitchEntities(string xpath = "/NetworkModel/Switches/SwitchEntity")
        {
            return GetEntities<SwitchEntity>(xpath, (xmlNode, entity) =>
            {
                entity.Status = xmlNode.SelectSingleNode("Status").InnerText;
            });
        }

        public IEnumerable<LineEntity> GetLineEntities(string xpath = "/NetworkModel/Lines/LineEntity")
        {
            var lineEntities = new List<LineEntity>();

            foreach (XmlNode node in _doc.DocumentElement.SelectNodes(xpath))
            {
                var line = new LineEntity
                {
                    Id = long.Parse(node.SelectSingleNode("Id").InnerText),
                    Name = node.SelectSingleNode("Name").InnerText,
                    IsUnderground = node.SelectSingleNode("IsUnderground").InnerText.Equals("true"),
                    R = float.Parse(node.SelectSingleNode("R").InnerText),
                    ConductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText,
                    LineType = node.SelectSingleNode("LineType").InnerText,
                    ThermalConstantHeat = long.Parse(node.SelectSingleNode("ThermalConstantHeat").InnerText),
                    FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText),
                    SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText)
                };

                lineEntities.Add(line);
            }

            return lineEntities;
        }

        private IEnumerable<T> GetEntities<T>(string xpath, Action<XmlNode, T> action = null) where T : PowerEntity, new()
        {
            var entityList = new List<T>();

            foreach (XmlNode item in _doc.DocumentElement.SelectNodes(xpath))
            {
                long id = long.Parse(item.SelectSingleNode("Id").InnerText);
                string name = item.SelectSingleNode("Name").InnerText;

                double utmX = double.Parse(item.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                double utmY = double.Parse(item.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);

                CoordinateConversion.ToLatLon(utmX, utmY, _zoneUtm, out double x, out double y);

                var entity = new T() { Id = id, Name = name, X = x, Y = y };

                action?.Invoke(item, entity);

                entityList.Add(entity);
            }

            return entityList;
        }
    }
}