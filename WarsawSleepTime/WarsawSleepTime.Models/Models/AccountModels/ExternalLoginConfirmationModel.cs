using System.ComponentModel.DataAnnotations;

namespace WarsawSleepTime.Models.Models.AccountModels
{
    public class ExternalLoginConfirmationModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}