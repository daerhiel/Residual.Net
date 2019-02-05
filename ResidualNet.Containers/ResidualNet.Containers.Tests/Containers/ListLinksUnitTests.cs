using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResidualNet.Containers.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResidualNet.Containers
{
#pragma warning disable CA1707 // Identifiers should not contain underscores
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ListLinksUnitTests : GeneralTests
    {
        //private const int _blockLength = 10;
        private const int _dynamicLength = 1000;
        private const int _dynamicBlock = 5;

        /// <summary>
        /// Tests linked list and adjacent nodes for correct linking and ownership.
        /// </summary>
        /// <typeparam name="T">The type of a value for the test container item.</typeparam>
        /// <param name="link">The source node to test.</param>
        /// <param name="prev">The preceding node adjacent to the source node.</param>
        /// <param name="next">The following node adjacent to the source node.</param>
        public static void AssertNode<T>(Container<T>.Node link, Container<T>.Node prev, Container<T>.Node next)
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
        public static void AssertNode<T>(Container<T> list, Container<T>.Node head, Container<T>.Node prev, Container<T>.Node next, Container<T>.Node tail)
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
        public static void AssertNode<T>(Container<T>.Node node, Container<T> list, Container<T>.Node head, Container<T>.Node prev, Container<T>.Node next, Container<T>.Node tail)
        {
            Assert.IsNotNull(node, $"Object: Node");
            Assert.IsNotNull(list, $"Container: List");
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
        public static void AssertSequence<T>(IList<Container<T>.Node> list, params T[] values)
        {
            Assert.IsNotNull(list, $"Container: List");
            Assert.IsNotNull(values, $"Sequence: Valies");
            Assert.AreEqual(values.Length, list.Count, "Entries Count");
            for (int index = 0; index < values.Length; index++)
                Assert.AreEqual(values[index], list[index].Value);
        }

        #region [ListLinks] Core Operations
        [TestMethod]
        public void ListLinks_ParentInsertPrev_NullNode_Fail()
        {
            AssertError<ArgumentNullException>(
                () => new Container<double>().InsertPrev(null),
                e => e.ParamName == "node");
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_NullNode_Fail()
        {
            AssertError<ArgumentNullException>(
                () => new Container<double>().InsertNext(null),
                e => e.ParamName == "node");
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrev_OriginUnlinked_Fail()
        {
            AssertError<InvalidOperationException>(
                () => new Container<double>().InsertPrev(new Container<double>.Node(0), new Container<double>.Node(1)),
                e => e.Message.Contains("not linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_OriginUnlinked_Fail()
        {
            AssertError<InvalidOperationException>(
                () => new Container<double>().InsertNext(new Container<double>.Node(0), new Container<double>.Node(-1)),
                e => e.Message.Contains("not linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrev_NodeLinked_Fail()
        {
            var container = new Container<double>().InsertPrev(new Container<double>.Node(0));
            AssertError<InvalidOperationException>(
                () => container.InsertPrev(new Container<double>.Node(1).InsertPrev(new Container<double>())),
                e => e.Message.Contains("is linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_NodeLinked_Fail()
        {
            var container = new Container<double>().InsertNext(new Container<double>.Node(0));
            AssertError<InvalidOperationException>(
                () => container.InsertNext(new Container<double>.Node(1).InsertNext(new Container<double>())),
                e => e.Message.Contains("is linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrev_NodeLinked_Same()
        {
            var container = new Container<double>();
            var test = new Container<double>.Node(0).InsertPrev(container).InsertPrev(container);
            AssertNode(test, container, test, null, null, test);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_NodeLinked_Same()
        {
            var container = new Container<double>();
            var test = new Container<double>.Node(0).InsertNext(container).InsertNext(container);
            AssertNode(test, container, test, null, null, test);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrev_OriginForeign_Fail()
        {
            AssertError<InvalidOperationException>(
                () => new Container<double>().InsertPrev(new Container<double>.Node(0), new Container<double>.Node(1).InsertPrev(new Container<double>())),
                e => e.Message.Contains("is not linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_OriginForeign_Fail()
        {
            AssertError<InvalidOperationException>(
                () => new Container<double>().InsertNext(new Container<double>.Node(0), new Container<double>.Node(1).InsertNext(new Container<double>())),
                e => e.Message.Contains("is not linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrev_ParentEmpty()
        {
            var test = new Container<double>.Node(0);
            var container = new Container<double>().InsertPrev(test);
            AssertNode(test, container, test, null, null, test);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_ParentEmpty()
        {
            var test = new Container<double>.Node(0);
            var container = new Container<double>().InsertNext(test);
            AssertNode(test, container, test, null, null, test);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrev_Parent1Item()
        {
            var node = new Container<double>.Node(0);
            var test = new Container<double>.Node(1);
            var container = new Container<double>().InsertPrev(node).InsertPrev(test, node);
            AssertNode(test, container, test, null, node, node);
            AssertNode(node, container, test, test, null, node);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_Parent1Item()
        {
            var node = new Container<double>.Node(0);
            var test = new Container<double>.Node(1);
            var container = new Container<double>().InsertNext(node).InsertNext(test, node);
            AssertNode(node, container, node, null, test, test);
            AssertNode(test, container, node, node, null, test);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrev_Parent2Item()
        {
            var node2 = new Container<double>.Node(2);
            var node1 = new Container<double>.Node(0);
            var test = new Container<double>.Node(1);
            var container = new Container<double>().InsertPrev(node2).InsertPrev(node1);
            container.InsertPrev(test);
            AssertNode(test, container, test, null, node1, node2);
            AssertNode(node1, container, test, test, node2, node2);
            AssertNode(node2, container, test, node1, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_Parent2Item()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(2);
            var test = new Container<double>.Node(1);
            var container = new Container<double>().InsertNext(node1).InsertNext(node2);
            container.InsertNext(test);
            AssertNode(node1, container, node1, null, node2, test);
            AssertNode(node2, container, node1, node1, test, test);
            AssertNode(test, container, node1, node2, null, test);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrev_Parent2ItemHead()
        {
            var node2 = new Container<double>.Node(2);
            var node1 = new Container<double>.Node(0);
            var test = new Container<double>.Node(1);
            var container = new Container<double>().InsertPrev(node2).InsertPrev(node1);
            container.InsertPrev(test, container.Head);
            AssertNode(test, container, test, null, node1, node2);
            AssertNode(node1, container, test, test, node2, node2);
            AssertNode(node2, container, test, node1, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_Parent2ItemHead()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(2);
            var test = new Container<double>.Node(1);
            var container = new Container<double>().InsertNext(node1).InsertNext(node2);
            container.InsertNext(test, container.Head);
            AssertNode(node1, container, node1, null, test, node2);
            AssertNode(test, container, node1, node1, node2, node2);
            AssertNode(node2, container, node1, test, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrev_Parent2ItemTail()
        {
            var node2 = new Container<double>.Node(2);
            var node1 = new Container<double>.Node(0);
            var test = new Container<double>.Node(1);
            var container = new Container<double>().InsertPrev(node2).InsertPrev(node1);
            container.InsertPrev(test, container.Tail);
            AssertNode(node1, container, node1, null, test, node2);
            AssertNode(test, container, node1, node1, node2, node2);
            AssertNode(node2, container, node1, test, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_Parent2ItemTail()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(2);
            var test = new Container<double>.Node(1);
            var container = new Container<double>().InsertNext(node1).InsertNext(node2);
            container.InsertNext(test, container.Tail);
            AssertNode(node1, container, node1, null, node2, test);
            AssertNode(node2, container, node1, node1, test, test);
            AssertNode(test, container, node1, node2, null, test);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrev_Parent3ItemMiddle()
        {
            var node3 = new Container<double>.Node(5);
            var node2 = new Container<double>.Node(2);
            var node1 = new Container<double>.Node(0);
            var test = new Container<double>.Node(1);
            var container = new Container<double>().InsertPrev(node3).InsertPrev(node2).InsertPrev(node1);
            container.InsertPrev(test, node2);
            AssertNode(node1, container, node1, null, test, node3);
            AssertNode(test, container, node1, node1, node2, node3);
            AssertNode(node2, container, node1, test, node3, node3);
            AssertNode(node3, container, node1, node2, null, node3);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNext_Parent3ItemMiddle()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(2);
            var node3 = new Container<double>.Node(5);
            var test = new Container<double>.Node(1);
            var container = new Container<double>().InsertNext(node1).InsertNext(node2).InsertNext(node3);
            container.InsertNext(test, node2);
            AssertNode(node1, container, node1, null, node2, node3);
            AssertNode(node2, container, node1, node1, test, node3);
            AssertNode(test, container, node1, node2, node3, node3);
            AssertNode(node3, container, node1, test, null, node3);
        }

        [TestMethod]
        public void ListLinks_ParentRemove_Parent3ItemMiddle()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(1);
            var node3 = new Container<double>.Node(2);
            var container = new Container<double>().InsertNext(node1).InsertNext(node2).InsertNext(node3).Remove(node2);
            AssertNode(node2, null, null);
            AssertNode(node1, container, node1, null, node3, node3);
            AssertNode(node3, container, node1, node1, null, node3);
        }

        [TestMethod]
        public void ListLinks_ParentRemove_Parent3ItemHead()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(1);
            var node3 = new Container<double>.Node(2);
            var container = new Container<double>().InsertNext(node1).InsertNext(node2).InsertNext(node3).Remove(node1);
            AssertNode(node1, null, null);
            AssertNode(node2, container, node2, null, node3, node3);
            AssertNode(node3, container, node2, node2, null, node3);
        }

        [TestMethod]
        public void ListLinks_ParentRemove_Parent3ItemTail()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(1);
            var node3 = new Container<double>.Node(2);
            var container = new Container<double>().InsertNext(node1).InsertNext(node2).InsertNext(node3).Remove(node3);
            AssertNode(node3, null, null);
            AssertNode(node1, container, node1, null, node2, node2);
            AssertNode(node2, container, node1, node1, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentRemove_Parent2ItemHead()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(1);
            var container = new Container<double>().InsertNext(node1).InsertNext(node2).Remove(node1);
            AssertNode(node1, null, null);
            AssertNode(node2, container, node2, null, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentRemove_Parent2ItemTail()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(1);
            var container = new Container<double>().InsertNext(node1).InsertNext(node2).Remove(node2);
            AssertNode(node2, null, null);
            AssertNode(node1, container, node1, null, null, node1);
        }

        [TestMethod]
        public void ListLinks_ParentRemove_Parent1Item()
        {
            var node = new Container<double>.Node(0);
            var container = new Container<double>().InsertNext(node).Remove(node);
            AssertNode(container, null, null, null, null);
            AssertNode(node, null, null);
        }

        [TestMethod]
        public void ListLinks_ParentRemove_Null_Fail()
        {
            AssertError<ArgumentNullException>(
                () => new Container<double>().Remove((Container<double>.Node)null),
                e => e.Message.Contains("node", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrevs_ParentEmpty()
        {
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            var container = new Container<double>().InsertPrevs(null, test1, test2);
            AssertNode(test1, container, test1, null, test2, test2);
            AssertNode(test2, container, test1, test1, null, test2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNexts_ParentEmpty()
        {
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            var container = new Container<double>().InsertNexts(null, test1, test2);
            AssertNode(test1, container, test1, null, test2, test2);
            AssertNode(test2, container, test1, test1, null, test2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrevs_ParentItem1()
        {
            var node = new Container<double>.Node(0);
            var container = new Container<double>().InsertPrevs(null, node);
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertPrevs(node, test1, test2);
            AssertNode(test1, container, test1, null, test2, node);
            AssertNode(test2, container, test1, test1, node, node);
            AssertNode(node, container, test1, test2, null, node);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNexts_ParentItem1()
        {
            var node = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, node);
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertNexts(node, test1, test2);
            AssertNode(node, container, node, null, test1, test2);
            AssertNode(test1, container, node, node, test2, test2);
            AssertNode(test2, container, node, test1, null, test2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrevs_ParentItem2Head()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertPrevs(null, node1, node2);
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertPrevs(container.Head, test1, test2);
            AssertNode(test1, container, test1, null, test2, node2);
            AssertNode(test2, container, test1, test1, node1, node2);
            AssertNode(node1, container, test1, test2, node2, node2);
            AssertNode(node2, container, test1, node1, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNexts_ParentItem2Head()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, node1, node2);
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertNexts(container.Head, test1, test2);
            AssertNode(node1, container, node1, null, test1, node2);
            AssertNode(test1, container, node1, node1, test2, node2);
            AssertNode(test2, container, node1, test1, node2, node2);
            AssertNode(node2, container, node1, test2, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrevs_ParentItem2Tail()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertPrevs(null, node1, node2);
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertPrevs(container.Tail, test1, test2);
            AssertNode(node1, container, node1, null, test1, node2);
            AssertNode(test1, container, node1, node1, test2, node2);
            AssertNode(test2, container, node1, test1, node2, node2);
            AssertNode(node2, container, node1, test2, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNexts_ParentItem2Tail()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, node1, node2);
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertNexts(container.Tail, test1, test2);
            AssertNode(node1, container, node1, null, node2, test2);
            AssertNode(node2, container, node1, node1, test1, test2);
            AssertNode(test1, container, node1, node2, test2, test2);
            AssertNode(test2, container, node1, test1, null, test2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrevs_ParentItem2()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertPrevs(null, node1, node2);
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertPrevs(null, test1, test2);
            AssertNode(test1, container, test1, null, test2, node2);
            AssertNode(test2, container, test1, test1, node1, node2);
            AssertNode(node1, container, test1, test2, node2, node2);
            AssertNode(node2, container, test1, node1, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNexts_ParentItem2()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, node1, node2);
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertNexts(null, test1, test2);
            AssertNode(node1, container, node1, null, node2, test2);
            AssertNode(node2, container, node1, node1, test1, test2);
            AssertNode(test1, container, node1, node2, test2, test2);
            AssertNode(test2, container, node1, test1, null, test2);
        }

        [TestMethod]
        public void ListLinks_ParentRemove_ParentItemsHead()
        {
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(0);
            var test3 = new Container<double>.Node(0);
            var test4 = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, test1, test2, test3, test4);
            container.Remove(test1, test2);
            AssertNode(test3, container, test3, null, test4, test4);
            AssertNode(test4, container, test3, test3, null, test4);
        }

        [TestMethod]
        public void ListLinks_ParentRemove_ParentItemsTail()
        {
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(0);
            var test3 = new Container<double>.Node(0);
            var test4 = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, test1, test2, test3, test4);
            container.Remove(test3, test4);
            AssertNode(test1, container, test1, null, test2, test2);
            AssertNode(test2, container, test1, test1, null, test2);
        }

        [TestMethod]
        public void ListLinks_ParentRemove_ParentItems()
        {
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(0);
            var test3 = new Container<double>.Node(0);
            var test4 = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, test1, test2, test3, test4);
            container.Remove(test2, test3);
            AssertNode(test1, container, test1, null, test4, test4);
            AssertNode(test4, container, test1, test1, null, test4);
        }

        [TestMethod]
        public void ListLinks_Clear_ParentItems()
        {
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(0);
            var test3 = new Container<double>.Node(0);
            var test4 = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, test1, test2, test3, test4);
            container.Clear();
            AssertNode(test1, null, null);
            AssertNode(test2, null, null);
            AssertNode(test3, null, null);
            AssertNode(test4, null, null);
            AssertNode(container, null, null, null, null);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrevs_ParentEmpty_Enumerable()
        {
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            var container = new Container<double>().InsertPrevs(null, new[] { test1, test2 }.AsEnumerable());
            AssertNode(test1, container, test1, null, test2, test2);
            AssertNode(test2, container, test1, test1, null, test2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNexts_ParentEmpty_Enumerable()
        {
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            var container = new Container<double>().InsertNexts(null, new[] { test1, test2 }.AsEnumerable());
            AssertNode(test1, container, test1, null, test2, test2);
            AssertNode(test2, container, test1, test1, null, test2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrevs_ParentItem1_Enumerable()
        {
            var node = new Container<double>.Node(0);
            var container = new Container<double>().InsertPrevs(null, new[] { node }.AsEnumerable());
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertPrevs(node, new[] { test1, test2 }.AsEnumerable());
            AssertNode(test1, container, test1, null, test2, node);
            AssertNode(test2, container, test1, test1, node, node);
            AssertNode(node, container, test1, test2, null, node);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNexts_ParentItem1_Enumerable()
        {
            var node = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, new[] { node }.AsEnumerable());
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertNexts(node, new[] { test1, test2 }.AsEnumerable());
            AssertNode(node, container, node, null, test1, test2);
            AssertNode(test1, container, node, node, test2, test2);
            AssertNode(test2, container, node, test1, null, test2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrevs_ParentItem2Head_Enumerable()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertPrevs(null, new[] { node1, node2 }.AsEnumerable());
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertPrevs(container.Head, new[] { test1, test2 }.AsEnumerable());
            AssertNode(test1, container, test1, null, test2, node2);
            AssertNode(test2, container, test1, test1, node1, node2);
            AssertNode(node1, container, test1, test2, node2, node2);
            AssertNode(node2, container, test1, node1, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNexts_ParentItem2Head_Enumerable()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, new[] { node1, node2 }.AsEnumerable());
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertNexts(container.Head, new[] { test1, test2 }.AsEnumerable());
            AssertNode(node1, container, node1, null, test1, node2);
            AssertNode(test1, container, node1, node1, test2, node2);
            AssertNode(test2, container, node1, test1, node2, node2);
            AssertNode(node2, container, node1, test2, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrevs_ParentItem2Tail_Enumerable()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertPrevs(null, new[] { node1, node2 }.AsEnumerable());
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertPrevs(container.Tail, new[] { test1, test2 }.AsEnumerable());
            AssertNode(node1, container, node1, null, test1, node2);
            AssertNode(test1, container, node1, node1, test2, node2);
            AssertNode(test2, container, node1, test1, node2, node2);
            AssertNode(node2, container, node1, test2, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNexts_ParentItem2Tail_Enumerable()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, new[] { node1, node2 }.AsEnumerable());
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertNexts(container.Tail, new[] { test1, test2 }.AsEnumerable());
            AssertNode(node1, container, node1, null, node2, test2);
            AssertNode(node2, container, node1, node1, test1, test2);
            AssertNode(test1, container, node1, node2, test2, test2);
            AssertNode(test2, container, node1, test1, null, test2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertPrevs_ParentItem2_Enumerable()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertPrevs(null, new[] { node1, node2 }.AsEnumerable());
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertPrevs(null, new[] { test1, test2 }.AsEnumerable());
            AssertNode(test1, container, test1, null, test2, node2);
            AssertNode(test2, container, test1, test1, node1, node2);
            AssertNode(node1, container, test1, test2, node2, node2);
            AssertNode(node2, container, test1, node1, null, node2);
        }

        [TestMethod]
        public void ListLinks_ParentInsertNexts_ParentItem2_Enumerable()
        {
            var node1 = new Container<double>.Node(0);
            var node2 = new Container<double>.Node(0);
            var container = new Container<double>().InsertNexts(null, new[] { node1, node2 }.AsEnumerable());
            var test1 = new Container<double>.Node(0);
            var test2 = new Container<double>.Node(2);
            container.InsertNexts(null, new[] { test1, test2 }.AsEnumerable());
            AssertNode(node1, container, node1, null, node2, test2);
            AssertNode(node2, container, node1, node1, test1, test2);
            AssertNode(test1, container, node1, node2, test2, test2);
            AssertNode(test2, container, node1, test1, null, test2);
        }
        #endregion

        #region [ListLinks] Node Core Operations
        [TestMethod]
        public void ListLinksNode_CoreInsert_NodesEmpty()
        {
            var test = new Container<double>.Node(0).Attach(null, null);
            AssertNode(test, test, test);
        }

        [TestMethod]
        public void ListLinksNode_CoreInsert_NodesSingle()
        {
            var node1 = new Container<double>.Node(0).Attach(null, null);
            var test = new Container<double>.Node(2).Attach(node1, node1);
            AssertNode(test, node1, node1);
        }

        [TestMethod]
        public void ListLinksNode_CoreInsert_NodesAdjacentDirect()
        {
            var node1 = new Container<double>.Node(0).Attach(null, null);
            var node2 = new Container<double>.Node(2).Attach(node1, node1);
            var test = new Container<double>.Node(1).Attach(node1, node2);
            AssertNode(test, node1, node2);
        }

        [TestMethod]
        public void ListLinksNode_CoreInsert_NodesAdjacentReverse()
        {
            var node1 = new Container<double>.Node(0).Attach(null, null);
            var node2 = new Container<double>.Node(2).Attach(node1, node1);
            var test = new Container<double>.Node(1).Attach(node2, node1);
            AssertNode(test, node2, node1);
        }

        [TestMethod]
        public void ListLinksNode_CoreInsert_NodesSparseReverse()
        {
            var node1 = new Container<double>.Node(0).Attach(null, null);
            var node2 = new Container<double>.Node(2).Attach(node1, node1);
            var node3 = new Container<double>.Node(4).Attach(node2, node1);
            var test = new Container<double>.Node(1).Attach(node3, node1);
            AssertNode(test, node3, node1);
        }

        [TestMethod]
        public void ListLinksNode_CoreInsert_NodesSparseDirect_Fail()
        {
            var node1 = new Container<double>.Node(0).Attach(null, null);
            var node2 = new Container<double>.Node(2).Attach(node1, node1);
            var node3 = new Container<double>.Node(4).Attach(node2, node1);
            AssertError<ArgumentException>(
                () => new Container<double>.Node(1).Attach(node1, node3),
                e => e.Message.Contains("be adjacent", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinksNode_CoreInsert_NodesUnlinked_Fail()
        {
            AssertError<ArgumentException>(
                () => new Container<double>.Node(1).Attach(new Container<double>.Node(0), new Container<double>.Node(2)),
                e => e.Message.Contains("be adjacent", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinksNode_CoreInsert_NodesLinked_Fail()
        {
            var node1 = new Container<double>.Node(0).Attach(null, null);
            var node2 = new Container<double>.Node(2).Attach(node1, node1);
            AssertError<InvalidOperationException>(
                () => new Container<double>.Node(1).Attach(node1, node2).Attach(node1, node2),
                e => e.Message.Contains("is linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinksNode_CoreRemove_NodeSingle()
        {
            var node1 = new Container<double>.Node(0).Attach(null, null);
            AssertNode(node1.Detach(), null, null);
        }

        [TestMethod]
        public void ListLinksNode_CoreRemove_NodeDouble()
        {
            var node1 = new Container<double>.Node(0).Attach(null, null);
            var node2 = new Container<double>.Node(2).Attach(node1, node1);
            AssertNode(node2.Detach(), null, null);
            AssertNode(node1, node1, node1);
        }

        [TestMethod]
        public void ListLinksNode_CoreRemove_NodeAdjacent()
        {
            var node1 = new Container<double>.Node(0).Attach(null, null);
            var node2 = new Container<double>.Node(2).Attach(node1, node1);
            var node3 = new Container<double>.Node(4).Attach(node2, node1);
            AssertNode(node2.Detach(), null, null);
            AssertNode(node1, node3, node3);
        }

        [TestMethod]
        public void ListLinksNode_CoreRemove_NodeUnlinked_Fail()
        {
            AssertError<InvalidOperationException>(
                () => new Container<double>.Node(0).Detach(),
                e => e.Message.Contains("is not linked", StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region [ListLinks] Node Parented Operations
        [TestMethod]
        public void ListLinksNode_ParentInsertPrev_NullContainer_Fail()
        {
            AssertError<ArgumentNullException>(
                () => new Container<double>.Node(0).InsertPrev(null),
                e => e.ParamName == "parent");
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertNext_NullContainer_Fail()
        {
            AssertError<ArgumentNullException>(
                () => new Container<double>.Node(0).InsertNext(null),
                e => e.ParamName == "parent");
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertPrev_OriginUnlinked_Fail()
        {
            var container = new Container<double>();
            AssertError<InvalidOperationException>(
                () => new Container<double>.Node(0).InsertPrev(container, new Container<double>.Node(1)),
                e => e.Message.Contains("not linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertNext_OriginUnlinked_Fail()
        {
            var container = new Container<double>();
            AssertError<InvalidOperationException>(
                () => new Container<double>.Node(0).InsertNext(container, new Container<double>.Node(-1)),
                e => e.Message.Contains("not linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertPrev_NodeLinked_Fail()
        {
            var container = new Container<double>().InsertPrev(new Container<double>.Node(1));
            AssertError<InvalidOperationException>(
                () => new Container<double>.Node(0).InsertPrev(new Container<double>()).InsertPrev(container),
                e => e.Message.Contains("is linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertNext_NodeLinked_Fail()
        {
            var container = new Container<double>().InsertNext(new Container<double>.Node(1));
            AssertError<InvalidOperationException>(
                () => new Container<double>.Node(0).InsertNext(new Container<double>()).InsertPrev(container),
                e => e.Message.Contains("is linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertPrev_NodeLinked_Same()
        {
            var container = new Container<double>();
            var node = new Container<double>.Node(0).InsertPrev(container).InsertPrev(container);
            AssertNode(node, container, node, null, null, node);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertNext_NodeLinked_Same()
        {
            var container = new Container<double>();
            var node = new Container<double>.Node(0).InsertNext(container).InsertNext(container);
            AssertNode(node, container, node, null, null, node);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertPrev_OriginForeign_Fail()
        {
            var container = new Container<double>();
            AssertError<InvalidOperationException>(
                () => new Container<double>.Node(0).InsertPrev(container, new Container<double>.Node(1).InsertPrev(new Container<double>())),
                e => e.Message.Contains("is not linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertNext_OriginForeign_Fail()
        {
            var container = new Container<double>();
            AssertError<InvalidOperationException>(
                () => new Container<double>.Node(0).InsertNext(container, new Container<double>.Node(1).InsertNext(new Container<double>())),
                e => e.Message.Contains("is not linked", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertPrev_ParentEmpty()
        {
            var container = new Container<double>();
            var test = new Container<double>.Node(0).InsertPrev(container);
            AssertNode(test, container, test, null, null, test);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertNext_ParentEmpty()
        {
            var container = new Container<double>();
            var test = new Container<double>.Node(0).InsertNext(container);
            AssertNode(test, container, test, null, null, test);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertPrev_Parent1Item()
        {
            var container = new Container<double>();
            var node = new Container<double>.Node(0).InsertPrev(container);
            var test = new Container<double>.Node(1).InsertPrev(container, node);
            AssertNode(test, container, test, null, node, node);
            AssertNode(node, container, test, test, null, node);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertNext_Parent1Item()
        {
            var container = new Container<double>();
            var node = new Container<double>.Node(0).InsertNext(container);
            var test = new Container<double>.Node(1).InsertNext(container, node);
            AssertNode(node, container, node, null, test, test);
            AssertNode(test, container, node, node, null, test);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertPrev_Parent2Item()
        {
            var container = new Container<double>();
            var node2 = new Container<double>.Node(2).InsertPrev(container);
            var node1 = new Container<double>.Node(0).InsertPrev(container);
            var test = new Container<double>.Node(1).InsertPrev(container);
            AssertNode(test, container, test, null, node1, node2);
            AssertNode(node1, container, test, test, node2, node2);
            AssertNode(node2, container, test, node1, null, node2);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertNext_Parent2Item()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(2).InsertNext(container);
            var test = new Container<double>.Node(1).InsertNext(container);
            AssertNode(node1, container, node1, null, node2, test);
            AssertNode(node2, container, node1, node1, test, test);
            AssertNode(test, container, node1, node2, null, test);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertPrev_Parent2ItemHead()
        {
            var container = new Container<double>();
            var node2 = new Container<double>.Node(2).InsertPrev(container);
            var node1 = new Container<double>.Node(0).InsertPrev(container);
            var test = new Container<double>.Node(1).InsertPrev(container, node1);
            AssertNode(test, container, test, null, node1, node2);
            AssertNode(node1, container, test, test, node2, node2);
            AssertNode(node2, container, test, node1, null, node2);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertNext_Parent2ItemHead()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(2).InsertNext(container);
            var test = new Container<double>.Node(1).InsertNext(container, node1);
            AssertNode(node1, container, node1, null, test, node2);
            AssertNode(test, container, node1, node1, node2, node2);
            AssertNode(node2, container, node1, test, null, node2);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertPrev_Parent2ItemTail()
        {
            var container = new Container<double>();
            var node2 = new Container<double>.Node(2).InsertPrev(container);
            var node1 = new Container<double>.Node(0).InsertPrev(container);
            var test = new Container<double>.Node(1).InsertPrev(container, node2);
            AssertNode(node1, container, node1, null, test, node2);
            AssertNode(test, container, node1, node1, node2, node2);
            AssertNode(node2, container, node1, test, null, node2);
        }

        [TestMethod]
        public void ListLinksNode_ParentInsertNext_Parent2ItemTail()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(2).InsertNext(container);
            var test = new Container<double>.Node(1).InsertNext(container, node2);
            AssertNode(node1, container, node1, null, node2, test);
            AssertNode(node2, container, node1, node1, test, test);
            AssertNode(test, container, node1, node2, null, test);
        }

        [TestMethod]
        public void ListLinksNode_ParentRemove_Parent3ItemMiddle()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            AssertNode(node2.Remove(), null, null);
            AssertNode(node1, container, node1, null, node3, node3);
            AssertNode(node3, container, node1, node1, null, node3);
        }

        [TestMethod]
        public void ListLinksNode_ParentRemove_Parent3ItemHead()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            AssertNode(node1.Remove(), null, null);
            AssertNode(node2, container, node2, null, node3, node3);
            AssertNode(node3, container, node2, node2, null, node3);
        }

        [TestMethod]
        public void ListLinksNode_ParentRemove_Parent3ItemTail()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            AssertNode(node3.Remove(), null, null);
            AssertNode(node1, container, node1, null, node2, node2);
            AssertNode(node2, container, node1, node1, null, node2);
        }

        [TestMethod]
        public void ListLinksNode_ParentRemove_Parent2ItemHead()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            AssertNode(node1.Remove(), null, null);
            AssertNode(node2, container, node2, null, null, node2);
        }

        [TestMethod]
        public void ListLinksNode_ParentRemove_Parent2ItemTail()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            AssertNode(node2.Remove(), null, null);
            AssertNode(node1, container, node1, null, null, node1);
        }

        [TestMethod]
        public void ListLinksNode_ParentRemove_Parent1Item()
        {
            var container = new Container<double>();
            var node = new Container<double>.Node(0).InsertNext(container);
            AssertNode(node.Remove(), null, null);
        }

        [TestMethod]
        public void ListLinksNode_ParentRemove_Unlinked_Fail()
        {
            AssertError<InvalidOperationException>(
                () => new Container<double>.Node(0).Remove(),
                e => e.Message.Contains("is not linked", StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region [ListLinks] Incrementor Enumerations
        [TestMethod]
        public void ListLinks_Incrementor_Open()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetIncrement().ToList(), 0, 1, 2, 3, 4);
        }

        [TestMethod]
        public void ListLinks_Incrementor_OpenLower()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetIncrement(upper: node3).ToList(), 0, 1, 2);
        }

        [TestMethod]
        public void ListLinks_Incrementor_OpenUpper()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetIncrement(lower: node3).ToList(), 2, 3, 4);
        }

        [TestMethod]
        public void ListLinks_Incrementor_Middle()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetIncrement(node2, node4).ToList(), 1, 2, 3);
        }

        [TestMethod]
        public void ListLinks_Incrementor_MiddleLower()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetIncrement(node2, node5).ToList(), 1, 2, 3, 4);
        }

        [TestMethod]
        public void ListLinks_Incrementor_MiddleUpper()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetIncrement(node1, node4).ToList(), 0, 1, 2, 3);
        }
        #endregion

        #region [ListLinks] Decrementor Enumerations
        [TestMethod]
        public void ListLinks_Decrementor_Open()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetDecrement().ToList(), 4, 3, 2, 1, 0);
        }

        [TestMethod]
        public void ListLinks_Decrementor_OpenLower()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetDecrement(upper: node3).ToList(), 2, 1, 0);
        }

        [TestMethod]
        public void ListLinks_Decrementor_OpenUpper()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetDecrement(lower: node3).ToList(), 4, 3, 2);
        }

        [TestMethod]
        public void ListLinks_Decrementor_Middle()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetDecrement(node4, node2).ToList(), 3, 2, 1);
        }

        [TestMethod]
        public void ListLinks_Decrementor_MiddleLower()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetDecrement(node4, node1).ToList(), 3, 2, 1, 0);
        }

        [TestMethod]
        public void ListLinks_Decrementor_MiddleUpper()
        {
            var container = new Container<double>();
            var node1 = new Container<double>.Node(0).InsertNext(container);
            var node2 = new Container<double>.Node(1).InsertNext(container);
            var node3 = new Container<double>.Node(2).InsertNext(container);
            var node4 = new Container<double>.Node(3).InsertNext(container);
            var node5 = new Container<double>.Node(4).InsertNext(container);
            AssertSequence(container.GetDecrement(node5, node2).ToList(), 4, 3, 2, 1);
        }
        #endregion

        /// <summary>
        /// Performs random structure tests on linked list.
        /// </summary>
        [TestMethod]
        public void ListLinks_RandomStructure()
        {
            // Prepares random sample array to make assertions to
            Random generator = new Random();
            List<int> chainMap = new List<int>(new int[_dynamicLength]);
            for (int index = 0; index < _dynamicLength; index++)
                chainMap[index] = index;
            var origins = GenerateRandom<Container<double>.Node>(_dynamicLength);

            // Randomly create linked container in the correct order to match source arrays
            var container = new Container<double>();
            while (chainMap.Count > 0)
            {
                int baseIndex = generator.Next(chainMap.Count);
                int length = Math.Min(generator.Next(_dynamicBlock + 1), chainMap.Count - baseIndex);
                for (int index = 0; index < length - 1; index++)
                    if (chainMap[baseIndex + index] + 1 != chainMap[baseIndex + index + 1])
                        length = index + 1;
                for (int index = 0; index < length; index++)
                    Assert.IsNull(origins[chainMap[baseIndex + index]].Parent);
                var nodes = SubArray(origins, chainMap[baseIndex], length);
                if (container.Head == null && container.Tail == null)
                    switch (generator.Next(2))
                    {
                        case 0:
                            if (nodes.Length == 1 && generator.Next(2) == 0)
                                container.InsertPrev(nodes[0]);
                            else
                                container.InsertPrevs(null, nodes);
                            break;

                        case 1:
                            if (nodes.Length == 1 && generator.Next(2) == 0)
                                container.InsertNext(nodes[0]);
                            else
                                container.InsertNexts(null, nodes);
                            break;
                    }
                else
                {
                    Container<double>.Node lower = null, upper = null;
                    int lowerLimit = 0, upperLimit = chainMap.Count;
                    int lowerIndex = chainMap[baseIndex] - 1;
                    int upperIndex = chainMap[baseIndex] + length;
                    for (int index = lowerIndex; index >= lowerLimit; index--)
                        if (origins[index].Parent != null)
                            lower = origins[lowerLimit = index];
                    for (int index = upperIndex; index < upperLimit; index++)
                        if (origins[index].Parent != null)
                            upper = origins[upperLimit = index];
                    switch (generator.Next(2))
                    {
                        case 0:
                            if (upper != null)
                                if (nodes.Length == 1 && generator.Next(2) == 0)
                                    container.InsertPrev(nodes[0], upper);
                                else
                                    container.InsertPrevs(upper, nodes);
                            else
                                goto case 1;
                            break;
                        case 1:
                            if (lower != null)
                                if (nodes.Length == 1 && generator.Next(2) == 0)
                                    container.InsertNext(nodes[0], lower);
                                else
                                    container.InsertNexts(lower, nodes);
                            else
                                goto case 0;
                            break;
                    }
                }
                chainMap.RemoveRange(baseIndex, length);
            }

            // Test resulting list against the source array.
            int current = 0;
            foreach (var node in container.GetIncrement())
            {
                Assert.AreEqual(origins[current], node);
                Assert.AreEqual(origins[current++].Value, node.Value);
            }
            Assert.AreEqual(origins.Length, current);

            // Randomly remove all nodes from linked container.
            if (container.Count > 0)
            {
                int lowerIndex = generator.Next(_dynamicLength * 0 / 4, _dynamicLength * 1 / 4);
                int upperIndex = generator.Next(_dynamicLength * 3 / 4, _dynamicLength * 4 / 4);
                int indexLower = 0;
                while (indexLower < lowerIndex)
                {
                    int length = Math.Min(generator.Next(_dynamicBlock), lowerIndex - indexLower);
                    var nodes = SubArray(origins, indexLower, length);
                    if (nodes.Length == 1 && generator.Next(2) == 0)
                        container.Remove(nodes[0]);
                    else
                        container.Remove(nodes);
                    indexLower += length;
                }
                int indexUpper = upperIndex;
                while (indexUpper < _dynamicLength)
                {
                    int length = Math.Min(generator.Next(_dynamicBlock), _dynamicLength - indexUpper);
                    var nodes = SubArray(origins, indexUpper, length);
                    if (nodes.Length == 1 && generator.Next(2) == 0)
                        container.Remove(nodes[0]);
                    else
                        container.Remove(nodes);
                    indexUpper += length;
                }
            }
            container.Clear();

            // Test resulting container
            Assert.AreEqual(container.Count, 0);
        }
    }
#pragma warning restore CA1707 // Identifiers should not contain underscores
}