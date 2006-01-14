using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories
    {
        public class Main : Ferda.FrontEnd.AddIns.AbstractMain
        {
            public override string NameOfObject
            {
                get
                {
                    return "EditCategories";
                }
            }

            public override Ice.Object IceObject
            {
                get
                {
                    return new Ferda.FrontEnd.AddIns.EditCategories.EditCategoriesIce(this.OwnerOfAddIn);
                }
            }
}
    }
}
