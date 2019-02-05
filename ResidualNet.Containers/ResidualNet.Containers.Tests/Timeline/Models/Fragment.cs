using System;
using System.Collections.Generic;

namespace ResidualNet.Containers.Tests.Models
{
    public class Fragment : Interval
    {
        public IList<Reference> Entries { get; set; } = Array.Empty<Reference>();

        public override string ToString()
        {
            return $"[{Arg1}-{Arg2}]: Entries={Entries?.Count ?? 0}";
        }
    }
}