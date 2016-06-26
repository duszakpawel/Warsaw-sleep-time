using System.ComponentModel.DataAnnotations;

namespace WarsawSleepTime.Shared.Enums
{
    /// <summary>
    /// Additional features set for customers population.
    /// </summary>
    public enum AdditionalFeature
    {
        [Display(Name = "Likes parties")]
        LikeParty,
        [Display(Name = "Wants to meet new people")]
        WantMeetPeople,
        [Display(Name = "Nearly absent")]
        NearlyAbsent,
        [Display(Name = "Wants to improve language skills")]
        ImproveLanguage
    }
}