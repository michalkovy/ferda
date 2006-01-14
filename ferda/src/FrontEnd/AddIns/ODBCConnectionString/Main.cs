using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd.AddIns.ODBCConnectionString
{
    class Main : Ferda.FrontEnd.AddIns.AbstractMain
    {
        public override string NameOfObject
        {
            get
            {
                return "ODBCConnectionString";
            }
        }

        public override Ice.Object IceObject
        {
            get
            {
                return new Ferda.FrontEnd.AddIns.ODBCConnectionString.MyIce.ConnectionStringIce(this.OwnerOfAddIn);
            }
        }
    }
}
