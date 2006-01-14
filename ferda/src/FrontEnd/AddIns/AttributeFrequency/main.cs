using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd.AddIns.AttributeFrequency
{
    public class Main : Ferda.FrontEnd.AddIns.AbstractMain
    {
        public override string NameOfObject
        {
            get
            {
                return "AttributeFrequency";
            }
        }

        public override Ice.Object IceObject
        {
            get
            {
                return new Ferda.FrontEnd.AddIns.AttributeFrequency.MyIce.AttributeFrequencyIce(this.OwnerOfAddIn);
            }
        }
    }
}