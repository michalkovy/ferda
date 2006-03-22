// ModuleForInteractionInfo.cs - Information about module for interaction
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

        /// <summary>
        /// Information about module for interaction
        /// </summary>
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
			
            /// <summary>
            /// Sockets of box on which this module has to run that are needed to have
            /// something inside before executing
            /// </summary>
			public string[] NeededConectedSockets
			{
				get {
					return neededConectedSockets;
				}
			}
			
            /// <summary>
            /// Accepted types of box on whit this module can be run
            /// </summary>
			public BoxType[] AcceptedBoxTypes
			{
				get {
					return acceptedBoxTypes;
				}
			}
			
            /// <summary>
            /// Label of this module
            /// </summary>
			public string Label
			{
				get {
					return label;
				}
			}
			
            /// <summary>
            /// Something more about this module
            /// </summary>
			public string Hint
			{
				get {
					return hint;
				}
			}
			
            /// <summary>
            /// Informations about helps of this module
            /// </summary>
			public HelpFileInfo[] HelpInfo
			{
				get {
					return helpInfo;
				}
			}
			
            /// <summary>
            /// Dynamic help structure
            /// </summary>
			public DynamicHelpItem[] DynamicHelp
			{
				get {
					return dynamicHelp;
				}
			}
			
            /// <summary>
            /// Icon of this module
            /// </summary>
			public byte[] Icon
			{
				get {
					return icon;
				}
			}
			
            /// <summary>
            /// Ice identity of this module
            /// </summary>
			public string IceIdentity
			{
				get {
					return iceIdentity;
				}
			}
		}
    }
}
