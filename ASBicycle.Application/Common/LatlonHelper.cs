using System;
using Abp.Logging;

namespace ASBicycle.Common
{
    public static class LatlonHelper
    {
        private const double EARTH_RADIUS = 6378.137;//地球半径  KM
        private static double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        /// <summary>
        /// 2个经纬度之间的距离
        /// </summary>
        /// <param name="lat1">纬度1</param>
        /// <param name="lon1">经度1</param>
        /// <param name="lat2">纬度2</param>
        /// <param name="lon2">经度2</param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double radLat1 = Rad(lat1);
            double radLat2 = Rad(lat2);
            double a = radLat1 - radLat2;
            double b = Rad(lon1) - Rad(lon2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
    }
}