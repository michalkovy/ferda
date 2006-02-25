using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.AbstractKLQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers
{
    /// <summary>
    /// Base for KL quantifiers.	
    /// </summary>
    /// <remarks>
    /// Defined properties: RowFrom, RowTo, ColumnFrom, ColumnTo, Relation and Treshold.
    /// </remarks>
    public abstract class AbstractKLTaskQuantifierFunctions : AbstractKLQuantifierFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI boxModule;
        //protected IBoxInfo boxInfo;

        #region IFunctions Members
        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            this.boxModule = boxModule;
            //this.boxInfo = boxInfo;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the row from.
        /// </summary>
        /// <value>The row from.</value>
        protected string RowFrom
        {
            get
            {
                return this.boxModule.GetPropertyString("RowFrom");
            }
        }

        /// <summary>
        /// Gets the row to.
        /// </summary>
        /// <value>The row to.</value>
        protected string RowTo
        {
            get
            {
                return this.boxModule.GetPropertyString("RowTo");
            }
        }

        /// <summary>
        /// Gets the column from.
        /// </summary>
        /// <value>The column from.</value>
        protected string ColumnFrom
        {
            get
            {
                return this.boxModule.GetPropertyString("ColumnFrom");
            }
        }

        /// <summary>
        /// Gets the column to.
        /// </summary>
        /// <value>The column to.</value>
        protected string ColumnTo
        {
            get
            {
                return this.boxModule.GetPropertyString("ColumnTo");
            }
        }

        /// <summary>
        /// Gets the relation.
        /// </summary>
        /// <value>The relation.</value>
        protected RelationEnum Relation
        {
            get
            {
                return (RelationEnum)Enum.Parse(typeof(RelationEnum), this.boxModule.GetPropertyString("Relation"));
            }
        }

        /// <summary>
        /// Gets the treshold.
        /// </summary>
        /// <value>The treshold.</value>
        protected double Treshold
        {
            get
            {
                return this.boxModule.GetPropertyDouble("Treshold");
            }
        }
        #endregion

        /// <summary>
        /// Gets the value function delegate.
        /// </summary>
        /// <value>The value function delegate.</value>
        protected abstract ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
        {
            get;
        }

        #region Functions
        /// <summary>
        /// Gets the quantifier box identifier.
        /// </summary>
        /// <param name="__current">The Ice __current.</param>
        /// <returns>Box type identifier.</returns>
        public override string QuantifierIdentifier(Ice.Current __current)
        {
            return boxModule.BoxInfo.Identifier;
        }

        /// <summary>
        /// Gets the validity of the quantifier.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
        public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
        {
            return ContingencyTable.Compare(Value(setting), Relation, Treshold);
        }

        /// <summary>
        /// Gets the value of the quantifier above specified <c>setting</c>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
        public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
        {
            TwoDimensionalContingencyTable table = new TwoDimensionalContingencyTable(setting.firstContingencyTableRows);
            table.StartColumnBound = ColumnFrom;
            table.StartRowBound = RowFrom;
            table.EndColumnBound = ColumnTo;
            table.EndRowBound = RowTo;

            return ContingencyTable.Value<TwoDimensionalContingencyTable>(
                valueFunctionDelegate,
                table);
        }
        #endregion
    }

    /// <summary>
    /// Base for KL quantifiers.	
    /// </summary>
    /// <remarks>
    /// Defined properties: RowFrom, RowTo, ColumnFrom, ColumnTo, Relation, Treshold and Units.
    /// </remarks>
    public abstract class AbstractKLTaskQuantifierFunctionsWithUnits : AbstractKLTaskQuantifierFunctions
    {
        #region Properties
        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <value>The units.</value>
        protected UnitsEnum Units
        {
            get
            {
                return (UnitsEnum)Enum.Parse(typeof(UnitsEnum), this.boxModule.GetPropertyString("Units"));
            }
        }
        #endregion

        /// <summary>
        /// Gets the value of the quantifier above specified <c>setting</c>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
        public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
        {
            TwoDimensionalContingencyTable table = new TwoDimensionalContingencyTable(setting.firstContingencyTableRows);
            table.StartColumnBound = ColumnFrom;
            table.StartRowBound = RowFrom;
            table.EndColumnBound = ColumnTo;
            table.EndRowBound = RowTo;

            return ContingencyTable.Value<TwoDimensionalContingencyTable>(
                valueFunctionDelegate,
                table,
                Units,
                setting.allObjectsCount);
        }
    }

    /// <summary>
    /// Base for KL quantifiers.	
    /// </summary>
    /// <remarks>
    /// Defined properties: RowFrom, RowTo, ColumnFrom, ColumnTo, Relation, Treshold and Direction.
    /// </remarks>
    public abstract class AbstractKLTaskQuantifierFunctionsWithDirection : AbstractKLTaskQuantifierFunctions
    {
        #region Properties
        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <value>The direction.</value>
        protected DirectionEnum Direction
        {
            get
            {
                return (DirectionEnum)Enum.Parse(typeof(DirectionEnum), this.boxModule.GetPropertyString("Direction"));
            }
        }
        #endregion

        /// <summary>
        /// Gets the value of the quantifier above specified <c>setting</c>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
        public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
        {
            TwoDimensionalContingencyTable table = new TwoDimensionalContingencyTable(setting.firstContingencyTableRows);
            table.StartColumnBound = ColumnFrom;
            table.StartRowBound = RowFrom;
            table.EndColumnBound = ColumnTo;
            table.EndRowBound = RowTo;

            return ContingencyTable.Value<TwoDimensionalContingencyTable>(
                valueFunctionDelegate,
                table);
        }
    }
}
