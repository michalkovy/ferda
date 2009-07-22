// Functions.cs - Function objects for the SEWEBAR Publisher box
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.cz>
//
// Copyright (c) 2009 Martin Ralbovský
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
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;

namespace Ferda.Modules.Boxes.SemanticWeb.SEWEBARPublisher
{
    /// <summary>
    /// Class is providing ICE functionality of the SEWEBARPublisher
    /// box module
    /// </summary>
    public class Functions : SEWEBARPublisherFunctionsDisp_, Ferda.Modules.IFunctions
    {
        #region ICE functions
        #endregion

        #region Protected fields

        /// <summary>
        /// The box module
        /// </summary>
        protected Ferda.Modules.BoxModuleI boxModule;

        /// <summary>
        /// The box info
        /// </summary>
        protected Ferda.Modules.Boxes.IBoxInfo boxInfo;

        #endregion

        #region Properties

        /// <summary>
        /// The URL address of the XML-RPC server where the web service uploading
        /// the articles is running
        /// </summary>
        public string XMLRPCHost
        {
            get
            {
                return boxModule.GetPropertyString(SockXMLRPCHost);
            }
        }

        /// <summary>
        /// The file where the article should be uploaded via the 
        /// XML-RPC web service
        /// </summary>
        public string ServerFile
        {
            get
            {
                return boxModule.GetPropertyString(SockServerFile);
            }
        }

        /// <summary>
        /// The user name to be authenticated in the SEWEBAR application
        /// </summary>
        public string UserName
        {
            get
            {
                return boxModule.GetPropertyString(SockUserName);
            }
        }

        /// <summary>
        /// The password to be authenticated in the SEWEBAR application
        /// </summary>
        public string Password
        {
            get
            {
                return boxModule.GetPropertyString(SockPassword);
            }
        }

        /// <summary>
        /// The title of the article to be published
        /// </summary>
        public string ArticleTitle
        {
            get
            {
                return boxModule.GetPropertyString(SockArticleTitle);
            }
        }

        /// <summary>
        /// The ID of the article to be published
        /// </summary>
        public long ArticleId
        {
            get
            {
                return boxModule.GetPropertyLong(SockArticleId);
            }
        }

        #endregion

        #region Sockets

        /// <summary>
        /// Name of the socket of the PMMLBuilder to be connected
        /// </summary>
        public const string SockPMMLBuilder = "PMMLBuilder";

        /// <summary>
        /// Name of the socket determining the address of the XML-RPC host
        /// </summary>
        public const string SockXMLRPCHost = "XMLRPCHost";

        /// <summary>
        /// Name of the socket determining the file on the server
        /// </summary>
        public const string SockServerFile = "ServerFile";

        /// <summary>
        /// Name of the socket determining the name of the user of the SEWEBAR
        /// application
        /// </summary>
        public const string SockUserName = "UserName";

        /// <summary>
        /// Name of the socket determining the user's password of the SEWEBAR
        /// application
        /// </summary>
        public const string SockPassword = "Password";

        /// <summary>
        /// Name of the socket determining the title of the article to be 
        /// published. 
        /// </summary>
        public const string SockArticleTitle = "ArticleTitle";

        /// <summary>
        /// Name of the socket determining the ID of the article to be 
        /// published. 
        /// </summary>
        public const string SockArticleId = "ArticleId";

        #endregion

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        void Ferda.Modules.IFunctions.setBoxModuleInfo(Ferda.Modules.BoxModuleI boxModule, Ferda.Modules.Boxes.IBoxInfo boxInfo)
        {
            this.boxModule = boxModule;
            this.boxInfo = boxInfo;
        }

        #endregion

        #region Public methods

        public void PublishToSEWEBAR()
        {
            ISewebar proxy = XmlRpcProxyGen.Create<ISewebar>();
            string response =
                proxy.uploadXML("text", UserName, Password, ArticleTitle, ArticleId);
        }

        #endregion
    }

    [XmlRpcUrl("http://sewebar-dev.vse.cz/xmlrpc")]
    public interface ISewebar : IXmlRpcProxy
    {
        [XmlRpcMethod("uploadXML")]
        string uploadXML(string articleText, string userName, 
            string password, string articleTitle, long articleId);
    }
}
