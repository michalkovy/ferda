using System;
using System.Collections.Generic;
using System.Text;
using Ferda.FrontEnd.External;
using Ferda.FrontEnd.Properties;

namespace Ferda.FrontEnd.AddIns.ResultBrowser
{
    public class Main : Ferda.FrontEnd.AddIns.AbstractMain, IPropertyProvider
    {
        IOtherObjectDisplayer displayer;
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

        public IOtherObjectDisplayer Displayer
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