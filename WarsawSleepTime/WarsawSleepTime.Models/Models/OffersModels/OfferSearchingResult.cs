using System.Collections.Generic;

namespace WarsawSleepTime.Models.Models.OffersModels
{
    public class OfferSearchingResult
    {
        public List<OfferModel> FreeOffers { get; set; } = new List<OfferModel>();
        public List<OfferModel> PaidOffers { get; set; } = new List<OfferModel>();
        public List<OfferModel> Dormitories { get; set; } = new List<OfferModel>();
        public List<OfferModel> Hostels { get; set; } = new List<OfferModel>();
    }
}