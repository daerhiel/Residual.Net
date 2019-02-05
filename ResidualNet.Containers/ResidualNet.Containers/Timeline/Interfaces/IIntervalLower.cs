using System;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the interface to a value interval that provides the lower contiguous interval boundary.
    /// </summary>
    public interface IIntervalLower<T>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// The lower contiguous interval boundary associated with a time interval.
        /// </summary>
        T? Lower { get; }

        /// <summary>
        /// True if the lower contiguous interval boundary includes the boundary value into the interval.
        /// </summary>
        bool LowerInclude { get; }

        /// <summary>
        /// Checks if the contiguous interval value meets the time fragment lower boundary condition.
        /// </summary>
        /// <param name="value">The contiguous interval value to check against.</param>
        /// <returns>True if the lower limit meets the boundary conditions; otherwise, false.</returns>
        bool IsLowerLimit(T? value);
    }
}