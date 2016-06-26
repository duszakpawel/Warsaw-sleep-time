using System.Collections.Generic;

namespace WarsawSleepTime.Models.Models.OffersModels
{
    public class UserOffers
    {
        public List<AddOfferModel> UsersOffers { get; set; } = new List<AddOfferModel>();
        public List<AddOfferModel> AssignedOffers { get; set; } = new List<AddOfferModel>();
    }
}