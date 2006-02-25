using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using System.Data;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.Modules.Helpers.Data;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.EquifrequencyIntervalsAttribute
{
	class EquifrequencyIntervalsAttributeFunctionsI : EquifrequencyIntervalsAttributeFunctionsDisp_, IFunctions, IAbstractDynamicAttribute, IAbstractAttribute
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

		protected string NameInLiterals
		{
			get
			{
				return this.boxModule.GetPropertyString("NameInLiterals");
			}
		}

		protected string XCategory
		{
			get
			{
				return this.boxModule.GetPropertyString("XCategory");
			}
		}
		protected long CountOfCategories
		{
			//UNDONE CountOfCategories (not true everytime)
			//neplati pokud je pocet distinct values v sloupci mensi
			get
			{
				return this.boxModule.GetPropertyLong("CountOfCategories");
			}
		}

		protected AttributeDomainEnum Domain
		{
			get
			{
				return (AttributeDomainEnum)Enum.Parse(typeof(AttributeDomainEnum),
					this.boxModule.GetPropertyString("Domain"));
			}
		}

		protected double From
		{
			get
			{
				return this.boxModule.GetPropertyDouble("From");
			}
		}

		protected double To
		{
			get
			{
				return this.boxModule.GetPropertyDouble("To");
			}
		}
        #endregion

		#region Categories (CategoriesStruct, Names, Count, includeNullCategoryname)
		private class categoriesCache : Ferda.Modules.Helpers.Caching.Cache
		{
			private GeneratedAttribute value;
			/// <summary>
			/// It is strongly recommended to call this functions before calling any other function in this class.
			/// </summary>
			public GeneratedAttribute Value(string boxIdentity, BoxModuleI boxModule, ColumnInfo columnInfo, AttributeDomainEnum domainType, double from, double to, long countOfCategories)
			{
                lock (this)
                {
                    Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                    cacheSetting.Add(Database.DatabaseBoxInfo.typeIdentifier + Database.DatabaseBoxInfo.OdbcConnectionStringPropertyName, columnInfo.dataMatrix.database.odbcConnectionString);
                    cacheSetting.Add(DataMatrix.DataMatrixBoxInfo.typeIdentifier + DataMatrix.DataMatrixBoxInfo.DataMatrixNamePropertyName, columnInfo.dataMatrix.dataMatrixName);
                    cacheSetting.Add(DataMatrix.DataMatrixBoxInfo.typeIdentifier + DataMatrix.DataMatrixBoxInfo.RecordCountPropertyName, columnInfo.dataMatrix.recordsCount);
                    cacheSetting.Add(Column.ColumnBoxInfo.typeIdentifier + Column.ColumnBoxInfo.ColumnSelectExpressionPropertyName, columnInfo.columnSelectExpression);
                    cacheSetting.Add("DomainType", domainType);
                    cacheSetting.Add("From", from);
                    cacheSetting.Add("To", to);
                    cacheSetting.Add("CountOfCategories", countOfCategories);
                    if (IsObsolete(columnInfo.dataMatrix.database.lastReloadInfo, cacheSetting))
                    {
                        try
                        {
                            value = EquidistantAlgorithm.Generate(
                                domainType,
                                from,
                                to,
                                countOfCategories,
                                columnInfo,
                                boxIdentity);
                        }
                        catch (Ferda.Modules.BadParamsError ex)
                        {
                            if (ex.restrictionType == restrictionTypeEnum.DbColumnDataType)
                            {
                                boxModule.OutputMessage(
                                    Ferda.ModulesManager.MsgType.Info,
                                    "UnsupportedColumnDatatype",
                                    "NumericDatatypesSupportedOnly");
                                value = new GeneratedAttribute();
                            }
                            else
                                throw Ferda.Modules.Exceptions.BoxRuntimeError(ex, boxModule.StringIceIdentity, null);
                        }
                    }
                    if (value == null)
                        value = new GeneratedAttribute();
                    return value;
                }
			}
		}
		private categoriesCache categoriesCached = new categoriesCache();
		private GeneratedAttribute getCategoriesInfo()
		{
			ColumnInfo columnInfo = getColumnFunctionsPrx().getColumnInfo();
            return categoriesCached.Value(boxModule.StringIceIdentity, this.boxModule, columnInfo, Domain, From, To, CountOfCategories);
		}
		private GeneratedAttribute getCategoriesInfo(ColumnInfo columnInfo)
		{
            return categoriesCached.Value(boxModule.StringIceIdentity, this.boxModule, columnInfo, Domain, From, To, CountOfCategories);
		}
		#endregion

		#region Functions
		public override AbstractAttributeStruct getAbstractAttribute(Ice.Current __current)
		{
			ColumnInfo columnInfo = this.getColumnFunctionsPrx().getColumnInfo();
			AbstractAttributeStruct result = new AbstractAttributeStruct();
			Ferda.Modules.Helpers.Data.Column.TestColumnSelectExpression(
                columnInfo.dataMatrix.database.odbcConnectionString,
				columnInfo.dataMatrix.dataMatrixName,
				columnInfo.columnSelectExpression,
                boxModule.StringIceIdentity);
			GeneratedAttribute categoriesInfo = getCategoriesInfo(columnInfo);
			result.column = columnInfo;
			result.categories = categoriesInfo.CategoriesStruct;
			//AttributeFunctionsI.TestCategoriesDisjunctivity(sumOfRowMax.categories, boxIdentity);
			//This test is useless here (vain / effort / wastage)
			result.identifier = boxModule.PersistentIdentity;
			result.countOfCategories = categoriesInfo.CategoriesCount;
			result.includeNullCategory = categoriesInfo.IncludeNullCategoryName;
			result.xCategory = XCategory;
            Ferda.Modules.Helpers.Data.Attribute.TestAreCategoriesInCategories(result.categories, new string[] { result.xCategory, result.includeNullCategory } , boxModule.StringIceIdentity);
			result.nameInLiterals = NameInLiterals;

			return result;
		}
		#endregion

		#region Sockets
		public BoxModulePrx GetColumnBoxModulePrx()
		{
			return SocketConnections.GetBoxModulePrx(boxModule, "ColumnOrDerivedColumn");
		}
		protected ColumnFunctionsPrx getColumnFunctionsPrx()
		{
			return ColumnFunctionsPrxHelper.checkedCast(
				SocketConnections.GetObjectPrx(boxModule, "ColumnOrDerivedColumn")
				);
		}
		#endregion

		#region Actions
		#endregion

        #region BoxInfo

        public GeneratedAttribute GetGeneratedAttribute()
        {
            try
            {
                return getCategoriesInfo();
            }
            catch (Ferda.Modules.BoxRuntimeError)
            {
                return new GeneratedAttribute();
            }
        }

        public SelectString[] GetPropertyCategoriesNames()
        {
            return this.GetGeneratedAttribute().CategoriesNames;
        }

        public PropertySetting[] GetSettingForNewAttributeBox()
        {
            try
            {
                GeneratedAttribute categoriesInfo = getCategoriesInfo();
                return Ferda.Modules.Helpers.Data.Attribute.GetSettingForNewAttributeBox(
                    categoriesInfo.CategoriesStruct,
                    XCategory,
                    categoriesInfo.IncludeNullCategoryName,
                    NameInLiterals);
            }
            catch (Ferda.Modules.BoxRuntimeError) { }
            return new PropertySetting[0];
        }

        #endregion
	}
}