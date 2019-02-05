using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResidualNet.Containers.Tests.Models;
using System;
using System.Linq;

namespace ResidualNet.Timeline.Tests
{
#pragma warning disable CA1707 // Identifiers should not contain underscores
    /// <summary>
    /// Contains the unit tests for the time fragments.
    /// </summary>
    [TestClass]
    public partial class TimeIntervalTests : GeneralTimeTests
    {
        protected static T[] Build<T>(T arg1, T arg2, Func<T, T, T[]> selector)
        {
            return selector?.Invoke(arg1, arg2);
        }

        protected static T[] Build<T>(params T[] values)
            where T: class
        {
            return values.Where(x => !(x is null)).ToArray();
        }

        #region [TimeInterval] IsWithin for Closed Range
        /// <summary>
        /// The range is closed, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_OL_CArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, _dp3).IsWithin(_dp1));
        }

        /// <summary>
        /// The range is closed, outside right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_OR_CArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, _dp3).IsWithin(_dp4));
        }

        /// <summary>
        /// The range is closed, on boundary left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_BL_CArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).IsWithin(_dp2));
        }

        /// <summary>
        /// The range is closed, on boundary right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_BR_CArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).IsWithin(_dp3));
        }

        /// <summary>
        /// The range is closed, inside
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_IN_CArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).IsWithin(_dpx));
        }
        #endregion

        #region [TimeInterval] IsWithin for Right-Open Range
        /// <summary>
        /// The range is right-open, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_OL_ORArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, (DateTime?)null).IsWithin(_dp1));
        }

        /// <summary>
        /// The range is right-open, inside right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_IR_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithin(_dp4));
        }

        /// <summary>
        /// The range is right-open, on boundary left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_BL_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithin(_dp2));
        }

        /// <summary>
        /// The range is right-open, on boundary right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_BR_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithin(_dp3));
        }

        /// <summary>
        /// The range is right-open, inside
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_IN_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithin(_dpx));
        }
        #endregion

        #region [TimeInterval] IsWithin for Left-Open Range
        /// <summary>
        /// The range is left-open, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_IL_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithin(_dp1));
        }

        /// <summary>
        /// The range is left-open, outside right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_OR_OLArg()
        {
            Assert.IsFalse(new TimeInterval(null, _dp3).IsWithin(_dp4));
        }

        /// <summary>
        /// The range is left-open, on boundary left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_BL_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithin(_dp2));
        }

        /// <summary>
        /// The range is left-open, on boundary right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_BR_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithin(_dp3));
        }

        /// <summary>
        /// The range is left-open, inside
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithin_IN_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithin(_dpx));
        }
        #endregion

        #region [TimeInterval] IsWithinLower for Closed Range
        /// <summary>
        /// The range is closed, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_OL_COArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, _dp3).IsWithinLower(null));
        }

        /// <summary>
        /// The range is closed, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_OL_CArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, _dp3).IsWithinLower(_dp1));
        }

        /// <summary>
        /// The range is closed, outside right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_OR_CArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, _dp3).IsWithinLower(_dp4));
        }

        /// <summary>
        /// The range is closed, on boundary left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_BL_CArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).IsWithinLower(_dp2));
        }

        /// <summary>
        /// The range is closed, on boundary right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_BR_CArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).IsWithinLower(_dp3));
        }

        /// <summary>
        /// The range is closed, inside
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_IN_CArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).IsWithinLower(_dpx));
        }
        #endregion

        #region [TimeInterval] IsWithinLower for Right-Open Range
        /// <summary>
        /// The range is right-open, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_OL_ORCArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, (DateTime?)null).IsWithinLower(null));
        }

        /// <summary>
        /// The range is right-open, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_OL_ORArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, (DateTime?)null).IsWithinLower(_dp1));
        }

        /// <summary>
        /// The range is right-open, inside right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_IR_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithinLower(_dp4));
        }

        /// <summary>
        /// The range is right-open, on boundary left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_BL_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithinLower(_dp2));
        }

        /// <summary>
        /// The range is right-open, on boundary right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_BR_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithinLower(_dp3));
        }

        /// <summary>
        /// The range is right-open, inside
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_IN_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithinLower(_dpx));
        }
        #endregion

        #region [TimeInterval] IsWithinLower for Left-Open Range
        /// <summary>
        /// The range is left-open, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_IL_OLCArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithinLower(null));
        }

        /// <summary>
        /// The range is left-open, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_IL_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithinLower(_dp1));
        }

        /// <summary>
        /// The range is left-open, outside right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_OR_OLArg()
        {
            Assert.IsFalse(new TimeInterval(null, _dp3).IsWithinLower(_dp4));
        }

        /// <summary>
        /// The range is left-open, on boundary left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_BL_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithinLower(_dp2));
        }

        /// <summary>
        /// The range is left-open, on boundary right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_BR_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithinLower(_dp3));
        }

        /// <summary>
        /// The range is left-open, inside
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinLower_IN_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithinLower(_dpx));
        }
        #endregion

        #region [TimeInterval] IsWithinUpper for Closed Range
        /// <summary>
        /// The range is closed, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_OL_COArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, _dp3).IsWithinUpper(null));
        }

        /// <summary>
        /// The range is closed, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_OL_CArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, _dp3).IsWithinUpper(_dp1));
        }

        /// <summary>
        /// The range is closed, outside right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_OR_CArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, _dp3).IsWithinUpper(_dp4));
        }

        /// <summary>
        /// The range is closed, on boundary left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_BL_CArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).IsWithinUpper(_dp2));
        }

        /// <summary>
        /// The range is closed, on boundary right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_BR_CArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).IsWithinUpper(_dp3));
        }

        /// <summary>
        /// The range is closed, inside
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_IN_CArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).IsWithinUpper(_dpx));
        }
        #endregion

        #region [TimeInterval] IsWithinUpper for Right-Open Range
        /// <summary>
        /// The range is right-open, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_OL_ORCArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithinUpper(null));
        }

        /// <summary>
        /// The range is right-open, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_OL_ORArg()
        {
            Assert.IsFalse(new TimeInterval(_dp2, (DateTime?)null).IsWithinUpper(_dp1));
        }

        /// <summary>
        /// The range is right-open, inside right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_IR_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithinUpper(_dp4));
        }

        /// <summary>
        /// The range is right-open, on boundary left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_BL_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithinUpper(_dp2));
        }

        /// <summary>
        /// The range is right-open, on boundary right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_BR_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithinUpper(_dp3));
        }

        /// <summary>
        /// The range is right-open, inside
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_IN_ORArg()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).IsWithinUpper(_dpx));
        }
        #endregion

        #region [TimeInterval] IsWithinUpper for Left-Open Range
        /// <summary>
        /// The range is left-open, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_IL_OLCArg()
        {
            Assert.IsFalse(new TimeInterval(null, _dp3).IsWithinUpper(null));
        }

        /// <summary>
        /// The range is left-open, outside left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_IL_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithinUpper(_dp1));
        }

        /// <summary>
        /// The range is left-open, outside right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_OR_OLArg()
        {
            Assert.IsFalse(new TimeInterval(null, _dp3).IsWithinUpper(_dp4));
        }

        /// <summary>
        /// The range is left-open, on boundary left
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_BL_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithinUpper(_dp2));
        }

        /// <summary>
        /// The range is left-open, on boundary right
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_BR_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithinUpper(_dp3));
        }

        /// <summary>
        /// The range is left-open, inside
        /// </summary>
        [TestMethod]
        public void TimeInterval_IsWithinUpper_IN_OLArg()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).IsWithinUpper(_dpx));
        }
        #endregion

        #region [TimeInterval] Overlaps for Closed Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_IN_CArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, _dp4).Overlaps(new TimeInterval(_dp2, _dp3)));
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_ON_CArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).Overlaps(new TimeInterval(_dp1, _dp4)));
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LX_CArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, _dp3).Overlaps(new TimeInterval(_dp2, _dp4)));
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RX_CArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp4).Overlaps(new TimeInterval(_dp1, _dp3)));
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_OLA_CArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, _dpx).Overlaps(new TimeInterval(_dpx, _dp4)));
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_ORA_CArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dpx, _dp4).Overlaps(new TimeInterval(_dp1, _dpx)));
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LIRA_CArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, _dp4).Overlaps(new TimeInterval(_dp2, _dp4)));
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RIRA_CArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp4).Overlaps(new TimeInterval(_dp1, _dp4)));
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LILA_CArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, _dp4).Overlaps(new TimeInterval(_dp1, _dp3)));
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RILA_CArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, _dp3).Overlaps(new TimeInterval(_dp1, _dp4)));
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LNX_CArg1_CArg2()
        {
            Assert.IsFalse(new TimeInterval(_dp1, _dp2).Overlaps(new TimeInterval(_dp3, _dp4)));
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RNX_CArg1_CArg2()
        {
            Assert.IsFalse(new TimeInterval(_dp3, _dp4).Overlaps(new TimeInterval(_dp1, _dp2)));
        }
        #endregion

        #region [TimeInterval] Overlaps for Right-Open and Closed Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_IN_ORArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, (DateTime?)null).Overlaps(new TimeInterval(_dp2, _dp3)));
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_ON_CArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).Overlaps(new TimeInterval(_dp1, (DateTime?)null)));
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LX_CArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, _dp3).Overlaps(new TimeInterval(_dp2, (DateTime?)null)));
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RX_ORArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).Overlaps(new TimeInterval(_dp1, _dp3)));
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_OLA_CArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, _dpx).Overlaps(new TimeInterval(_dpx, (DateTime?)null)));
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_ORA_ORArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dpx, (DateTime?)null).Overlaps(new TimeInterval(_dp1, _dpx)));
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LILA_CArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, _dp4).Overlaps(new TimeInterval(_dp1, (DateTime?)null)));
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LIRA_ORArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, (DateTime?)null).Overlaps(new TimeInterval(_dp1, _dp4)));
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LNX_CArg1_ORArg2()
        {
            Assert.IsFalse(new TimeInterval(_dp1, _dp2).Overlaps(new TimeInterval(_dp3, (DateTime?)null)));
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RNX_ORArg1_CArg2()
        {
            Assert.IsFalse(new TimeInterval(_dp3, (DateTime?)null).Overlaps(new TimeInterval(_dp1, _dp2)));
        }
        #endregion

        #region [TimeInterval] Overlaps for Closed and Left-Open Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_IN_OLArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dp4).Overlaps(new TimeInterval(_dp2, _dp3)));
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_ON_CArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp3).Overlaps(new TimeInterval(null, _dp4)));
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LX_OLArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).Overlaps(new TimeInterval(_dp2, _dp4)));
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RX_CArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp2, _dp4).Overlaps(new TimeInterval(null, _dp3)));
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_OLA_OLArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dpx).Overlaps(new TimeInterval(_dpx, _dp4)));
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_ORA_CArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval(_dpx, _dp4).Overlaps(new TimeInterval(null, _dpx)));
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RILA_OLArg1_CArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dp4).Overlaps(new TimeInterval(_dp1, _dp4)));
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RIRA_CArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, _dp4).Overlaps(new TimeInterval(null, _dp4)));
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LNX_OLArg1_CArg2()
        {
            Assert.IsFalse(new TimeInterval(null, _dp2).Overlaps(new TimeInterval(_dp3, _dp4)));
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RNX_CArg1_OLArg2()
        {
            Assert.IsFalse(new TimeInterval(_dp3, _dp4).Overlaps(new TimeInterval(null, _dp2)));
        }
        #endregion

        #region [TimeInterval] Overlaps for Left-Open and Right-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LX_OLArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).Overlaps(new TimeInterval(_dp2, (DateTime?)null)));
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RX_ORArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).Overlaps(new TimeInterval(null, _dp3)));
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_OLA_OLArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dpx).Overlaps(new TimeInterval(_dpx, (DateTime?)null)));
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_ORA_ORArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval(_dpx, (DateTime?)null).Overlaps(new TimeInterval(null, _dpx)));
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LNX_OLArg1_ORArg2()
        {
            Assert.IsFalse(new TimeInterval(null, _dp2).Overlaps(new TimeInterval(_dp3, (DateTime?)null)));
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RNX_ORArg1_OLArg2()
        {
            Assert.IsFalse(new TimeInterval(_dp3, (DateTime?)null).Overlaps(new TimeInterval(null, _dp2)));
        }
        #endregion

        #region [TimeInterval] Overlaps for Right-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LX_ORArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp2, (DateTime?)null).Overlaps(new TimeInterval(_dp3, (DateTime?)null)));
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RX_ORArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp3, (DateTime?)null).Overlaps(new TimeInterval(_dp2, (DateTime?)null)));
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LILA_ORArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, (DateTime?)null).Overlaps(new TimeInterval(_dp1, (DateTime?)null)));
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RILA_ORArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp1, (DateTime?)null).Overlaps(new TimeInterval(_dp1, (DateTime?)null)));
        }
        #endregion

        #region [TimeInterval] Overlaps for Left-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LX_OLArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dp2).Overlaps(new TimeInterval(null, _dp3)));
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RX_OLArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dp3).Overlaps(new TimeInterval(null, _dp2)));
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LIRA_OLArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dp4).Overlaps(new TimeInterval(null, _dp4)));
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RIRA_OLArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dp4).Overlaps(new TimeInterval(null, _dp4)));
        }
        #endregion

        #region [TimeInterval] Overlaps for Right-Open and Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LX_ORArg1_OArg2()
        {
            Assert.IsTrue(new TimeInterval(_dp4, (DateTime?)null).Overlaps(new TimeInterval()));
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RX_OArg1_ORArg2()
        {
            Assert.IsTrue(new TimeInterval().Overlaps(new TimeInterval(_dp4, (DateTime?)null)));
        }
        #endregion

        #region [TimeInterval] Overlaps for Left-Open and Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LX_OLArg1_OArg2()
        {
            Assert.IsTrue(new TimeInterval(null, _dp1).Overlaps(new TimeInterval()));
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_RX_OArg1_OLArg2()
        {
            Assert.IsTrue(new TimeInterval().Overlaps(new TimeInterval(null, _dp1)));
        }
        #endregion

        #region [TimeInterval] Overlaps for Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Overlaps_LX_OArg1_OArg2()
        {
            Assert.IsTrue(new TimeInterval().Overlaps(new TimeInterval()));
        }
        #endregion

        #region [TimeInterval] Intersect for Closed Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_IN_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dp4).Intersect(new TimeInterval(_dp2, _dp3)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_ON_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp2, _dp3).Intersect(new TimeInterval(_dp1, _dp4)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LX_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dp3).Intersect(new TimeInterval(_dp2, _dp4)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RX_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp2, _dp4).Intersect(new TimeInterval(_dp1, _dp3)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_OLA_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dpx).Intersect(new TimeInterval(_dpx, _dp4)), IntervalType.Flex, _dpx, true, _dpx, true, false);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_ORA_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dpx, _dp4).Intersect(new TimeInterval(_dp1, _dpx)), IntervalType.Flex, _dpx, true, _dpx, true, false);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LIRA_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dp4).Intersect(new TimeInterval(_dp2, _dp4)), IntervalType.Flex, _dp2, true, _dp4, true, false);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RIRA_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp2, _dp4).Intersect(new TimeInterval(_dp1, _dp4)), IntervalType.Flex, _dp2, true, _dp4, true, false);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LILA_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dp4).Intersect(new TimeInterval(_dp1, _dp3)), IntervalType.Flex, _dp1, true, _dp3, true, false);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RILA_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dp3).Intersect(new TimeInterval(_dp1, _dp4)), IntervalType.Flex, _dp1, true, _dp3, true, false);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LNX_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dp2).Intersect(new TimeInterval(_dp3, _dp4)), IntervalType.Flex, _dp2, true, _dp2, true, false);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RNX_CArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp3, _dp4).Intersect(new TimeInterval(_dp1, _dp2)), IntervalType.Flex, _dp2, true, _dp2, true, false);
        }
        #endregion

        #region [TimeInterval] Intersect for Right-Open and Closed Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_IN_ORArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, (DateTime?)null).Intersect(new TimeInterval(_dp2, _dp3)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_ON_CArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp2, _dp3).Intersect(new TimeInterval(_dp1, (DateTime?)null)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LX_CArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dp3).Intersect(new TimeInterval(_dp2, (DateTime?)null)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RX_ORArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp2, (DateTime?)null).Intersect(new TimeInterval(_dp1, _dp3)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_OLA_CArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dpx).Intersect(new TimeInterval(_dpx, (DateTime?)null)), IntervalType.Flex, _dpx, true, _dpx, true, false);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_ORA_ORArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dpx, (DateTime?)null).Intersect(new TimeInterval(_dp1, _dpx)), IntervalType.Flex, _dpx, true, _dpx, true, false);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LILA_CArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dp4).Intersect(new TimeInterval(_dp1, (DateTime?)null)), IntervalType.Flex, _dp1, true, _dp4, true, false);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LIRA_ORArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, (DateTime?)null).Intersect(new TimeInterval(_dp1, _dp4)), IntervalType.Flex, _dp1, true, _dp4, true, false);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LNX_CArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dp2).Intersect(new TimeInterval(_dp3, (DateTime?)null)), IntervalType.Flex, _dp2, true, _dp2, true, false);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RNX_ORArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp3, (DateTime?)null).Intersect(new TimeInterval(_dp1, _dp2)), IntervalType.Flex, _dp2, true, _dp2, true, false);
        }
        #endregion

        #region [TimeInterval] Intersect for Closed and Left-Open Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_IN_OLArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp4).Intersect(new TimeInterval(_dp2, _dp3)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_ON_CArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp2, _dp3).Intersect(new TimeInterval(null, _dp4)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LX_OLArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp3).Intersect(new TimeInterval(_dp2, _dp4)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RX_CArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp2, _dp4).Intersect(new TimeInterval(null, _dp3)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_OLA_OLArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dpx).Intersect(new TimeInterval(_dpx, _dp4)), IntervalType.Flex, _dpx, true, _dpx, true, false);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_ORA_CArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(_dpx, _dp4).Intersect(new TimeInterval(null, _dpx)), IntervalType.Flex, _dpx, true, _dpx, true, false);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RILA_OLArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp4).Intersect(new TimeInterval(_dp1, _dp4)), IntervalType.Flex, _dp1, true, _dp4, true, false);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RIRA_CArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, _dp4).Intersect(new TimeInterval(null, _dp4)), IntervalType.Flex, _dp1, true, _dp4, true, false);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LNX_OLArg1_CArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp2).Intersect(new TimeInterval(_dp3, _dp4)), IntervalType.Flex, _dp2, true, _dp2, true, false);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RNX_CArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp3, _dp4).Intersect(new TimeInterval(null, _dp2)), IntervalType.Flex, _dp2, true, _dp2, true, false);
        }
        #endregion

        #region [TimeInterval] Intersect for Left-Open and Right-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LX_OLArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp3).Intersect(new TimeInterval(_dp2, (DateTime?)null)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RX_ORArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp2, (DateTime?)null).Intersect(new TimeInterval(null, _dp3)), IntervalType.Flex, _dp2, true, _dp3, true, false);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_OLA_OLArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dpx).Intersect(new TimeInterval(_dpx, (DateTime?)null)), IntervalType.Flex, _dpx, true, _dpx, true, false);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_ORA_ORArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(_dpx, (DateTime?)null).Intersect(new TimeInterval(null, _dpx)), IntervalType.Flex, _dpx, true, _dpx, true, false);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LNX_OLArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp2).Intersect(new TimeInterval(_dp3, (DateTime?)null)), IntervalType.Flex, _dp2, true, _dp2, true, false);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RNX_ORArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp3, (DateTime?)null).Intersect(new TimeInterval(null, _dp2)), IntervalType.Flex, _dp2, true, _dp2, true, false);
        }
        #endregion

        #region [TimeInterval] Intersect for Right-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LX_ORArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp2, (DateTime?)null).Intersect(new TimeInterval(_dp3, (DateTime?)null)), IntervalType.Flex, _dp3, true, null, true, false);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RX_ORArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp3, (DateTime?)null).Intersect(new TimeInterval(_dp2, (DateTime?)null)), IntervalType.Flex, _dp3, true, null, true, false);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LILA_ORArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, (DateTime?)null).Intersect(new TimeInterval(_dp1, (DateTime?)null)), IntervalType.Flex, _dp1, true, null, true, false);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RILA_ORArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp1, (DateTime?)null).Intersect(new TimeInterval(_dp1, (DateTime?)null)), IntervalType.Flex, _dp1, true, null, true, false);
        }
        #endregion

        #region [TimeInterval] Intersect for Left-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LX_OLArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp2).Intersect(new TimeInterval(null, _dp3)), IntervalType.Flex, null, true, _dp2, true, false);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RX_OLArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp3).Intersect(new TimeInterval(null, _dp2)), IntervalType.Flex, null, true, _dp2, true, false);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LIRA_OLArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp4).Intersect(new TimeInterval(null, _dp4)), IntervalType.Flex, null, true, _dp4, true, false);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RIRA_OLArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp4).Intersect(new TimeInterval(null, _dp4)), IntervalType.Flex, null, true, _dp4, true, false);
        }
        #endregion

        #region [TimeInterval] Intersect for Right-Open and Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LX_ORArg1_OArg2()
        {
            AssertTimeInterval(new TimeInterval(_dp4, (DateTime?)null).Intersect(new TimeInterval()), IntervalType.Flex, _dp4, true, null, true, false);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RX_OArg1_ORArg2()
        {
            AssertTimeInterval(new TimeInterval().Intersect(new TimeInterval(_dp4, (DateTime?)null)), IntervalType.Flex, _dp4, true, null, true, false);
        }
        #endregion

        #region [TimeInterval] Intersect for Left-Open and Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LX_OLArg1_OArg2()
        {
            AssertTimeInterval(new TimeInterval(null, _dp1).Intersect(new TimeInterval()), IntervalType.Flex, null, true, _dp1, true, false);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_RX_OArg1_OLArg2()
        {
            AssertTimeInterval(new TimeInterval().Intersect(new TimeInterval(null, _dp1)), IntervalType.Flex, null, true, _dp1, true, false);
        }
        #endregion

        #region [TimeInterval] Intersect for Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Intersect_LX_OArg1_OArg2()
        {
            AssertTimeInterval(new TimeInterval().Intersect(new TimeInterval()), IntervalType.Flex, null, true, null, true, false);
        }
        #endregion

        #region [TimeInterval] Union for Closed Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_IN_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp2, _dp3), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp2, _dp3), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_ON_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(_dp1, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(_dp1, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LX_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp2, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp2, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RX_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(_dp1, _dp3), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(_dp1, _dp3), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_OLA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dpx), new TimeInterval(_dpx, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dpx), new TimeInterval(_dpx, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_ORA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dpx, _dp4), new TimeInterval(_dp1, _dpx), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dpx, _dp4), new TimeInterval(_dp1, _dpx), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LIRA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp2, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp2, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RIRA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(_dp1, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(_dp1, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LILA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp1, _dp3), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp1, _dp3), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RILA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp1, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp1, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LNX_CArg1_CArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp2), new TimeInterval(_dp3, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp2), new TimeInterval(_dp3, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RNX_CArg1_CArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(_dp3, _dp4), new TimeInterval(_dp1, _dp2), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp3, _dp4), new TimeInterval(_dp1, _dp2), (x, y) => Build(x, x.UnionAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Union for Right-Open and Closed Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_IN_ORArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp2, _dp3), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp2, _dp3), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_ON_CArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(_dp1, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(_dp1, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LX_CArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp2, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp2, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RX_ORArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(_dp1, _dp3), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(_dp1, _dp3), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_OLA_CArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dpx), new TimeInterval(_dpx, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dpx), new TimeInterval(_dpx, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_ORA_ORArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dpx, (DateTime?)null), new TimeInterval(_dp1, _dpx), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dpx, (DateTime?)null), new TimeInterval(_dp1, _dpx), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LILA_CArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp1, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp1, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LIRA_ORArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LNX_CArg1_ORArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp2), new TimeInterval(_dp3, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp2), new TimeInterval(_dp3, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RNX_ORArg1_CArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(_dp1, _dp2), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(_dp1, _dp2), (x, y) => Build(x, x.UnionAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Union for Closed and Left-Open Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_IN_OLArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(_dp2, _dp3), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(_dp2, _dp3), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_ON_CArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(null, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(null, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LX_OLArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(_dp2, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(_dp2, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RX_CArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(null, _dp3), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(null, _dp3), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_OLA_OLArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dpx), new TimeInterval(_dpx, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dpx), new TimeInterval(_dpx, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_ORA_CArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dpx, _dp4), new TimeInterval(null, _dpx), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dpx, _dp4), new TimeInterval(null, _dpx), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RILA_OLArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(_dp1, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(_dp1, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RIRA_CArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(null, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(null, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LNX_OLArg1_CArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(_dp3, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(_dp3, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RNX_CArg1_OLArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(_dp3, _dp4), new TimeInterval(null, _dp2), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp3, _dp4), new TimeInterval(null, _dp2), (x, y) => Build(x, x.UnionAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Union for Left-Open and Right-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LX_OLArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(_dp2, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(_dp2, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RX_ORArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(null, _dp3), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(null, _dp3), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_OLA_OLArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dpx), new TimeInterval(_dpx, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dpx), new TimeInterval(_dpx, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_ORA_ORArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dpx, (DateTime?)null), new TimeInterval(null, _dpx), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dpx, (DateTime?)null), new TimeInterval(null, _dpx), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LNX_OLArg1_ORArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(_dp3, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(_dp3, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RNX_ORArg1_OLArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(null, _dp2), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(null, _dp2), (x, y) => Build(x, x.UnionAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Union for Right-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LX_ORArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp2, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(_dp3, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(_dp3, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RX_ORArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp2, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(_dp2, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(_dp2, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LILA_ORArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RILA_ORArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Union for Left-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LX_OLArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp3, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(null, _dp3), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(null, _dp3), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RX_OLArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp3, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(null, _dp2), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(null, _dp2), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LIRA_OLArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(null, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(null, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RIRA_OLArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(null, _dp4), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(null, _dp4), (x, y) => Build(x, x.UnionAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Union for Right-Open and Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LX_ORArg1_OArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp4, (DateTime?)null), new TimeInterval(), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp4, (DateTime?)null), new TimeInterval(), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RX_OArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(_dp4, (DateTime?)null), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(_dp4, (DateTime?)null), (x, y) => Build(x, x.UnionAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Union for Left-Open and Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LX_OLArg1_OArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp1), new TimeInterval(), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp1), new TimeInterval(), (x, y) => Build(x, x.UnionAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_RX_OArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(null, _dp1), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(null, _dp1), (x, y) => Build(x, x.UnionAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Union for Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Union_LX_OArg1_OArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(), (x, y) => x.Union(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(), (x, y) => Build(x, x.UnionAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Subtract for Closed Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_IN_CArg1_CArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp2, _dp3), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp2, _dp3), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_ON_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(_dp1, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(_dp1, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LX_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp2, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp2, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RX_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(_dp1, _dp3), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(_dp1, _dp3), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_OLA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dpx, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dpx), new TimeInterval(_dpx, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dpx), new TimeInterval(_dpx, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_ORA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dpx, Include1 = false, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dpx, _dp4), new TimeInterval(_dp1, _dpx), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dpx, _dp4), new TimeInterval(_dp1, _dpx), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LIRA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp2, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp2, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RIRA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(_dp1, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(_dp1, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LILA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp1, _dp3), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp1, _dp3), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RILA_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp1, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp1, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LNX_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp2), new TimeInterval(_dp3, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp2), new TimeInterval(_dp3, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RNX_CArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp3, _dp4), new TimeInterval(_dp1, _dp2), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp3, _dp4), new TimeInterval(_dp1, _dp2), (x, y) => Build(x, x.SubtractAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Subtract for Right-Open and Closed Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_IN_ORArg1_CArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = false, Arg2 = null, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp2, _dp3), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp2, _dp3), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_ON_CArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(_dp1, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(_dp1, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LX_CArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp2, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp3), new TimeInterval(_dp2, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RX_ORArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = false, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(_dp1, _dp3), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(_dp1, _dp3), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_OLA_CArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dpx, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dpx), new TimeInterval(_dpx, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dpx), new TimeInterval(_dpx, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_ORA_ORArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dpx, Include1 = false, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dpx, (DateTime?)null), new TimeInterval(_dp1, _dpx), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dpx, (DateTime?)null), new TimeInterval(_dp1, _dpx), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LILA_CArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp1, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(_dp1, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LIRA_ORArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp4, Include1 = false, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LNX_CArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp2), new TimeInterval(_dp3, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp2), new TimeInterval(_dp3, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RNX_ORArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(_dp1, _dp2), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(_dp1, _dp2), (x, y) => Build(x, x.SubtractAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Subtract for Closed and Left-Open Ranges
        /// <summary>
        /// The ranges are inner nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_IN_OLArg1_CArg2()
        {
            var target = new[]
            {
                new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp2, Include2 = false, IsEmpty = false },
                new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true, IsEmpty = false }
            };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(_dp2, _dp3), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(_dp2, _dp3), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are outer nested
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_ON_CArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(null, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp3), new TimeInterval(null, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LX_OLArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp2, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(_dp2, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(_dp2, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RX_CArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = false, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(null, _dp3), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, _dp4), new TimeInterval(null, _dp3), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_OLA_OLArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dpx, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dpx), new TimeInterval(_dpx, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dpx), new TimeInterval(_dpx, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_ORA_CArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dpx, Include1 = false, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dpx, _dp4), new TimeInterval(null, _dpx), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dpx, _dp4), new TimeInterval(null, _dpx), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RILA_OLArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp1, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(_dp1, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(_dp1, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RIRA_CArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(null, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, _dp4), new TimeInterval(null, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LNX_OLArg1_CArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(_dp3, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(_dp3, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RNX_CArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = _dp4, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp3, _dp4), new TimeInterval(null, _dp2), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp3, _dp4), new TimeInterval(null, _dp2), (x, y) => Build(x, x.SubtractAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Subtract for Left-Open and Right-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LX_OLArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp2, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(_dp2, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(_dp2, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RX_ORArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = false, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(null, _dp3), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(null, _dp3), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_OLA_OLArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dpx, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dpx), new TimeInterval(_dpx, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dpx), new TimeInterval(_dpx, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right adjacent
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_ORA_ORArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dpx, Include1 = false, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dpx, (DateTime?)null), new TimeInterval(null, _dpx), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dpx, (DateTime?)null), new TimeInterval(null, _dpx), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are left non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LNX_OLArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp2, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(_dp3, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(_dp3, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right non-crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RNX_ORArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp3, Include1 = true, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(null, _dp2), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(null, _dp2), (x, y) => Build(x, x.SubtractAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Subtract for Right-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LX_ORArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp2, Include1 = true, Arg2 = _dp3, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(_dp3, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp2, (DateTime?)null), new TimeInterval(_dp3, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RX_ORArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(_dp2, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp3, (DateTime?)null), new TimeInterval(_dp2, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LILA_ORArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RILA_ORArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp1, (DateTime?)null), new TimeInterval(_dp1, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Subtract for Left-Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LX_OLArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(null, _dp3), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp2), new TimeInterval(null, _dp3), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RX_OLArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp2, Include1 = false, Arg2 = _dp3, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(null, _dp2), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp3), new TimeInterval(null, _dp2), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LIRA_OLArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(null, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(null, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RIRA_OLArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(null, _dp4), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp4), new TimeInterval(null, _dp4), (x, y) => Build(x, x.SubtractAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Subtract for Right-Open and Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LX_ORArg1_OArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(_dp4, (DateTime?)null), new TimeInterval(), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(_dp4, (DateTime?)null), new TimeInterval(), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RX_OArg1_ORArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = true, Arg2 = _dp4, Include2 = false, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(_dp4, (DateTime?)null), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(_dp4, (DateTime?)null), (x, y) => Build(x, x.SubtractAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Subtract for Left-Open and Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LX_OLArg1_OArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(null, _dp1), new TimeInterval(), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(null, _dp1), new TimeInterval(), (x, y) => Build(x, x.SubtractAt(y))), target);
        }

        /// <summary>
        /// The ranges are right crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_RX_OArg1_OLArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = _dp1, Include1 = false, Arg2 = null, Include2 = true, IsEmpty = false } };
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(null, _dp1), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(null, _dp1), (x, y) => Build(x, x.SubtractAt(y))), target);
        }
        #endregion

        #region [TimeInterval] Subtract for Open Ranges
        /// <summary>
        /// The ragnes are left crossing
        /// </summary>
        [TestMethod]
        public void TimeInterval_Subtract_LX_OArg1_OArg2()
        {
            var target = new[] { new Interval { Type = IntervalType.Flex, Arg1 = null, Include1 = false, Arg2 = null, Include2 = false, IsEmpty = true } };
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(), (x, y) => x.Subtract(y)), target);
            AssertTimeIntervals(Build(new TimeInterval(), new TimeInterval(), (x, y) => Build(x, x.SubtractAt(y))), target);
        }
        #endregion
    }
#pragma warning restore CA1707 // Identifiers should not contain underscores
}