using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd.AddIns.DatabaseInfo
{
    public class Main : Ferda.FrontEnd.AddIns.AbstractMain
    {
        public override string NameOfObject
        {
            get
            {
                return "DatabaseInfo";
            }
        }

        public override Ice.Object IceObject
        {
            get
            {
                return new Ferda.FrontEnd.AddIns.DatabaseInfo.MyIce.DatabaseInfoIce(this.OwnerOfAddIn);
            }
        }
    }
}