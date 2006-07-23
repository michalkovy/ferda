using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Guha.MiningProcessor.Results
{
    /// <summary>
    /// Represents effective form of <see cref="T:Ferda.Guha.MiningProcessor.Results.SerializableResult"/>.
    /// This form should be used to working with the result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Count of all objects in analyzed data table.
        /// </summary>
        public long AllObjectsCount;
        
        /// <summary>
        /// Collection of hypotheses.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<Hypothesis> Hypotheses = new List<Hypothesis>();
        
        /// <summary>
        /// Type of the task. The type of the task indicates 
        /// list of used boolean/categorial attributes (method <code>GetSemanticMarks()</code>)
        /// and usage of one or two contingecy tables (field <code>TwoContingencyTables</code>).
        /// </summary>
        public TaskTypeEnum TaskTypeEnum;

        public static bool SupportsTwoContingencyTables(TaskTypeEnum taskType)
        {
            switch (taskType)
            {
                case TaskTypeEnum.FourFold:
                case TaskTypeEnum.CF:
                case TaskTypeEnum.KL:
                    return false;
                case TaskTypeEnum.SDFourFold:
                case TaskTypeEnum.SDCF:
                case TaskTypeEnum.SDKL:
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether the current task type uses two contingency tables.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if two contingency tables are used by the current task type; otherwise, <c>false</c>.
        /// </value>
        public bool TwoContingencyTables
        {
            get
            {
                return SupportsTwoContingencyTables(TaskTypeEnum);
            }
        }
        
        /// <summary>
        /// Gets the semantic marks of boolean/categorial attributes
        /// used in current task.
        /// </summary>
        /// <returns></returns>
        public MarkEnum[] GetSemanticMarks()
        {
            switch (TaskTypeEnum)
            {
                case TaskTypeEnum.FourFold:
                    return new MarkEnum[] { MarkEnum.Antecedent, MarkEnum.Succedent, MarkEnum.Condition };
                case TaskTypeEnum.SDFourFold:
                    return new MarkEnum[] { MarkEnum.Antecedent, MarkEnum.Succedent, MarkEnum.Condition, MarkEnum.FirstSet, MarkEnum.SecondSet };
                case TaskTypeEnum.KL:
                    return new MarkEnum[] { MarkEnum.RowAttribute, MarkEnum.ColumnAttribute, MarkEnum.Condition };
                case TaskTypeEnum.SDKL:
                    return new MarkEnum[] { MarkEnum.RowAttribute, MarkEnum.ColumnAttribute, MarkEnum.Condition, MarkEnum.FirstSet, MarkEnum.SecondSet };
                case TaskTypeEnum.CF:
                    return new MarkEnum[] { MarkEnum.Attribute, MarkEnum.Condition };
                case TaskTypeEnum.SDCF:
                    return new MarkEnum[] { MarkEnum.Attribute, MarkEnum.Condition, MarkEnum.FirstSet, MarkEnum.SecondSet };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
