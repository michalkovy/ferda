using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;

namespace Sewebar
{
    /// <summary>
    /// The class provides static methods for accessing the XMLRPC web service
    /// to be used in the SEWEBAR project. The class (via a library) is shared 
    /// by the Ferda and LISp-Miner systems and refactors the common functionality
    /// for the two systems. 
    /// </summary>
    public class Sewebar
    {
        /// <summary>
        /// The method uploads the XML file into designated section and directory.
        /// It is intended for PMML reports to be exported from Ferda and
        /// LISp-Miner DM tools directly into Joomla for the follow-up usage
        /// with the gInclude plugin. 
        /// </summary>
        /// <param name="XMLRPCHost">The URL of the XMLRPC server side service,
        /// full path to the service (including the directory)</param>
        /// <param name="pmml">Text of the article (PMML)</param>
        /// <param name="username">User name for authentication</param>
        /// <param name="password">User password for authentication</param>
        /// <param name="atricleTitle">Title of the article</param>
        /// <param name="articleID">ID of the article</param>
        /// <returns>Message from the XMLRPC service</returns>
        public static string PublishToSewebar(
            string XMLRPCHost,
            string pmml,
            string username,
            string password,
            string atricleTitle,
            string articleID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows RPC client to retrieve all articles from specified 
        /// section and category for specified user. The goal is to enable
        /// user to overwrite and update previously updated PMML reports. 
        /// </summary>
        /// <param name="XMLRPCHost">The URL of the XMLRPC server side service,
        /// full path to the service (including the directory)</param>
        /// <param name="username">User name for authentication</param>
        /// <param name="password">User password for authentication</param>
        /// <returns></returns>
        public IDictionary<int, string> ListFiles(
            string XMLRPCHost,
            string username,
            string password)
        {
            throw new NotImplementedException();
        }
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
        /// Message from the XMLRPC service
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
        /// <returns>A structure containing files and their ID's</returns>
        [XmlRpcMethod("uploadXML.listFiles")]
        XmlRpcStruct[] listFiles(string userName, string password,
            string section, string category);
    }
}
