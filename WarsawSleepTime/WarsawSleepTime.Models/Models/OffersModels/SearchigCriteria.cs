using System;

namespace WarsawSleepTime.Models.Models.OffersModels
{
    public class SearchingCriteria
    {
        public bool Dormitories { get; set; }
        public bool Hostels { get; set; }
        public bool Paid { get; set; }
        public bool Free { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public bool MatchPreferences { get; set; }
        public bool DateSpecified { get; set; }
        public DateTime Date { get; set; }
        public double southWestX { get; set; }
        public double southWestY { get; set; }
        public double northEastX { get; set; }
        public double northEastY { get; set; }
        public int zoom { get; set; }
    }
}
