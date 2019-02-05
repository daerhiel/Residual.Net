using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ResidualNet.Containers
{
    /// <summary>
    /// Represents the generic double linked collection. The types that require collection functionality should derive from this class.
    /// </summary>
    /// <typeparam name="TList">The type of a derived collection.</typeparam>
    /// <typeparam name="TNode">The type of a derived collection node.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class ListLinks<TList, TNode>
        where TList : ListLinks<TList, TNode>
        where TNode : ListLinks<TList, TNode>.Link
    {
        /// <summary>
        /// The incrementor enumerable for the double linked collection.
        /// </summary>
        protected IEnumerable<TNode> Increment => new Incrementor.Collection(this);

        /// <summary>
        /// The decrementor enumerable for the double linked collection.
        /// </summary>
        protected IEnumerable<TNode> Decrement => new Decrementor.Collection(this);

        /// <summary>
        /// The number of double linked nodes in the double linked collection.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// The first node in the double linked collection if it contains any; othwerise, null;
        /// </summary>
        public TNode Head { get; private set; }

        /// <summary>
        /// The last node in the double linked collection if it contains any; othwerise, null;
        /// </summary>
        public TNode Tail { get; private set; }

        /// <summary>
        /// Initializes the new instance of a double linked collection object.
        /// </summary>
        protected ListLinks()
        {
        }

        #region [Virtuals]: Notifications
        /// <summary>
        /// This method is called after the node is inserted into the double linked collection.
        /// </summary>
        /// <param name="node">The node to be inserted into the double linked list.</param>
        protected virtual void AfterInserting(TNode node)
        {
        }

        /// <summary>
        /// This method is called before the node is removed into the double linked collection.
        /// </summary>
        /// <param name="node">The node to be removed from the double linked list.</param>
        protected virtual void BeforeDeleting(TNode node)
        {
        }
        #endregion

        #region [Virtuals]: Parental forwarding
        /// <summary>
        /// Inserts the node into the double linked collection at the origin node if specified.
        /// </summary>
        /// <param name="node">The node to be inserted into the double linked list.</param>
        /// <param name="position">The preferred insertion order to use relative to the origin node.</param>
        /// <param name="origin">The origin node to use as a reference to insert the specified node at; the node will be inserted
        /// into the head if none is specified.</param>
        /// <returns>The current double linked collection that will contain the inserted node.</returns>
        protected virtual TList CoreInsert(TNode node, InsertPosition position, TNode origin = null)
        {
            if (node != null)
            {
                var list = (TList)this;
                node.CoreInsert(list, position, origin);
                return list;
            }
            else
                throw new ArgumentNullException(nameof(node));
        }

        /// <summary>
        /// Inserts the array of nodes into the double linked collection at the origin node if specified.
        /// </summary>
        /// <param name="position">The preferred insertion order to use relative to the origin node.</param>
        /// <param name="origin">The origin node to use as a reference to insert the specified node at; the node will be inserted
        /// into the head if none is specified.</param>
        /// <param name="nodes">The array of nodes to be inserted into the double linked list.</param>
        /// <returns>The current double linked collection that will contain the inserted nodes.</returns>
        protected virtual TList CoreInsert(InsertPosition position, TNode origin, params TNode[] nodes)
        {
            TNode root = null;
            var list = (TList)this;
            foreach (var node in nodes ?? Array.Empty<TNode>())
                if (node != null)
                {
                    if (root == null)
                        node.CoreInsert(list, position, origin);
                    else
                        node.CoreInsert(list, InsertPosition.Following, root);
                    root = node;
                }
            return list;
        }

        /// <summary>
        /// Inserts the enumerable sequence of nodes into the double linked collection at the origin node if specified.
        /// </summary>
        /// <param name="position">The preferred insertion order to use relative to the origin node.</param>
        /// <param name="origin">The origin node to use as a reference to insert the specified node at; the node will be inserted
        /// into the head if none is specified.</param>
        /// <param name="nodes">The sequence of nodes to be inserted into the double linked list.</param>
        /// <returns>The current double linked collection that will contain the inserted nodes.</returns>
        protected virtual TList CoreInsert(InsertPosition position, TNode origin, IEnumerable<TNode> nodes)
        {
            TNode root = null;
            var list = (TList)this;
            foreach (var node in nodes ?? Array.Empty<TNode>())
                if (node != null)
                {
                    if (root == null)
                        node.CoreInsert(list, position, origin);
                    else
                        node.CoreInsert(list, InsertPosition.Following, root);
                    root = node;
                }
            return list;
        }

        /// <summary>
        /// Removes the node from the double linked collection.
        /// </summary>
        /// <param name="node">The node to be removed from the double linked list.</param>
        /// <returns>The current double linked collection that contained the removed node.</returns>
        protected virtual TList CoreRemove(TNode node)
        {
            if (node != null)
            {
                node.CoreRemove();
                return (TList)this;
            }
            else
                throw new ArgumentNullException(nameof(node));
        }

        /// <summary>
        /// Removes the array of nodes from the double linked collection.
        /// </summary>
        /// <param name="nodes">The array of nodes to be removed from the double linked list.</param>
        /// <returns>The current double linked collection that contained the removed nodes.</returns>
        protected virtual TList CoreRemove(params TNode[] nodes)
        {
            var list = (TList)this;
            foreach (var node in nodes ?? Array.Empty<TNode>())
                node?.CoreRemove();
            return list;
        }

        /// <summary>
        /// Removes the sequence of nodes from the double linked collection.
        /// </summary>
        /// <param name="nodes">The sequence of nodes to be removed from the double linked list.</param>
        /// <returns>The current double linked collection that contained the removed nodes.</returns>
        protected virtual TList CoreRemove(IEnumerable<TNode> nodes)
        {
            var list = (TList)this;
            foreach (var node in nodes ?? Array.Empty<TNode>())
                node?.CoreRemove();
            return list;
        }

        /// <summary>
        /// Removes all existing nodes from the double linked collection.
        /// </summary>
        protected virtual void CoreClear()
        {
            foreach (var node in Increment)
                node.CoreRemove();
        }
        #endregion

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"Count = {Count}";

        /// <summary>
        /// Represents the generic double linked container node. The types that require the node functionality should derive from this class.
        /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
        public class Link
#pragma warning restore CA1034 // Nested types should not be visible
        {
            private TNode _prev;
            private TNode _next;

            /// <summary>
            /// The internal reference to a preceding node in the double linked collection.
            /// </summary>
            protected virtual TNode DirectPrev => _prev;

            /// <summary>
            /// The internal reference to a following node in the double linked collection.
            /// </summary>
            protected virtual TNode DirectNext => _next;

            /// <summary>
            /// The reference to a parent double linked collection.
            /// </summary>
            public TList Parent { get; private set; }

            /// <summary>
            /// The virtual reference to a preceding node in the double linked collection.
            /// </summary>
            public TNode Prev => Parent != null && _prev != Parent.Tail ? _prev : null;

            /// <summary>
            /// The virtual reference to a following node in the double linked collection.
            /// </summary>
            public TNode Next => Parent != null && _next != Parent.Head ? _next : null;

            #region [Virtuals]: Notifications
            /// <summary>
            /// This method is called after the node is inserted into the double linked collection.
            /// </summary>
            protected virtual void AfterInserting()
            {
            }

            /// <summary>
            /// This method is called before the node is removed into the double linked collection.
            /// </summary>
            protected virtual void BeforeDeleting()
            {
            }
            #endregion

            #region [Virtuals]: Linking Constraints
            /// <summary>
            /// Finds the pair of preceding and following nodes for insertion based on origin and insertion position.
            /// </summary>
            /// <param name="parent">The double linked list collection to use to insert the current node into.</param>
            /// <param name="origin">The origin node to use as a reference to insert the current node; the node will be inserted
            /// into the reference following position if none is specified.</param>
            /// <param name="preceding">The preceding node found to insert the current node after.</param>
            /// <param name="following">The following node found to insert the current node before.</param>
            /// <param name="position">The preferred insertion order to use relative to the origin node.</param>
            /// <returns>True if the correct insertion position is found; otherwise, false.</returns>
            protected virtual bool FindPosition(TList parent, ref TNode origin, out TNode preceding, out TNode following, ref InsertPosition position)
            {
                if ((Parent != parent) is bool isFound)
                    switch (position)
                    {
                        case InsertPosition.Preceding:
                            following = origin = origin ?? parent.Head;
                            preceding = following?._prev;
                            break;
                        case InsertPosition.Following:
                            preceding = origin = origin ?? parent.Tail;
                            following = preceding?._next;
                            break;
                        default:
                            throw new InvalidOperationException($"The insertion position is not supported: {position}.");
                    }
                else
                    origin = (TNode)this;
                return isFound;
            }
            #endregion

            #region [Virtuals]: Core Linking
            /// <summary>
            /// Attached the current node in between the preceding and following node into a double linked collection.
            /// </summary>
            /// <param name="preceding">The preceding node to insert the current node after.</param>
            /// <param name="following">The following node to insert the current node before.</param>
            /// <returns>The current node converted to actual node type when inserted.</returns>
            protected virtual TNode CoreAttach(TNode preceding, TNode following)
            {
                if (_next == null && _prev == null)
                    if ((preceding == null || preceding._next == following) && (following == null || following._prev == preceding))
                    {
                        var node = (TNode)this;
                        _prev = preceding ?? node;
                        _next = following ?? node;
                        _prev._next = node;
                        _next._prev = node;
                        return node;
                    }
                    else
                        throw new ArgumentException($"The nodes should be adjacent in double linked collection: {preceding}, {following}.");
                else
                    throw new InvalidOperationException($"Can't insert the node that is linked: {this}.");
            }

            /// <summary>
            /// Detaches the current node from a double linked collection and collapses conncetion between adjacent nodes.
            /// </summary>
            /// <returns>The current node converted to actual node type when removed.</returns>
            protected virtual TNode CoreDetach()
            {
                if (_next != null && _prev != null)
                {
                    var node = (TNode)this;
                    _prev._next = _next;
                    _next._prev = _prev;
                    _prev = _next = null;
                    return node;
                }
                else
                    throw new InvalidOperationException($"Can't remove the node that is not linked: {this}.");
            }
            #endregion

            #region [Virtuals]: Parental Linking
            /// <summary>
            /// Binds the node being inserted into the parent container according to the requested insertion position.
            /// </summary>
            /// <param name="parent">The double linked list collection to use to insert the current node into.</param>
            /// <param name="position">The preferred insertion order to use relative to the origin node.</param>
            /// <returns>The current node converted to actual node type when bound.</returns>
            protected TNode Bind(TList parent, InsertPosition position)
            {
                if (parent != null)
                    if (Parent == null)
                    {
                        Parent = parent;
                        if (Parent.Head == null || (position == InsertPosition.Preceding && parent.Head == _next))
                            Parent.Head = (TNode)this;
                        if (Parent.Tail == null || (position == InsertPosition.Following && Parent.Tail == _prev))
                            Parent.Tail = (TNode)this;
                        Parent.Count++;
                        Parent.AfterInserting((TNode)this);
                        AfterInserting();
                        return (TNode)this;
                    }
                    else
                        throw new InvalidOperationException($"Can't bind the node that is linked to another list: {this}.");
                else
                    throw new ArgumentNullException(nameof(parent));
            }

            /// <summary>
            /// Unbinds the node being removed from the parent container.
            /// </summary>
            /// <returns>The current node converted to actual node type when unbound.</returns>
            protected TNode Unbind()
            {
                if (Parent != null)
                {
                    BeforeDeleting();
                    Parent.BeforeDeleting((TNode)this);
                    Parent.Count--;
                    if (Parent.Head == this)
                        Parent.Head = _next != this ? _next : null;
                    if (Parent.Tail == this)
                        Parent.Tail = _prev != this ? _prev : null;
                    Parent = null;
                    return (TNode)this;
                }
                else
                    throw new InvalidOperationException($"Can't delete element that is not linked to any container: {this}.");
            }

            /// <summary>
            /// Inserts the current node to a double linked collection at the origin node and links it to a parent collection.
            /// </summary>
            /// <param name="parent">The double linked list collection to use to insert the current node into.</param>
            /// <param name="position">The preferred insertion order to use relative to the origin node.</param>
            /// <param name="origin">The origin node to use as a reference to insert the current node; the node will be inserted
            /// into the reference following position if none is specified.</param>
            /// <returns>The current node converted to actual node type when inserted.</returns>
            protected internal virtual TNode CoreInsert(TList parent, InsertPosition position, TNode origin = null)
            {
                if (parent != null)
                    if (Parent == null || parent == Parent)
                        if (origin == null || parent == origin.Parent)
                            if (FindPosition(parent, ref origin, out var preceding, out var following, ref position))
                                return CoreAttach(preceding, following).Bind(parent, position);
                            else
                                return origin;
                        else
                            throw new InvalidOperationException($"The origin node is not linked to requested list: {origin}.");
                    else
                        throw new InvalidOperationException($"Can't insert the node that is linked to another list: {this}.");
                else
                    throw new ArgumentNullException(nameof(parent));
            }

            /// <summary>
            /// Removes the current node from a double linked collection and unlinks it from a parent collection.
            /// </summary>
            /// <returns>The current node converted to actual node type when removed.</returns>
            protected internal virtual TNode CoreRemove()
            {
                if (Parent != null)
                    return Unbind().CoreDetach();
                else
                    throw new InvalidOperationException($"Can't delete element that is not linked to any container: {this}.");
            }
            #endregion

            #region [Static] Value Coersion
            /// <summary>
            /// Coarses the insertion node adjacency schema when removing an source node from the parent container.
            /// </summary>
            /// <param name="source">The origin node to use as a reference to insert the current node; the node will be inserted
            /// into the reference following position if none is specified.</param>
            /// <param name="preceding">The preceding node that will be used as an insertion position.</param>
            /// <param name="following">The following node that will be used as an insertion position.</param>
            /// <returns>The actual node that should be removed from the parent container.</returns>
            protected static TNode Coerse(ref TNode source, ref TNode preceding, ref TNode following)
            {
                switch (source)
                {
                    case TNode reference when reference == following && reference == preceding:
                        source = preceding = following = null;
                        return reference;
                    case TNode reference when reference == following:
                        following = source = following._next;
                        preceding = following._prev == reference ? reference._prev : following._prev;
                        return reference;
                    case TNode reference when reference == preceding:
                        preceding = source = preceding._prev;
                        following = preceding._next == reference ? reference._next : preceding._next;
                        return reference;
                    default:
                        return source;
                }
            }

            /// <summary>
            /// Coarses the insertion node adjacency schema when inserting a target node relative to a source node into the parent container.
            /// </summary>
            /// <param name="source">The origin node to use as a reference to insert the current node; the node will be inserted
            /// into the reference following position if none is specified.</param>
            /// <param name="target">The new node that should coarsed relative to an origin node.</param>
            /// <param name="preceding">The preceding node that will be used as an insertion position.</param>
            /// <param name="following">The following node that will be used as an insertion position.</param>
            /// <returns>The actual node that is inserted into the parent container.</returns>
            protected static TNode Coerse(TNode source, TNode target, ref TNode preceding, ref TNode following)
            {
                if (preceding?._next == target && following?._prev == target)
                    switch (source)
                    {
                        case TNode reference when reference == preceding:
                            following = target;
                            break;
                        case TNode reference when reference == following:
                            preceding = target;
                            break;
                    }
                return target;
            }
            #endregion
        }

        /// <summary>
        /// Represents the incrementing enumerator for a double linked collection.
        /// </summary>
        protected sealed class Incrementor : IEnumerator<TNode>
        {
            private readonly ListLinks<TList, TNode> _collection;
            private readonly TNode _lower;
            private readonly TNode _upper;
            private TNode _current;
            private TNode _node;
            private bool _disposed;

            #region [IEnumerator]: Enumerator Implementation
            object IEnumerator.Current =>
                _node != null || _current != null ? _current :
                throw new InvalidOperationException("The enumerator needs to be reset.");

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            public TNode Current =>
                _node != null || _current != null ? _current :
                throw new InvalidOperationException("The enumerator needs to be reset.");
            #endregion

            /// <summary>
            /// Initializes the new instance of an incrementing enumerator for a double linked collection.
            /// </summary>
            /// <param name="collection">The double linked collection to enumerate.</param>
            /// <param name="lower">The lower node to use as a reference for incremental enumeration.</param>
            /// <param name="upper">The upper node to use as a reference for incremental enumeration.</param>
            internal Incrementor(ListLinks<TList, TNode> collection, TNode lower = null, TNode upper = null)
            {
                if (collection != null)
                {
                    _collection = collection;
                    _lower = _node = lower ?? _collection.Head;
                    _upper = upper ?? _collection.Tail;
                    _current = null;
                }
            }

            #region [IEnumerator]: Enumerator Implementation
            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>True if the enumerator was successfully advanced to the next element; false if the enumerator has
            /// passed the end of the collection.</returns>
            bool IEnumerator.MoveNext()
            {
                if (_current != _upper && _node != null)
                {
                    _current = _node;
                    _node = _node.Next;
                    return true;
                }
                else
                    return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            void IEnumerator.Reset()
            {
                _node = _lower;
                _current = null;
            }
            #endregion

            #region [IDisposable]: Disposable Pattern Implementation
            /// <summary>
            /// Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disposing">True if managed resources should be released; otherwise, false.</param>
            private void Dispose(bool disposing)
            {
                if (!_disposed)
                    _disposed = true;
            }

            /// <summary>
            /// Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
            }
            #endregion

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString() => $"[{_collection}]/{(_node != null || _current != null ? _current : null)}, Active={_node != null}";

            /// <summary>
            /// The factory for the incrementing enumerator for a double linked collection.
            /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
            public class Collection : IEnumerable<TNode>
#pragma warning restore CA1034 // Nested types should not be visible
            {
                private readonly ListLinks<TList, TNode> _collection;
                private readonly TNode _lower;
                private readonly TNode _upper;

                /// <summary>
                /// Initializes the new instance of an incrementing enumerable for a double linked collection.
                /// </summary>
                /// <param name="collection">The double linked collection to enumerate.</param>
                public Collection(ListLinks<TList, TNode> collection)
                {
                    _collection = collection;
                }

                /// <summary>
                /// Initializes the new instance of an incrementing enumerable for a double linked collection.
                /// </summary>
                /// <param name="collection">The double linked collection to enumerate.</param>
                /// <param name="lower">The lower node to use as a reference for incremental enumeration.</param>
                /// <param name="upper">The upper node to use as a reference for incremental enumeration.</param>
                public Collection(ListLinks<TList, TNode> collection, TNode lower = null, TNode upper = null)
                {
                    _collection = collection;
                    if (lower == null || lower.Parent == collection)
                        _lower = lower;
                    else
                        throw new ArgumentException($"The lower node is not linked to requested list: {lower}.", nameof(lower));
                    if (upper == null || upper.Parent == collection)
                        _upper = upper;
                    else
                        throw new ArgumentException($"The upper node is not linked to requested list: {upper}.", nameof(upper));
                }

                #region [IEnumerable]: Enumerable Implementation
                IEnumerator IEnumerable.GetEnumerator() => new Incrementor(_collection, _lower, _upper);

                /// <summary>
                /// Returns an enumerator that iterates through the collection.
                /// </summary>
                /// <returns>An enumerator that can be used to iterate through the collection.</returns>
                public IEnumerator<TNode> GetEnumerator() => new Incrementor(_collection, _lower, _upper);
                #endregion

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString()
                {
                    return string.Join(", ", this.Select(x => x.ToString()).ToArray());
                }
            }
        }

        /// <summary>
        /// Represents the decrementing enumerator for a double linked collection.
        /// </summary>
        protected sealed class Decrementor : IEnumerator<TNode>
        {
            private readonly ListLinks<TList, TNode> _collection;
            private readonly TNode _lower;
            private readonly TNode _upper;
            private TNode _current;
            private TNode _node;
            private bool _disposed;

            #region [IEnumerator]: Enumerator Implementation
            object IEnumerator.Current =>
                _node != null || _current != null ? _current :
                throw new InvalidOperationException("The enumerator needs to be reset.");

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            public TNode Current =>
                _node != null || _current != null ? _current :
                throw new InvalidOperationException("The enumerator needs to be reset.");
            #endregion

            /// <summary>
            /// Initializes the new instance of a decrementing enumerator for a double linked collection.
            /// </summary>
            /// <param name="collection">The double linked collection to enumerate.</param>
            /// <param name="lower">The lower node to use as a reference for incremental enumeration.</param>
            /// <param name="upper">The upper node to use as a reference for incremental enumeration.</param>
            internal Decrementor(ListLinks<TList, TNode> collection, TNode upper = null, TNode lower = null)
            {
                if (collection != null)
                {
                    _collection = collection;
                    _lower = lower ?? _collection.Head;
                    _upper = _node = upper ?? _collection.Tail;
                    _current = null;
                }
            }

            #region [IEnumerator]: Enumerator Implementation
            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>True if the enumerator was successfully advanced to the next element; false if the enumerator has
            /// passed the end of the collection.</returns>
            bool IEnumerator.MoveNext()
            {
                if (_current != _lower && _node != null)
                {
                    _current = _node;
                    _node = _node.Prev;
                    return true;
                }
                else
                    return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            void IEnumerator.Reset()
            {
                _node = _upper;
                _current = null;
            }
            #endregion

            #region [IDisposable]: Disposable Pattern Implementation
            /// <summary>
            /// Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disposing">True if managed resources should be released; otherwise, false.</param>
            private void Dispose(bool disposing)
            {
                if (!_disposed)
                    _disposed = true;
            }

            /// <summary>
            /// Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
            }
            #endregion

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString() => $"[{_collection}]/{(_node != null || _current != null ? _current : null)}, Active={_node != null}";

            /// <summary>
            /// The factory for the decrementing enumerator for a double linked collection.
            /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
            public class Collection : IEnumerable<TNode>
#pragma warning restore CA1034 // Nested types should not be visible
            {
                private readonly ListLinks<TList, TNode> _collection;
                private readonly TNode _lower;
                private readonly TNode _upper;

                /// <summary>
                /// Initializes the new instance of a decrementing enumerable for a double linked collection.
                /// </summary>
                /// <param name="collection">The double linked collection to enumerate.</param>
                public Collection(ListLinks<TList, TNode> collection)
                {
                    _collection = collection;
                }

                /// <summary>
                /// Initializes the new instance of an decrementing enumerable for a double linked collection.
                /// </summary>
                /// <param name="collection">The double linked collection to enumerate.</param>
                /// <param name="upper">The upper node to use as a reference for incremental enumeration.</param>
                /// <param name="lower">The lower node to use as a reference for incremental enumeration.</param>
                public Collection(ListLinks<TList, TNode> collection, TNode upper = null, TNode lower = null)
                {
                    _collection = collection;
                    if (lower == null || lower.Parent == collection)
                        _lower = lower;
                    else
                        throw new ArgumentException($"The lower node is not linked to requested list: {lower}.", nameof(lower));
                    if (upper == null || upper.Parent == collection)
                        _upper = upper;
                    else
                        throw new ArgumentException($"The upper node is not linked to requested list: {upper}.", nameof(upper));
                }

                #region [IEnumerable]: Enumerable Implementation
                IEnumerator IEnumerable.GetEnumerator() => new Decrementor(_collection, _upper, _lower);

                /// <summary>
                /// Returns an enumerator that iterates through the collection.
                /// </summary>
                /// <returns>An enumerator that can be used to iterate through the collection.</returns>
                public IEnumerator<TNode> GetEnumerator() => new Decrementor(_collection, _upper, _lower);
                #endregion

                /// <summary>
                /// Returns a string that represents the current object.
                /// </summary>
                /// <returns>A string that represents the current object.</returns>
                public override string ToString() => string.Join(", ", this.Select(x => x.ToString()).ToArray());
            }
        }
    }
}