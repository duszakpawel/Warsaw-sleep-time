using System.ComponentModel;

namespace WarsawSleepTime.Shared.Enums
{
    /// <summary>
    /// Mark feature for couchsurfing offers.
    /// </summary>
    public enum Mark
    {
        [Description("Bad")]
        Bad,
        [Description("Not bad")]
        NotBad,
        [Description("Average")]
        Average,
        [Description("Pretty good")]
        PrettyGood,
        [Description("Awesome")]
        Awesome
    }
}