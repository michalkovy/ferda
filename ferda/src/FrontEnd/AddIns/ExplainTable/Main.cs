using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd.AddIns.ExplainTable
{
    public class Main : Ferda.FrontEnd.AddIns.AbstractMain
    {
        public override string NameOfObject
        {
            get
            {
                return "ExplainTable";
            }
        }

        public override Ice.Object IceObject
        {
            get
            {
                return new Ferda.FrontEnd.AddIns.ExplainTable.MyIce.ExplainTableIce(this.OwnerOfAddIn);
            }
        }
    }
}