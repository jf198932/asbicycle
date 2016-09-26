namespace ASBicycle.Rental.Track.Dto
{
    public class TrackInput
    {
        public TrackInput()
        {
            Pagesize = 20;
            Index = 1;
        }

        public int User_id { get; set; }
        //public string Token { get; set; }
        public int Index { get; set; }
        public int Pagesize { get; set; }
    }
}