﻿namespace ASBicycle.Web.Models.Alipay
{
    public class Notifyrul
    {
        public string notify_id { get; set; }
        public string sign { get; set; }
        public string out_trade_no { get; set; }
        public string trade_no { get; set; }
        public string trade_status { get; set; }
    }
}