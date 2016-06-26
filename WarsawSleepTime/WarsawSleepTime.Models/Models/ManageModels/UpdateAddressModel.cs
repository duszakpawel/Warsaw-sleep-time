using System.ComponentModel.DataAnnotations;

namespace WarsawSleepTime.Models.Models.ManageModels
{
    public class UpdateAddressModel
    {
        [Required]
        public string Street { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
    }
}