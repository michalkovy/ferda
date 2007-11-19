// Functions.cs - Function objects for the Compare box module
//
// Author: Michal Kováč
//
// Copyright (c) 2007 Michal Kováč <michal.kovac.develop@centrum.cz>
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
using Ice;

namespace Ferda.Modules.Boxes.Language.Math.Compare
{
    /// <summary>
    /// Class is providing ICE functionality of the Compare function
    /// </summary>
    public class Functions : Ferda.Modules.BoolTI, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        //protected IBoxInfo _boxInfo;

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
            //_boxInfo = boxInfo;
        }

        #endregion
        
        #region DoubleTInterfacePrx Members
        
        /// <summary>
        /// Method getBoolValue
        /// </summary>
        /// <returns>A bool</returns>
        /// <param name="__current">An Ice.Current</param>
        public override bool getBoolValue(Current __current)
        {
        	double value1 = this._boxModule.GetPropertyDouble("value1");
        	double value2 = this._boxModule.GetPropertyDouble("value2");
        	string type = this._boxModule.GetPropertyString("type");
        	switch (type)
        	{
        		case "<" : return value1 < (value2 - 0.00000001);
        		case ">" : return value1 > (value2 + 0.00000001);
        		case "<=" : return value1 <= (value2 + 0.00000001);
        		case ">=" : return value1 >= (value2 - 0.00000001);
        		case "=" : return (value1 >= (value2 - 0.00000001) &&
        							value1 <= (value2 + 0.00000001));
        		case "!=" : return (value1 < (value2 - 0.00000001) ||
        							value1 > (value2 + 0.00000001));
        		default: throw new System.Exception("Unexpected type");
        	}
        }

        public override String getStringValue(Current __current)
        {
            return this.getDoubleValue(__current).ToString();
        }
        
 		#endregion       
    }
}