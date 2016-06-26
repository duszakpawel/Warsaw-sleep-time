using System.Collections.Generic;

namespace WarsawSleepTime.Entities.Entities
{
    public class UserPreference : Entity
    {
        public virtual ICollection<LanguageInfo> Languages { get; set; } = new HashSet<LanguageInfo>();
        public virtual ICollection<AdditionalFeatureInfo> AdditionalFeatures { get; set; } = new HashSet<AdditionalFeatureInfo>();
    }
}