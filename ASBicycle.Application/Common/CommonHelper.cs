using System.Collections.Generic;
using System.Linq;
using ASBicycle.Bike.Dto;

namespace ASBicycle.Common
{
    public class CommonHelper
    {
        public static void OrderByRule(List<CouponDto> list, double pay)
        {
            var dk1 = list.Where(t => double.Parse(t.Value) == 1 && int.Parse(t.Type) == 1).OrderBy(t=> t.EndTime).ToList();
            var dk5 = list.Where(t => double.Parse(t.Value) == 5 && int.Parse(t.Type) == 1).OrderBy(t => t.EndTime).ToList();
            var zk7 = list.Where(t => double.Parse(t.Value) == 0.7 && int.Parse(t.Type) == 2).OrderBy(t => t.EndTime).ToList();
            var zk8 = list.Where(t => double.Parse(t.Value) == 0.8 && int.Parse(t.Type) == 2).OrderBy(t => t.EndTime).ToList();

            var result = new List<CouponDto>();

            #region rule
            if (pay <= 1)
            {
                result.AddRange(dk1);
                result.AddRange(dk5);
                result.AddRange(zk7);
                result.AddRange(zk8);
            }
            else if (pay <= 3.33)
            {
                result.AddRange(dk5);
                result.AddRange(dk1);
                result.AddRange(zk7);
                result.AddRange(zk8);
            }
            else if (pay <= 5)
            {
                result.AddRange(dk5);
                result.AddRange(zk7);
                result.AddRange(dk1);
                result.AddRange(zk8);
            }
            else if (pay <= 16.66)
            {
                result.AddRange(dk5);
                result.AddRange(zk7);
                result.AddRange(zk8);
                result.AddRange(dk1);
            }
            else if (pay <= 25)
            {
                result.AddRange(zk7);
                result.AddRange(dk5);
                result.AddRange(zk8);
                result.AddRange(dk1);
            }
            else
            {
                result.AddRange(zk7);
                result.AddRange(zk8);
                result.AddRange(dk5);
                result.AddRange(dk1);
            }

            #endregion

            list.Clear();
            list.AddRange(result);
        }
    }
}