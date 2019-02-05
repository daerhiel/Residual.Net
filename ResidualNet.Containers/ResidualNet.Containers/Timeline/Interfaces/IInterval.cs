using System;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the interface to a value interval that has the lower and upper contiguous interval boundaries.
    /// </summary>
    public interface IInterval<T> : IIntervalLower<T>, IIntervalUpper<T>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// Checks if the contiguous interval value lies within the contiguous interval.
        /// </summary>
        /// <param name="value">The contiguous interval value to check against.</param>
        /// <returns>True if contiguous value is within the contiguous interval; otherwise, false.</returns>
        bool IsWithin(T value);

        /// <summary>
        /// Checks if the contiguous interval value lies within the contiguous interval considering that null is a -infinity.
        /// </summary>
        /// <param name="value">The contiguous interval value to check against.</param>
        /// <returns>True if contiguous value is within the contiguous interval; otherwise, false.</returns>
        bool IsWithinLower(T? value);

        /// <summary>
        /// Checks if the contiguous interval value lies within the contiguous interval considering that null is a +infinity.
        /// </summary>
        /// <param name="value">The contiguous interval value to check against.</param>
        /// <returns>True if contiguous value is within the contiguous interval; otherwise, false.</returns>
        bool IsWithinUpper(T? value);
    }
}