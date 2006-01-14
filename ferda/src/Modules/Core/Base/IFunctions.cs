using System;
using System.Collections.Generic;
using System.Text;
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
