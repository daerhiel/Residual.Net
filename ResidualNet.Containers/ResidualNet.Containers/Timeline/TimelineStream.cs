using ResidualNet.Containers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the time series stream block container that handles time series data cache.
    /// </summary>
    /// <typeparam name="T">The type of data to handle in time series.</typeparam>
    public class TimelineStream<T> : OrderedListLinks<TimelineStream<T>, TimelineStream<T>.Fragment>
        where T : Timeline<T>.Entry
    {
        /// <summary>
        /// The contiguous time series container that holds time series for the time series block container and maintains its actual sequence.
        /// </summary>
        protected Timeline<T> Timeline { get; } = new Timeline<T>();

        /// <summary>
        /// The receiver buffer segment that accumulates the time series.
        /// </summary>
        protected Fragment Receiver { get; }

        /// <summary>
        /// The time span to allocate for the receiver buffer segment.
        /// </summary>
        public TimeSpan ReceiveSpan { get; }

        /// <summary>
        /// The time span to allocate for a regular segment that the receiver data bleed into.
        /// </summary>
        public TimeSpan SegmentSpan { get; }

        /// <summary>
        /// The time span to allocate for a regular segment to expire and be recycled.
        /// </summary>
        public TimeSpan ExpiredSpan { get; }

        /// <summary>
        /// The date and time value that is limited by the receiver segment.
        /// </summary>
        public DateTime? ReceiveLower => Receiver?.Lower;

        /// <summary>
        /// Initializes the new instance of a time series stream block container without receiver segment.
        /// </summary>
        /// <param name="expiredSpan">The time span to use as a stream segmentation expiration.</param>
        protected TimelineStream(TimeSpan expiredSpan)
        {
            ExpiredSpan = expiredSpan;
        }

        /// <summary>
        /// Initializes the new instance of a time series stream block container with persistent receiver segment.
        /// </summary>
        /// <param name="reference">The date and time to set as a reference point for the receiver segment.</param>
        /// <param name="receiveSpan">The time span to allocate for the receiver buffer segment.</param>
        /// <param name="segmentSpan">The time span to use as a stream segmentation size.</param>
        /// <param name="expiredSpan">The time span to use as a stream segmentation expiration.</param>
        protected TimelineStream(DateTime reference, TimeSpan receiveSpan, TimeSpan segmentSpan, TimeSpan expiredSpan)
        {
            if (receiveSpan > TimeSpan.Zero)
                if (segmentSpan > TimeSpan.Zero)
                {
                    Receiver = new Fragment(reference.RoundLower(1, TimeFragment.Minute) - receiveSpan).CoreInsert(this, InsertPosition.Following);
                    ReceiveSpan = receiveSpan;
                    SegmentSpan = segmentSpan;
                    ExpiredSpan = expiredSpan;
                }
                else
                    throw new ArgumentException($"The segment span can't be negative or zero: {segmentSpan}.", nameof(segmentSpan));
            else
                throw new ArgumentException($"The receive span can't be negative or zero: {receiveSpan}.", nameof(receiveSpan));
        }

        /// <summary>
        /// Creates or gets the existing time series entry on the receiver segment of the time series stream block container.
        /// </summary>
        /// <param name="reference">The date and time reference point to get the time series entry for.</param>
        /// <param name="factory">The time series entry factory function to create the time series entry when required.</param>
        /// <returns>The time series entry requested.</returns>
        protected T CreateOrGet(DateTime reference, Func<DateTime, T> factory)
        {
            if (Receiver != null)
                return Receiver.CreateOrGet(reference, factory);
            else
                throw new InvalidOperationException($"The receiver is required to stream data.");
        }

        /// <summary>
        /// Creates or gets the existing time series entry on the receiver segment of the time series stream block container.
        /// </summary>
        /// <param name="reference">The date and time reference point to get the time series entry for.</param>
        /// <param name="factory">The time series entry factory function to create the time series entry when required.</param>
        /// <returns>The time series entry requested.</returns>
        protected Task<T> CreateOrGetAsync(DateTime reference, Func<DateTime, Task<T>> factory)
        {
            if (Receiver != null)
                return Receiver.CreateOrGetAsync(reference, factory);
            else
                throw new InvalidOperationException($"The receiver is required to stream data.");
        }

        /// <summary>
        /// Gets the time intervals missing in the contiguous space of time series stream blocks.
        /// </summary>
        /// <param name="lower">The lower date and time interval boundary to get the segments for.</param>
        /// <param name="upper">The upper date and time interval boundary to get the segments for.</param>
        /// <returns>The array of missing time intervals found.</returns>
        protected IInterval<DateTime>[] GetIntervals(DateTime? lower = null, DateTime? upper = null)
        {
            var intervals = new List<TimeInterval>(new[] { new TimeInterval(lower, upper) });
            foreach (var fragment in Increment)
                switch (intervals.Find(x => x.Overlaps(fragment)))
                {
                    case TimeInterval interval when interval.SubtractAt(fragment) is TimeInterval adjacent:
                        if (intervals.BinarySearch(adjacent) is var index && index < 0)
                            intervals.Insert(~index, adjacent);
                        break;
                    case TimeInterval interval when interval.IsEmpty:
                        intervals.Remove(interval);
                        break;
                }
            return intervals.Cast<IInterval<DateTime>>().ToArray();
        }

        /// <summary>
        /// Sets the time fragments missing in the time series stream blocks and updates the state of existing blocks.
        /// </summary>
        /// <param name="entries">The sequence of time series entries to set for the missing segments as a content.</param>
        /// <param name="lower">The lower date and time interval boundary to set the segments for.</param>
        /// <param name="upper">The upper date and time interval boundary to set the segments for.</param>
        /// <returns>The sequence of time series entries contained within the required time interval.</returns>
        protected IEnumerable<T> SetSegment(IEnumerable<T> entries, DateTime? lower = null, DateTime? upper = null)
        {
            var interval = new TimeInterval(lower, upper);
            var received = Array.Empty<T>().AsEnumerable();
            if (Receiver?.Overlaps(interval) == true)
            {
                interval.SubtractAt(Receiver);
                received = Receiver.Get(lower, upper);
            }
            if (!interval.IsEmpty)
                return new Fragment(interval).CoreInsert(this, InsertPosition.Following).Set(entries, lower, upper).Union(received);
            else
                return received;
        }

        /// <summary>
        /// Updates the sequence of time series entries on the receiver segment of the time series stream block container.
        /// </summary>
        /// <param name="entries">The sequence of time series entries to set for the receiver segment as a content.</param>
        protected void PushReceiver(IEnumerable<T> entries)
        {
            if (Receiver != null)
                Receiver.PushEntries(entries);
            else
                throw new InvalidOperationException($"The receiver is required to stream data.");
        }

        /// <summary>
        /// Cleans up the time series stream blocks that are expired.
        /// </summary>
        /// <param name="expiresOn">The date and time to use as a current expiration reference point.</param>
        protected void Cleanup(DateTime expiresOn)
        {
            foreach (var fragment in Increment)
                if (fragment != Receiver && fragment.ExpiresOn <= expiresOn)
                    fragment.CoreRemove();
        }

        /// <summary>
        /// Represents the time series stream container segment, that defines the range of time series entries.
        /// </summary>
#pragma warning disable CA1034, CA1710 // Nested types should not be visible; Identifiers should have correct suffix
        public class Fragment : OrderedLink, IInterval<DateTime>, IEnumerable<T>
#pragma warning restore CA1034, CA1710 // Nested types should not be visible; Identifiers should have correct suffix
        {
            private readonly TimeInterval _interval = new TimeInterval();
            private readonly bool _persistent;

            /// <summary>
            /// The lower contiguous time series entry that holds the series buffer.
            /// </summary>
            protected internal T LowerEntry { get; private set; }

            /// <summary>
            /// The upper contiguous time series entry that holds the series buffer.
            /// </summary>
            protected internal T UpperEntry { get; private set; }

            /// <summary>
            /// The lower date and time boundary of the time series stream block.
            /// </summary>
            public DateTime? Lower => _interval.Lower;

            /// <summary>
            /// The upper date and time boundary of the time series stream block.
            /// </summary>
            public DateTime? Upper => _interval.Upper;

            /// <summary>
            /// Time span of an interval. It always affects limits, treats lower limit as a base point.
            /// </summary>
            public TimeSpan? Span => _interval.Span;

            /// <summary>
            /// True if the lower date and time boundary includes the boundary value into the interval.
            /// </summary>
            public bool LowerInclude => _interval.LowerInclude;

            /// <summary>
            /// True if the upper date and time boundary includes the boundary value into the interval.
            /// </summary>
            public bool UpperInclude => _interval.UpperInclude;

            /// <summary>
            /// The date and time of the time series stream block expiration.
            /// </summary>
            public DateTime? ExpiresOn { get; private set; }

            /// <summary>
            /// Initializes the new instance of the time series stream block.
            /// </summary>
            /// <param name="interval">The time interval to initialize the stream block with.</param>
            public Fragment(IInterval<DateTime> interval)
            {
                _interval = new TimeInterval(interval);
            }

            /// <summary>
            /// Initializes the new instance of the stream block fragment.
            /// </summary>
            /// <param name="lower">The lower date and time interval boundary to set the segments for.</param>
            /// <param name="upper">The upper date and time interval boundary to set the segments for.</param>
            public Fragment(DateTime? lower = null, DateTime? upper = null)
            {
                _interval = new TimeInterval(lower, upper);
                _persistent = upper == null;
            }


            /// <summary>
            /// Initializes the new instance of the stream block fragment based on upper and lower range/span limit values.
            /// </summary>
            /// <param name="upper">The interface to the upper interval boundary to set the interval with.</param>
            /// <param name="lower">The interface to the lower interval boundary to set the interval with.</param>
            public Fragment(IIntervalUpper<DateTime> upper, IIntervalLower<DateTime> lower)
            {
                _interval = new TimeInterval(upper, lower);
            }

            /// <summary>
            /// Checks if the date and time value meets the time fragment lower boundary condition.
            /// </summary>
            /// <param name="value">The date and time value to check against.</param>
            /// <returns>True if the lower limit meets the boundary conditions; otherwise, false.</returns>
            public bool IsLowerLimit(DateTime? value) => _interval.IsLowerLimit(value);

            /// <summary>
            /// Checks if the date and time value meets the time fragment upper boundary condition.
            /// </summary>
            /// <param name="value">The date and time value to check against.</param>
            /// <returns>True if the upper limit meets the boundary conditions; otherwise, false.</returns>
            public bool IsUpperLimit(DateTime? value) => _interval.IsUpperLimit(value);

            /// <summary>
            /// Checks if the date and time value is within the time fragment range including boundaries.
            /// </summary>
            /// <param name="value">The date and time value to check against.</param>
            /// <returns>True if date and time is within the time iterval; otherwise, false.</returns>
            public bool IsWithin(DateTime value) => _interval.IsWithin(value);

            /// <summary>
            /// Checks if the date and time value is within the time fragment range including boundaries considering that null is a -infinity.
            /// </summary>
            /// <param name="value">The date and time value to check against.</param>
            /// <returns>True if date and time is within the time iterval; otherwise, false.</returns>
            public bool IsWithinLower(DateTime? value) => _interval.IsWithinLower(value);

            /// <summary>
            /// Checks if the date and time value is within the time fragment range including boundaries considering that null is a +infinity.
            /// </summary>
            /// <param name="value">The date and time value to check against.</param>
            /// <returns>True if date and time is within the time iterval; otherwise, false.</returns>
            public bool IsWithinUpper(DateTime? value) => _interval.IsWithinUpper(value);

            /// <summary>
            /// Checks if the time interval overlaps with specified time interval inclusively.
            /// </summary>
            /// <param name="interval">The time interval to check overlapping with.</param>
            /// <returns>True if two intervals are overlapping; otherwise, false.</returns>
            public bool Overlaps(IInterval<DateTime> interval) => _interval.Overlaps(interval);

            /// <summary>
            /// Checks if the date and time value is valid within the ownership rules for the time series stream block.
            /// </summary>
            /// <param name="value">The date and time value to check against.</param>
            /// <returns>True if the time series stream block is valid to own the date and time value; otherwise, false.</returns>
            public bool IsValid(DateTime value) => Parent != null && _interval.IsWithin(value);

            /// <summary>
            /// Returns an enumerator that iterates through the time series stream block entries.
            /// </summary>
            /// <returns>A time series stream block entries enumerator.</returns>
            IEnumerator IEnumerable.GetEnumerator() => (Parent?.Timeline.Get(this) ?? Array.Empty<T>()).GetEnumerator();

            /// <summary>
            /// Returns an enumerator that iterates through the time series stream block entries.
            /// </summary>
            /// <returns>A time series stream block entries enumerator.</returns>
            public IEnumerator<T> GetEnumerator() => (Parent?.Timeline.Get(this) ?? Array.Empty<T>()).GetEnumerator();

            /// <summary>
            /// This method is called after the node is inserted into the double linked collection.
            /// </summary>
            protected override void AfterInserting()
            {
                if (this != Parent.Receiver)
                {
                    ExpiresOn = DateTime.UtcNow + Parent.ExpiredSpan;
                    RebindTimeline();
                }
                base.AfterInserting();
            }

            /// <summary>
            /// This method is called before the node is removed into the double linked collection.
            /// </summary>
            protected override void BeforeDeleting()
            {
                if (LowerEntry != null && UpperEntry != null)
                {
                    Parent?.Timeline.Remove(this);
                    DetachTimeline();
                }
                base.BeforeDeleting();
            }

            /// <summary>
            /// Removes the current node from a double linked collection and unlinks it from a parent collection.
            /// </summary>
            /// <returns>The current node converted to actual node type when removed.</returns>
            protected internal override Fragment CoreRemove()
            {
                if (_persistent)
                {
                    BeforeDeleting();
                    return null;
                }
                else
                    return base.CoreRemove();
            }

            /// <summary>
            /// Rebinds the contiguous time series entries according to the current date and time boundaries.
            /// </summary>
            /// <returns>The current time series stream container segment.</returns>
            protected Fragment RebindTimeline()
            {
                LowerEntry = Parent?.Timeline.FindLower(_interval, LowerEntry);
                UpperEntry = Parent?.Timeline.FindUpper(_interval, UpperEntry);
                if (LowerEntry?.Sample > UpperEntry?.Sample)
                    Debugger.Break();
                return this;
            }

            /// <summary>
            /// Detaches the contiguous time series entries from the current 
            /// </summary>
            /// <returns>The current time series stream container segment.</returns>
            protected Fragment DetachTimeline()
            {
                LowerEntry = null;
                UpperEntry = null;
                return this;
            }

            /// <summary>
            /// Compares and processes the current node with another an origin node and returns an integer that indicates whether the current
            /// instance precedes, follows, or occurs in the same position in the sort order as the other object. The preceding and following
            /// nodes will also be adjusted according to the result of a comparison.
            /// </summary>
            /// <param name="origin">An origin node to compare with the current instance.</param>
            /// <param name="preceding">The preceding node found to compare the current node with.</param>
            /// <param name="following">The following node found to compare the current node with.</param>
            /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less
            /// than zero This instance precedes other in the sort order. Zero This instance occurs in the same position in the sort order as other.
            /// Greater than zero This instance follows other in the sort order.</returns>
            protected override int CompareTo(ref Fragment origin, ref Fragment preceding, ref Fragment following)
            {
                var reference = 0;
                while (origin?._interval is TimeInterval interval && (reference = _interval.CompareTo(interval)) == 0)
                    switch ((_interval.CompareToLower(interval), _interval.CompareToUpper(interval)))
                    {
                        case var result when result.Item1 > 1 || result.Item2 < -1:
                            if (interval.SubtractAt(_interval) is TimeInterval adjacent)
                                Coerse(origin, new Fragment(adjacent).CoreInsert(origin.Parent, InsertPosition.Following, origin), ref preceding, ref following);
                            origin.RebindTimeline();
                            break;
                        case var result:
                            Coerse(ref origin, ref preceding, ref following).DetachTimeline().CoreRemove();
                            break;
                    }
                return reference;
            }

            /// <summary>
            /// Realigns the lower boundary of the specified time series stream block and its preceding neighbor.
            /// </summary>
            /// <param name="reference">The date and time reference point to use as a current time.</param>
            protected void RealignLower(DateTime reference)
            {
                if (reference - Lower >= Parent.ReceiveSpan)
                {
                    var segment = Prev;
                    if (segment == null || segment.Upper < Lower || segment.Span >= Parent.SegmentSpan)
                        segment = new Fragment(segment, this).CoreInsert(Parent, InsertPosition.Preceding, this);
                    segment._interval.Upper = _interval.Lower = reference.RoundUpper(1, TimeFragment.Minute) - Parent.ReceiveSpan;
                    segment.RebindTimeline();
                    RebindTimeline();
                }
            }

            /// <summary>
            /// Creates or gets the existing time series entry on the receiver segment of the time series stream block container.
            /// </summary>
            /// <param name="reference">The date and time reference point to get the time series entry for.</param>
            /// <param name="factory">The time series entry factory function to create the time series entry when required.</param>
            /// <returns>The time series entry requested.</returns>
            protected internal T CreateOrGet(DateTime reference, Func<DateTime, T> factory)
            {
                if (factory != null)
                    if (IsWithin(reference))
                    {
                        RealignLower(reference);
                        if (Parent?.Timeline.Find(reference, UpperEntry) is T entry)
                            return entry;
                        else
                            return UpdateEntry(factory(reference), UpperEntry);
                    }
                    else
                        throw new ArgumentException($"The date and time is not within the range: {reference}.", nameof(reference));
                else
                    throw new ArgumentNullException(nameof(factory));
            }

            /// <summary>
            /// Creates or gets the existing time series entry on the receiver segment of the time series stream block container.
            /// </summary>
            /// <param name="reference">The date and time reference point to get the time series entry for.</param>
            /// <param name="factory">The time series entry factory function to create the time series entry when required.</param>
            /// <returns>The time series entry requested.</returns>
            protected internal async Task<T> CreateOrGetAsync(DateTime reference, Func<DateTime, Task<T>> factory)
            {
                if (factory != null)
                    if (IsWithin(reference))
                    {
                        RealignLower(reference);
                        if (Parent?.Timeline.Find(reference, UpperEntry) is T entry)
                            return entry;
                        else
                            return UpdateEntry(await factory(reference).ConfigureAwait(false), UpperEntry);
                    }
                    else
                        throw new ArgumentException($"The date and time is not within the range: {reference}.", nameof(reference));
                else
                    throw new ArgumentNullException(nameof(factory));
            }

            /// <summary>
            /// Updates the time series entry in the time series stream block and returns the boolean operation result.
            /// </summary>
            /// <param name="entry">The time series entry to set to the time series stream block as a content.</param>
            /// <param name="origin">The origin node to use as a reference to insert the current node; the node will be inserted
            /// into the reference following position if none is specified.</param>
            /// <returns>The current node converted to actual node type when updated in the container.</returns>
            protected T UpdateEntry(T entry, T origin)
            {
                if (entry?.Sample is DateTime reference && _interval.IsWithin(reference))
                    if (Parent?.Timeline.Insert(entry, origin) is T instance)
                    {
                        if (LowerEntry == null || instance.Sample < LowerEntry?.Sample)
                            LowerEntry = instance;
                        if (UpperEntry == null || instance.Sample > UpperEntry?.Sample)
                            UpperEntry = instance;
                        if (LowerEntry?.Sample > UpperEntry?.Sample)
                            Debugger.Break();
                        return instance;
                    }
                return null;
            }

            /// <summary>
            /// Gets the time series entries from the time series stream block for the requested time interval.
            /// </summary>
            /// <param name="lower">The lower date and time interval boundary to set the segments for.</param>
            /// <param name="upper">The upper date and time interval boundary to set the segments for.</param>
            /// <returns>The sequence of time series entries contained within the required time interval.</returns>
            public IEnumerable<T> Get(DateTime? lower = null, DateTime? upper = null)
            {
                return Parent?.Timeline.Get(this, lower, upper) ?? Array.Empty<T>();
            }

            /// <summary>
            /// Sets the sequence of time series entries to the time series stream block and returns the sequence of entries for the requested time interval.
            /// </summary>
            /// <param name="entries">The sequence of time series entries to set for the time series stream block as a content.</param>
            /// <param name="lower">The lower date and time interval boundary to set the segments for.</param>
            /// <param name="upper">The upper date and time interval boundary to set the segments for.</param>
            /// <returns>The sequence of time series entries contained within the required time interval.</returns>
            protected internal IEnumerable<T> Set(IEnumerable<T> entries, DateTime? lower = null, DateTime? upper = null)
            {
                T origin = null;
                foreach (var entry in entries ?? Array.Empty<T>())
                    origin = UpdateEntry(entry, origin ?? (origin = GetOrigin(entry)));
                return Parent?.Timeline.Get(this, lower, upper) ?? Array.Empty<T>();
            }

            /// <summary>
            /// Determines the closest time series entry origin based on the time and date reference value.
            /// </summary>
            /// <param name="entry">The time series entry to find the insertion origin for.</param>
            /// <returns>The closest known time series entry origin if found; otherwise, null.</returns>
            protected T GetOrigin(T entry)
            {
                if (entry != null)
                    switch (((LowerEntry?.Sample - entry.Sample)?.Duration(), (UpperEntry?.Sample - entry.Sample)?.Duration()))
                    {
                        case var result when result.Item1 != null && result.Item2 != null:
                            if (result.Item1.Value > result.Item2.Value)
                                return LowerEntry;
                            else
                                return UpperEntry;
                        case var result when result.Item1 != null:
                            return LowerEntry;
                        case var result when result.Item2 != null:
                            return UpperEntry;
                        default:
                            return null;
                    }
                else
                    throw new ArgumentNullException(nameof(entry));
            }

            /// <summary>
            /// Pushes the sequence of time series entries onto the receiver segment of the time series stream block container.
            /// </summary>
            /// <param name="entries">The sequence of time series entries to push onto the receiver segment as a content.</param>
            protected internal void PushEntries(IEnumerable<T> entries)
            {
                T origin = null;
                foreach (var entry in entries ?? Array.Empty<T>())
                    if (entry != null && IsWithin(entry.Sample))
                    {
                        RealignLower(entry.Sample);
                        origin = UpdateEntry(entry, origin ?? (origin = GetOrigin(entry)));
                    }
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return $"{(_persistent ? "Receiver" : "Segment")}: {(LowerInclude ? '[' : '(')}{Lower ?? (object)"[-Inf]"}-{Upper ?? (object)"[+Inf]"}{(UpperInclude ? ']' : ')')}/{(Upper - Lower) ?? (object)"[Inf]"}";
            }

            /// <summary>
            /// The default comparer for the time series stream block entries.
            /// </summary>
            protected static Comparer<T> Comparer { get; } = Comparer<T>.Create(Compare);

            /// <summary>
            /// Compares two time series stream block entries.
            /// </summary>
            /// <param name="x">The 1-st time series stream block entry.</param>
            /// <param name="y">The 2-nd time series stream block entry.</param>
            /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less
            /// than zero This instance precedes other in the sort order. Zero This instance occurs in the same position in the sort order as other.
            /// Greater than zero This instance follows other in the sort order.</returns>
            protected static int Compare(T x, T y) => Comparer<DateTime?>.Default.Compare(x?.Sample, y?.Sample);
        }
    }
}