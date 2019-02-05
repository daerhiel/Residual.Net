using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResidualNet.Containers.Tests.Models;
using System;

namespace ResidualNet.Timeline.Tests
{
    /// <summary>
    /// Contains the unit tests for the contiguous time series container.
    /// </summary>
    [TestClass]
    public class TimelineTests : GeneralTimeTests
    {
        private static readonly TimeInterval _inclusive = new TimeInterval(_ep12a, _ep34c, IntervalLimits.Lower | IntervalLimits.Upper);
        private static readonly TimeInterval _exclusive = new TimeInterval(_ep12a, _ep34c, IntervalLimits.None);
        private static readonly TimeInterval _missingl = new TimeInterval(_epp1a - TimeSpan.FromDays(2), _epp1a - TimeSpan.FromDays(1), IntervalLimits.Lower | IntervalLimits.Upper);
        private static readonly TimeInterval _missingr = new TimeInterval(_ep4fc + TimeSpan.FromDays(1), _ep4fc + TimeSpan.FromDays(2), IntervalLimits.Lower | IntervalLimits.Upper);

        protected readonly Reference _ref2xa = new Reference(_ep2xa);
        protected readonly Reference _ref2xb = new Reference(_ep2xb);
        protected readonly Reference _ref2xc = new Reference(_ep2xc);
        protected readonly Reference _refx3a = new Reference(_epx3a);
        protected readonly Reference _refx3b = new Reference(_epx3b);
        protected readonly Reference _refx3c = new Reference(_epx3c);

        #region [Timeline] Find Operations
        [TestMethod]
        public void Timeline_Find_Empty()
        {
            var timeline = new Timeline<Reference>();

            var entry = timeline.Find(_ep2xc);
            Assert.AreEqual(null, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_Find_BadOrigin()
        {
            var timeline = new Timeline<Reference>(new[] { _ref2xa, _ref2xb, _ref2xc, _refx3a, _refx3b, _refx3c });

            AssertError<ArgumentException>(
                () => timeline.Find(_ep2xc, new Reference(_ep2xc)),
                e => e.Message.Contains("is bound to other", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void Timeline_Find_SMT()
        {
            var timeline = new Timeline<Reference>(new[] { _ref2xa });

            var entry = timeline.Find(_ep2xa);
            Assert.AreEqual(_ep2xa, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_Find_SMS()
        {
            var timeline = new Timeline<Reference>(new[] { _ref2xa });

            var entry = timeline.Find(_ep2xc);
            Assert.AreEqual(null, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_Find_LON()
        {
            var timeline = new Timeline<Reference>(new[] { _ref2xa, _ref2xb, _ref2xc, _refx3a, _refx3b, _refx3c });

            var entry = timeline.Find(_ep2xc, _ref2xc);
            Assert.AreEqual(_ep2xc, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_Find_RON()
        {
            var timeline = new Timeline<Reference>(new[] { _ref2xa, _ref2xb, _ref2xc, _refx3a, _refx3b, _refx3c });

            var entry = timeline.Find(_epx3a, _refx3a);
            Assert.AreEqual(_epx3a, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_Find_LMT()
        {
            var timeline = new Timeline<Reference>(new[] { _ref2xa, _ref2xb, _ref2xc, _refx3a, _refx3b, _refx3c });

            var entry = timeline.Find(_ep2xb, _refx3b);
            Assert.AreEqual(_ep2xb, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_Find_LMS()
        {
            var timeline = new Timeline<Reference>(new[] { _ref2xa, _ref2xb, _ref2xc, _refx3a, _refx3b, _refx3c });

            var entry = timeline.Find(_ep2xb - TimeSpan.FromSeconds(10), _refx3b);
            Assert.AreEqual(null, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_Find_RMT()
        {
            var timeline = new Timeline<Reference>(new[] { _ref2xa, _ref2xb, _ref2xc, _refx3a, _refx3b, _refx3c });

            var entry = timeline.Find(_epx3b, _ref2xb);
            Assert.AreEqual(_epx3b, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_Find_RMS()
        {
            var timeline = new Timeline<Reference>(new[] { _ref2xa, _ref2xb, _ref2xc, _refx3a, _refx3b, _refx3c });

            var entry = timeline.Find(_epx3b + TimeSpan.FromSeconds(10), _ref2xb);
            Assert.AreEqual(null, entry?.Sample);
        }
        #endregion

        #region [Timeline] Find Boundaries Operations
        [TestMethod]
        public void Timeline_FindLower_Empty()
        {
            var timeline = new Timeline<Reference>();

            var entry = timeline.FindLower(_inclusive, timeline.Find(_ep12a));
            Assert.AreEqual(null, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindLower_BadOrigin()
        {
            var timeline = new Timeline<Reference>(_rppf);

            AssertError<ArgumentException>(
                () => timeline.FindLower(_inclusive, new Reference(_ep2xc)),
                e => e.Message.Contains("is bound to other", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void Timeline_FindLower_Inclusive_LON()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindLower(_inclusive, timeline.Find(_ep12a));
            Assert.AreEqual(_ep12a, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindLower_Inclusive_LIN()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindLower(_inclusive, timeline.Find(_ep2xa));
            Assert.AreEqual(_ep12a, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindLower_Inclusive_LOUT()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindLower(_inclusive, timeline.Find(_epp1a));
            Assert.AreEqual(_ep12a, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindLower_Exclusive_LON()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindLower(_exclusive, timeline.Find(_ep12a));
            Assert.AreEqual(_ep12b, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindLower_Exclusive_LIN()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindLower(_exclusive, timeline.Find(_ep2xa));
            Assert.AreEqual(_ep12b, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindLower_Exclusive_LOUT()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindLower(_exclusive, timeline.Find(_epp1a));
            Assert.AreEqual(_ep12b, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindUpper_Empty()
        {
            var timeline = new Timeline<Reference>();

            var entry = timeline.FindUpper(_inclusive, timeline.Find(_ep34c));
            Assert.AreEqual(null, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindUpper_BadOrigin()
        {
            var timeline = new Timeline<Reference>(_rppf);

            AssertError<ArgumentException>(
                () => timeline.FindUpper(_inclusive, new Reference(_ep2xc)),
                e => e.Message.Contains("is bound to other", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void Timeline_FindUpper_Inclusive_LON()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindUpper(_inclusive, timeline.Find(_ep34c));
            Assert.AreEqual(_ep34c, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindUpper_Inclusive_LIN()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindUpper(_inclusive, timeline.Find(_ep2xc));
            Assert.AreEqual(_ep34c, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindUpper_Inclusive_LOUT()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindUpper(_inclusive, timeline.Find(_ep4fc));
            Assert.AreEqual(_ep34c, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindUpper_Exclusive_LON()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindUpper(_exclusive, timeline.Find(_ep34c));
            Assert.AreEqual(_ep34b, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindUpper_Exclusive_LIN()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindUpper(_exclusive, timeline.Find(_ep2xc));
            Assert.AreEqual(_ep34b, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindUpper_Exclusive_LOUT()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindUpper(_exclusive, timeline.Find(_ep4fc));
            Assert.AreEqual(_ep34b, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindLower_LOUTL()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindLower(_missingl, timeline.Find(_epp1a));
            Assert.AreEqual(null, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindUpper_LOUTL()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindUpper(_missingl, timeline.Find(_ep4fc));
            Assert.AreEqual(null, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindLower_LOUTR()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindLower(_missingr, timeline.Find(_epp1a));
            Assert.AreEqual(null, entry?.Sample);
        }

        [TestMethod]
        public void Timeline_FindUpper_LOUTR()
        {
            var timeline = new Timeline<Reference>(_rppf);

            var entry = timeline.FindUpper(_missingr, timeline.Find(_ep4fc));
            Assert.AreEqual(null, entry?.Sample);
        }
        #endregion
    }
#pragma warning restore CA1707 // Identifiers should not contain underscores
}