namespace Ferda.FrontEnd.AddIns
{
	public interface IOwnerOfAddIn
	{
		void ShowForm(System.Windows.Forms.Form form);
		
		System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.Form form);
		
		void ShowDockableControl(System.Windows.Forms.UserControl userControl, string name);
	}
}
