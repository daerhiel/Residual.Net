using System;
using System.Collections.Generic;

namespace ResidualNet.Containers.Tests.Models
{
    public class Container<T> : ListLinks<Container<T>, Container<T>.Node>
    {
        public IEnumerable<Node> GetIncrement(Node lower = null, Node upper = null) => new Incrementor.Collection(this, lower, upper);
        public IEnumerable<Node> GetDecrement(Node upper = null, Node lower = null) => new Decrementor.Collection(this, upper, lower);
        public Container<T> InsertPrev(Node node, Node origin = null) => CoreInsert(node, InsertPosition.Preceding, origin);
        public Container<T> InsertPrevs(Node origin, params Node[] nodes) => CoreInsert(InsertPosition.Preceding, origin, nodes);
        public Container<T> InsertPrevs(Node origin, IEnumerable<Node> nodes) => CoreInsert(InsertPosition.Preceding, origin, nodes);
        public Container<T> InsertNext(Node node, Node origin = null) => CoreInsert(node, InsertPosition.Following, origin);
        public Container<T> InsertNexts(Node origin, params Node[] nodes) => CoreInsert(InsertPosition.Following, origin, nodes);
        public Container<T> InsertNexts(Node origin, IEnumerable<Node> nodes) => CoreInsert(InsertPosition.Following, origin, nodes);
        public Container<T> Remove(Node node) => CoreRemove(node);
        public Container<T> Remove(params Node[] nodes) => CoreRemove(nodes);
        public void Clear() => CoreClear();

#pragma warning disable CA1034 // Nested types should not be visible
        public class Node : Link
#pragma warning restore CA1034 // Nested types should not be visible
        {
            public T Value { get; }
            public Node InternalPrev => DirectPrev;
            public Node InternalNext => DirectNext;

            public Node(T value)
            {
                Value = value;
            }

            public Node Attach(Node prev, Node next) => CoreAttach(prev, next);
            public Node Detach() => CoreDetach();
            public Node InsertPrev(Container<T> parent, Node origin = null) => CoreInsert(parent, InsertPosition.Preceding, origin);
            public Node InsertNext(Container<T> parent, Node origin = null) => CoreInsert(parent, InsertPosition.Following, origin);
            public Node Remove() => CoreRemove();
            public override string ToString() => Value.ToString();
        }
    }
}