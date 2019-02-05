using System;
using System.Runtime.Serialization;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the enumeration that defines interval boundary inclusion flags.
    /// </summary>
    [DataContract, Flags]
    public enum IntervalLimits
    {
        /// <summary>
        /// No boundaries are included into the associated interval.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// The lower boundary is included into the associated interval.
        /// </summary>
        Lower = 0x00000001,

        /// <summary>
        /// The upper boundary is included into the associated interval.
        /// </summary>
        Upper = 0x00000002
    }
}