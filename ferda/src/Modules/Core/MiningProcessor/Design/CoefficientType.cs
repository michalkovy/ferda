namespace Design
{
    /// <summary>
    /// Enumerates available types of coefficients.
    /// </summary>
    /// <remarks>
    /// The order of defined types is important, because it is used for sorting
    /// and merging of coefficients. More general coefficients are placed first.
    /// </remarks>
    public enum CoefficientType
    {
        /// <summary>
        /// Coefficient is constructed as an arbitrary subset of categories.
        /// Can be used for both ordered and unordered categories.
        /// </summary>
        Subsets,

        /// <summary>
        /// Coefficient is constructed as a cyclic interval from ordered categories.
        /// Useful for days of a week or months in a year.
        /// Cannot be used for unordered categories.
        /// </summary>
        CyclicIntervals,

        /// <summary>
        /// Coefficient is constructed as an interval from ordered categories.
        /// Cannot be used for unordered categories.
        /// </summary>
        Intervals,

        /// <summary>
        /// Coefficient is constructed as left or right cuts from ordered categories.
        /// Cuts are intervals that include either minimum value or maximum value.
        /// Cannot be used for unordered categories.
        /// </summary>
        Cuts,

        /// <summary>
        /// Coefficient is constructed as left cuts from ordered categories.
        /// Left cuts are intervals that include minimum value.
        /// Cannot be used for unordered categories.
        /// </summary>
        LeftCuts,

        /// <summary>
        /// Coefficient is constructed as right cuts from ordered categories.
        /// Right cuts are intervals that include maximum value.
        /// Cannot be used for unordered categories.
        /// </summary>
        RightCuts,
    }
}