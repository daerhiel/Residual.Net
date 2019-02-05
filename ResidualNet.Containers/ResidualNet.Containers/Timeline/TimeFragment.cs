using System.Runtime.Serialization;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the task scheduler time fragment.
    /// </summary>
    [DataContract]
    public enum TimeFragment
    {
        /// <summary>
        /// Day time interval.
        /// </summary>
        [EnumMember]
        Day,

        /// <summary>
        /// Hour time interval.
        /// </summary>
        [EnumMember]
        Hour,

        /// <summary>
        /// Minute time interval.
        /// </summary>
        [EnumMember]
        Minute,

        /// <summary>
        /// Second time interval.
        /// </summary>
        [EnumMember]
        Second
    }
}