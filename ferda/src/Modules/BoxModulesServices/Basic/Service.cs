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
            registerPropertyBox("BoolT",
                                new BoolTI(true),
                                delegate(ObjectPrx prx)
                                    {
                                        return new BoolTI(BoolTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            registerPropertyBox("ShortT",
                                new ShortTI(0),
                                delegate(ObjectPrx prx)
                                    {
                                        return new ShortTI(ShortTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            registerPropertyBox("IntT",
                                new IntTI(0),
                                delegate(ObjectPrx prx)
                                    {
                                        return new IntTI(IntTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            registerPropertyBox("LongT",
                                new LongTI(0),
                                delegate(ObjectPrx prx)
                                    {
                                        return new LongTI(LongTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            registerPropertyBox("FloatT",
                                new FloatTI(0),
                                delegate(ObjectPrx prx)
                                    {
                                        return new FloatTI(FloatTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            registerPropertyBox("DoubleT",
                                new DoubleTI(0),
                                delegate(ObjectPrx prx)
                                    {
                                        return new DoubleTI(DoubleTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            registerPropertyBox("StringT",
                                new StringTI(""),
                                delegate(ObjectPrx prx)
                                    {
                                        return new StringTI(StringTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            registerPropertyBox("TimeT",
                                new TimeTI(0, 0, 0),
                                delegate(ObjectPrx prx)
                                    {
                                        return new TimeTI(TimeTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            registerPropertyBox("DateT",
                                new DateTI(0, 0, 0),
                                delegate(ObjectPrx prx)
                                    {
                                        return new DateTI(DateTInterfacePrxHelper.checkedCast(prx));
                                    },
                                "");
            registerPropertyBox("DateTimeT",
                                new DateTimeTI(0, 0, 0, 0, 0, 0),
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