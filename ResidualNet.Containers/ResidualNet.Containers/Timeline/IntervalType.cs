using System.Runtime.Serialization;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the enumeration that defines the time interval flexibility modes.
    /// </summary>
    [DataContract]
    public enum IntervalType
    {
        /// <summary>
        /// Flexible intervals can change limits arbitrary and shift the limit on failure.
        /// </summary>
        [EnumMember]
        Free,

        /// <summary>
        /// Flexible intervals can change limits arbitrary and keep the limit unchanged on failure.
        /// </summary>
        [EnumMember]
        Flex,

        /// <summary>
        /// Strict intervals respect interval limits and allow shifting into inner area only.
        /// </summary>
        [EnumMember]
        Strict,

        /// <summary>
        /// Fixed intervals respect span size and try to move span if any of the limits change.
        /// </summary>
        [EnumMember]
        Fixed
    }
}