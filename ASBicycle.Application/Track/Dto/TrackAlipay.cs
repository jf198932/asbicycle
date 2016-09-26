namespace ASBicycle.Track.Dto
{
    public class TrackAlipay
    {
        public string notify_id { get; set; }
        public string sign { get; set; }
        public string out_trade_no { get; set; }
        public string trade_no { get; set; }
        public string trade_status { get; set; }
        public double total_fee { get; set; }
        public int pay_status { get; set; }
    }
}