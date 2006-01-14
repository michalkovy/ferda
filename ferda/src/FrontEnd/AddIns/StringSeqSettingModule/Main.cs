namespace Ferda.FrontEnd.AddIns.StringSeqSettingModule
{
	public class Main : Ferda.FrontEnd.AddIns.AbstractMain
	{
		public override string NameOfObject
		{
			get
			{
				return "StringSeqSettingModule";
			}
		}
		
		public override Ice.Object IceObject
		{
			get
			{
				return new Ferda.Modules.StringSeqSettingModule(this.OwnerOfAddIn);
			}
		}
	}
}
