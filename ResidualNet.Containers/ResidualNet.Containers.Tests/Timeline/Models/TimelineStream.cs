using ResidualNet.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResidualNet.Containers.Tests.Models
{
    public class TimelineStream : TimelineStream<Reference>
    {
        public Fragment[] GetItems() => Increment.ToArray();

        public TimelineStream(params Models.Fragment[] fragments)
            : base(TimeSpan.FromMinutes(1))
        {
            foreach (var fragment in fragments ?? Array.Empty<Models.Fragment>())
                SetSegment(fragment.Entries, fragment.Arg1, fragment.Arg2);
        }

        public TimelineStream(TimeSpan receiveSpan, TimeSpan segmentSpan)
            : base(DateTime.UtcNow, receiveSpan, segmentSpan, TimeSpan.FromMilliseconds(100))
        {
        }

        public IInterval<DateTime>[] Get(DateTime? lower = null, DateTime? upper = null)
        {
            return GetIntervals(lower, upper);
        }

        public IEnumerable<Reference> Set(IEnumerable<Reference> values, DateTime? lower = null, DateTime? upper = null)
        {
            return SetSegment(values, lower, upper);
        }

        public Reference Create(DateTime sample)
        {
            return CreateOrGet(sample, x => new Reference(x));
        }

        public void Push(params Reference[] entries)
        {
            PushReceiver(entries);
        }

        public void CleanupExpired(DateTime expiresOn)
        {
            Cleanup(expiresOn);
        }

        public void Clear()
        {
            CoreClear();
        }
    }
}