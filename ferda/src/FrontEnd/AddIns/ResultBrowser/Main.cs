using System;
using System.Collections.Generic;
using System.Text;
using Ferda.FrontEnd.External;
using Ferda.FrontEnd.Properties;

namespace Ferda
{
    namespace FrontEnd.AddIns.ResultBrowser
    {
        public class Main : Ferda.FrontEnd.AddIns.AbstractMain, IPropertyProvider
        {
            IPropertiesDisplayer displayer;

            public override string NameOfObject
            {
                get
                {
                    return "ResultBrowser";
                }
            }

            public override Ice.Object IceObject
            {
                get
                {
                    return new Ferda.FrontEnd.AddIns.ResultBrowser.ResultBrowserIce(this.OwnerOfAddIn, this.displayer);
                }
            }




            #region IPropertyProvider Members

            public IPropertiesDisplayer Displayer
            {
                set
                {
                    displayer = value;
                }
                get
                {
                    return displayer;
                }
            }

            #endregion


        } 
    }

}