using ResidualNet.Timeline;
using System;

namespace ResidualNet.Containers.Tests.Models
{
    public class Reference : Timeline<Reference>.Entry
    {
        public override DateTime Sample { get; protected set; }

        public Reference(DateTime sample)
        {
            Sample = sample;
        }

        public override string ToString()
        {
            return $"Reference: {Sample}";
        }

        protected override void Reimport(Reference entry)
        {
        }
    }
}