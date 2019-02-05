using System;
using System.Collections.Generic;

namespace ResidualNet.Containers.Tests.Models
{
    public class OrderedContainer<T> : OrderedListLinks<OrderedContainer<T>, OrderedContainer<T>.OrderedNode>
        where T : IComparable<T>
    {
        public IEnumerable<OrderedNode> GetIncrement(OrderedNode lower = null, OrderedNode upper = null) => new Incrementor.Collection(this, lower, upper);
        public IEnumerable<OrderedNode> GetDecrement(OrderedNode upper = null, OrderedNode lower = null) => new Decrementor.Collection(this, upper, lower);
        public OrderedContainer<T> InsertPrev(OrderedNode node, OrderedNode origin = null) => CoreInsert(node, InsertPosition.Preceding, origin);
        public OrderedContainer<T> InsertPrevs(OrderedNode origin, params OrderedNode[] nodes) => CoreInsert(InsertPosition.Preceding, origin, nodes);
        public OrderedContainer<T> InsertPrevs(OrderedNode origin, IEnumerable<OrderedNode> nodes) => CoreInsert(InsertPosition.Preceding, origin, nodes);
        public OrderedContainer<T> InsertNext(OrderedNode node, OrderedNode origin = null) => CoreInsert(node, InsertPosition.Following, origin);
        public OrderedContainer<T> InsertNexts(OrderedNode origin, params OrderedNode[] nodes) => CoreInsert(InsertPosition.Following, origin, nodes);
        public OrderedContainer<T> InsertNexts(OrderedNode origin, IEnumerable<OrderedNode> nodes) => CoreInsert(InsertPosition.Following, origin, nodes);
        public OrderedContainer<T> Remove(OrderedNode node) => CoreRemove(node);
        public OrderedContainer<T> Remove(params OrderedNode[] nodes) => CoreRemove(nodes);
        public void Clear() => CoreClear();

#pragma warning disable CA1034 // Nested types should not be visible
        public class OrderedNode : OrderedLink
#pragma warning restore CA1034 // Nested types should not be visible
        {
            public T Value { get; }
            public OrderedNode InternalPrev => DirectPrev;
            public OrderedNode InternalNext => DirectNext;

            public OrderedNode(T value)
            {
                Value = value;
            }

            protected override int CompareTo(ref OrderedNode origin, ref OrderedNode preceding, ref OrderedNode following) => Value.CompareTo(origin.Value);
            public OrderedNode Attach(OrderedNode prev, OrderedNode next) => CoreAttach(prev, next);
            public OrderedNode Detach() => CoreDetach();
            public OrderedNode InsertPrev(OrderedContainer<T> parent, OrderedNode origin = null) => CoreInsert(parent, InsertPosition.Preceding, origin);
            public OrderedNode InsertNext(OrderedContainer<T> parent, OrderedNode origin = null) => CoreInsert(parent, InsertPosition.Following, origin);
            public OrderedNode Remove() => CoreRemove();
            public override string ToString() => Value.ToString();
        }
    }
}