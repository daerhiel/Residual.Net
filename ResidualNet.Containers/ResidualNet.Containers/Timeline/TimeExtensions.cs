using System;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the date and time specific calculation utilities.
    /// </summary>
    public static partial class TimeExtensions
    {
        /// <summary>
        /// Converts the time series interval unit to the time span value.
        /// </summary>
        /// <param name="interval">The time series interval to calculate the time span for.</param>
        /// <returns>The time span corresponding to the time series interval unit.</returns>
        public static TimeSpan GetSpan(this SeriesInterval interval)
        {
            switch (interval)
            {
                case SeriesInterval.Series1Min:
                case SeriesInterval.Series5Min:
                case SeriesInterval.Series15Min:
                case SeriesInterval.Series30Min:
                    return TimeFragment.Minute.GetSpan((int)interval);
                default:
                    throw new ArgumentException($"Unsupported time series interval unit '{interval}'.", nameof(interval));
            }
        }

        /// <summary>
        /// Converts the time series interval unit to the time span value.
        /// </summary>
        /// <param name="interval">The time series interval to calculate the time span for.</param>
        /// <param name="count">The number of time series intervals covered by the series.</param>
        /// <returns>The time span corresponding to the time series interval unit.</returns>
        public static TimeSpan GetSpan(this SeriesInterval interval, int count)
        {
            switch (interval)
            {
                case SeriesInterval.Series1Min:
                case SeriesInterval.Series5Min:
                case SeriesInterval.Series15Min:
                case SeriesInterval.Series30Min:
                    return TimeFragment.Minute.GetSpan((int)interval * count);
                default:
                    throw new ArgumentException($"Unsupported time series interval unit '{interval}'.", nameof(interval));
            }
        }

        /// <summary>
        /// Converts the date and time fragment unit to the time span value.
        /// </summary>
        /// <param name="unit">The date and time fragment unit covered by the interval.</param>
        /// <returns>The time span corresponding to time fragment unit.</returns>
        public static TimeSpan GetSpan(this TimeFragment unit)
        {
            switch (unit)
            {
                case TimeFragment.Day:
                    return TimeSpan.FromDays(1);
                case TimeFragment.Hour:
                    return TimeSpan.FromHours(1);
                case TimeFragment.Minute:
                    return TimeSpan.FromMinutes(1);
                case TimeFragment.Second:
                    return TimeSpan.FromSeconds(1);
                default:
                    throw new ArgumentException($"Unsupported time fragment span unit '{unit}'.", nameof(unit));
            }
        }

        /// <summary>
        /// Converts the date and time fragment unit to the time span value.
        /// </summary>
        /// <param name="unit">The date and time fragment unit covered by the interval.</param>
        /// <param name="count">The number of time unit intervals covered by the series.</param>
        /// <returns>The time span corresponding to time fragment unit.</returns>
        public static TimeSpan GetSpan(this TimeFragment unit, int count)
        {
            switch (unit)
            {
                case TimeFragment.Day:
                    return TimeSpan.FromDays(count);
                case TimeFragment.Hour:
                    return TimeSpan.FromHours(count);
                case TimeFragment.Minute:
                    return TimeSpan.FromMinutes(count);
                case TimeFragment.Second:
                    return TimeSpan.FromSeconds(count);
                default:
                    throw new ArgumentException($"Unsupported time fragment span unit '{unit}'.", nameof(unit));
            }
        }

        /// <summary>
        /// Checks if the date and time value is before the reference point within specified number of intervals.
        /// </summary>
        /// <param name="source">The date and time reference point to check the vicinity for.</param>
        /// <param name="target">The date and time value to check the vicinity for.</param>
        /// <param name="unit">The date and time fragment unit covered by the interval.</param>
        /// <param name="count">The number of time series intervals to check around.</param>
        /// <returns>True if the requested date and time value is within the range before the point; otherwise, false.</returns>
        public static bool IsBefore(this DateTime source, DateTime target, TimeFragment unit, int count)
        {
            return target <= source + unit.GetSpan(count);
        }

        /// <summary>
        /// Checks if the date and time value is after the reference point within specified number of intervals.
        /// </summary>
        /// <param name="source">The date and time reference point to check the vicinity for.</param>
        /// <param name="target">The date and time value to check the vicinity for.</param>
        /// <param name="unit">The date and time fragment unit covered by the interval.</param>
        /// <param name="count">The number of time series intervals to check around.</param>
        /// <returns>True if the requested date and time value is within the range after the point; otherwise, false.</returns>
        public static bool IsAfter(this DateTime source, DateTime target, TimeFragment unit, int count)
        {
            return target >= source + unit.GetSpan(count);
        }

        /// <summary>
        /// Checks if the date and time value is before the reference point within specified number of intervals.
        /// </summary>
        /// <param name="source">The date and time reference point to check the vicinity for.</param>
        /// <param name="target">The date and time value to check the vicinity for.</param>
        /// <param name="interval">The time series interval to calculate the time span for.</param>
        /// <returns>True if the requested date and time value is within the range before the point; otherwise, false.</returns>
        public static bool IsBefore(this DateTime source, DateTime target, SeriesInterval interval)
        {
            return target <= source + interval.GetSpan();
        }

        /// <summary>
        /// Checks if the date and time value is after the reference point within specified number of intervals.
        /// </summary>
        /// <param name="source">The date and time reference point to check the vicinity for.</param>
        /// <param name="target">The date and time value to check the vicinity for.</param>
        /// <param name="interval">The time series interval to calculate the time span for.</param>
        /// <returns>True if the requested date and time value is within the range after the point; otherwise, false.</returns>
        public static bool IsAfter(this DateTime source, DateTime target, SeriesInterval interval)
        {
            return target >= source + interval.GetSpan();
        }

        /// <summary>
        /// Checks if the date and time value is before the reference point within specified number of intervals.
        /// </summary>
        /// <param name="source">The date and time reference point to check the vicinity for.</param>
        /// <param name="target">The date and time value to check the vicinity for.</param>
        /// <param name="interval">The time series interval to calculate the time span for.</param>
        /// <param name="count">The number of time series intervals to check around.</param>
        /// <returns>True if the requested date and time value is within the range before the point; otherwise, false.</returns>
        public static bool IsBefore(this DateTime source, DateTime target, SeriesInterval interval, int count)
        {
            return target <= source + interval.GetSpan(count);
        }

        /// <summary>
        /// Checks if the date and time value is after the reference point within specified number of intervals.
        /// </summary>
        /// <param name="source">The date and time reference point to check the vicinity for.</param>
        /// <param name="target">The date and time value to check the vicinity for.</param>
        /// <param name="interval">The time series interval to calculate the time span for.</param>
        /// <param name="count">The number of time series intervals to check around.</param>
        /// <returns>True if the requested date and time value is within the range after the point; otherwise, false.</returns>
        public static bool IsAfter(this DateTime source, DateTime target, SeriesInterval interval, int count)
        {
            return target >= source + interval.GetSpan(count);
        }

        /// <summary>
        /// Aligns the date and time value to the lower boundary of a time span.
        /// </summary>
        /// <param name="value">The date and time value that needs to be snapped onto the interval span.</param>
        /// <param name="span">The time span in ticks to align the date and time value onto.</param>
        /// <returns>The date and time value aligned to the lower time span boundary.</returns>
        public static DateTime RoundLower(this DateTime value, long span)
        {
            return value.AddTicks(-value.Ticks % span);
        }

        /// <summary>
        /// Aligns the date and time value to the upper boundary of a time span.
        /// </summary>
        /// <param name="value">The date and time value that needs to be snapped onto the interval span.</param>
        /// <param name="span">The time span in ticks to align the date and time value onto.</param>
        /// <returns>The date and time value aligned to the upper time span boundary.</returns>
        public static DateTime RoundUpper(this DateTime value, long span)
        {
            return value.AddTicks(span - value.Ticks % span);
        }

        /// <summary>
        /// Aligns the date and time value to the lower boundary of a time span.
        /// </summary>
        /// <param name="value">The date and time value that needs to be snapped onto the interval span.</param>
        /// <param name="count">The time span in time units to align the date and time value onto.</param>
        /// <param name="unit">The time span unit covered by the time series interval.</param>
        /// <returns>The date and time value aligned to the lower time span boundary.</returns>
        public static DateTime RoundLower(this DateTime value, int count, TimeFragment unit = TimeFragment.Minute)
        {
            return value.RoundLower(unit.GetSpan(count).Ticks);
        }

        /// <summary>
        /// Aligns the date and time value to the upper boundary of a time span.
        /// </summary>
        /// <param name="value">The date and time value that needs to be snapped onto the interval span.</param>
        /// <param name="count">The time span in time units to align the date and time value onto.</param>
        /// <param name="unit">The time span unit covered by the time series interval.</param>
        /// <returns>The date and time value aligned to the upper time span boundary.</returns>
        public static DateTime RoundUpper(this DateTime value, int count, TimeFragment unit = TimeFragment.Minute)
        {
            return value.RoundUpper(unit.GetSpan(count).Ticks);
        }

        /// <summary>
        /// Aligns the date and time value to the lower boundary of a time span.
        /// </summary>
        /// <param name="value">The date and time value that needs to be snapped onto the interval span.</param>
        /// <param name="count">The time span in time units to align the date and time value onto.</param>
        /// <param name="unit">The time span unit covered by the time series interval.</param>
        /// <returns>The date and time value aligned to the lower time span boundary.</returns>
        public static DateTime? RoundLower(this DateTime? value, int count, TimeFragment unit = TimeFragment.Minute)
        {
            if (value != null)
                return value.Value.RoundLower(unit.GetSpan(count).Ticks);
            else
                return null;
        }

        /// <summary>
        /// Aligns the date and time value to the upper boundary of a time span.
        /// </summary>
        /// <param name="value">The date and time value that needs to be snapped onto the interval span.</param>
        /// <param name="count">The time span in time units to align the date and time value onto.</param>
        /// <param name="unit">The time span unit covered by the time series interval.</param>
        /// <returns>The date and time value aligned to the upper time span boundary.</returns>
        public static DateTime? RoundUpper(this DateTime? value, int count, TimeFragment unit = TimeFragment.Minute)
        {
            if (value != null)
                return value.Value.RoundUpper(unit.GetSpan(count).Ticks);
            else
                return null;
        }

        /// <summary>
        /// Aligns the date and time value to the lower boundary of a time span.
        /// </summary>
        /// <param name="value">The date and time value that needs to be snapped onto the interval span.</param>
        /// <param name="interval">The time series interval to align the date and time value onto.</param>
        /// <returns>The date and time value aligned to the lower time span boundary.</returns>
        public static DateTime RoundLower(this DateTime value, SeriesInterval interval)
        {
            return value.RoundLower(interval.GetSpan().Ticks);
        }

        /// <summary>
        /// Aligns the date and time value to the upper boundary of a time span.
        /// </summary>
        /// <param name="value">The date and time value that needs to be snapped onto the interval span.</param>
        /// <param name="interval">The time series interval to align the date and time value onto.</param>
        /// <returns>The date and time value aligned to the upper time span boundary.</returns>
        public static DateTime RoundUpper(this DateTime value, SeriesInterval interval)
        {
            return value.RoundUpper(interval.GetSpan().Ticks);
        }

        /// <summary>
        /// Aligns the date and time value to the lower boundary of a time span.
        /// </summary>
        /// <param name="value">The date and time value that needs to be snapped onto the interval span.</param>
        /// <param name="interval">The time series interval to align the date and time value onto.</param>
        /// <returns>The date and time value aligned to the lower time span boundary.</returns>
        public static DateTime? RoundLower(this DateTime? value, SeriesInterval interval)
        {
            if (value != null)
                return value.Value.RoundLower(interval.GetSpan().Ticks);
            else
                return null;
        }

        /// <summary>
        /// Aligns the date and time value to the upper boundary of a time span.
        /// </summary>
        /// <param name="value">The date and time value that needs to be snapped onto the interval span.</param>
        /// <param name="interval">The time series interval to align the date and time value onto.</param>
        /// <returns>The date and time value aligned to the upper time span boundary.</returns>
        public static DateTime? RoundUpper(this DateTime? value, SeriesInterval interval)
        {
            if (value != null)
                return value.Value.RoundUpper(interval.GetSpan().Ticks);
            else
                return null;
        }
    }
}