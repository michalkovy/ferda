using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;
public delegate void FerdaCloseEvent(CategoriesStruct categories);

namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories.NoGUIclasses
    {
        interface ControlClose
        {
            event FerdaCloseEvent ControlClosing;
            void OnControlClosing();
        }
    }
}
