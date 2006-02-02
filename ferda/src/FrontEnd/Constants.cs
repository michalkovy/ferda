using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Ferda
{
    namespace FrontEnd
    {
        /// <summary>
        /// Defines the constants needed for Ferda FrontEnd
        /// </summary>
        public class Constants
        {
            /// <summary>
            /// Width of stripes such as FerdaMenu or FerdaToolBar.
            /// These numbers are used when resizing FerdaForm
            /// </summary>
            public int StripWidth
            {
                get
                {
                    return 24;
                }
            }

            /// <summary>
            /// Offset of the vertical edges of the form for the docking manager
            /// </summary>
            public int WidthFormOffset
            {
                get
                {
                    return 8;
                }
            }

            /// <summary>
            /// Offset for the heigth of the docking manager
            /// </summary>
            public int HeightFormOffset
            {
                get
                {
                    return 75;
                }
            }


            /// <summary>
            /// Total amount of blank space in archive 10 pixels from the left
            /// side and 20 from the right
            /// </summary>
            public int ArchiveBlankSpace
            {
                get
                {
                    return ArchiveLeftBlank + ArchiveRightBlank;
                }
            }

            /// <summary>
            /// Height of treeview in archive
            /// </summary>
            public int ArchiveTreeViewHeight
            {
                get
                {
                    return 400;
                }
            }

            /// <summary>
            /// Height of buttons in archive
            /// </summary>
            public int ArchiveButtonsHeight
            {
                get
                {
                    return 35;
                }
            }

            /// <summary>
            /// Height of combobox in archive
            /// </summary>
            public int ArchiveComboHeight
            {
                get
                {
                    return 22;
                }
            }

            /// <summary>
            /// Amount of right blank space in the archive
            /// </summary>
            public int ArchiveRightBlank
            {
                get
                {
                    return 1;
                }
            }

            /// <summary>
            /// Amount of left blank space in the archive
            /// </summary>
            public int ArchiveLeftBlank
            {
                get
                {
                    return 1;
                }
            }

            /// <summary>
            /// InitialSize of the StringComboAddingControl
            /// </summary>
            public Size StringComboAddingControlInitialSize
            {
                get
                {
                    return new Size(129, 183);
                }
            }
        }
    }
}
