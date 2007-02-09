#if DEBUG
using Ferda.Guha.Data;

namespace Ferda.Guha.Math.Quantifiers.IceDesignDocumentation
{
    /// <summary>
    /// Specifies type of handling missing information in analyzed data.
    /// Now is supported deleting for all tasks and specially for 4ft 
    /// (but not SD) tasks is allowed other setting.
    /// <para>
    /// There are 3 possible handling of incomplete information in GUHA
    /// (as defined in Tomáš Kuchaø: Experimentální GUHA procedury, Diplomová
    /// práce, Matematicko-fyzikální fakulta, Univerzita Karlova, 2006) part 8.3        
    /// </para>
    /// </summary>
    public enum MissingInformationHandlingEnum
    {
        /// <summary>
        /// Deletion of objects where analyzed data are not specified 
        /// i.e. they are missing.
        /// </summary>
        Deleting,

        /// <summary>
        /// Optimistic completion of missing information.
        /// </summary>
        Optimistic,

        /// <summary>
        /// Secured completion of missing infomration.
        /// </summary>
        Secured
    }

    ///<summary>
    /// Determines the operation mode for the SD quantifiers
        
    /// By setting of the operation mode, the quantifier can be used both
    /// for the SD and nonSD tasks. 
    /// SD4FT, SDKL and SDCF tasks examine differences between two disjunct
    /// sets determined by boolean attributes. There are contingency tables
    /// created above each of the sets and subsequently, the quantifier 
    /// examines the differences in the tables.
    ///</summary>
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
        /// studied value is Q(Abs(A - B)). (Both A and B are converted by specifed units.)
        /// </summary>
        DifferenceOfFrequencies,

        /// <summary>
        /// Let quantifer function Q(contingecy_table), contingecy table
        /// for the first set is A, contingency table for second set is B than 
        /// studied value is (Q(A) - Q(B)). (Both A and B are converted by specifed units.)
        /// </summary>
        DifferenceOfQuantifierValues,
    }

    /// <summary>
    /// Some quantifiers has defined value (e.g. Sum in KL-quantifier),
    /// some quantifiers has not defined value but other significant
    /// value can be generated (see FunctionOfRowEachRow or FunctionOfRow).
    /// And of course there are also quantifiers with has neither
    /// defined value nor other sighnificant value (see E-Quantifier). 
    /// </summary>
    /// <remarks>
    /// This enum is obsolete in the current version
    /// </remarks>
    enum EvaluationPropertiesEnum
    {
        /// <summary>
        /// Quantifiers that has defined its value. (Most of quantifiers has 
        /// been modified for this purpose e.g. Fouded Implication has in definition
        /// condition on p and Base, the Base condition has been removed to separately
        /// quantifier and thats why we can speculates value of FI as (a/a+b) which 
        /// can be compared with user specified p parameter.)
        /// </summary>
        HasValue,

        /// <summary>
        /// Some quantifiers has not defined value but other significant
        /// value can be generated. As example you can see documentation for
        /// following (Two-dimensional i.e. KL) quantifeirs: 
        /// FunctionOfRowEachRow, FunctionOfRow.
        /// </summary>
        GeneratesSignificantNumber,

        /// <summary>
        /// Some quantifiers does not provide significant numeric value.
        /// E.g. let us have quantifier, which checks following conditions:
        /// <![CDATA[
        /// b / (a + b) >= Treshold
        /// c / (c + d) >= Treshold
        /// ]]>
        /// What can be interpeted as value of quantifier in this case?
        /// </summary>
        HasNoValue
    };

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
    /// <para>
    /// In some cases, user could restrict the size of contingency table
    /// (for categorial attributes used in KL and CF procedures). 
    /// User (or programmer) determines the size of contingency table
    /// by setting this enum. When the 'All' option is selected, the whole
    /// contingency table is taken into consideration. When the 'Half' option
    /// is selected, the table is split into two parts. When the 'Number'
    /// option is selected, user selects the starting and ending number of
    /// rows and columns.
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
    /// Defines the classes of quantifiers. The classes of quantifiers have
    /// similar properties. The classes are defined in: 
    /// Rauch J. (2005): Logic of Association Rules, Applied Inteligence, 
    /// Vol. 22, Issue 1, pp. 9-28
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
        SigmaEquivalency,

        /// <summary>
        /// Let us have Phi, Psi and Cond are boolean attributes 
        /// than quantifier ~ is symetrical if Phi~Psi/Cond is true
        /// if and only if Psi~Phi/Cond is true.
        /// </summary>
        Symetrical,

        /// <summary>
        /// Have the same improtant theoretical properties as 
        /// Fisher`s quantifier. 
        /// </summary>
        FPropertyQuantifier
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
    /// Contingency table is converted to specified units.
    /// E.g. if units are relative to number N than (new CT)ij = ((old CT)ij / N) * 100 == (old CT)ij * (100 / N)
    /// hence items in newly created (relative) contingecy table are
    /// from interval &lt;0; 100&gt;
    /// </summary>
    public enum UnitsEnum
    {
        /// <summary>
        /// Value of the quantifier does not depends on units.
        /// E.g. Q(CT) = CT[0, 0] / CT[0, 1]
        /// </summary>
        Irrelevant,

        /// <summary>
        /// Contingecy table is used as is i.e. no units conversion
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
        /// data even object with missing information in analyzed attributes.
        /// </summary>
        RelativeToAllObjects,

        /// <summary>
        /// Contingency table is relative to maximal frequency item inside.
        /// </summary>
        RelativeToMaxFrequency
    } ;

    /// <summary>
    /// The *-Miner needs to know some information about the quantifier.
    /// </summary>
    public class QuantifierSetting
    {
        /// <summary>
        /// Ice identity of the quantifier box module. TODO WHY NEEDED?
        /// </summary>
        string stringIceIdentity;

        /// <summary>
        /// Specifies type of handling missing information in analyzed data.
        /// </summary>
        MissingInformationHandlingEnum missingInformationHandling;

        /// <summary>
        /// Key information for SD*-Miners. This specifies how
        /// the SD*-Miner will serve contingecy tables to quantifiers.
        /// </summary>
        OperationModeEnum operationMode;

        /// <summary>
        /// This describes the semantic of values returned by 
        /// quantifier (if any is returned). This property is fictive
        /// i.e. this property is not in fact definec in slice and passed
        /// by quantifier to generator. But it can be equivalently determined
        /// from implemented quantifier function interface.
        /// </summary>
        EvaluationPropertiesEnum evaluationProperty;

        /// <summary>
        /// Quantifiers with defined values often tries minimalize or
        /// maximalize the value. Only values in the <c>relation</c> to specified
        /// <c>treshold</c> can be interpreted as true.
        /// </summary>
        Ferda.Guha.Math.RelationEnum relation;

        /// <summary>
        /// This makes sense only if 
        /// </summary>
        double treshold;

        /// <summary>
        /// Quantifier expects only submatrix of contingency table
        /// of specified shape. (Default is All see <see cref="T:Ferda.Guha.Math.Quantifiers.IceDesignDocumentation.BoundTypeEnum"/>)
        /// </summary>
        Bound FromRow;

        Bound ToRow;
        Bound FromColumn;
        Bound ToColumn;

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
        /// Specifies presumption of calculating the quantifier.
        /// Evaluating of quantifiers should be in order to save
        /// performance i.e. take into account "failure" of quantifiers,
        /// its performance requirements, ... (quantifier uses numeric values, ...).
        /// </summary>
        PerformanceDifficultyEnum performanceDifficulty;

        /// <summary>
        /// Indicates wheter the quantifier requires numeric values
        /// (some CF quantifiers).
        /// </summary>
        bool needsNumericValues;

        /// <summary>
        /// Some quantifiers may to work only with ordinal data etc.
        /// (e.g. Kendal quantifier observe ordinal dependence of two attributes)
        /// </summary>
        CardinalityEnum supportedData;

        /// <summary>
        /// Used units in quantifier. Please not that some quantifiers 
        /// supports only some units, so this setting is sometimes hard specified
        /// by programmer, sometimes by user.
        /// </summary>
        UnitsEnum units;

        /// <summary>
        /// Some quantifiers does not support computation over float/double
        /// contingency table i.e. they needs integer values. (For Example 
        /// <c>Sum(i=a..a+b)(...)</c>)
        /// </summary>
        bool supportsFloatContingencyTable;
    }
}
#endif 