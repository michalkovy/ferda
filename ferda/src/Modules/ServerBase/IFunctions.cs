// IFunctions.cs - fundamental functionality for boxes functions
//
// Authors: 
//   Michal Kováč <michal.kovac.develop@centrum.cz>
//   Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Michal Kováč, Tomáš Kuchař 
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

using Ferda.Modules.Boxes;

namespace Ferda.Modules
{
    /// <summary>
    /// Each box module`s functions object has to implement this interface.
    /// </summary>
    public interface IFunctions
    {
        /*
        protected BoxModuleI boxModule;
        protected IBoxInfo boxInfo;
        */

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        /// <remarks>
        /// It is essential that the box module`s functions object has 
        /// pointer to the box module; otherwise, the functions object 
        /// could not work witch box module`s sockets and properties.
        /// </remarks>
        /// <example>
        /// Exemplary implementation
        /// <code>
        /// 	class SomeFunctionsI : SomeFunctionsDisp_, IFunctions
        /// 	{
        /// 	    protected BoxModuleI boxModule;
        /// 	    protected IBoxInfo boxInfo;
        /// 
        /// 	    #region IFunctions Members
        /// 	    public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        /// 	    {
        /// 	        this.boxModule = boxModule;
        /// 	        this.boxInfo = boxInfo;
        ///         }
        ///         #endregion
        ///         
        ///         /*...*/
        ///     }
        /// </code>
        /// </example>
        void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo);
    }
}