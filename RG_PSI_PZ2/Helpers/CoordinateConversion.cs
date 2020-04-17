using System;

namespace RG_PSI_PZ2.Helpers
{
    public static class CoordinateConversion
    {
        /// <summary>
        /// Converts coordinates from UTM format into decimal
        /// </summary>
        /// <param name="zoneUTM">ex. 34</param>
        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            const bool isNorthHemisphere = true;

            const double diflat = -0.00066286966871111111111111111111111111;
            const double diflon = -0.0003868060578;

            int zone = zoneUTM;
            const double c_sa = 6378137.000000;
            const double c_sb = 6356752.314245;
            double e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            double e2cuadrada = Math.Pow(e2, 2);
            double c = Math.Pow(c_sa, 2) / c_sb;
            double x = utmX - 500000;
            double y = isNorthHemisphere ? utmY : utmY - 10000000;

            double s = ((zone * 6.0) - 183.0);
            double lat = y / (c_sa * 0.9996);
            double v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            double a = x / v;
            double a1 = Math.Sin(2 * lat);
            double a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            double j2 = lat + (a1 / 2.0);
            double j4 = ((3 * j2) + a2) / 4.0;
            double j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            double alfa = (3.0 / 4.0) * e2cuadrada;
            double beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            double gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            double bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            double b = (y - bm) / v;
            double epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            double eps = a * (1 - (epsi / 3.0));
            double nab = (b * (1 - epsi)) + lat;
            double senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            double delt = Math.Atan(senoheps / (Math.Cos(nab)));
            double tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }

        public static double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            return minScale + ((value - min) / (max - min) * (maxScale - minScale));
        }
    }
}