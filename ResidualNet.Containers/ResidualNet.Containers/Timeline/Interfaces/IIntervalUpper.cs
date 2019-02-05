using System;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the interface to a value interval that provides the upper contiguous interval boundary.
    /// </summary>
    public interface IIntervalUpper<T>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// The upper contiguous interval boundary associated with a time interval.
        /// </summary>
        T? Upper { get; }

        /// <summary>
        /// True if the upper contiguous interval boundary includes the boundary value into the interval.
        /// </summary>
        bool UpperInclude { get; }

        /// <summary>
        /// Checks if the contiguous interval value meets the time fragment upper boundary condition.
        /// </summary>
        /// <param name="value">The contiguous interval value to check against.</param>
        /// <returns>True if the upper limit meets the boundary conditions; otherwise, false.</returns>
        bool IsUpperLimit(T? value);
    }
}