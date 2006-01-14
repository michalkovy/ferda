using System;
using System.Drawing.Design;
using System.Windows.Forms;
namespace Netron.GraphLib.UI
{
	public class TextUIEditor : UITypeEditor 
	{ 
		#region Constructor
		public TextUIEditor() : base()
		{}

		#endregion

		#region Methods
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context) 
		{ 
			// We will use a window for property editing. 
			return UITypeEditorEditStyle.Modal; 
		} 

		

		
		public override object EditValue( System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) 
		{ 
			
			GenericTextEditor editor = new GenericTextEditor();
			editor.TextToEdit = (string) value;
			DialogResult res=editor.ShowDialog();

			// Return the new value. 
			if(res==DialogResult.OK)			
				return editor.TextToEdit;
			else
				return (string) value;
		} 

		public override bool GetPaintValueSupported(	System.ComponentModel.ITypeDescriptorContext context) 
		{ 
			// No special thumbnail will be shown for the grid. 
			return false; 
		} 

		#endregion
	} 

}
