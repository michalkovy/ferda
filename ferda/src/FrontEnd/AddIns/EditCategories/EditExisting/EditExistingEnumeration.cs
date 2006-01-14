using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;
using System.Resources;
using System.Reflection;

namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories.EditExisting
    {
        public class EditExistingEnumeration : Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory.CreateSetWizard
        {

            #region Private variables

            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// DataList to work with
            /// </summary>
            FerdaSmartDataList datalist;

            /// <summary>
            /// Edited enumeration.
            /// </summary>
            Category enumeration;

            /// <summary>
            /// Edited enumeration index
            /// </summary>
            int index = 0;

            #endregion


            #region Constructor
            public EditExistingEnumeration(FerdaSmartDataList dataList, Category Interval, ResourceManager rm): base(dataList, rm)
            {
                this.datalist = dataList;
                this.enumeration = Interval;

                this.ButtonSubmit.Click -= new EventHandler(Submit_Click);
                this.ButtonSubmit.Click += new EventHandler(Submit_Click_New);

                index = this.datalist.GetIndex(this.enumeration);

                this.TextBoxNewName.Text = this.enumeration.Name;

                foreach(object value in this.enumeration.Set.Values)
                {
                    this.ListBoxExistingValues.Items.Add(value);
                }

                // This call is required by the Windows Form Designer.
                InitializeComponent();
            }
            #endregion


            #region Button handlers

            private void Submit_Click_New(object sender, EventArgs e)
            {
                this.enumeration.RemoveSetValues();

                ArrayList tempList = new ArrayList();
                foreach(object item in ListBoxExistingValues.Items)
                {
                    tempList.Add(item);
                }

                SingleSet newSet = new SingleSet(tempList);

                this.enumeration.AddSingleSet(newSet);

                this.datalist.RemoveMultiSet(this.index);
                this.enumeration.Name = this.TextBoxNewName.Text;
                this.datalist.AddNewMultiSetDirect(this.enumeration);

                this.Dispose();
            }

            #endregion


            #region VS generated code
            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
           protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                base.Dispose(disposing);
            }

            #endregion


            #region Designer generated code
            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                components = new System.ComponentModel.Container();
            }
            #endregion
        }
    }

}