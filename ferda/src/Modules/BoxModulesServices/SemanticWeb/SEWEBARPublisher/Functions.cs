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
using Ferda.Modules.Boxes.SemanticWeb.PMMLBuilder;

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

        /// <summary>
        /// The dictionary of ID's and articles of the logged user
        /// </summary>
        protected List<string> IdsArticles = null;

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
        //public IntTI ArticleId
        //{
        //    get
        //    {
        //        //return boxModule.GetPropertyInt(SockArticleId);
        //        return new IntTI(-1);
        //    }
        //}

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

        /// <summary>
        /// Returns the array of articles and their ID's.
        /// If the IdsArticles is null, the function returns only one entry
        /// saying New article
        /// </summary>
        /// <returns></returns>
        public string[] GetIdsArticles()
        {
            if (IdsArticles == null)
            {
                return new string[] { "NewArticle(ID=-1)" };
            }
            else
            {
                return IdsArticles.ToArray();
            }
        }

        /// <summary>
        /// The main method that publishes the PMML article to SEWEBAR Joomla! web
        /// service
        /// </summary>
        public void PublishToSEWEBAR()
        {
            PMMLBuilderFunctionsPrx pmmlPrx =
                SocketConnections.GetPrx<PMMLBuilderFunctionsPrx>(
                boxModule, SockPMMLBuilder,
                PMMLBuilderFunctionsPrxHelper.checkedCast,
                true);
            string pmml = pmmlPrx.getPMML();

            ISewebar proxy = XmlRpcProxyGen.Create<ISewebar>();
            proxy.Url = XMLRPCHost;

            string response =
                proxy.uploadFile(pmml, UserName, Password, GetTitle(ArticleTitle), GetId(ArticleTitle));
        }

        /// <summary>
        /// Retrieves the files of the user from the SEWEBAR CMS repository. 
        /// The files are stored into the IdsArticles, which should not be
        /// null.
        /// </summary>
        public void ListFiles()
        {
            ISewebar proxy = XmlRpcProxyGen.Create<ISewebar>();
            proxy.Url = XMLRPCHost;
            XmlRpcStruct [] response = null;

            response = proxy.listFiles(UserName, Password, string.Empty,
                string.Empty);


            if (response.Length == 0)
            {
                return;
            }

            IdsArticles = new List<string>(response.Length);
            foreach (XmlRpcStruct s in response)
            {
                string str = s["title"].ToString();
                str = str + "(ID=" + s["id"].ToString() + ')';
                IdsArticles.Add(str);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// The method retrieves the numeric ID of the article out of the 
        /// <paramref name="ArticleTitle"/> parameter, which is in form 
        /// ArticleTitle(Id=x)
        /// </summary>
        /// <param name="ArticleTitle">The article title in form ArticleTitle(Id=x)</param>
        /// <returns>ID of the article</returns>
        private int GetId(string ArticleTitle)
        {
            if (ArticleTitle.Contains("(ID="))
            {
                string number = ArticleTitle.Substring(
                    ArticleTitle.LastIndexOf('=')+1,
                    ArticleTitle.LastIndexOf(')') - ArticleTitle.LastIndexOf('=')-1);
                return Convert.ToInt32(number);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// The method retrieves the title of the article out of the 
        /// <paramref name="ArticleTitle"/> parameter, which is in form 
        /// ArticleTitle(Id=x)
        /// </summary>
        /// <param name="ArticleTitle">The article title in form ArticleTitle(Id=x)</param>
        /// <returns>Title of the article</returns>
        private string GetTitle(string ArticleTitle)
        {
            if (ArticleTitle.Contains("(ID="))
            {
                return ArticleTitle.Substring(0, ArticleTitle.LastIndexOf('('));
            }
            else
            {
                return ArticleTitle;
            }
        }

        #endregion 
    }

    /// <summary>
    /// The interface for the web service
    /// </summary>
    public interface ISewebar : IXmlRpcProxy
    {
        /// <summary>
        /// The method uploads the XML file into designated section and directory.
        /// It is intended for PMML reports to be exported from Ferda and
        /// LISp-Miner DM tools directly into Joomla for the follow-up usage
        /// with the gInclude plugin. 
        /// </summary>
        /// <param name="articleText">Text of the article (PMML)</param>
        /// <param name="userName">User name for authentication</param>
        /// <param name="password">User password for authentication</param>
        /// <param name="articleTitle">Title of the article</param>
        /// <param name="articleId">ID of the article</param>
        /// <returns>
        /// TODO
        /// </returns>
        [XmlRpcMethod("uploadXML.uploadFile")]
        string uploadFile(string articleText, string userName, 
            string password, string articleTitle, int articleId);

        /// <summary>
        /// Allows RPC client to retrieve all articles from specified 
        /// section and category for specified user. The goal is to enable
        /// user to overwrite and update previously updated PMML reports. 
        /// </summary>
        /// <param name="userName">User name for authentication</param>
        /// <param name="password">User password for authentication</param>
        /// <param name="section">Joomla section (optional)</param>
        /// <param name="category">Joomla category (optional)</param>
        /// <returns></returns>
        [XmlRpcMethod("uploadXML.listFiles")]
        XmlRpcStruct [] listFiles(string userName, string password,
            string section, string category);
    }
}
