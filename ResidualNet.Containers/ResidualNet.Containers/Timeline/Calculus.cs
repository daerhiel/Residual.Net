using System;
using System.Collections.Generic;
using System.Linq;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Provides constants and static methods for typed trigonometric, logarithmic, and other common mathematical functions.
    /// </summary>
    public static partial class Calculus<T>
        where T : struct, IComparable<T>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        private static Comparer<IIntervalLower<T>> LowerComparer { get; } = Comparer<IIntervalLower<T>>.Create(CompareLower);
        private static Comparer<IIntervalUpper<T>> UpperComparer { get; } = Comparer<IIntervalUpper<T>>.Create(CompareUpper);

        /// <summary>
        /// Compares two interfaces to the lower contiguous interval boundaries.
        /// </summary>
        /// <param name="x">The 1-st lower contiguous interval boundary to compare.</param>
        /// <param name="y">The 2-nd lower contiguous interval boundary to compare.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less
        /// than zero This instance precedes other in the sort order. Zero This instance occurs in the same position in the sort order as other.
        /// Greater than zero This instance follows other in the sort order.</returns>
        public static int CompareLower(IIntervalLower<T> x, IIntervalLower<T> y)
        {
            if (!(x is null))
                if (!(y is null))
                    switch (Comparer<T?>.Default.Compare(x.Lower, y.Lower))
                    {
                        case 0: return Comparer<bool>.Default.Compare(x.LowerInclude, y.LowerInclude);
                        case int result when x.Lower == null: return -1;
                        case int result when y.Lower == null: return 1;
                        case int result: return result;
                    }
                else
                    throw new ArgumentNullException(nameof(y));
            else
                throw new ArgumentNullException(nameof(x));
        }

        /// <summary>
        /// Compares two interfaces to the upper contiguous interval boundaries.
        /// </summary>
        /// <param name="x">The 1-st upper contiguous interval boundary to compare.</param>
        /// <param name="y">The 2-nd upper contiguous interval boundary to compare.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less
        /// than zero This instance precedes other in the sort order. Zero This instance occurs in the same position in the sort order as other.
        /// Greater than zero This instance follows other in the sort order.</returns>
        public static int CompareUpper(IIntervalUpper<T> x, IIntervalUpper<T> y)
        {
            if (!(x is null))
                if (!(y is null))
                    switch (Comparer<T?>.Default.Compare(x.Upper, y.Upper))
                    {
                        case 0: return Comparer<bool>.Default.Compare(x.UpperInclude, y.UpperInclude);
                        case int result when x.Upper == null: return 1;
                        case int result when y.Upper == null: return -1;
                        case int result: return result;
                    }
                else
                    throw new ArgumentNullException(nameof(y));
            else
                throw new ArgumentNullException(nameof(x));
        }

        /// <summary>
        /// Compares the interfaces to a lower and an upper contiguous interval boundaries.
        /// </summary>
        /// <param name="x">The 1-st lower contiguous interval boundary to compare.</param>
        /// <param name="y">The 2-nd upper contiguous interval boundary to compare.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less
        /// than zero This instance precedes other in the sort order. Zero This instance occurs in the same position in the sort order as other.
        /// Greater than zero This instance follows other in the sort order.</returns>
        public static int CompareLowerUpper(IIntervalLower<T> x, IIntervalUpper<T> y)
        {
            if (!(x is null))
                if (!(y is null))
                    switch (Comparer<T?>.Default.Compare(x.Lower, y.Upper))
                    {
                        case 0 when x.Lower != null || y.Upper != null:
                            return x.LowerInclude != y.UpperInclude ? 1 : 0;
                        case int result when x.Lower == null || y.Upper == null:
                            return -1;
                        case int result:
                            return result;
                    }
                else
                    throw new ArgumentNullException(nameof(y));
            else
                throw new ArgumentNullException(nameof(x));
        }

        /// <summary>
        /// Compares the interfaces to an upper and a lower contiguous interval boundaries.
        /// </summary>
        /// <param name="x">The 1-st upper contiguous interval boundary to compare.</param>
        /// <param name="y">The 2-nd lower contiguous interval boundary to compare.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less
        /// than zero This instance precedes other in the sort order. Zero This instance occurs in the same position in the sort order as other.
        /// Greater than zero This instance follows other in the sort order.</returns>
        public static int CompareUpperLower(IIntervalUpper<T> x, IIntervalLower<T> y)
        {
            if (!(x is null))
                if (!(y is null))
                    switch (Comparer<T?>.Default.Compare(x.Upper, y.Lower))
                    {
                        case 0 when x.Upper != null || y.Lower != null:
                            return x.UpperInclude != y.LowerInclude ? -1 : 0;
                        case int result when x.Upper == null || y.Lower == null:
                            return 1;
                        case int result:
                            return result;
                    }
                else
                    throw new ArgumentNullException(nameof(y));
            else
                throw new ArgumentNullException(nameof(x));
        }

        /// <summary>
        /// Finds an interface to a lower contiguous interval boundary that has the minimum value.
        /// </summary>
        /// <param name="values">The array of interfaces to lower contiguous interval boundary to look at.</param>
        /// <returns>The interface to lower contiguous interval boundary that has the minimum value.</returns>
        public static IIntervalLower<T> MinLower(params IIntervalLower<T>[] values)
        {
            return values.Aggregate((x, y) => LowerComparer.Compare(x, y) < 0 ? x : y);
        }

        /// <summary>
        /// Finds an interface to a lower contiguous interval boundary that has the maximum value.
        /// </summary>
        /// <param name="values">The array of interfaces to lower contiguous interval boundary to look at.</param>
        /// <returns>The interface to lower contiguous interval boundary that has the maximum value.</returns>
        public static IIntervalLower<T> MaxLower(params IIntervalLower<T>[] values)
        {
            return values.Aggregate((x, y) => LowerComparer.Compare(x, y) > 0 ? x : y);
        }

        /// <summary>
        /// Finds an interface to a upper contiguous interval boundary that has the minimum value.
        /// </summary>
        /// <param name="values">The array of interfaces to upper contiguous interval boundary to look at.</param>
        /// <returns>The interface to upper contiguous interval boundary that has the minimum value.</returns>
        public static IIntervalUpper<T> MinUpper(params IIntervalUpper<T>[] values)
        {
            return values.Aggregate((x, y) => UpperComparer.Compare(x, y) < 0 ? x : y);
        }

        /// <summary>
        /// Finds an interface to a upper contiguous interval boundary that has the maximum value.
        /// </summary>
        /// <param name="values">The array of interfaces to upper contiguous interval boundary to look at.</param>
        /// <returns>The interface to upper contiguous interval boundary that has the maximum value.</returns>
        public static IIntervalUpper<T> MaxUpper(params IIntervalUpper<T>[] values)
        {
            return values.Aggregate((x, y) => UpperComparer.Compare(x, y) > 0 ? x : y);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}