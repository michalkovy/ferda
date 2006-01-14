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
                return new Ferda.FrontEnd.AddIns.ColumnFrequency.MyIce.ColumnFrICe(this.OwnerOfAddIn);
            }
        }
    }
}