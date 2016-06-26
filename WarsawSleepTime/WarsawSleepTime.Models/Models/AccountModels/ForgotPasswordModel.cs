using System.ComponentModel.DataAnnotations;

namespace WarsawSleepTime.Models.Models.AccountModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}