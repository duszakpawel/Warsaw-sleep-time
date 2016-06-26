using System.ComponentModel.DataAnnotations;

namespace WarsawSleepTime.Shared.Enums
{
    /// <summary>
    /// Language feature for customers population.
    /// </summary>
    public enum Language
    {
        [Display(Name = "English")]
        English,
        [Display(Name = "Polish")]
        Polish,
        [Display(Name = "Russian")]
        Russian,
        [Display(Name = "German")]
        German,
        [Display(Name = "Chinese")]
        Chinese
    }
}