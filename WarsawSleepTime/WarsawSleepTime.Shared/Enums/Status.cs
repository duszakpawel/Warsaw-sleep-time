using System.ComponentModel;

namespace WarsawSleepTime.Shared.Enums
{
    /// <summary>
    /// Status feature for couchsurfing offers.
    /// </summary>
    public enum Status
    {
        [Description("Ready")]
        Ready,
        [Description("Accepted")]
        Accepted,
        [Description("Done")]
        Done
    }
}