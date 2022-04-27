// FrontEndConfig.cs - FrontEnd configuration (mainly ICE)
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Martin Ralbovský
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

using System.IO;
using System.Xml.Serialization;
using System;
using System.Reflection;
using System.Text;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// Class deals with the configuration of the Ferda FrontEnd environment.
    /// It includes fields that are filled with the options for the Project
    /// Manager and other configuration of the FrontEnd
    /// </summary>
	public struct FrontEndConfig
	{
        /// <summary>
        /// Name of the configuration file
        /// </summary>
		public const string FileName = "FrontEndConfig.xml";

        /// <summary>
        /// Options for Project Manager
        /// </summary>
        public Ferda.ProjectManager.ProjectManagerOptions ProjectManagerOptions;
		
        /// <summary>
        /// FrontEnd ICE objects
        /// </summary>
		public string[] FrontEndIceObjects;

        /// <summary>
        /// Determines if the PropertyGrid should show the VisibleSockets group
        /// of properties that should be shown as sockets
        /// </summary>
        public bool ShowVisibleSockets;

        /// <summary>
        /// User determines if the progress bars should display a 
        /// dialog showing exact time elapsed for each progress bar
        /// running. This can be useful i.e. when timing hypotheses 
        /// generation.
        /// </summary>
        public bool DisplayTiming;

        /// <summary>
        /// Loads the FrontEndConfig.xml file from the directory, where
        /// the FerdaFrontEnd.exe file is located
        /// </summary>
        /// <exception cref="T:System.Exception">
        /// The loading of the file failed (perhaps the file is not there)
        /// </exception>
        /// <returns>FrontEndConfig object that contains project configuration info
        /// </returns>
		public static FrontEndConfig Load()
		{
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var path = Path.Combine(folder, FileName);

            System.IO.FileStream fs;
            //tries to open the file
            try
            {
                fs = new System.IO.FileStream(path, System.IO.FileMode.Open);
            }
            catch (Exception e)
            {
                throw new Exception("Application was not able to load FrontEndConfig.xml", e);
            }

			FrontEndConfig config = new FrontEndConfig();

            //tries to deserialize the FrontEndConfig.xml file
			try
			{
				XmlSerializer s = new XmlSerializer( typeof( FrontEndConfig ) );
				TextReader r = new StreamReader(fs);
				config = (FrontEndConfig)s.Deserialize( r );
				r.Close();
			}
			finally
			{
				fs.Close();
			}
			return config;
		}
		
        /// <summary>
        /// Saves the into the location where the executing assembly (.exe file) 
        /// resides
        /// </summary>
        /// <param name="config">Config file that should be saved</param>
        /// <exception cref="T:System.Exception">
        /// The serialization of the file failed
        /// </exception> 
		public static void Save(FrontEndConfig config)
		{
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var path = Path.Combine(folder, FileName);

            System.IO.FileStream fs = null;
            try
            {
                fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
                XmlSerializer s = new XmlSerializer(typeof(FrontEndConfig));
                TextWriter w = new StreamWriter(fs);
                s.Serialize(w, config);
                w.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Application was not able to save FrontEndConfig.xml", e);
            }
			finally
			{
				fs.Close();
			}
		}
	}
}
