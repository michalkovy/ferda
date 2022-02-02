// DataMiningCommon.cs - registering of boxes to the service
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Tomáš Kuchaø
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
using Ice;

namespace Ferda.Modules.Boxes.Basic
{
    /// <summary>
    /// Represents a IceBox service for common boxes for data mining
    /// </summary>
    public class Service : FerdaServiceI
    {
        /// <summary>
        /// method register basic property boxes
        /// </summary>
        protected override void registerPropertyBoxes()
        {
            //Basic property types
            var boolT = new BoolTI(true);
            registerPropertyBox("BoolT",
                                boolT,
                                new BoolTInterfaceTie_(boolT),
                                delegate (ObjectPrx prx)
                                    {
                                        return new BoolTI(BoolTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            var shortT = new ShortTI(0);
            registerPropertyBox("ShortT",
                                shortT,
                                new ShortTInterfaceTie_(shortT),
                                delegate(ObjectPrx prx)
                                    {
                                        return new ShortTI(ShortTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            var intT = new IntTI(0);
            registerPropertyBox("IntT",
                                intT,
                                new IntTInterfaceTie_(intT),
                                delegate(ObjectPrx prx)
                                    {
                                        return new IntTI(IntTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            var longT = new LongTI(0);
            registerPropertyBox("LongT",
                                longT,
                                new LongTInterfaceTie_(longT),
                                delegate(ObjectPrx prx)
                                    {
                                        return new LongTI(LongTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            var floatT = new FloatTI(0);
            registerPropertyBox("FloatT",
                                floatT,
                                new FloatTInterfaceTie_(floatT),
                                delegate(ObjectPrx prx)
                                    {
                                        return new FloatTI(FloatTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            var doubleT = new DoubleTI(0);
            registerPropertyBox("DoubleT",
                                doubleT,
                                new DoubleTInterfaceTie_(doubleT),
                                delegate(ObjectPrx prx)
                                    {
                                        return new DoubleTI(DoubleTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            var stringT = new StringTI("");
            registerPropertyBox("StringT",
                                stringT,
                                new StringTInterfaceTie_(stringT),
                                delegate(ObjectPrx prx)
                                    {
                                        return new StringTI(StringTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            var timeT = new TimeTI(0,0,0);
            registerPropertyBox("TimeT",
                                timeT,
                                new TimeTInterfaceTie_(timeT),
                                delegate(ObjectPrx prx)
                                    {
                                        return new TimeTI(TimeTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            var dateT = new DateTI(0, 0, 0);
            registerPropertyBox("DateT",
                                dateT,
                                new DateTInterfaceTie_(dateT),
                                delegate(ObjectPrx prx)
                                    {
                                        return new DateTI(DateTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            var dateTimeT = new DateTimeTI(0, 0, 0, 0, 0, 0);
            registerPropertyBox("DateTimeT",
                                dateTimeT,
                                new DateTimeTInterfaceTie_(dateTimeT),
                                delegate(ObjectPrx prx)
                                    {
                                        return new DateTimeTI(DateTimeTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            //registerPropertyBox("CategoriesT",
            //                    new CategoriesTI(),
            //                    delegate(ObjectPrx prx)
            //                        {
            //                            return new CategoriesTI(CategoriesTInterfacePrxHelper.checkedCast(prx));
            //                        },
            //                    "CategoriesT");
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
        }

        /// <summary>
        /// Says that this service has property boxes
        /// </summary>
        protected override bool havePropertyBoxes
        {
            get { return true; }
        }
    }
}