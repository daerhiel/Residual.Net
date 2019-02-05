using System.Runtime.Serialization;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the set of statistical intervals for time series analytics.
    /// </summary>
    [DataContract]
    public enum SeriesInterval
    {
        /// <summary>
        /// OHLC interval for 1 minute.
        /// </summary>
        [EnumMember]
        Series1Min = 1,

        /// <summary>
        /// OHLC interval for 5 minute.
        /// </summary>
        [EnumMember]
        Series5Min = 5,

        /// <summary>
        /// OHLC interval for 15 minute.
        /// </summary>
        [EnumMember]
        Series15Min = 15,

        /// <summary>
        /// OHLC interval for 30 minute.
        /// </summary>
        [EnumMember]
        Series30Min = 30
    }
}