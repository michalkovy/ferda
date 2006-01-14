//#define CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.DataMiningCommon
{
    public class Service : Ferda.Modules.FerdaServiceI
	{

		protected override void registerPropertyBoxes()
		{
			//Basic property types
			registerPropertyBox("BoolT",
								new BoolTI(true),
								delegate(Ice.ObjectPrx prx) { return new BoolTI(BoolTInterfacePrxHelper.checkedCast(prx)); });
			registerPropertyBox("ShortT",
								new ShortTI(0),
								delegate(Ice.ObjectPrx prx) { return new ShortTI(ShortTInterfacePrxHelper.checkedCast(prx)); });
			registerPropertyBox("IntT",
								new IntTI(0),
								delegate(Ice.ObjectPrx prx) { return new IntTI(IntTInterfacePrxHelper.checkedCast(prx)); });
			registerPropertyBox("LongT",
								new LongTI(0),
								delegate(Ice.ObjectPrx prx) { return new LongTI(LongTInterfacePrxHelper.checkedCast(prx)); });
			registerPropertyBox("FloatT",
								new FloatTI(0),
								delegate(Ice.ObjectPrx prx) { return new FloatTI(FloatTInterfacePrxHelper.checkedCast(prx)); });
			registerPropertyBox("DoubleT",
								new DoubleTI(0),
								delegate(Ice.ObjectPrx prx) { return new DoubleTI(DoubleTInterfacePrxHelper.checkedCast(prx)); });
			registerPropertyBox("StringT",
								new StringTI(""),
								delegate(Ice.ObjectPrx prx) { return new StringTI(StringTInterfacePrxHelper.checkedCast(prx)); });
			registerPropertyBox("TimeT",
								new TimeTI(0, 0, 0),
								delegate(Ice.ObjectPrx prx) { return new TimeTI(TimeTInterfacePrxHelper.checkedCast(prx)); });
			registerPropertyBox("DateT",
								new DateTI(0, 0, 0),
								delegate(Ice.ObjectPrx prx) { return new DateTI(DateTInterfacePrxHelper.checkedCast(prx)); });
			registerPropertyBox("DateTimeT",
								new DateTimeTI(0, 0, 0, 0, 0, 0),
								delegate(Ice.ObjectPrx prx) { return new DateTimeTI(DateTimeTInterfacePrxHelper.checkedCast(prx)); });
		}

		/// <summary>
		/// Register box modules to Ice.ObjectAdapter.
		/// </summary>
		/// <remarks>
		/// Remember if you are adding registering of new box module,
		/// you must also change application.xml filePath in config directory.
		/// </remarks>
		protected override void registerBoxes()
		{
			this.registerBox("AtomSettingFactoryCreator", new Boxes.DataMiningCommon.AtomSetting.AtomSettingBoxInfo());
			this.registerBox("AttributesAttributeFactoryCreator", new Boxes.DataMiningCommon.Attributes.Attribute.AttributeBoxInfo());
			this.registerBox("AttributesEachValueOneCategoryAttributeFactoryCreator", new Boxes.DataMiningCommon.Attributes.EachValueOneCategoryAttribute.EachValueOneCategoryAttributeBoxInfo());
			this.registerBox("AttributesEquidistantIntervalsAttributeFactoryCreator", new Boxes.DataMiningCommon.Attributes.EquidistantIntervalsAttribute.EquidistantIntervalsAttributeBoxInfo());
			this.registerBox("AttributesEquifrequencyIntervalsAttributeFactoryCreator", new Boxes.DataMiningCommon.Attributes.EquifrequencyIntervalsAttribute.EquifrequencyIntervalsAttributeBoxInfo());
			this.registerBox("BooleanPartialCedentSettingFactoryCreator", new Boxes.DataMiningCommon.BooleanPartialCedentSetting.BooleanPartialCedentSettingBoxInfo());
			this.registerBox("CategorialPartialCedentSettingFactoryCreator", new Boxes.DataMiningCommon.CategorialPartialCedentSetting.CategorialPartialCedentSettingBoxInfo());
			this.registerBox("ColumnFactoryCreator", new Boxes.DataMiningCommon.Column.ColumnBoxInfo());
			this.registerBox("DatabaseFactoryCreator", new Boxes.DataMiningCommon.Database.DatabaseBoxInfo());
			this.registerBox("DataMatrixFactoryCreator", new Boxes.DataMiningCommon.DataMatrix.DataMatrixBoxInfo());
			this.registerBox("DerivedColumnFactoryCreator", new Boxes.DataMiningCommon.DerivedColumn.DerivedColumnBoxInfo());
			this.registerBox("EquivalenceClassFactoryCreator", new Boxes.DataMiningCommon.EquivalenceClass.EquivalenceClassBoxInfo());
			this.registerBox("LiteralSettingFactoryCreator", new Boxes.DataMiningCommon.LiteralSetting.LiteralSettingBoxInfo());
			//this.registerBox("FactoryCreator", new Boxes.DataMiningCommon());
		}

        protected override bool havePropertyBoxes
        {
            get { return true; }
        }


#if CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
        public static void Main(string[] args)
        {
            new Boxes.DataMiningCommon.AtomSetting.AtomSettingBoxInfo();
            new Boxes.DataMiningCommon.Attributes.Attribute.AttributeBoxInfo();
            new Boxes.DataMiningCommon.Attributes.EachValueOneCategoryAttribute.EachValueOneCategoryAttributeBoxInfo();
            new Boxes.DataMiningCommon.Attributes.EquidistantIntervalsAttribute.EquidistantIntervalsAttributeBoxInfo();
            new Boxes.DataMiningCommon.Attributes.EquifrequencyIntervalsAttribute.EquifrequencyIntervalsAttributeBoxInfo();
            new Boxes.DataMiningCommon.BooleanPartialCedentSetting.BooleanPartialCedentSettingBoxInfo();
            new Boxes.DataMiningCommon.CategorialPartialCedentSetting.CategorialPartialCedentSettingBoxInfo();
            new Boxes.DataMiningCommon.Column.ColumnBoxInfo();
            new Boxes.DataMiningCommon.Database.DatabaseBoxInfo();
            new Boxes.DataMiningCommon.DataMatrix.DataMatrixBoxInfo();
            new Boxes.DataMiningCommon.DerivedColumn.DerivedColumnBoxInfo();
            new Boxes.DataMiningCommon.EquivalenceClass.EquivalenceClassBoxInfo();
            new Boxes.DataMiningCommon.LiteralSetting.LiteralSettingBoxInfo();
        }
#endif
    }
}
