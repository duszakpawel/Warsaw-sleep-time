using System;

namespace WarsawSleepTime.Entities.Entities
{
        public class HistoricalOffer : Entity
        {
            public string Owner { get; set; }
            public string Client { get; set; }
            public DateTime? Date { get; set; }
        }
}