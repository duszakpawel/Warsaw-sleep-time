using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WarsawSleepTime.Shared.Enums;

namespace WarsawSleepTime.Models.Models.AccountModels
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
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