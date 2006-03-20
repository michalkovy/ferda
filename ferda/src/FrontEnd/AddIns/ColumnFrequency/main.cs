// Main.cs - class instantiated by FrontEnd
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd.AddIns.ColumnFrequency
{
    public class Main : Ferda.FrontEnd.AddIns.AbstractMain
    {
        public override string NameOfObject
        {
            get
            {
                return "ColumnFrequency";
            }
        }

        public override Ice.Object IceObject
        {
            get
            {
                return new Ferda.FrontEnd.AddIns.ColumnFrequency.MyIce.ColumnFrequencyIce(this.OwnerOfAddIn);
            }
        }
    }
}