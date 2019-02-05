using System;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the interface to an object that has the date and time reference point.
    /// </summary>
    public interface ITimeReference
    {
        /// <summary>
        /// The date and time reference point associated with an object.
        /// </summary>
        DateTime Sample { get; }
    }
}