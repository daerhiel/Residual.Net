using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResidualNet.Containers.Tests.Models;
using System;
using System.Linq;

namespace ResidualNet.Timeline.Tests
{
    /// <summary>
    /// Represents thge general time assertion methods.
    /// </summary>
    public partial class GeneralTimeTests : GeneralTests
    {
#pragma warning disable CA1707 // Identifiers should not contain underscores
        protected static readonly DateTime _dp1 = new DateTime(2018, 1, 1, 0, 0, 0);
        protected static readonly DateTime _dp2 = new DateTime(2018, 1, 1, 6, 0, 0);
        protected static readonly DateTime _dpx = new DateTime(2018, 1, 1, 12, 0, 0);
        protected static readonly DateTime _dp3 = new DateTime(2018, 1, 1, 18, 0, 0);
        protected static readonly DateTime _dp4 = new DateTime(2018, 1, 2, 0, 0, 0);
        protected static readonly DateTime _dpp = _dp1 - TimeSpan.FromDays(1);
        protected static readonly DateTime _dpf = _dp4 + TimeSpan.FromDays(1);

        protected static readonly DateTime _epp1a = new DateTime(2017, 12, 31, 12, 1, 0);
        protected static readonly DateTime _epp1b = new DateTime(2017, 12, 31, 12, 2, 0);
        protected static readonly DateTime _epp1c = new DateTime(2017, 12, 31, 12, 3, 0);
        protected static readonly DateTime _ep12a = new DateTime(2018, 1, 1, 3, 1, 0);
        protected static readonly DateTime _ep12b = new DateTime(2018, 1, 1, 3, 2, 0);
        protected static readonly DateTime _ep12c = new DateTime(2018, 1, 1, 3, 3, 0);
        protected static readonly DateTime _ep2xa = new DateTime(2018, 1, 1, 9, 1, 0);
        protected static readonly DateTime _ep2xb = new DateTime(2018, 1, 1, 9, 2, 0);
        protected static readonly DateTime _ep2xc = new DateTime(2018, 1, 1, 9, 3, 0);
        protected static readonly DateTime _epx3a = new DateTime(2018, 1, 1, 15, 1, 0);
        protected static readonly DateTime _epx3b = new DateTime(2018, 1, 1, 15, 2, 0);
        protected static readonly DateTime _epx3c = new DateTime(2018, 1, 1, 15, 3, 0);
        protected static readonly DateTime _ep34a = new DateTime(2018, 1, 1, 21, 1, 0);
        protected static readonly DateTime _ep34b = new DateTime(2018, 1, 1, 21, 2, 0);
        protected static readonly DateTime _ep34c = new DateTime(2018, 1, 1, 21, 3, 0);
        protected static readonly DateTime _ep4fa = new DateTime(2018, 1, 2, 12, 1, 0);
        protected static readonly DateTime _ep4fb = new DateTime(2018, 1, 2, 12, 2, 0);
        protected static readonly DateTime _ep4fc = new DateTime(2018, 1, 2, 12, 3, 0);

        protected static readonly DateTime[] _epp1 = new[] { _epp1a, _epp1b, _epp1c };
        protected static readonly DateTime[] _ep12 = new[] { _ep12a, _ep12b, _ep12c };
        protected static readonly DateTime[] _ep2x = new[] { _ep2xa, _ep2xb, _ep2xc };
        protected static readonly DateTime[] _epx3 = new[] { _epx3a, _epx3b, _epx3c };
        protected static readonly DateTime[] _ep34 = new[] { _ep34a, _ep34b, _ep34c };
        protected static readonly DateTime[] _ep4f = new[] { _ep4fa, _ep4fb, _ep4fc };
        protected static readonly DateTime[] _ep13 = new[] { _ep12a, _ep12b, _ep12c, _ep2xa, _ep2xb, _ep2xc, _epx3a, _epx3b, _epx3c };
        protected static readonly DateTime[] _ep23 = new[] { _ep2xa, _ep2xb, _ep2xc, _epx3a, _epx3b, _epx3c };
        protected static readonly DateTime[] _ep24 = new[] { _ep2xa, _ep2xb, _ep2xc, _epx3a, _epx3b, _epx3c, _ep34a, _ep34b, _ep34c };
        protected static readonly DateTime[] _ep1x = new[] { _ep12a, _ep12b, _ep12c, _ep2xa, _ep2xb, _ep2xc };
        protected static readonly DateTime[] _epx4 = new[] { _epx3a, _epx3b, _epx3c, _ep34a, _ep34b, _ep34c };
        protected static readonly DateTime[] _ep14 = new[] { _ep12a, _ep12b, _ep12c, _ep2xa, _ep2xb, _ep2xc, _epx3a, _epx3b, _epx3c, _ep34a, _ep34b, _ep34c };
        protected static readonly DateTime[] _epp2 = new[] { _epp1a, _epp1b, _epp1c, _ep12a, _ep12b, _ep12c };
        protected static readonly DateTime[] _epp3 = new[] { _epp1a, _epp1b, _epp1c, _ep12a, _ep12b, _ep12c, _ep2xa, _ep2xb, _ep2xc, _epx3a, _epx3b, _epx3c };
        protected static readonly DateTime[] _ep3f = new[] { _ep34a, _ep34b, _ep34c, _ep4fa, _ep4fb, _ep4fc };
        protected static readonly DateTime[] _ep2f = new[] { _ep2xa, _ep2xb, _ep2xc, _epx3a, _epx3b, _epx3c, _ep34a, _ep34b, _ep34c, _ep4fa, _ep4fb, _ep4fc };
        protected static readonly DateTime[] _eppf = new[] { _epp1a, _epp1b, _epp1c, _ep12a, _ep12b, _ep12c, _ep2xa, _ep2xb, _ep2xc, _epx3a, _epx3b, _epx3c, _ep34a, _ep34b, _ep34c, _ep4fa, _ep4fb, _ep4fc };

        protected readonly Reference[] _rpp1 = ToReference(_epp1);
        protected readonly Reference[] _rp12 = ToReference(_ep12);
        protected readonly Reference[] _rp2x = ToReference(_ep2x);
        protected readonly Reference[] _rpx3 = ToReference(_epx3);
        protected readonly Reference[] _rp34 = ToReference(_ep34);
        protected readonly Reference[] _rp4f = ToReference(_ep4f);
        protected readonly Reference[] _rp13 = ToReference(_ep13);
        protected readonly Reference[] _rp23 = ToReference(_ep23);
        protected readonly Reference[] _rp24 = ToReference(_ep24);
        protected readonly Reference[] _rp1x = ToReference(_ep1x);
        protected readonly Reference[] _rpx4 = ToReference(_epx4);
        protected readonly Reference[] _rp14 = ToReference(_ep14);
        protected readonly Reference[] _rpp2 = ToReference(_epp2);
        protected readonly Reference[] _rpp3 = ToReference(_epp3);
        protected readonly Reference[] _rp3f = ToReference(_ep3f);
        protected readonly Reference[] _rp2f = ToReference(_ep2f);
        protected readonly Reference[] _rppf = ToReference(_eppf);
#pragma warning restore CA1707 // Identifiers should not contain underscores

        protected static Reference[] ToReference(params DateTime[] values)
        {
            return values.Select(x => new Reference(x)).ToArray();
        }

        protected static void AssertTimeInterval(Interval expected, IInterval<DateTime> actual)
        {
            Assert.IsNotNull(expected, $"The expected sample: {nameof(TimeInterval)}.");
            Assert.IsNotNull(actual, $"The actual sample: {nameof(IInterval<DateTime>)}.");
            Assert.AreEqual(expected.Arg1, actual.Lower, $"The property: {nameof(TimeInterval.Lower)}.");
            Assert.AreEqual(expected.Arg2, actual.Upper, $"The property: {nameof(TimeInterval.Upper)}.");
            Assert.AreEqual(expected.Include1, actual.LowerInclude, $"The property: {nameof(TimeInterval.LowerInclude)}.");
            Assert.AreEqual(expected.Include2, actual.UpperInclude, $"The property: {nameof(TimeInterval.UpperInclude)}.");
        }

        protected static void AssertTimeInterval(IInterval<DateTime> interval, DateTime? arg1, bool include1, DateTime? arg2, bool include2)
        {
            Assert.IsNotNull(interval, $"The interface: {nameof(IInterval<DateTime>)}.");
            Assert.AreEqual(arg1, interval.Lower, $"The property: {nameof(TimeInterval.Lower)}.");
            Assert.AreEqual(arg2, interval.Upper, $"The property: {nameof(TimeInterval.Upper)}.");
            Assert.AreEqual(include1, interval.LowerInclude, $"The property: {nameof(TimeInterval.LowerInclude)}.");
            Assert.AreEqual(include2, interval.UpperInclude, $"The property: {nameof(TimeInterval.UpperInclude)}.");
        }

        protected static void AssertTimeInterval(TimeInterval interval, IntervalType type, DateTime? arg1, bool include1, DateTime? arg2, bool include2, bool isEmpty)
        {
            Assert.IsNotNull(interval, $"The interval: {nameof(TimeInterval)}.");
            Assert.AreEqual(type, interval.Type, $"The property: {nameof(TimeInterval.Type)}.");
            Assert.AreEqual(isEmpty ? TimeSpan.Zero : arg2 - arg1, interval.Span, $"The property: {nameof(TimeInterval.Span)}.");
            Assert.AreEqual(isEmpty, interval.IsEmpty, $"The property: {nameof(TimeInterval.IsEmpty)}.");
            AssertTimeInterval(interval, arg1, include1, arg2, include2);
        }

        protected static void AssertTimeInterval(TimeInterval interval, Interval reference)
        {
            Assert.IsNotNull(interval, $"The interval: {nameof(TimeInterval)}.");
            Assert.IsNotNull(reference, $"The reference sample: {nameof(TimeInterval)}.");
            Assert.AreEqual(reference.Type, interval.Type, $"The property: {nameof(TimeInterval.Type)}.");
            Assert.AreEqual(reference.IsEmpty ? TimeSpan.Zero : reference.Arg2 - reference.Arg1, interval.Span, $"The property: {nameof(TimeInterval.Span)}.");
            Assert.AreEqual(reference.IsEmpty, interval.IsEmpty, $"The property: {nameof(TimeInterval.IsEmpty)}.");
            AssertTimeInterval(interval, reference.Arg1, reference.Include1, reference.Arg2, reference.Include2);
        }

        protected static void AssertFragment(Fragment expected, TimelineStream.Fragment actual)
        {
            Assert.IsNotNull(expected, $"The expected sample: {nameof(Fragment)}.");
            Assert.IsNotNull(actual, $"The actual fragment: {nameof(TimelineStream.Fragment)}.");
            AssertTimeInterval(expected, actual);
            AssertSamples(expected.Entries?.ToArray(), actual.Get()?.ToArray());
        }

        protected static void AssertSample(ITimeReference expected, ITimeReference actual)
        {
            Assert.IsNotNull(expected, $"The reference sample: {nameof(Fragment)}.");
            Assert.IsNotNull(actual, $"The sample: {nameof(TimelineStream.Fragment)}.");
            Assert.AreEqual(expected.Sample, actual.Sample, $"The time sample: {nameof(ITimeReference.Sample)}.");
        }

        protected static void AssertSamples(ITimeReference[] expected, ITimeReference[] actual)
        {
            Assert.IsNotNull(expected, $"The expected samples: {nameof(Fragment)}.");
            Assert.IsNotNull(actual, $"The actual samples: {nameof(TimelineStream.Fragment)}.");
            Assert.AreEqual(expected.Length, actual.Length, "Entries Count");
            for (int index = 0; index < expected.Length; index++)
                AssertSample(expected[index], actual[index]);
        }

        protected static void AssertTimeIntervals(IInterval<DateTime>[] results, params Interval[] references)
        {
            Assert.IsNotNull(results, "Intervals");
            Assert.IsNotNull(references, "Expected Intervals");
            Assert.AreEqual(references.Length, results.Length, "Intervals Count");
            for (int index = 0; index < references.Length; index++)
                AssertTimeInterval(results[index], references[index].Arg1, references[index].Include1, references[index].Arg2, references[index].Include2);
        }

        protected static void AssertTimeIntervals(TimeInterval[] intervals, params Interval[] references)
        {
            Assert.IsNotNull(intervals, "Intervals");
            Assert.IsNotNull(references, "Expected Intervals");
            Assert.AreEqual(references.Length, intervals.Length, "Intervals Count");
            for (int index = 0; index < references.Length; index++)
                AssertTimeInterval(intervals[index], references[index]);
        }

        protected static void AssertFragments(Fragment[] expected, TimelineStream.Fragment[] actual)
        {
            Assert.IsNotNull(expected, "Expected Intervals");
            Assert.IsNotNull(actual, "Actual Intervals");
            Assert.AreEqual(expected.Length, actual.Length, "Intervals Count");
            for (int index = 0; index < expected.Length; index++)
                AssertFragment(expected[index], actual[index]);
        }
    }
}