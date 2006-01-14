using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd.AddIns.ShowTable
{
    public class Main : Ferda.FrontEnd.AddIns.AbstractMain
    {
        public override Ice.Object IceObject
        {
            get
            {
                return new Ferda.FrontEnd.AddIns.ShowTable.MyIce.ShowTableIce(this.OwnerOfAddIn);
            }
        }

        public override string NameOfObject
        {
            get
            {
                return "ShowTable";
            }
        }
    }
}
