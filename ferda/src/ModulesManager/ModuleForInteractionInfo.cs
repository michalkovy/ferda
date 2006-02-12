// 
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
using Ferda.Modules;


namespace Ferda {
    namespace ModulesManager {
		public class ModuleForInteractionInfo {

			public ModuleForInteractionInfo(ModuleForInteractionPrx prx, Helper helper)
			{
				this.neededConectedSockets = prx.getNeededConnectedSockets();
				this.acceptedBoxTypes = prx.getAcceptedBoxTypes();
				this.label = prx.getLabel(helper.LocalePrefs);
				this.hint = prx.getHint(helper.LocalePrefs);
				this.helpInfo = prx.getHelpFileInfoSeq(helper.LocalePrefs);
				this.dynamicHelp = prx.getDynamicHelpItems(helper.LocalePrefs);
				this.icon = prx.getIcon();
				this.iceIdentity = Ice.Util.identityToString(prx.ice_getIdentity());
			}
			
        	private string[] neededConectedSockets;
			private BoxType[] acceptedBoxTypes;
			private string label;
			private string hint;
			private HelpFileInfo[] helpInfo;
			private DynamicHelpItem[] dynamicHelp;
			private byte[] icon;
            private string iceIdentity;
			
			public string[] NeededConectedSockets
			{
				get {
					return neededConectedSockets;
				}
			}
			
			public BoxType[] AcceptedBoxTypes
			{
				get {
					return acceptedBoxTypes;
				}
			}
			
			public string Label
			{
				get {
					return label;
				}
			}
			
			public string Hint
			{
				get {
					return hint;
				}
			}
			
			public HelpFileInfo[] HelpInfo
			{
				get {
					return helpInfo;
				}
			}
			
			public DynamicHelpItem[] DynamicHelp
			{
				get {
					return dynamicHelp;
				}
			}
			
			public byte[] Icon
			{
				get {
					return icon;
				}
			}
			
			public string IceIdentity
			{
				get {
					return iceIdentity;
				}
			}
		}
    }
}
