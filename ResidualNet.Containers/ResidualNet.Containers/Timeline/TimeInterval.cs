using System;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the time interval limited by the lower and upper date and time boundaries.
    /// </summary>
    public class TimeInterval : IInterval<DateTime>, IComparable<IInterval<DateTime>>
    {
        private DateTime? _lower;
        private DateTime? _upper;
        private TimeSpan? _span;
        private IntervalLimits _limits;

        /// <summary>
        /// Determine the span dynamic type of a time range object.
        /// </summary>
        public IntervalType Type { get; set; }

        /// <summary>
        /// The lower date and time boundary associated with a time interval.
        /// </summary>
        public DateTime? Lower
        {
            get { return _lower; }
            set
            {
                if (_lower != value)
                    if (Type != IntervalType.Fixed)
                    {
                        if (Type == IntervalType.Strict && (value < _lower || value == null))
                            value = _lower;
                        if (value > _upper)
                            if (Type == IntervalType.Free)
                                _upper = value;
                            else
                                value = _upper;
                        _span = _upper - (_lower = value);
                    }
                    else
                        _upper = (_lower = value) != null ? value + _span : value;
            }
        }

        /// <summary>
        /// The upper date and time boundary associated with a time interval.
        /// </summary>
        public DateTime? Upper
        {
            get { return _upper; }
            set
            {
                if (_upper != value)
                    if (Type != IntervalType.Fixed)
                    {
                        if (value < _lower)
                            if (Type == IntervalType.Free)
                                _lower = value;
                            else
                                value = _lower;
                        if (Type == IntervalType.Strict && (value > _upper || value == null))
                            value = _upper;
                        _span = (_upper = value) - _lower;
                    }
                    else
                        _lower = (_upper = value) != null ? value - _span : value;
            }
        }

        /// <summary>
        /// The time span to use as a length of a new time interval. It affects the limits and treats the lower limit as a base point.
        /// </summary>
        public TimeSpan? Span
        {
            get { return _span; }
            set
            {
                if (_span != value)
                {
                    if (value != null)
                    {
                        if (value < TimeSpan.Zero)
                            value = TimeSpan.Zero;
                        if (_lower != null)
                            _upper = _lower + value;
                        else if (_upper != null)
                            _lower = _upper - value;
                    }
                    else
                        _upper = null;
                    _span = value;
                }
            }
        }

        /// <summary>
        /// True if the lower date and time boundary includes the boundary value into the interval.
        /// </summary>
        public bool LowerInclude => (_limits & IntervalLimits.Lower) != IntervalLimits.None;

        /// <summary>
        /// True if the upper date and time boundary includes the boundary value into the interval.
        /// </summary>
        public bool UpperInclude => (_limits & IntervalLimits.Upper) != IntervalLimits.None;

        /// <summary>
        /// True if the time span contains zero number of ticks and it doesn't contain its boundaries.
        /// </summary>
        public bool IsEmpty => _span == TimeSpan.Zero && (_limits & (IntervalLimits.Lower | IntervalLimits.Upper)) != (IntervalLimits.Lower | IntervalLimits.Upper);

        /// <summary>
        /// Sums all time spans of time intervals into a single value.
        /// </summary>
        /// <returns>Calculated TimeSpan value.</returns>
        public static TimeSpan? Combine(params TimeInterval[] intervals)
        {
            TimeSpan? span = TimeSpan.Zero;
            foreach (var interval in intervals ?? Array.Empty<TimeInterval>())
                if (interval != null)
                    span += interval.Span;
            return span;
        }

        /// <summary>
        /// Initializes new instance of a time interval.
        /// </summary>
        /// <param name="type">The time interval flexibility mode to set to the time interval.</param>
        public TimeInterval(IntervalType type = IntervalType.Flex)
        {
            _limits = IntervalLimits.Lower | IntervalLimits.Upper;
            Type = type;
        }

        /// <summary>
        /// Initializes new instance of a time interval from a lower date and time reference value.
        /// </summary>
        /// <param name="time">The date and time of a lower boundary to set the interval with.</param>
        /// <param name="type">The time interval flexibility mode to set to the time interval.</param>
        public TimeInterval(DateTime? time, IntervalType type = IntervalType.Flex)
        {
            _limits = IntervalLimits.Lower | IntervalLimits.Upper;
            _lower = time;
            Type = type;
        }

        /// <summary>
        /// Initializes new instance of a time interval from a time span value.
        /// </summary>
        /// <param name="span">The time span to use as a length of a new time interval.</param>
        /// <param name="type">The time interval flexibility mode to set to the time interval.</param>
        public TimeInterval(TimeSpan? span, IntervalType type = IntervalType.Flex)
        {
            _limits = IntervalLimits.Lower | IntervalLimits.Upper;
            if (span < TimeSpan.Zero)
                span = TimeSpan.Zero;
            _span = span;
            Type = type;
        }

        /// <summary>
        /// Initializes new instance of a time interval from a time span value.
        /// </summary>
        /// <param name="span">The time span to use as a length of a new time interval.</param>
        /// <param name="limits">The interval boundary inclusion flags that defines the boundary mode.</param>
        /// <param name="type">The time interval flexibility mode to set to the time interval.</param>
        public TimeInterval(TimeSpan? span, IntervalLimits limits, IntervalType type = IntervalType.Flex)
        {
            _limits = limits;
            if (span < TimeSpan.Zero)
                span = TimeSpan.Zero;
            _span = span;
            Type = type;
        }

        /// <summary>
        /// Initializes new instance of a time interval from a lower date and time reference value and a time span values.
        /// </summary>
        /// <param name="time">The date and time of a lower boundary to set the interval with.</param>
        /// <param name="span">The time span to use as a length of a new time interval.</param>
        /// <param name="type">The time interval flexibility mode to set to the time interval.</param>
        public TimeInterval(DateTime? time, TimeSpan? span, IntervalType type = IntervalType.Flex)
        {
            _limits = IntervalLimits.Lower | IntervalLimits.Upper;
            _lower = time;
            if (span < TimeSpan.Zero)
                span = TimeSpan.Zero;
            _upper = time + span;
            _span = span;
            Type = type;
        }

        /// <summary>
        /// Initializes new instance of a time interval from an upper and a lower date and time reference values.
        /// </summary>
        /// <param name="lower">The lower date and time interval boundary to set the interval with.</param>
        /// <param name="upper">The upper date and time interval boundary to set the interval with.</param>
        /// <param name="type">The time interval flexibility mode to set to the time interval.</param>
        public TimeInterval(DateTime? lower, DateTime? upper, IntervalType type = IntervalType.Flex)
        {
            _limits = IntervalLimits.Lower | IntervalLimits.Upper;
            _lower = lower;
            if (lower == null || upper == null || upper > lower)
                _upper = upper;
            else
                _upper = lower;
            _span = _upper - _lower;
            Type = type;
        }

        /// <summary>
        /// Initializes new instance of a time interval from an upper and a lower date and time reference values.
        /// </summary>
        /// <param name="lower">The lower date and time interval boundary to set the interval with.</param>
        /// <param name="upper">The upper date and time interval boundary to set the interval with.</param>
        /// <param name="limits">The interval boundary inclusion flags that defines the boundary mode.</param>
        /// <param name="type">The time interval flexibility mode to set to the time interval.</param>
        public TimeInterval(DateTime? lower, DateTime? upper, IntervalLimits limits, IntervalType type = IntervalType.Flex)
        {
            _limits = limits;
            _lower = lower;
            if (lower == null || upper == null || upper > lower)
                _upper = upper;
            else
                _upper = lower;
            _span = _upper - _lower;
            Type = type;
        }

        /// <summary>
        /// Initializes new instance of a time interval from a time interval source and target reference values.
        /// </summary>
        /// <param name="limits">The interval boundary inclusion flags that defines the boundary mode.</param>
        /// <param name="source">The source time interval to get the interval parameters from.</param>
        /// <param name="target">The target time interval to adjust the source interval with.</param>
        public TimeInterval(TimeInterval source, IInterval<DateTime> target, IntervalLimits limits)
        {
            if (source != null)
                if (target != null)
                {
                    _limits = source._limits;
                    if ((limits & IntervalLimits.Lower) != IntervalLimits.None)
                    {
                        if (target.UpperInclude)
                            _limits &= ~IntervalLimits.Lower;
                        _lower = target.Upper;
                    }
                    else
                        _lower = source._lower;
                    if ((limits & IntervalLimits.Upper) != IntervalLimits.None)
                    {
                        if (target.LowerInclude)
                            _limits &= ~IntervalLimits.Upper;
                        _upper = target.Lower;
                    }
                    else
                        _upper = source._upper;
                    _span = _upper - _lower;
                    Type = source.Type;
                }
                else
                    throw new ArgumentNullException(nameof(target));
            else
                throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// Initializes new instance of a time interval from the interfaces an upper and a lower interval reference values.
        /// </summary>
        /// <param name="lower">The interface to the lower interval boundary to set the interval with.</param>
        /// <param name="upper">The interface to the upper interval boundary to set the interval with.</param>
        /// <param name="type">The time interval flexibility mode to set to the time interval.</param>
        public TimeInterval(IIntervalLower<DateTime> lower, IIntervalUpper<DateTime> upper, IntervalType type = IntervalType.Flex)
        {
            if (lower != null)
            {
                if (lower.LowerInclude)
                    _limits |= IntervalLimits.Lower;
                _lower = lower.Lower;
            }
            if (upper != null)
            {
                if (upper.UpperInclude)
                    _limits |= IntervalLimits.Upper;
                _upper = upper.Upper;
            }
            _span = _upper - _lower;
            Type = type;
        }

        /// <summary>
        /// Initializes new instance of a time interval from the interfaces an upper and a lower interval reference values.
        /// </summary>
        /// <param name="upper">The interface to the upper interval boundary to set the interval with.</param>
        /// <param name="lower">The interface to the lower interval boundary to set the interval with.</param>
        /// <param name="type">The time interval flexibility mode to set to the time interval.</param>
        public TimeInterval(IIntervalUpper<DateTime> upper, IIntervalLower<DateTime> lower, IntervalType type = IntervalType.Flex)
        {
            _limits = IntervalLimits.Lower | IntervalLimits.Upper;
            if (upper != null)
            {
                if (upper.UpperInclude)
                    _limits &= ~IntervalLimits.Lower;
                _lower = upper.Upper;
            }
            if (lower != null)
            {
                if (upper == null)
                    _lower = lower.Lower;
                if (lower.LowerInclude)
                    _limits &= ~IntervalLimits.Upper;
                _upper = lower.Lower;
            }
            _span = _upper - _lower;
            Type = type;
        }

        /// <summary>
        /// Initializes new instance of a time interval from an existing time interval.
        /// </summary>
        /// <param name="source">The time interval to get the interval parameters from.</param>
        public TimeInterval(TimeInterval source)
        {
            if (source != null)
            {
                _limits = source._limits;
                _lower = source._lower;
                _upper = source._upper;
                _span = _upper - _lower;
                Type = source.Type;
            }
            else
                throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// Initializes new instance of a time interval from an existing time interval.
        /// </summary>
        /// <param name="interval">The time interval to get the interval parameters from.</param>
        /// <param name="type">The time interval flexibility mode to set to the time interval.</param>
        public TimeInterval(IInterval<DateTime> interval, IntervalType type = IntervalType.Flex)
        {
            if (interval != null)
            {
                if (interval.LowerInclude)
                    _limits |= IntervalLimits.Lower;
                if (interval.UpperInclude)
                    _limits |= IntervalLimits.Upper;
                _lower = interval.Lower;
                _upper = interval.Upper;
                _span = _upper - _lower;
            }
            Type = type;
        }

        /// <summary>
        /// Checks if the date and time value meets the current time interval lower boundary condition.
        /// </summary>
        /// <param name="value">The date and time value to check against.</param>
        /// <returns>True if the lower limit meets the boundary conditions; otherwise, false.</returns>
        public bool IsLowerLimit(DateTime? value) => _lower == null || (LowerInclude ? _lower <= value : _lower < value);

        /// <summary>
        /// Checks if the date and time value meets the time current interval upper boundary condition.
        /// </summary>
        /// <param name="value">The date and time value to check against.</param>
        /// <returns>True if the upper limit meets the boundary conditions; otherwise, false.</returns>
        public bool IsUpperLimit(DateTime? value) => _upper == null || (UpperInclude ? _upper >= value : _upper > value);

        /// <summary>
        /// Checks if the date and time value is within the time interval range including boundaries.
        /// </summary>
        /// <param name="value">The date and time value to check against.</param>
        /// <returns>True if date and time is within the time iterval; otherwise, false.</returns>
        public bool IsWithin(DateTime value)
        {
            return IsLowerLimit(value) && IsUpperLimit(value);
        }

        /// <summary>
        /// Checks if the date and time value is within the current time interval range including boundaries considering that null is a -infinity.
        /// </summary>
        /// <param name="value">The date and time value to check against.</param>
        /// <returns>True if date and time is within the time iterval; otherwise, false.</returns>
        public bool IsWithinLower(DateTime? value)
        {
            return LowerInclude && value == _lower || IsLowerLimit(value) && IsUpperLimit(value);
        }

        /// <summary>
        /// Checks if the date and time value is within the current time interval range including boundaries considering that null is a +infinity.
        /// </summary>
        /// <param name="value">The date and time value to check against.</param>
        /// <returns>True if date and time is within the time iterval; otherwise, false.</returns>
        public bool IsWithinUpper(DateTime? value)
        {
            return UpperInclude && value == _upper || IsLowerLimit(value) && IsUpperLimit(value);
        }

        /// <summary>
        /// Checks if the current time interval overlaps with specified time interval.
        /// </summary>
        /// <param name="interval">The time interval to check overlapping with.</param>
        /// <returns>True if two intervals are overlapping; otherwise, false.</returns>
        public bool Overlaps(IInterval<DateTime> interval)
        {
            return Calculus<DateTime>.CompareLowerUpper(Calculus<DateTime>.MaxLower(this, interval), Calculus<DateTime>.MinUpper(this, interval)) <= 0;
        }

        /// <summary>
        /// Intersects the specified time interval with the current time interval.
        /// </summary>
        /// <param name="interval">The time interval to intersect the current interval with.</param>
        /// <returns>Time interval repsesenting the result of intersection.</returns>
        public TimeInterval Intersect(IInterval<DateTime> interval)
        {
            switch ((Calculus<DateTime>.MaxLower(this, interval), Calculus<DateTime>.MinUpper(this, interval)))
            {
                case var result when Calculus<DateTime>.CompareLowerUpper(result.Item1, result.Item2) > 0:
                    return new TimeInterval(result.Item2.Upper, TimeSpan.Zero, Type);
                case var result:
                    return new TimeInterval(result.Item1, result.Item2, Type);
            }
        }

        /// <summary>
        /// Unions the specified time interval with the current time interval.
        /// </summary>
        /// <param name="interval">The time interval to union the current interval with.</param>
        /// <returns>The combination of time intervals representing the result of a union.</returns>
        public TimeInterval[] Union(IInterval<DateTime> interval)
        {
            switch ((Calculus<DateTime>.MaxLower(this, interval), Calculus<DateTime>.MinUpper(this, interval)))
            {
                case var result when Calculus<DateTime>.CompareLowerUpper(result.Item1, result.Item2) > 0:
                    return new[] { new TimeInterval(this), new TimeInterval(interval) };
                default:
                    return new[] { new TimeInterval(Calculus<DateTime>.MinLower(this, interval), Calculus<DateTime>.MaxUpper(this, interval), Type) };
            }
        }

        /// <summary>
        /// Unions the specified time interval with the current time interval and applies the result to it.
        /// </summary>
        /// <param name="interval">The time interval to union the current interval with.</param>
        /// <returns>The second time interval representing the result of union if exists; otherwise, null.</returns>
        public TimeInterval UnionAt(IInterval<DateTime> interval)
        {
            switch ((Calculus<DateTime>.MaxLower(this, interval), Calculus<DateTime>.MinUpper(this, interval)))
            {
                case var result when Calculus<DateTime>.CompareLowerUpper(result.Item1, result.Item2) > 0:
                    return new TimeInterval(interval);
                default:
                    if (Calculus<DateTime>.MinLower(this, interval) is IIntervalLower<DateTime> lower)
                    {
                        if (lower.LowerInclude)
                            _limits |= IntervalLimits.Lower;
                        _lower = lower.Lower;
                    }
                    if (Calculus<DateTime>.MaxUpper(this, interval) is IIntervalUpper<DateTime> upper)
                    {
                        if (upper.UpperInclude)
                            _limits |= IntervalLimits.Upper;
                        _upper = upper.Upper;
                    }
                    _span = _upper - _lower;
                    return null;
            }
        }

        /// <summary>
        /// Subtracts the specified time interval from the current time interval.
        /// </summary>
        /// <param name="interval">The time interval to subtract from the current time interval.</param>
        /// <returns>The combination of time intervals representing the result of a subtraction.</returns>
        public TimeInterval[] Subtract(IInterval<DateTime> interval)
        {
            switch ((CompareToLower(interval), CompareToUpper(interval)))
            {
                case var result when result.Item1 == 0 && result.Item2 == 0:
                    return new[] { new TimeInterval(this, interval, IntervalLimits.Upper), new TimeInterval(this, interval, IntervalLimits.Lower) };
                case var result when result.Item1 >= 1 && result.Item2 <= -1:
                    return new[] { new TimeInterval(TimeSpan.Zero, IntervalLimits.None, Type) };
                case var result when result.Item1 >= -1 && result.Item2 < 0:
                    return new[] { new TimeInterval(this, interval, IntervalLimits.Upper) };
                case var result when result.Item1 > 0 && result.Item2 <= 1:
                    return new[] { new TimeInterval(this, interval, IntervalLimits.Lower) };
                default:
                    return new[] { new TimeInterval(this) };
            }
        }

        /// <summary>
        /// Subtracts the specified time interval from the current time interval and applies the result to it.
        /// </summary>
        /// <param name="interval">The time interval to subtract from the current time interval.</param>
        /// <returns>The second time interval representing the result of subtraction if exists; otherwise, null.</returns>
        public TimeInterval SubtractAt(IInterval<DateTime> interval)
        {
            switch ((CompareToLower(interval), CompareToUpper(interval)))
            {
                case var result when result.Item1 == 0 && result.Item2 == 0:
                    var adjacent = new TimeInterval(this, interval, IntervalLimits.Lower);
                    if (interval.LowerInclude)
                        _limits &= ~IntervalLimits.Upper;
                    Upper = interval.Lower;
                    return adjacent;
                case var result when result.Item1 >= 1 && result.Item2 <= -1:
                    _limits = IntervalLimits.None;
                    Lower = null;
                    Upper = null;
                    Span = TimeSpan.Zero;
                    return null;
                case var result when result.Item1 >= -1 && result.Item2 < 0:
                    if (interval.LowerInclude)
                        _limits &= ~IntervalLimits.Upper;
                    Upper = interval.Lower;
                    return null;
                case var result when result.Item1 > 0 && result.Item2 <= 1:
                    if (interval.UpperInclude)
                        _limits &= ~IntervalLimits.Lower;
                    Lower = interval.Upper;
                    return null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Compares the current instance of time interval with an interface to another time interval and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="interval">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less
        /// than zero This instance precedes other in the sort order. Zero This instance occurs in the same position in the sort order as other.
        /// Greater than zero This instance follows other in the sort order.</returns>
        public int CompareTo(IInterval<DateTime> interval)
        {
            switch (interval)
            {
                case IInterval<DateTime> value when Calculus<DateTime>.CompareUpperLower(this, value) < 0:
                    return -1;
                case IInterval<DateTime> value when Calculus<DateTime>.CompareLowerUpper(this, value) > 0:
                    return 1;
                case IInterval<DateTime> _:
                    return 0;
                default:
                    throw new ArgumentNullException(nameof(interval));
            }
        }

        /// <summary>
        /// Compares the current instance of time interval with an interface to a lower contiguous interval boundary.
        /// </summary>
        /// <param name="boundary">The lower contiguous interval boundary to compare to.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less
        /// than zero This instance precedes other in the sort order. Zero This instance occurs in the same position in the sort order as other.
        /// Greater than zero This instance follows other in the sort order.</returns>
        public int CompareToLower(IIntervalLower<DateTime> boundary)
        {
            switch ((Calculus<DateTime>.CompareLower(this, boundary), Calculus<DateTime>.CompareUpperLower(this, boundary)))
            {
                case var result when result.Item1 < 0 && result.Item2 < 0:
                    return -2;
                case var result when result.Item1 < 0 && result.Item2 == 0:
                    return -1;
                case var result when result.Item1 > 0 && result.Item2 > 0:
                    return 2;
                case var result when result.Item1 == 0 && result.Item2 > 0:
                    return 1;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Compares the current instance of time interval with an interface to an upper contiguous interval boundary.
        /// </summary>
        /// <param name="boundary">The upper contiguous interval boundary to compare to.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less
        /// than zero This instance precedes other in the sort order. Zero This instance occurs in the same position in the sort order as other.
        /// Greater than zero This instance follows other in the sort order.</returns>
        public int CompareToUpper(IIntervalUpper<DateTime> boundary)
        {
            switch ((Calculus<DateTime>.CompareLowerUpper(this, boundary), Calculus<DateTime>.CompareUpper(this, boundary)))
            {
                case var result when result.Item1 < 0 && result.Item2 < 0:
                    return -2;
                case var result when result.Item1 < 0 && result.Item2 == 0:
                    return -1;
                case var result when result.Item1 > 0 && result.Item2 > 0:
                    return 2;
                case var result when result.Item1 == 0 && result.Item2 > 0:
                    return 1;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="value">The object to compare to this instance.</param>
        /// <returns>True if value is an instance of System.DateTime and equals the value of this instance; otherwise, false.</returns>
        public override bool Equals(object value)
        {
            switch (value)
            {
                case TimeInterval interval when ReferenceEquals(this, interval):
                    return true;
                case TimeInterval interval:
                    return Equals(_lower, interval._lower) && Equals(_upper, interval._upper) &&
                        Equals(_limits, interval._limits);
                case IInterval<DateTime> interval:
                    return Equals(_lower, interval.Lower) && Equals(LowerInclude, interval.LowerInclude) &&
                        Equals(_upper, interval.Upper) && Equals(UpperInclude, interval.UpperInclude);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => (_upper?.GetHashCode() ?? 0) ^ (_lower?.GetHashCode() ?? 0);

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{Type}: {(LowerInclude ? '[' : '(')}{Lower ?? (object)"[-Inf]"}-{Upper ?? (object)"[+Inf]"}{(UpperInclude ? ']' : ')')}/{Span ?? (object)"[Inf]"}";
        }

        /// <summary>
        /// Determines whether two specified instances of time interval are equal.
        /// </summary>
        /// <param name="x">The 1-st time interval value.</param>
        /// <param name="y">The 2-nd time interval value.</param>
        /// <returns>True if x and y represent the same time interval; otherwise, false</returns>
        public static bool operator ==(TimeInterval x, TimeInterval y) => x is null ? y is null : x.Equals(y);

        /// <summary>
        /// Determines whether two specified instances of time interval are not equal.
        /// </summary>
        /// <param name="x">The 1-st time interval value.</param>
        /// <param name="y">The 2-nd time interval value.</param>
        /// <returns>True if x and y do not represent the same time interval; otherwise, false</returns>
        public static bool operator !=(TimeInterval x, TimeInterval y) => !(x is null ? y is null : x.Equals(y));

        /// <summary>
        /// Determines whether one specified time interval is earlier than another specified time interval.
        /// </summary>
        /// <param name="x">The 1-st time interval value.</param>
        /// <param name="y">The 2-nd time interval value.</param>
        /// <returns>True if x is earlier than y; otherwise, false</returns>
        public static bool operator <(TimeInterval x, TimeInterval y) => x is null ? !(y is null) : x.CompareTo(y) < 0;

        /// <summary>
        /// Determines whether one specified time interval is later than another specified time interval.
        /// </summary>
        /// <param name="x">The 1-st time interval value.</param>
        /// <param name="y">The 2-nd time interval value.</param>
        /// <returns>True if x is later than y; otherwise, false</returns>
        public static bool operator >(TimeInterval x, TimeInterval y) => !(x is null) && x.CompareTo(y) > 0;

        /// <summary>
        /// Determines whether one specified time interval is the same as or earlier than another specified time interval.
        /// </summary>
        /// <param name="x">The 1-st time interval value.</param>
        /// <param name="y">The 2-nd time interval value.</param>
        /// <returns>True if x is the same as or earlier than y; otherwise, false</returns>
        public static bool operator <=(TimeInterval x, TimeInterval y) => x is null || x.CompareTo(y) <= 0;

        /// <summary>
        /// Determines whether one specified time interval is the same as or later than another specified time interval.
        /// </summary>
        /// <param name="x">The 1-st time interval value.</param>
        /// <param name="y">The 2-nd time interval value.</param>
        /// <returns>True if x is the same as or later than y; otherwise, false</returns>
        public static bool operator >=(TimeInterval x, TimeInterval y) => x is null ? (y is null) : x.CompareTo(y) >= 0;
    }
}