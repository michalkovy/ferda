
using System;
using System.Collections.Generic;
using Ferda.Modules.Helpers.Caching;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.Attribute;
using Ice;
using Exception = System.Exception;

namespace Ferda.Modules.Boxes.GuhaMining.VirtualAttributes.VirtualFFTBooleanAttribute
{
    internal class Functions : BooleanAttributeSettingWithBSGenerationAbilityFunctionsDisp_, IFunctions
    {

        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;
       
        #region IFunctions Members


        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
        }

        #endregion

        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override BitStringGeneratorPrx GetBitStringGenerator(GuidStruct attributeId, Current current__)
        {
            return (BitStringGeneratorPrx)this;
        }

        public override IEntitySetting GetEntitySetting(Current current__)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override string GetSourceDataTableId(Current current__)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override CardinalityEnum GetAttributeCardinality(Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override GuidStruct GetAttributeId(Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override BitStringIce GetBitString(string categoryId, Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetCategoriesIds(Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override double[] GetCategoriesNumericValues(Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int[] GetCountVector(Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetMissingInformationCategoryId(Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override BitStringIce GetNextBitString(Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}