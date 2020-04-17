using RG_PSI_PZ2.Model;
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

        public List<NodeEntity> GetNodeEntities(string xpath = "/NetworkModel/Nodes/NodeEntity")
        {
            var entityList = new List<NodeEntity>();

            foreach (XmlNode item in _doc.DocumentElement.SelectNodes(xpath))
            {
                double utmX = double.Parse(item.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                double utmY = double.Parse(item.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);

                CoordinateConversion.ToLatLon(utmX, utmY, _zoneUtm, out double x, out double y);

                entityList.Add(new NodeEntity() { X = x, Y = y });
            }

            return entityList;
        }
    }
}