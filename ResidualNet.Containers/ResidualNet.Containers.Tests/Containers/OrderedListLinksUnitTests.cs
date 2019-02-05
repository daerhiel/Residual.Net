using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResidualNet.Containers.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResidualNet.Containers
{
#pragma warning disable CA1707 // Identifiers should not contain underscores
    [TestClass]
    public class OrderedListLinksUnitTests : GeneralTests
    {
        /// <summary>
        /// Tests linked list and adjacent nodes for correct linking and ownership.
        /// </summary>
        /// <typeparam name="T">The type of a value for the test container item.</typeparam>
        /// <param name="link">The source node to test.</param>
        /// <param name="prev">The preceding node adjacent to the source node.</param>
        /// <param name="next">The following node adjacent to the source node.</param>
        public static void AssertNode<T>(OrderedContainer<T>.OrderedNode link, OrderedContainer<T>.OrderedNode prev, OrderedContainer<T>.OrderedNode next)
            where T : IComparable<T>
        {
            Assert.IsNotNull(link);
            Assert.AreEqual(prev, link.InternalPrev, $"Node Property: {nameof(link.Prev)}");
            Assert.AreEqual(next, link.InternalNext, $"Node Property: {nameof(link.Next)}");
            if (prev != null)
            {
                Assert.AreEqual(link, prev.InternalNext, $"Prev Property: {nameof(prev.Next)}");
            }
            if (next != null)
            {
                Assert.AreEqual(link, next.InternalPrev, $"Next Property: {nameof(next.Prev)}");
            }
        }

        /// <summary>
        /// Tests linked list and adjacent nodes for correct linking and ownership.
        /// </summary>
        /// <typeparam name="T">The type of a value for the test container item.</typeparam>
        /// <param name="list">Linked list owning the nodes being tested.</param>
        /// <param name="head">The head node of a container.</param>
        /// <param name="prev">The preceding node adjacent to the source node.</param>
        /// <param name="next">The following node adjacent to the source node.</param>
        /// <param name="tail">The tail node of a container.</param>
        public static void AssertNode<T>(OrderedContainer<T> list, OrderedContainer<T>.OrderedNode head, OrderedContainer<T>.OrderedNode prev, OrderedContainer<T>.OrderedNode next, OrderedContainer<T>.OrderedNode tail)
            where T : IComparable<T>
        {
            Assert.AreEqual(head, list.Head, $"Container Property: {nameof(list.Head)}");
            Assert.AreEqual(tail, list.Tail, $"Container Property: {nameof(list.Tail)}");
            if (prev != null)
            {
                Assert.AreEqual(list, prev.Parent, $"Prev Property: {nameof(prev.Parent)}");
            }
            if (next != null)
            {
                Assert.AreEqual(list, next.Parent, $"Next Property: {nameof(next.Parent)}");
            }
        }

        /// <summary>
        /// Tests linked list and adjacent nodes for correct linking and ownership.
        /// </summary>
        /// <typeparam name="T">The type of a value for the test container item.</typeparam>
        /// <param name="node">Source node to test.</param>
        /// <param name="list">Linked list owning the nodes being tested.</param>
        /// <param name="head">The head node of a container.</param>
        /// <param name="prev">The preceding node adjacent to the source node.</param>
        /// <param name="next">The following node adjacent to the source node.</param>
        /// <param name="tail">The tail node of a container.</param>
        public static void AssertNode<T>(OrderedContainer<T>.OrderedNode node, OrderedContainer<T> list, OrderedContainer<T>.OrderedNode head, OrderedContainer<T>.OrderedNode prev, OrderedContainer<T>.OrderedNode next, OrderedContainer<T>.OrderedNode tail)
            where T : IComparable<T>
        {
            Assert.IsNotNull(node, $"Object: Node");
            Assert.AreEqual(head, list.Head, $"Container Property: {nameof(list.Head)}");
            Assert.AreEqual(tail, list.Tail, $"Container Property: {nameof(list.Tail)}");
            Assert.AreEqual(list, node.Parent, $"Node Property: {nameof(node.Parent)}");
            Assert.AreEqual(prev, node.Prev, $"Node Property: {nameof(node.Prev)}");
            Assert.AreEqual(next, node.Next, $"Node Property: {nameof(node.Next)}");
            if (prev != null)
            {
                Assert.AreEqual(list, prev.Parent, $"Prev Property: {nameof(node.Parent)}");
                Assert.AreEqual(node, prev.Next, $"Prev Property: {nameof(node.Next)}");
            }
            if (next != null)
            {
                Assert.AreEqual(list, next.Parent, $"Next Property: {nameof(node.Parent)}");
                Assert.AreEqual(node, next.Prev, $"Next Property: {nameof(node.Prev)}");
            }
        }

        /// <summary>
        /// Tests the sequence of item values against the fixed value array.
        /// </summary>
        /// <typeparam name="T">The type of a value for the test container item.</typeparam>
        /// <param name="list">Linked list owning the nodes being tested.</param>
        /// <param name="values">The array of values to compare with the container values.</param>
        public static void AssertSequence<T>(IList<OrderedContainer<T>.OrderedNode> list, params T[] values)
            where T : IComparable<T>
        {
            Assert.IsNotNull(list, $"Container: List");
            Assert.IsNotNull(values, $"Sequence: Valies");
            Assert.AreEqual(values.Length, list.Count, "Entries Count");
            for (int index = 0; index < values.Length; index++)
                Assert.AreEqual(values[index], list[index].Value);
        }

        #region [OrderedListLinks] Ordered Inserts
        [TestMethod]
        public void OrderedListLinks_InsertNext_Items1()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            AssertNode<double>(node1, container, node1, null, null, node1);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items2Lower()
        {
            var container = new OrderedContainer<double>();
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node2);
            AssertNode(node2, container, node1, node1, null, node2);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items2Upper()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node2);
            AssertNode(node2, container, node1, node1, null, node2);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items3LowerLower()
        {
            var container = new OrderedContainer<double>();
            var node3 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items3LowerUpper()
        {
            var container = new OrderedContainer<double>();
            var node3 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node1 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items3UpperLower()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items3UpperUpper()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items1Same1()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node2);
            AssertNode(node2, container, node1, node1, null, node2);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items1Same2()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items2LowerSame1()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items2LowerSame2()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node4);
            AssertNode(node2, container, node1, node1, node3, node4);
            AssertNode(node3, container, node1, node2, node4, node4);
            AssertNode(node4, container, node1, node3, null, node4);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items2UpperSame1()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items2UpperSame2()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node4);
            AssertNode(node2, container, node1, node1, node3, node4);
            AssertNode(node3, container, node1, node2, node4, node4);
            AssertNode(node4, container, node1, node3, null, node4);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items5SequenceDirect()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node5);
            AssertNode(node2, container, node1, node1, node3, node5);
            AssertNode(node3, container, node1, node2, node4, node5);
            AssertNode(node4, container, node1, node3, node5, node5);
            AssertNode(node5, container, node1, node4, null, node5);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items5SequenceReverse()
        {
            var container = new OrderedContainer<double>();
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node5);
            AssertNode(node2, container, node1, node1, node3, node5);
            AssertNode(node3, container, node1, node2, node4, node5);
            AssertNode(node4, container, node1, node3, node5, node5);
            AssertNode(node5, container, node1, node4, null, node5);
        }

        [TestMethod]
        public void OrderedListLinks_InsertNext_Items5DenseLeave()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, node5);
            AssertNode(node2, container, node1, node1, node3, node5);
            AssertNode(node3, container, node1, node2, node4, node5);
            AssertNode(node4, container, node1, node3, node5, node5);
            AssertNode(node5, container, node1, node4, null, node5);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items1()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            AssertNode<double>(node1, container, node1, null, null, node1);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items2Lower()
        {
            var container = new OrderedContainer<double>();
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node2);
            AssertNode(node2, container, node1, node1, null, node2);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items2Upper()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(4).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node2);
            AssertNode(node2, container, node1, node1, null, node2);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items3LowerLower()
        {
            var container = new OrderedContainer<double>();
            var node3 = new OrderedContainer<double>.OrderedNode(3).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertPrev(container);
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items3LowerUpper()
        {
            var container = new OrderedContainer<double>();
            var node3 = new OrderedContainer<double>.OrderedNode(3).InsertPrev(container);
            var node1 = new OrderedContainer<double>.OrderedNode(1).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items3UpperLower()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(1).InsertPrev(container);
            var node3 = new OrderedContainer<double>.OrderedNode(3).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items3UpperUpper()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(1).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(3).InsertPrev(container);
            var node3 = new OrderedContainer<double>.OrderedNode(4).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items1Same1()
        {
            var container = new OrderedContainer<double>();
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node1 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node2);
            AssertNode(node2, container, node1, node1, null, node2);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items1Same2()
        {
            var container = new OrderedContainer<double>();
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node1 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items2LowerSame1()
        {
            var container = new OrderedContainer<double>();
            var node2 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items2LowerSame2()
        {
            var container = new OrderedContainer<double>();
            var node3 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            var node4 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node4);
            AssertNode(node2, container, node1, node1, node3, node4);
            AssertNode(node3, container, node1, node2, node4, node4);
            AssertNode(node4, container, node1, node3, null, node4);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items2UpperSame1()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items2UpperSame2()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            var node4 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node4);
            AssertNode(node2, container, node1, node1, node3, node4);
            AssertNode(node3, container, node1, node2, node4, node4);
            AssertNode(node4, container, node1, node3, null, node4);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items5SequenceDirect()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertPrev(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertPrev(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node5);
            AssertNode(node2, container, node1, node1, node3, node5);
            AssertNode(node3, container, node1, node2, node4, node5);
            AssertNode(node4, container, node1, node3, node5, node5);
            AssertNode(node5, container, node1, node4, null, node5);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items5SequenceReverse()
        {
            var container = new OrderedContainer<double>();
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertPrev(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertPrev(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertPrev(container);
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node5);
            AssertNode(node2, container, node1, node1, node3, node5);
            AssertNode(node3, container, node1, node2, node4, node5);
            AssertNode(node4, container, node1, node3, node5, node5);
            AssertNode(node5, container, node1, node4, null, node5);
        }

        [TestMethod]
        public void OrderedListLinks_InsertPrev_Items5DenseLeave()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertPrev(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertPrev(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertPrev(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertPrev(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertPrev(container);
            AssertNode(node1, container, node1, null, node2, node5);
            AssertNode(node2, container, node1, node1, node3, node5);
            AssertNode(node3, container, node1, node2, node4, node5);
            AssertNode(node4, container, node1, node3, node5, node5);
            AssertNode(node5, container, node1, node4, null, node5);
        } 
        #endregion

        #region [OrderedListLinks] Incrementor Enumerations
        [TestMethod]
        public void OrderedListLinks_Incrementor_Open()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetIncrement().ToList(), 0, 1, 2, 3, 4);
        }

        [TestMethod]
        public void OrderedListLinks_Incrementor_OpenLower()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetIncrement(upper: node3).ToList(), 0, 1, 2);
        }

        [TestMethod]
        public void OrderedListLinks_Incrementor_OpenUpper()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetIncrement(lower: node3).ToList(), 2, 3, 4);
        }

        [TestMethod]
        public void OrderedListLinks_Incrementor_Middle()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetIncrement(node2, node4).ToList(), 1, 2, 3);
        }

        [TestMethod]
        public void OrderedListLinks_Incrementor_MiddleLower()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetIncrement(node2, node5).ToList(), 1, 2, 3, 4);
        }

        [TestMethod]
        public void OrderedListLinks_Incrementor_MiddleUpper()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetIncrement(node1, node4).ToList(), 0, 1, 2, 3);
        }
        #endregion

        #region [OrderedListLinks] Decrementor Enumerations
        [TestMethod]
        public void OrderedListLinks_Decrementor_Open()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetDecrement().ToList(), 4, 3, 2, 1, 0);
        }

        [TestMethod]
        public void OrderedListLinks_Decrementor_OpenLower()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetDecrement(upper: node3).ToList(), 2, 1, 0);
        }

        [TestMethod]
        public void OrderedListLinks_Decrementor_OpenUpper()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetDecrement(lower: node3).ToList(), 4, 3, 2);
        }

        [TestMethod]
        public void OrderedListLinks_Decrementor_Middle()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetDecrement(node4, node2).ToList(), 3, 2, 1);
        }

        [TestMethod]
        public void OrderedListLinks_Decrementor_MiddleLower()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetDecrement(node4, node1).ToList(), 3, 2, 1, 0);
        }

        [TestMethod]
        public void OrderedListLinks_Decrementor_MiddleUpper()
        {
            var container = new OrderedContainer<double>();
            var node1 = new OrderedContainer<double>.OrderedNode(0).InsertNext(container);
            var node2 = new OrderedContainer<double>.OrderedNode(1).InsertNext(container);
            var node3 = new OrderedContainer<double>.OrderedNode(2).InsertNext(container);
            var node4 = new OrderedContainer<double>.OrderedNode(3).InsertNext(container);
            var node5 = new OrderedContainer<double>.OrderedNode(4).InsertNext(container);
            AssertSequence(container.GetDecrement(node5, node2).ToList(), 4, 3, 2, 1);
        }
        #endregion
    }
#pragma warning restore CA1707 // Identifiers should not contain underscores
}