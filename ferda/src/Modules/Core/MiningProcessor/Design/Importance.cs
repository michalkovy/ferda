namespace Design
{
    /// <summary>
    /// Enumerates possible importance levels.
    /// </summary>
    public enum Importance
    {
        /// <summary>
        /// Forced entity must be present in every cedent.
        /// </summary>
        Forced,

        /// <summary>
        /// Basic entity can be used freely. It may or may not 
        /// be present in cedent. This is default.
        /// </summary>
        Basic,

        /// <summary>
        /// Auxiliary entity can be used only if there is some 
        /// other forced or basic entity. In other words, there 
        /// cannot be created entity instance from auxiliary entity 
        /// only, there must be at least one basic entity present.
        /// </summary>
        Auxiliary,
    }
}