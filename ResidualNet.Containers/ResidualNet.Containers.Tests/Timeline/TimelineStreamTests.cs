using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResidualNet.Containers.Tests.Models;
using System;
using System.Linq;

namespace ResidualNet.Timeline.Tests
{
#pragma warning disable CA1707 // Identifiers should not contain underscores
    /// <summary>
    /// Contains the unit tests for the time series stream block container.
    /// </summary>
    [TestClass]
    public class TimelineStreamTests : GeneralTimeTests
    {
        #region [TimelineStream] Get Segments Operations
        /// <summary>
        /// Tests getting the segments from empty stream blocks.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_Empty()
        {
            var target = new[] { new Interval { Arg1 = _dpx, Include1 = true, Arg2 = _dp4, Include2 = true } };
            var segments = new TimelineStream();
            var result = segments.Get(_dpx, _dp4);
            AssertTimeIntervals(result, target);
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with same block.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_SXB()
        {
            var source = new[] { new Fragment { Arg1 = _dp2, Arg2 = _dp3, Entries = _rp23 } };
            var segments = new TimelineStream(source);
            var result = segments.Get(_dp2, _dp3);
            AssertTimeIntervals(result);
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with right adjacency.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_RAB()
        {
            var source = new[] { new Fragment { Arg1 = _dpx, Arg2 = _dp4, Entries = _rpx4 } };
            var target = new[] { new Interval { Arg1 = _dp1, Include1 = true, Arg2 = _dpx, Include2 = false } };
            var segments = new TimelineStream(source);
            var result = segments.Get(_dp1, _dpx);
            AssertTimeIntervals(result, target);
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with left adjacency.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_LAB()
        {
            var source = new[] { new Fragment { Arg1 = _dp1, Arg2 = _dpx, Entries = _rp1x } };
            var target = new[] { new Interval { Arg1 = _dpx, Include1 = false, Arg2 = _dp4, Include2 = true } };
            var segments = new TimelineStream(source);
            var result = segments.Get(_dpx, _dp4);
            AssertTimeIntervals(result, target);
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with right overlapping.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_RXB()
        {
            var source = new[] { new Fragment { Arg1 = _dp2, Arg2 = _dp4, Entries = _rp24 } };
            var target = new[] { new Interval { Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false } };
            var segments = new TimelineStream(source);
            var result = segments.Get(_dp1, _dp3);
            AssertTimeIntervals(result, target);
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with left overlapping.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_LXB()
        {
            var source = new[] { new Fragment { Arg1 = _dp1, Arg2 = _dp3, Entries = _rp13 } };
            var target = new[] { new Interval { Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true } };
            var segments = new TimelineStream(source);
            var result = segments.Get(_dp2, _dp4);
            AssertTimeIntervals(result, target);
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with same crossing.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_IS()
        {
            var source = new[] { new Fragment { Arg1 = _dp1, Arg2 = _dp4, Entries = _rp14 } };
            var segments = new TimelineStream(source);
            var result = segments.Get(_dp1, _dp4);
            AssertTimeIntervals(result);
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with outside crossing.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_ON()
        {
            var source = new[] { new Fragment { Arg1 = _dp1, Arg2 = _dp4, Entries = _rp14 } };
            var segments = new TimelineStream(source);
            var result = segments.Get(_dp2, _dp3);
            AssertTimeIntervals(result);
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with inside crossing.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_IN()
        {
            var source = new[] { new Fragment { Arg1 = _dp2, Arg2 = _dp3, Entries = _rp23 } };
            var target = new[]
            {
                new Interval { Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false },
                new Interval { Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true }
            };
            var segments = new TimelineStream(source);
            var result = segments.Get(_dp1, _dp4);
            AssertTimeIntervals(result, target);
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with Reference left-right overlapping.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_LRXB()
        {
            var source = new[]
            {
                new Fragment { Arg1 = _dpp, Arg2 = _dp2, Entries = _rpp2 },
                new Fragment { Arg1 = _dp3, Arg2 = _dpf, Entries = _rp3f }
            };
            var target = new[] { new Interval { Arg1 = _dp2, Include1 = false, Arg2 = _dp3, Include2 = false } };
            var segments = new TimelineStream(source);
            var result = segments.Get(_dp1, _dp4);
            AssertTimeIntervals(result, target);
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with Reference left-right overlapping.
        /// </summary>
        [TestMethod]
        public void TimelineStream_GetIntervals_LRIN()
        {
            var source = new[]
            {
                new Fragment { Arg1 = _dp1, Arg2 = _dp2, Entries = _rp12 },
                new Fragment { Arg1 = _dp3, Arg2 = _dp4, Entries = _rp34 }
            };
            var target = new[]
            {
                new Interval { Arg1 = _dpp, Include1 = true, Arg2 = _dp1, Include2 = false },
                new Interval { Arg1 = _dp2, Include1 = false, Arg2 = _dp3, Include2 = false },
                new Interval { Arg1 = _dp4, Include1 = false, Arg2 = _dpf, Include2 = true }
            };
            var segments = new TimelineStream(source);
            var result = segments.Get(_dpp, _dpf);
            AssertTimeIntervals(result, target);
        }
        #endregion

        #region [TimelineStream] Set Segments Operations
        /// <summary>
        /// Tests getting the segments from empty stream blocks.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_Empty()
        {
            var target = new[] { new Fragment { Arg1 = _dpx, Include1 = true, Arg2 = _dp4, Include2 = true, Entries = _rpx4 } };
            var segments = new TimelineStream();
            var result = segments.Set(_rpx4, _dpx, _dp4);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with same block.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_SXB()
        {
            var source = new[] { new Fragment { Arg1 = _dp1, Arg2 = _dp4, Entries = _rp14 } };
            var target = new[] { new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, Entries = _rp14 } };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp14, _dp1, _dp4);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with right adjacency.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_RAB()
        {
            var source = new[] { new Fragment { Arg1 = _dpx, Arg2 = _dp4, Entries = _rpx4 } };
            var target = new[]
            {
                new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dpx, Include2 = true, Entries = _rp1x },
                new Fragment { Arg1 = _dpx, Include1 = false, Arg2 = _dp4, Include2 = true, Entries = _rpx4 }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp1x, _dp1, _dpx);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with left adjacency.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_LAB()
        {
            var source = new[] { new Fragment { Arg1 = _dp1, Arg2 = _dpx, Entries = _rp1x } };
            var target = new[]
            {
                new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dpx, Include2 = false, Entries = _rp1x },
                new Fragment { Arg1 = _dpx, Include1 = true, Arg2 = _dp4, Include2 = true, Entries = _rpx4 }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rpx4, _dpx, _dp4);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with right overlapping.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_RXB()
        {
            var source = new[] { new Fragment { Arg1 = _dp2, Arg2 = _dp4, Entries = _rp24 } };
            var target = new[]
            {
                new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dp3, Include2 = true, Entries = _rp13 },
                new Fragment { Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true, Entries = _rp34 }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp13, _dp1, _dp3);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with left overlapping.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_LXB()
        {
            var source = new[] { new Fragment { Arg1 = _dp1, Arg2 = _dp3, Entries = _rp13 } };
            var target = new[]
            {
                new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false, Entries = _rp12 },
                new Fragment { Arg1 = _dp2, Include1 = true, Arg2 = _dp4, Include2 = true, Entries = _rp24 }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp24, _dp2, _dp4);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with outside crossing.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_ON()
        {
            var source = new[] { new Fragment { Arg1 = _dp1, Arg2 = _dp4, Entries = _rp14 } };
            var target = new[]
            {
                new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false, Entries = _rp12 },
                new Fragment { Arg1 = _dp2, Include1 = true, Arg2 = _dp3, Include2 = true, Entries = _rp23 },
                new Fragment { Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true, Entries = _rp34 }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp23, _dp2, _dp3);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with outside crossing and left adjacency.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_ONLA()
        {
            var source = new[] { new Fragment { Arg1 = _dp1, Arg2 = _dp4, Entries = _rp14 } };
            var target = new[]
            {
                new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dp3, Include2 = true, Entries = _rp13 },
                new Fragment { Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true, Entries = _rp34 }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp13, _dp1, _dp3);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with outside crossing and right adjacency.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_ONRA()
        {
            var source = new[] { new Fragment { Arg1 = _dp1, Arg2 = _dp4, Entries = _rp14 } };
            var target = new[]
            {
                new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false, Entries = _rp12 },
                new Fragment { Arg1 = _dp2, Include1 = true, Arg2 = _dp4, Include2 = true, Entries = _rp24 }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp24, _dp2, _dp4);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with inside crossing.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_IN()
        {
            var source = new[] { new Fragment { Arg1 = _dp2, Arg2 = _dp3, Entries = _rp23 } };
            var target = new[] { new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, Entries = _rp14 } };
            var segments = new TimelineStream();
            var result = segments.Set(_rp14, _dp1, _dp4);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with Reference left-right overlapping.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_LRXB()
        {
            var source = new[]
            {
                new Fragment { Arg1 = _dpp, Arg2 = _dp2, Entries = _rpp2 },
                new Fragment { Arg1 = _dp3, Arg2 = _dpf, Entries = _rp3f }
            };
            var target = new[]
            {
                new Fragment { Arg1 = _dpp, Include1 = true, Arg2 = _dp1, Include2 = false, Entries = _rpp1 },
                new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, Entries = _rp14 },
                new Fragment { Arg1 = _dp4, Include1 = false, Arg2 = _dpf, Include2 = true, Entries = _rp4f }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp14, _dp1, _dp4);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with Reference left-right overlapping.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_LRIN()
        {
            var source = new[]
            {
                new Fragment { Arg1 = _dp1, Arg2 = _dp2, Entries = _rp12 },
                new Fragment { Arg1 = _dp3, Arg2 = _dp4, Entries = _rp34 }
            };
            var target = new[] { new Fragment { Arg1 = _dpp, Include1 = true, Arg2 = _dpf, Include2 = true, Entries = _rppf } };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rppf, _dpp, _dpf);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with left-right adjacency and inside crossing.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_LRON()
        {
            var source = new[]
            {
                new Fragment { Arg1 = _dpp, Arg2 = _dp1, Entries = _rpp1 },
                new Fragment { Arg1 = _dp4, Arg2 = _dpf, Entries = _rp4f },
                new Fragment { Arg1 = _dp1, Arg2 = _dp4, Entries = _rp14 }
            };
            var target = new[]
            {
                new Fragment { Arg1 = _dpp, Include1 = true, Arg2 = _dp1, Include2 = false, Entries = _rpp1 },
                new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false, Entries = _rp12 },
                new Fragment { Arg1 = _dp2, Include1 = true, Arg2 = _dp3, Include2 = true, Entries = _rp23 },
                new Fragment { Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true, Entries = _rp34 },
                new Fragment { Arg1 = _dp4, Include1 = false, Arg2 = _dpf, Include2 = true, Entries = _rp4f }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp23, _dp2, _dp3);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with partial left merging/splitting.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_LPMS()
        {
            var source = new[]
            {
                new Fragment { Arg1 = _dp1, Arg2 = _dp2, Entries = _rp12 },
                new Fragment { Arg1 = _dp2, Arg2 = _dpx, Entries = _rp2x },
                new Fragment { Arg1 = _dpx, Arg2 = _dp4, Entries = _rpx4 },
                new Fragment { Arg1 = _dp4, Arg2 = _dpf, Entries = _rp4f }
            };
            var target = new[]
            {
                new Fragment { Arg1 = _dpp, Include1 = true, Arg2 = _dp3, Include2 = true, Entries = _rpp3 },
                new Fragment { Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = false, Entries = _rp34 },
                new Fragment { Arg1 = _dp4, Include1 = true, Arg2 = _dpf, Include2 = true, Entries = _rp4f }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rpp3, _dpp, _dp3);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests getting the segments from stream blocks with partial left merging/splitting.
        /// </summary>
        [TestMethod]
        public void TimelineStream_SetSegment_RPMS()
        {
            var source = new[]
            {
                new Fragment { Arg1 = _dpp, Arg2 = _dp1, Entries = _rpp1 },
                new Fragment { Arg1 = _dp1, Arg2 = _dpx, Entries = _rp1x },
                new Fragment { Arg1 = _dpx, Arg2 = _dp3, Entries = _rpx3 },
                new Fragment { Arg1 = _dp3, Arg2 = _dp4, Entries = _rp34 }
            };
            var target = new[]
            {
                new Fragment { Arg1 = _dpp, Include1 = true, Arg2 = _dp1, Include2 = false, Entries = _rpp1 },
                new Fragment { Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false, Entries = _rp12 },
                new Fragment { Arg1 = _dp2, Include1 = true, Arg2 = _dpf, Include2 = true, Entries = _rp2f }
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp2f, _dp2, _dpf);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests clearing the segments for the complex stream configuration.
        /// </summary>
        [TestMethod]
        public void TimelineStream_Clear()
        {
            var source = new[]
            {
                new Fragment { Arg1 = _dpp, Arg2 = _dp1, Entries = _rpp1 },
                new Fragment { Arg1 = _dp4, Arg2 = _dpf, Entries = _rp4f },
                new Fragment { Arg1 = _dp1, Arg2 = _dp4, Entries = _rp14 }
            };
            var target = new Fragment[]
            {
            };
            var segments = new TimelineStream(source);
            var result = segments.Set(_rp23, _dp2, _dp3);
            segments.Clear();
            AssertFragments(target, segments.GetItems());
        }
        #endregion

        #region [TimelineStream] Receiver Stream
        [TestMethod]
        public void TimelineStream_Receiver_SetSegmentation()
        {
            var receiveSpan = TimeSpan.FromMinutes(15);
            var segmentSpan = TimeSpan.FromMinutes(30);
            var now = DateTime.UtcNow - receiveSpan;
            var segments = new TimelineStream(receiveSpan, segmentSpan);
            var target = new[]
            {
                new Fragment
                {
                    Arg1 = now.RoundLower(1), Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan, Include2 = false,
                    Entries = ToReference(Enumerable.Range(0, 30).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan, Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan * 2, Include2 = false,
                    Entries = ToReference(Enumerable.Range(30, 30).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan * 2, Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include2 = false,
                    Entries = ToReference(Enumerable.Range(60, 15).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include1 = true,
                    Arg2 = null, Include2 = true,
                    Entries = ToReference(Enumerable.Range(75, 15).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                }
            };

            for (var i = 0; i < 90; i++)
                segments.Create(now + TimeSpan.FromMinutes(i));
            AssertFragments(target, segments.GetItems());
        }

        [TestMethod]
        public void TimelineStream_Receiver_PushSegmentation()
        {
            var receiveSpan = TimeSpan.FromMinutes(15);
            var segmentSpan = TimeSpan.FromMinutes(30);
            var now = DateTime.UtcNow - receiveSpan;
            var segments = new TimelineStream(receiveSpan, segmentSpan);
            var target = new[]
            {
                new Fragment
                {
                    Arg1 = now.RoundLower(1), Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan, Include2 = false,
                    Entries = ToReference(Enumerable.Range(0, 30).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan, Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan * 2, Include2 = false,
                    Entries = ToReference(Enumerable.Range(30, 30).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan * 2, Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include2 = false,
                    Entries = ToReference(Enumerable.Range(60, 15).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include1 = true,
                    Arg2 = null, Include2 = true,
                    Entries = ToReference(Enumerable.Range(75, 15).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                }
            };

            segments.Push(ToReference(Enumerable.Range(0, 90).Select(x => now + TimeSpan.FromMinutes(x)).ToArray()));
            AssertFragments(target, segments.GetItems());
        }

        [TestMethod]
        public void TimelineStream_Receiver_SetSegment_LX()
        {
            var receiveSpan = TimeSpan.FromMinutes(15);
            var segmentSpan = TimeSpan.FromMinutes(30);
            var now = DateTime.UtcNow - receiveSpan;
            var segments = new TimelineStream(receiveSpan, segmentSpan);
            var target = new[]
            {
                new Fragment
                {
                    Arg1 = now.RoundLower(1), Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include2 = false,
                    Entries = ToReference(Enumerable.Range(0, 75).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include1 = true,
                    Arg2 = null, Include2 = true,
                    Entries = ToReference(Enumerable.Range(75, 15).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                }
            };

            for (var i = 0; i < 90; i++)
                segments.Create(now + TimeSpan.FromMinutes(i));
            var result = segments.Set(Array.Empty<Reference>(), now.RoundLower(1), now.RoundLower(1) + TimeSpan.FromDays(1));
            AssertFragments(target, segments.GetItems());
            AssertSamples(result.ToArray(), ToReference(Enumerable.Range(0, 90).Select(x => now + TimeSpan.FromMinutes(x)).ToArray()));
        }

        [TestMethod]
        public void TimelineStream_Receiver_SetSegment_IN()
        {
            var receiveSpan = TimeSpan.FromMinutes(15);
            var segmentSpan = TimeSpan.FromMinutes(30);
            var now = DateTime.UtcNow - receiveSpan;
            var segments = new TimelineStream(receiveSpan, segmentSpan);
            var target = new[]
            {
                new Fragment
                {
                    Arg1 = now.RoundLower(1), Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan, Include2 = false,
                    Entries = ToReference(Enumerable.Range(0, 30).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan, Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan * 2, Include2 = false,
                    Entries = ToReference(Enumerable.Range(30, 30).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan * 2, Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include2 = false,
                    Entries = ToReference(Enumerable.Range(60, 15).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include1 = true,
                    Arg2 = null, Include2 = true,
                    Entries = ToReference(Enumerable.Range(75, 15).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                }
            };

            for (var i = 0; i < 90; i++)
                segments.Create(now + TimeSpan.FromMinutes(i));
            var result = segments.Set(Array.Empty<Reference>(), now.RoundLower(1) + TimeSpan.FromMinutes(80), now.RoundLower(1) + TimeSpan.FromDays(1));
            AssertFragments(target, segments.GetItems());
            AssertSamples(result.ToArray(), ToReference(Enumerable.Range(80, 10).Select(x => now + TimeSpan.FromMinutes(x)).ToArray()));
        }

        [TestMethod]
        public void TimelineStream_Receiver_Cleanup()
        {
            var receiveSpan = TimeSpan.FromMinutes(15);
            var segmentSpan = TimeSpan.FromMinutes(30);
            var now = DateTime.UtcNow - receiveSpan;
            var segments = new TimelineStream(receiveSpan, segmentSpan);
            var target = new[]
            {
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan * 2, Include1 = true,
                    Arg2 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include2 = false,
                    Entries = ToReference(Enumerable.Range(60, 15).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                },
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include1 = true,
                    Arg2 = null, Include2 = true,
                    Entries = ToReference(Enumerable.Range(75, 15).Select(x => now + TimeSpan.FromMinutes(x)).ToArray())
                }
            };

            for (var i = 0; i < 90; i++)
                segments.Create(now + TimeSpan.FromMinutes(i));
            segments.CleanupExpired(segments.GetItems().Skip(1).Select(x => x.ExpiresOn).First().Value);
            AssertFragments(target, segments.GetItems());
        }

        /// <summary>
        /// Tests clearing the segments for the complex stream configuration with receiver.
        /// </summary>
        [TestMethod]
        public void TimelineStream_Receiver_Clear()
        {
            var receiveSpan = TimeSpan.FromMinutes(15);
            var segmentSpan = TimeSpan.FromMinutes(30);
            var now = DateTime.UtcNow - receiveSpan;
            var segments = new TimelineStream(receiveSpan, segmentSpan);
            var target = new[]
            {
                new Fragment
                {
                    Arg1 = now.RoundLower(1) + segmentSpan * 2 + receiveSpan, Include1 = true,
                    Arg2 = null, Include2 = true,
                    Entries = Array.Empty<Reference>().ToArray()
                }
            };

            for (var i = 0; i < 90; i++)
                segments.Create(now + TimeSpan.FromMinutes(i));
            segments.Clear();
            AssertFragments(target, segments.GetItems());
        }
        #endregion
    }
#pragma warning restore CA1707 // Identifiers should not contain underscores
}