using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WarsawSleepTime.Entities.Entities;

namespace WarsawSleepTime.Entities.Context
{
    /// <summary>
    /// Database context.
    /// </summary>
    public class WarsawSleepTimeContext : IdentityDbContext<ApplicationUser>
    {
        public virtual IDbSet<AdditionalFeatureInfo> AdditionalFeatureInfos { get; set; }
        public virtual IDbSet<CouchsurfingOffer> CouchsurfingOffers { get; set; }
        public virtual IDbSet<Friendship> Friendships { get; set; }
        public virtual IDbSet<LanguageInfo> LanguageInfos { get; set; }
        public virtual IDbSet<HistoricalOffer> HistoricalOffers { get; set; }
        public virtual IDbSet<Customer> Customers { get; set; }
        public virtual IDbSet<UserPreference> UserPreferences { get; set; }
        public WarsawSleepTimeContext() : base("WarsawSleepTimeContext", false){ } //DefaultConnection
        public static WarsawSleepTimeContext Create()
        {
            return new WarsawSleepTimeContext();
        }
    }
}