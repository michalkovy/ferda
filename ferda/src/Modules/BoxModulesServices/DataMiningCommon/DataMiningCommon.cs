// DataMiningCommon.cs - registering of boxes to the service
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Tomáš Kuchař
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA


//#define CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon
{
    /// <summary>
    /// Represents a IceBox service for common boxes for data mining
    /// </summary>
    public class Service : Ferda.Modules.FerdaServiceI
    {
        /// <summary>
        /// method register basic property boxes
        /// </summary>
        protected override void registerPropertyBoxes()
        {
            //Basic property types
            registerPropertyBox("BoolT",
                                new BoolTI(true),
                                delegate(Ice.ObjectPrx prx) { return new BoolTI(BoolTInterfacePrxHelper.checkedCast(prx)); },
                                "");
            registerPropertyBox("ShortT",
                                new ShortTI(0),
                                delegate(Ice.ObjectPrx prx) { return new ShortTI(ShortTInterfacePrxHelper.checkedCast(prx)); },
                                "");
            registerPropertyBox("IntT",
                                new IntTI(0),
                                delegate(Ice.ObjectPrx prx) { return new IntTI(IntTInterfacePrxHelper.checkedCast(prx)); },
                                "");
            registerPropertyBox("LongT",
                                new LongTI(0),
                                delegate(Ice.ObjectPrx prx) { return new LongTI(LongTInterfacePrxHelper.checkedCast(prx)); },
                                "");
            registerPropertyBox("FloatT",
                                new FloatTI(0),
                                delegate(Ice.ObjectPrx prx) { return new FloatTI(FloatTInterfacePrxHelper.checkedCast(prx)); },
                                "");
            registerPropertyBox("DoubleT",
                                new DoubleTI(0),
                                delegate(Ice.ObjectPrx prx) { return new DoubleTI(DoubleTInterfacePrxHelper.checkedCast(prx)); },
                                "");
            registerPropertyBox("StringT",
                                new StringTI(""),
                                delegate(Ice.ObjectPrx prx) { return new StringTI(StringTInterfacePrxHelper.checkedCast(prx)); },
                                "");
            registerPropertyBox("TimeT",
                                new TimeTI(0, 0, 0),
                                delegate(Ice.ObjectPrx prx) { return new TimeTI(TimeTInterfacePrxHelper.checkedCast(prx)); },
                                "");
            registerPropertyBox("DateT",
                                new DateTI(0, 0, 0),
                                delegate(Ice.ObjectPrx prx) { return new DateTI(DateTInterfacePrxHelper.checkedCast(prx)); },
                                "");
            registerPropertyBox("DateTimeT",
                                new DateTimeTI(0, 0, 0, 0, 0, 0),
                                delegate(Ice.ObjectPrx prx) { return new DateTimeTI(DateTimeTInterfacePrxHelper.checkedCast(prx)); },
                                "");
            registerPropertyBox("CategoriesT",
                                new CategoriesTI(),
                                delegate(Ice.ObjectPrx prx) { return new CategoriesTI(CategoriesTInterfacePrxHelper.checkedCast(prx)); },
                                "CategoriesT");
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

        /// <summary>
        /// Says that this service has property boxes
        /// </summary>
        protected override bool havePropertyBoxes
        {
            get { return true; }
        }


#if CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
        private void registerBox(string factoryIdentifier, BoxInfo boxInfo)
        { }

        public static void Main(string[] args)
        {
            Service service = new Service();
            service.registerBoxes();
        }
#endif
    }
}
