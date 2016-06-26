using System;
using System.ComponentModel.DataAnnotations;
using WarsawSleepTime.Shared.Enums;

namespace WarsawSleepTime.Entities.Entities
{
    public class Customer : Entity
    {
        public byte[] Image { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int PhoneNumber { get; set; }
        public Address PlaceOfResidence { get; set; } = new Address();
        public string Email { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual UserPreference UserPreference { get; set; }
    }
}