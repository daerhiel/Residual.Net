using ResidualNet.Timeline;
using System;

namespace ResidualNet.Containers.Tests.Models
{
    public class Interval
    {
        public IntervalType Type { get; set; }
        public DateTime? Arg1 { get; set; }
        public DateTime? Arg2 { get; set; }
        public bool Include1 { get; set; }
        public bool Include2 { get; set; }
        public bool IsEmpty { get; set; }

        public override string ToString()
        {
            return $"[{Arg1}-{Arg2}]";
        }
    }
}