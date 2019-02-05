using System;

namespace ResidualNet.Containers
{
    /// <summary>
    /// Represents the generic double linked ordered collection. The types that require collection functionality should derive from this class.
    /// </summary>
    /// <typeparam name="TList">The type of a derived ordered collection.</typeparam>
    /// <typeparam name="TNode">The type of a derived ordered collection node.</typeparam>
    public class OrderedListLinks<TList, TNode> : ListLinks<TList, TNode>
        where TList : OrderedListLinks<TList, TNode>
        where TNode : OrderedListLinks<TList, TNode>.OrderedLink
    {
        /// <summary>
        /// Represents the generic double linked ordered container node. The types that require the node functionality should derive from this class.
        /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
        public abstract class OrderedLink : Link
#pragma warning restore CA1034 // Nested types should not be visible
        {
            /// <summary>
            /// Compares and processes the current node with another an origin node and returns an integer that indicates whether the current
            /// instance precedes, follows, or occurs in the same position in the sort order as the other object. The preceding and following
            /// nodes will also be adjusted according to the result of a comparison.
            /// </summary>
            /// <param name="origin">An origin node to compare with the current instance.</param>
            /// <param name="preceding">The preceding node found to compare the current node with.</param>
            /// <param name="following">The following node found to compare the current node with.</param>
            /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less
            /// than zero This instance precedes other in the sort order. Zero This instance occurs in the same position in the sort order as other.
            /// Greater than zero This instance follows other in the sort order.</returns>
            protected abstract int CompareTo(ref TNode origin, ref TNode preceding, ref TNode following);

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
            protected override bool FindPosition(TList parent, ref TNode origin, out TNode preceding, out TNode following, ref InsertPosition position)
            {
                if (base.FindPosition(parent, ref origin, out preceding, out following, ref position) is bool isFound && origin != null)
                {
                    var reference = CompareTo(ref origin, ref preceding, ref following);
                    while (origin != null)
                        switch (reference)
                        {
                            case int last when last < 0:
                                preceding = (following = origin).DirectPrev;
                                switch (CompareTo(ref preceding, ref preceding, ref following))
                                {
                                    case int next when next > 0: default: origin = null; break;
                                    case int next when next < 0: origin = origin.Prev; reference = next; break;
                                }
                                position = InsertPosition.Preceding;
                                break;
                            case int last when last > 0:
                                following = (preceding = origin).DirectNext;
                                switch (CompareTo(ref following, ref preceding, ref following))
                                {
                                    case int next when next > 0: origin = origin.Next; reference = next; break;
                                    case int next when next < 0: default: origin = null; break;
                                }
                                position = InsertPosition.Following;
                                break;
                            default: origin = null; break;
                        }
                }
                return isFound;
            }
            #endregion
        }
    }
}