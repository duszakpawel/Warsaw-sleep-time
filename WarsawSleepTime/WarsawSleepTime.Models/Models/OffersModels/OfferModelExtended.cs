using System;

namespace WarsawSleepTime.Models.Models.OffersModels
{
    public class OfferModelExtended
    {
        public bool IsAssigned { get; set; }
        public bool IsOwning { get; set; }
        public byte[] Image { get; set; }
        public int ownerId { get; set; }
        public int? clientId { get; set; }
        public string ownerName { get; set; }
        public string ClientName { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
        public string Steet { get; set; }
        public string Number { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
    }
}