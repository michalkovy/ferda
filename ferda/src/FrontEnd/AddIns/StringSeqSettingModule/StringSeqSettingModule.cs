using System;
using System.Text;
using Ferda.Modules;
using Ferda.ModulesManager;
using Ice;

namespace Ferda.Modules
{
	class StringSeqSettingModule : SettingModuleWithStringAbilityDisp_
	{
		Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn;
		
		public StringSeqSettingModule(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
		{
			this.ownerOfAddIn = ownerOfAddIn;
		}
		
		public override PropertyValue convertFromStringAbout(string about, string[] localePrefs, Ice.Current __current)
		{
			string[] stringSeq = about.Split(new char[]{','});
			for(int i=0;i < stringSeq.Length;i++)
			{
				stringSeq[i] = stringSeq[i].Trim();
			}
			return new StringSeqTI(stringSeq);
		}
		
		public override string getLabel(String[] localePrefs, Ice.Current __current)
		{
			return "Test Setting Module";
		}
		
		public override string getPropertyAbout(PropertyValue value, Ice.Current __current)
		{
			string[] stringSeq = ((StringSeqT)value).stringSeqValue;
			if(stringSeq.Length > 0)
			{
				string result = stringSeq[0];
				for(int i = 1;i<stringSeq.Length;i++)
				{
					result += "," + stringSeq[i];
				}
				return result;
			}
			else
				return "";
		}
		
		public override String getIdentifier(Ice.Current __current)
		{
			return "StringSeqT";
		}
		
		public override PropertyValue run(PropertyValue valueBefore, BoxModulePrx boxModuleParam, String[] localePrefs, ManagersEnginePrx manager, out String about, Ice.Current __current)
		{
			
			Ferda.FrontEnd.StringSeqForm f = new Ferda.FrontEnd.StringSeqForm();
			f.Items = ((StringSeqTI)valueBefore).stringSeqValue;
			f.ShowInTaskbar = false;
			System.Windows.Forms.DialogResult result = this.ownerOfAddIn.ShowDialog(f);
			if(result == System.Windows.Forms.DialogResult.OK)
			{
				PropertyValue resultValue = new StringSeqTI(f.Items);
				about = this.getPropertyAbout(resultValue);
				return resultValue;
			}
			else
			{
				about = this.getPropertyAbout(valueBefore);
				return valueBefore;
			}
		}
		
		
	}
}
