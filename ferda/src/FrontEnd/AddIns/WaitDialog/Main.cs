using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd.AddIns.WaitDialog
{
    class Main : Ferda.FrontEnd.AddIns.AbstractMain
    {
        public override Ice.Object IceObject
        {
            get
            {
                return new Ferda.FrontEnd.AddIns.WaitDialog.WaitDialogIce(this.OwnerOfAddIn);
            }
        }

        public override string NameOfObject
        {
            get
            {
                return "WaitDialog";
            }
        }
    }
}
