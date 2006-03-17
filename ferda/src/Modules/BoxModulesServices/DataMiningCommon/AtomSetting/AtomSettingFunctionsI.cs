using System;
using System.Collections.Generic;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute;

namespace Ferda.Modules.Boxes.DataMiningCommon.AtomSetting
{
	class AtomSettingFunctionsI : AtomSettingFunctionsDisp_, IFunctions
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
		protected CoefficientTypeEnum CoefficientType
		{
			get
			{
				return (CoefficientTypeEnum)Enum.Parse(
					typeof(CoefficientTypeEnum),
					this.boxModule.GetPropertyString("CoefficientType"),
					true);
			}
		}

		protected string[] Category
		{
			get
			{
				string category = this.boxModule.GetPropertyString("Category");
				if (String.IsNullOrEmpty(category))
					return new string[0] { };
				else
					return new string[1] { category };
			}
		}

		protected long MinLen
		{
			get
			{
				return this.boxModule.GetPropertyLong("MinLen");
			}
		}

		protected long MaxLen
		{
			get
			{
				return this.boxModule.GetPropertyLong("MaxLen");
			}
		}
		#endregion

		#region Functions
		public override AtomSettingStruct getAtomSetting(Ice.Current __current)
		{
			AtomSettingStruct result = new AtomSettingStruct();
			result.abstractAttribute = getAbstractAttributeFunctionsPrx().getAbstractAttribute();
            result.identifier = boxModule.PersistentIdentity;
			result.category = Category;
			result.coefficientType = CoefficientType;
			if (result.coefficientType == CoefficientTypeEnum.OneParticularCategory)
			{
				string category = null;
				if (result.category.Length > 0)
					category = result.category[0];
				if ((String.IsNullOrEmpty(category))
					|| !(Ferda.Modules.Helpers.Data.Attribute.TryIsCategoryInCategories(result.abstractAttribute.categories, category)))
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "Category must be selected when coefficient type is one particular rowValue!", new string[] { "Category" }, restrictionTypeEnum.NotInSelectOptions);
				}
			}
			result.maxLen = MaxLen;
			result.minLen = MinLen;
			if (result.minLen > result.maxLen)
			{
                throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "Min length is greater than max length!", new string[] { "MinLen", "MaxLen" }, restrictionTypeEnum.Other);
			}

			return result;
		}
		#endregion

		#region Sockets
		protected AbstractAttributeFunctionsPrx getAbstractAttributeFunctionsPrx()
		{
			return AbstractAttributeFunctionsPrxHelper.checkedCast(
				SocketConnections.GetObjectPrx(boxModule, "Attribute")
				);
		}
		#endregion

		#region Actions
		#endregion

		#region BoxInfo
		#region Cache: CategoriesNames
        private categoriesNamesCache categoriesNamesCached = new categoriesNamesCache(new TimeSpan(0, 0, 10));
		private class categoriesNamesCache : Ferda.Modules.Helpers.Caching.TimeOut
		{
			public categoriesNamesCache(TimeSpan timeout)
				: base(timeout) { }
			private SelectString[] value;
			public SelectString[] Value(string boxIdentity, AbstractAttributeFunctionsPrx abstractAttributeFunctionsPrx)
			{
				if (IsObsolete())
				{
					AbstractAttributeStruct abstractAttributeStruct = abstractAttributeFunctionsPrx.getAbstractAttribute();
                    if (abstractAttributeStruct.countOfCategories >= Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeConstants.MaxLengthOfCategoriesNamesSelectStringArray)
                        value = new SelectString[0];
                    else
                    {
                        value = new SelectString[0];
                        value =
                            Ferda.Modules.Boxes.BoxInfoHelper.StringArrayToSelectStringArray(
                                Ferda.Modules.Helpers.Data.Attribute.GetCategoriesNames(
                                    abstractAttributeStruct.categories,
                                    Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeConstants.MaxLengthOfCategoriesNamesSelectStringArray
                                    )
                                );
                    }
				}
				return value;
			}
		}

		#endregion
		public SelectString[] GetPropertyCategoriesNames()
		{
            try
            {
                return categoriesNamesCached.Value(boxModule.StringIceIdentity, getAbstractAttributeFunctionsPrx());
            }
            catch (Ferda.Modules.BoxRuntimeError) { }
            //Ferda.Modules.BadValueError ... Number of categories is 0
            
			return new SelectString[0];
		}
		#endregion
	}
}