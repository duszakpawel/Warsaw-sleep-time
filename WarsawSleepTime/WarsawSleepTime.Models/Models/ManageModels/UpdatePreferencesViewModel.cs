using System.Collections.Generic;
using WarsawSleepTime.Shared.Enums;

namespace WarsawSleepTime.Models.Models.ManageModels
{
    public class LanguageItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FeatureItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdatePreferencesViewModel
    {
        public IEnumerable<int> ChosenLanguages { get; set; }

        public IEnumerable<int> ChosenFeatures { get; set; }

        #region helpers
        public IEnumerable<LanguageItem> AvailableLanguages => new[]
        {
            new LanguageItem { Id = 1, Name = EnumHelper<Language>.GetDisplayValue(Language.Polish) },
            new LanguageItem { Id = 2, Name = EnumHelper<Language>.GetDisplayValue(Language.Chinese)},
            new LanguageItem { Id = 3, Name = EnumHelper<Language>.GetDisplayValue(Language.English) },
            new LanguageItem { Id = 4, Name = EnumHelper<Language>.GetDisplayValue(Language.German) },
            new LanguageItem { Id = 5, Name = EnumHelper<Language>.GetDisplayValue(Language.Russian) },

        };

        public IEnumerable<FeatureItem> AvailableFeatures => new[]
        {
            new FeatureItem { Id = 1, Name = EnumHelper<AdditionalFeature>.GetDisplayValue(AdditionalFeature.ImproveLanguage) },
            new FeatureItem { Id = 2, Name = EnumHelper<AdditionalFeature>.GetDisplayValue(AdditionalFeature.LikeParty) },
            new FeatureItem { Id = 3, Name = EnumHelper<AdditionalFeature>.GetDisplayValue(AdditionalFeature.NearlyAbsent) },
            new FeatureItem { Id = 4, Name = EnumHelper<AdditionalFeature>.GetDisplayValue(AdditionalFeature.WantMeetPeople) },
        };
        #endregion
    }
}