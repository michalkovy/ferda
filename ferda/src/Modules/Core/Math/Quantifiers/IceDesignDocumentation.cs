using Ferda.Guha.Data;

namespace Ferda.Guha.Math.Quantifiers.IceDesignDocumentation
{
    /// <summary>
    /// Key information for SD*-Miners. This specifies how
    /// the SD*-Miner will serve contingecy tables to quantifiers.
    /// </summary>
    public enum OperationModeEnum
    {
        /// <summary>
        /// Let quantifer function Q(contingecy_table), contingecy table
        /// for the first set is A, contingency table for second set is B than 
        /// studied value is Q(A).
        /// </summary>
        FirstSetFrequencies,

        /// <summary>
        /// Let quantifer function Q(contingecy_table), contingecy table
        /// for the first set is A, contingency table for second set is B than 
        /// studied value is Q(B).
        /// </summary>
        SecondSetFrequencies,

        /// <summary>
        /// Let quantifer function Q(contingecy_table), contingecy table
        /// for the first set is A, contingency table for second set is B than 
        /// studied value is Q(Abs(A - B)).
        /// </summary>
        DifferencesOfAbsoluteFrequencies,

        /// <summary>
        /// Let quantifer function Q(contingecy_table), contingecy table
        /// for the first set is A, contingency table for second set is B than 
        /// studied value is Q(Abs((A in percents) - (B in percents))).
        /// </summary>
        DifferencesOfRelativeFrequencies,

        /// <summary>
        /// Let quantifer function Q(contingecy_table), contingecy table
        /// for the first set is A, contingency table for second set is B than 
        /// studied value is (Q(A) - Q(B)).
        /// </summary>
        DifferenceOfQuantifierValues,

        /// <summary>
        /// Let quantifer function Q(contingecy_table), contingecy table
        /// for the first set is A, contingency table for second set is B than 
        /// studied value is Abs(Q(A) - Q(B)).
        /// </summary>
        AbsoluteDifferenceOfQuantifierValues
    }

    /// <summary>
    /// The quantifier tries to increase or decrease its value.
    /// Quantifier semantic is in the quantifier instance defined
    /// by the <c>Relation</c> property.
    /// </summary>
    /// <remarks>
    /// In the front of hypothesis will be quantifers sorted by the
    /// semantic of the quantifier form best to worse.
    /// </remarks>
    public enum QuantifierSemanticEnum
    {
        /// <summary>
        /// Relation used in quantifier is &lt;= or &lt;
        /// i. e. lesser value is better value.
        /// </summary>
        MinimizeValue,

        /// <summary>
        /// Relation used in quantifier is =&gt; or &gt;
        /// i. e. greater value is better value.
        /// </summary>
        MaximizeValue,

        /// <summary>
        /// Relation used in quantifier is ==
        /// i. e. only values equals to specified treshold are accepted.
        /// </summary>
        OnlyEquals
    }

    /// <summary>
    /// Qunatifier may be applied on some submatix of contingecy table.
    /// This enumeration specifies boundery which will be used to get 
    /// the sumbatrix.
    /// </summary>
    public enum BoundTypeEnum
    {
        /// <summary>
        /// Specified index is used in the dirrection. 
        /// See <see cref="F:Ferda.Guha.Math.Quantifiers.Bound.number"/>
        /// (<see cref="F:Ferda.Guha.Math.Quantifiers.IceDesignDocumentation.Bound.number"/>)
        /// </summary>
        Number,

        /// <summary>
        /// Default value ... contingecy table is not 
        /// cutted in the dirrection.
        /// </summary>
        All,

        /// <summary>
        /// Half (rounded down) ... contingecy table is cutted in
        /// the dirrecton to half.
        /// </summary>
        Half
    }

    /// <summary>
    /// Hold type of the boundary (in some dirrection) for specification 
    /// of submatrix of contingecy table to be used by the quantifier.
    /// </summary>
    public struct Bound
    {
        /// <summary>
        /// Specifies type of the boudary. Iff its equal to <c>Number</c>
        /// than specified <c>number</c> index is used.
        /// </summary>
        public BoundTypeEnum boundType;

        /// <summary>
        /// Counted form zero.
        /// </summary>
        public int number;
    }

    /// <summary>
    /// Quantifiers are divided to some classes by the <b>Truth preservation condition (TCP)</b>.
    /// TCP means: if quantifier is satsisfied for (four fold) contigency table 
    /// (a, b, c, d) than it is satisfied for (four fold) contingecy table 
    /// (a', b', c', d'). 
    /// </summary>
    public enum QuantifierClassEnum
    {
        /// <summary>
        /// Quantifier does not belong to any specified class.
        /// </summary>
        Unknown,

        /// <summary>
        /// TCP:
        /// <![CDATA[
        /// a' >= a
        /// b' <= b
        /// ]]>
        /// </summary>
        Implicational,

        /// <summary>
        /// TCP:
        /// <![CDATA[
        /// a' >= a
        /// b' <= b
        /// c' <= c
        /// ]]>
        /// </summary>
        DoubleImplicational,

        /// <summary>
        /// TCP:
        /// <![CDATA[
        /// a' >= a
        /// b' + c' <= b + c
        /// ]]>
        /// </summary>
        SigmaDoubleImplicational,

        /// <summary>
        /// TCP:
        /// <![CDATA[
        /// a' >= a
        /// b' <= b
        /// c' <= c
        /// d' >= d
        /// ]]>
        /// </summary>
        Equivalency,

        /// <summary>
        /// TCP:
        /// <![CDATA[
        /// a' + d' >= a + d
        /// b' + c' <= b + c
        /// ]]>
        /// </summary>
        SigmaEquivalency
    }

    /// <summary>
    /// Presumption of performance difficulty.
    /// </summary>
    public enum PerformanceDifficultyEnum
    {
        /// <summary>
        /// Few arithmetic operation or constant number of "easy" operations.
        /// </summary>
        Easy,

        /// <summary>
        /// Simple arithemtic operations.
        /// </summary>
        QuiteEasy,

        /// <summary>
        /// Default value
        /// </summary>
        Medium,

        /// <summary>
        /// Combinatorics
        /// </summary>
        QuiteDifficult,

        /// <summary>
        /// Plenty of combinatorics.
        /// </summary>
        Difficult
    } ;

    /// <summary>
    /// The *-Miner needs to know some information about the quantifier.
    /// </summary>
    public class QuantifierSetting
    {
        /// <summary>
        /// Key information for SD*-Miners. This specifies how
        /// the SD*-Miner will serve contingecy tables to quantifiers.
        /// </summary>
        OperationModeEnum operationMode;

        /// <summary>
        /// The quantifier tries to increase or decrease its value.
        /// Quantifier semantic is in the quantifier instance defined
        /// by the <c>Relation</c> property.
        /// </summary>
        QuantifierSemanticEnum quantifierSemantic;

        /// <summary>
        /// Specifies presumption of calculating the quantifier.
        /// Evaluating of quantifiers should be in order to save
        /// performance i.e. take into account "failure" of quantifiers,
        /// its performance requirements, ... (quantifier uses numeric values, ...).
        /// </summary>
        PerformanceDifficultyEnum quantifierPerformanceDifficulty;

        /// <summary>
        /// Indicates wheter the quantifier requires numeric values
        /// (some CF quantifiers).
        /// </summary>
        bool needsNumericValues;

        /// <summary>
        /// Quantifier expects only submatrix of contingency table
        /// of specified shape. (Default is All see <see cref="T:Ferda.Guha.Math.Quantifiers.IceDesignDocumentation.BoundTypeEnum"/>)
        /// </summary>
        Bound FromRow;

        Bound ToRow;
        Bound FromColumn;
        Bound ToColumn;

        /// <summary>
        /// Some quantifiers may to work only with ordinal data etc.
        /// (e.g. Kendal quantifier observe ordinal dependence of two attributes)
        /// </summary>
        CardinalityEnum supportedData;

        /// <summary>
        /// Specifies the classes of quantifiers, where this one belongs to.
        /// Classes are defined by <b>Truth preservation condition</b>.
        /// Please not that some classes are in inclusion so there will be 
        /// mentioned always the top class of inclusion, where this quantifier 
        /// belongs to.
        /// </summary>
        /// <remarks>
        /// Empty array means the same as <c>QuantifierClassEnum.Unknown</c>.
        /// </remarks>
        QuantifierClassEnum[] quantifierClasses;

        /// <summary>
        /// Some quantifiers does not support computation over float/double
        /// contingency table i.e. they needs integer values. (For Example 
        /// <c>Sum(i=a..a+b)(...)</c>)
        /// </summary>
        bool supportsFloatContingencyTable;
    }

    /// <summary>
    /// Contingency table is converted to specified units.
    /// E.g. if units are relative to number N than (new CT)ij = ((old CT)ij / N) * 100 == (old CT)ij * (100 / N)
    /// hence items in newly created (relative) contingecy table are
    /// from interval &lt;0; 100&gt;
    /// </summary>
    public enum UnitsEnum
    {
        /// <summary>
        /// Value of the quantifier does not depends on units.
        /// E.g. Q(CT) = (CT)00 / (CT)01
        /// </summary>
        Irrelevant,

        /// <summary>
        /// Contingecy table is useda as is i.e. no units conversion
        /// is applied.
        /// </summary>
        AbsoluteNumber,

        /// <summary>
        /// Contingecy table is relative to Sum of all items in current contingecy
        /// table. 
        /// E.g. for SD tasks the act codition is union of following conditions: 
        /// a) (global) condition; 
        /// b) first/second set specified condition.
        /// </summary>
        RelativeToActCondition,

        /// <summary>
        /// Contingency table is relative to number of all records in analysed 
        /// data.  
        /// </summary>
        //TODO co s missing information
        RelativeToAllObjects,

        /// <summary>
        /// Contingency table is relative to maximal frequency item inside.
        /// </summary>
        RelativeToMaxFrequency
    } ;
}