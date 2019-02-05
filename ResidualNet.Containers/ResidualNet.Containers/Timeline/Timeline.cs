using ResidualNet.Containers;
using System;
using System.Collections.Generic;

namespace ResidualNet.Timeline
{
    /// <summary>
    /// Represents the contiguous time series container that has order sequence of entries that is not splitted into segments.
    /// </summary>
    /// <typeparam name="T">The type of data to handle in time series.</typeparam>
    public class Timeline<T> : OrderedListLinks<Timeline<T>, T>
        where T : Timeline<T>.Entry
    {
        /// <summary>
        /// Initializes the new instance of a contiguous time series container.
        /// </summary>
        public Timeline()
        {
        }

        /// <summary>
        /// Initializes the new instance of a contiguous time series container with the sequence of entries.
        /// </summary>
        /// <param name="entries">The sequence of contiguous time series entries to initialize with.</param>
        public Timeline(IEnumerable<T> entries)
        {
            CoreInsert(InsertPosition.Following, null, entries);
        }

        /// <summary>
        /// Initializes the new instance of a contiguous time series container with the array of entries.
        /// </summary>
        /// <param name="entries">The array of contiguous time series entries to initialize with.</param>
        public Timeline(params T[] entries)
        {
            CoreInsert(InsertPosition.Following, null, entries);
        }

        /// <summary>
        /// Finds the contiguous time series entry that is specified by the date and time reference point.
        /// </summary>
        /// <param name="instance">The date and time reference point to get the contiguous time series entry for.</param>
        /// <param name="origin">The origin contiguous time series entry to use as a reference for incremental search.</param>
        /// <returns>The contiguous time series entry if found; otherwise, null.</returns>
        public T Find(DateTime instance, T origin = null)
        {
            T entry = null;
            if ((origin = origin ?? Head) != null)
                if (origin.Parent == this)
                {
                    var reference = instance.CompareTo(origin.Sample);
                    while (origin != null)
                        switch (reference)
                        {
                            case int last when last < 0:
                                if ((origin = origin.Prev) != null)
                                    switch (instance.CompareTo(origin.Sample))
                                    {
                                        case int next when next < 0: entry = origin; reference = next; break;
                                        case int next when next > 0: entry = origin = null; break;
                                        default: entry = origin; origin = null; break;
                                    }
                                break;
                            case int last when last > 0:
                                if ((origin = origin.Next) != null)
                                    switch (instance.CompareTo(origin.Sample))
                                    {
                                        case int next when next < 0: entry = origin = null; break;
                                        case int next when next > 0: entry = origin; reference = next; break;
                                        default: entry = origin; origin = null; break;
                                    }
                                break;
                            default: entry = origin; origin = null; break;
                        }
                }
                else
                    throw new ArgumentException($"The origin time series entry is bound to other container: {origin}", nameof(origin));
            return entry;
        }

        /// <summary>
        /// Finds the contiguous time series entry that is the most close to the lower interval boundary.
        /// </summary>
        /// <param name="interval">The interface to the lower interval boundary to find the lower entry.</param>
        /// <param name="origin">The origin lower entry position to start the search from.</param>
        /// <returns>The contiguous time series entry if found; otherwise, null.</returns>
        public T FindLower(IInterval<DateTime> interval, T origin)
        {
            if (interval != null)
            {
                var entry = origin = origin ?? Head;
                if (entry != null)
                    if (origin.Parent == this)
                    {
                        var reference = interval.IsLowerLimit(entry.Sample);
                        while (entry != null)
                            switch (reference)
                            {
                                case bool last when last:
                                    entry = origin.Prev;
                                    if (entry != null)
                                        switch (interval.IsLowerLimit(entry.Sample))
                                        {
                                            case bool next when next: origin = entry; reference = next; break;
                                            default: entry = null; break;
                                        }
                                    break;
                                case bool last when !last:
                                    entry = origin = origin.Next;
                                    if (entry != null)
                                        switch (interval.IsLowerLimit(entry.Sample))
                                        {
                                            case bool next when !next: reference = next; break;
                                            default: entry = null; break;
                                        }
                                    break;
                            }
                        if (origin != null && !interval.IsUpperLimit(origin.Sample))
                            origin = null;
                    }
                    else
                        throw new ArgumentException($"The origin time series entry is bound to other container: {origin}", nameof(origin));
                return origin;
            }
            else
                throw new ArgumentNullException(nameof(interval));
        }

        /// <summary>
        /// Finds the contiguous time series entry that is the most close to the upper interval boundary.
        /// </summary>
        /// <param name="interval">The interface to the upper interval boundary to find the upper entry.</param>
        /// <param name="origin">The origin upper entry position to start the search from.</param>
        /// <returns>The contiguous time series entry if found; otherwise, null.</returns>
        public T FindUpper(IInterval<DateTime> interval, T origin)
        {
            if (interval != null)
            {
                var entry = origin = origin ?? Head;
                if (entry != null)
                    if (origin.Parent == this)
                    {
                        var reference = interval.IsUpperLimit(entry.Sample);
                        while (entry != null)
                            switch (reference)
                            {
                                case bool last when last:
                                    entry = origin.Next;
                                    if (entry != null)
                                        switch (interval.IsUpperLimit(entry.Sample))
                                        {
                                            case bool next when next: origin = entry; reference = next; break;
                                            default: entry = null; break;
                                        }
                                    break;
                                case bool last when !last:
                                    entry = origin = origin.Prev;
                                    if (entry != null)
                                        switch (interval.IsUpperLimit(entry.Sample))
                                        {
                                            case bool next when !next: reference = next; break;
                                            default: entry = null; break;
                                        }
                                    break;
                            }
                        if (origin != null && !interval.IsLowerLimit(origin.Sample))
                            origin = null;
                    }
                    else
                        throw new ArgumentException($"The origin time series entry is bound to other container: {origin}", nameof(origin));
                return origin;
            }
            else
                throw new ArgumentNullException(nameof(interval));
        }

        /// <summary>
        /// Gets the block of contiguous time series entries for the specific time series stream container segment.
        /// </summary>
        /// <param name="fragment">The time series stream block to get the block of contiguous time series entries for.</param>
        /// <param name="lower">The lower date and time interval boundary to get the nodes for.</param>
        /// <param name="upper">The upper date and time interval boundary to get the nodes for.</param>
        /// <returns>The sequence of contiguous time series entries contained within the required time series stream container segment.</returns>
        public IEnumerable<T> Get(TimelineStream<T>.Fragment fragment, DateTime? lower = null, DateTime? upper = null)
        {
            if (fragment != null)
            {
                var lowerEntry = fragment.LowerEntry;
                if (lower != null)
                    while (lowerEntry?.Sample < lower)
                        lowerEntry = lowerEntry.Next;
                var upperEntry = fragment.UpperEntry;
                if (upper != null)
                    while (upperEntry?.Sample > upper)
                        upperEntry = upperEntry.Prev;
                if (lowerEntry != null && upperEntry != null)
                    return new Incrementor.Collection(this, lowerEntry, upperEntry);
                else
                    return Array.Empty<T>();
            }
            else
                throw new ArgumentNullException(nameof(fragment));
        }

        /// <summary>
        /// Inserts the node into the double linked collection at the origin node if specified.
        /// </summary>
        /// <param name="entry">The node to be inserted into the double linked list.</param>
        /// <param name="origin">The origin node to use as a reference to insert the specified node at; the node will be inserted
        /// into the head if none is specified.</param>
        /// <returns>The specified node when inserted; otherwise the existing node with the same date and time references.</returns>
        public T Insert(T entry, T origin = null)
        {
            return entry?.CoreInsert(this, InsertPosition.Following, origin);
        }

        /// <summary>
        /// Removes the array of nodes from the double linked collection.
        /// </summary>
        /// <param name="fragment">The time series stream block to get the block of contiguous time series entries for.</param>
        /// <param name="lower">The lower date and time interval boundary to remove the nodes for.</param>
        /// <param name="upper">The upper date and time interval boundary to remove the nodes for.</param>
        /// <returns>The specified array of nodes removed from the double linked collection.</returns>
        public Timeline<T> Remove(TimelineStream<T>.Fragment fragment, DateTime? lower = null, DateTime? upper = null)
        {
            return CoreRemove(Get(fragment, lower, upper));
        }

        /// <summary>
        /// Represents the contiguous time series entry, that contains time series data.
        /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
        public abstract class Entry : OrderedLink, ITimeReference
#pragma warning restore CA1034 // Nested types should not be visible
        {
            /// <summary>
            /// The date and time reference point associated with an object.
            /// </summary>
            public abstract DateTime Sample { get; protected set; }

            /// <summary>
            /// Reimports the data from the existing entry instead of inserting the new entry into the container.
            /// </summary>
            /// <param name="entry">The contiguous time series entry to import the data from.</param>
            protected abstract void Reimport(T entry);

            /// <summary>
            /// Attached the current node in between the preceding and following node into a double linked collection.
            /// </summary>
            /// <param name="preceding">The preceding node to insert the current node after.</param>
            /// <param name="following">The following node to insert the current node before.</param>
            /// <returns>The current node converted to actual node type when inserted.</returns>
            protected override T CoreAttach(T preceding, T following)
            {
                if (preceding?.Sample != Sample && following?.Sample != Sample)
                    return base.CoreAttach(preceding, following);
                else
                    throw new InvalidOperationException($"The time series entry already exists for the same reference: {Sample}.");
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
            protected override int CompareTo(ref T origin, ref T preceding, ref T following) => Sample.CompareTo(origin.Sample);

            #region [Virtuals]: Linking Constraints
            /// <summary>
            /// Finds the pair of preceding and following nodes for insertion based on origin and insertion position.
            /// </summary>
            /// <param name="parent">The double linked list collection to use to insert the current node into.</param>
            /// <param name="origin">The origin node to use as a reference to insert the current node; the node will be inserted
            /// into the reference following position if none is specified.</param>
            /// <param name="preceding">The preceding node found to insert the current node after.</param>
            /// <param name="following">The following node found to insert the current node before.</param>
            /// <param name="position">The preferred insertion order to use relative to the origin node.</param>
            /// <returns>True if the correct insertion position is found; otherwise, false.</returns>
            protected override bool FindPosition(Timeline<T> parent, ref T origin, out T preceding, out T following, ref InsertPosition position)
            {
                if (base.FindPosition(parent, ref origin, out preceding, out following, ref position) is bool isFound)
                    switch (position)
                    {
                        case InsertPosition.Preceding when preceding?.Sample == Sample:
                            (origin = preceding).Reimport((T)this);
                            return false;
                        case InsertPosition.Following when following?.Sample == Sample:
                            (origin = following).Reimport((T)this);
                            return false;
                    }
                return isFound;
            }
            #endregion
        }
    }
}