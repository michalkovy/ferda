// SampleStringSeqSettingModule.cs - example of Setting Module implementation
//
// Author: Michal Kováč <michal.kovac.develop@centrum.cz>
//
// Copyright (c) 2005 Michal Kováč 
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Text;
using Ferda.Modules;
using Ferda.ModulesManager;
using Ice;

namespace Ferda.Modules
{
    /// <summary>
    /// Example of Setting Module implementation (in this time historical class,
    /// not used for NUnit test any more)
    /// </summary>
	class SampleStringSeqSettingModule : SettingModuleWithStringAbilityDisp_
	{
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
		
		public override PropertyValue run(PropertyValue valueBefore, BoxModulePrx boxModuleParam, String[] localePrefs, ManagersEnginePrx manager, out String about, Ice.Current __current)
		{
			about = "ttt";
			return new StringSeqTI(new string[]{"ttt"});
	}



    public override string getIdentifier(Current current__)
    {
        return "SampleStringSeq";
    }
}
}
