using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WarsawSleepTime.Shared.Enums;

namespace WarsawSleepTime.Models.Models.ManageModels
{
    public class UpdateUserModel
    {
        public byte[] Image { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "First name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "Last name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.DateTime)]
        public DateTime? DateTime { get; set; }
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; set; }
    }
}