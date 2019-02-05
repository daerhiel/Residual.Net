namespace ResidualNet.Containers
{
    /// <summary>
    /// Represents the enumeration that define pussible insertion order states.
    /// </summary>
    public enum InsertionOrder
    {
        /// <summary>
        /// The node should be inserted as a precedent to the origin node.
        /// </summary>
        Precedent,

        /// <summary>
        /// The node should de inserted as a dependent to the origin node.
        /// </summary>
        Dependent
    }
}