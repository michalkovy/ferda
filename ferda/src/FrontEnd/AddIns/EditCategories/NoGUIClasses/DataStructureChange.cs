using System;
using System.Collections;
using System.Text;
public delegate void FerdaEvent();

namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories.NoGUIclasses
    {
        interface DataStructureChange
        {
            event FerdaEvent StructureChange;
            void OnDataStructureChange();
        }
    }
}