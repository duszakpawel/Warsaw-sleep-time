using System;
using System.ComponentModel.DataAnnotations;
using WarsawSleepTime.Shared.Enums;

namespace WarsawSleepTime.Entities.Entities
{
    public class CouchsurfingOffer : Entity
    {
        [Required]
        public virtual Customer Owner { get; set; }
        public virtual Customer Client { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public Status Status { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string District { get; set; }
        public byte[] Image { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public string Apartment { get; set; }
    }
}