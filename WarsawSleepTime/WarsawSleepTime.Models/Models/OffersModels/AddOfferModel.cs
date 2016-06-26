using System;
using System.ComponentModel.DataAnnotations;

namespace WarsawSleepTime.Models.Models.OffersModels
{
    public class AddOfferModel
    {
        public byte[] Image { get; set; }
        public int Id { get; set; }
        public double longtitude { get; set; }
        public double latitude { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public int OwnerId { get; set; }
    }
}