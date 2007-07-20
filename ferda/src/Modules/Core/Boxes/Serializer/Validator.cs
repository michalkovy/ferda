// Validator.cs - Validates the XML configuration files agains the 
// specified XSD files
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
using System.Xml;

namespace Ferda.Modules.Boxes.Serializer
{
    /// <summary>
    /// Validates input XML files against specified XSD files.
    /// </summary>
    public static class XmlValidator
    {
        /// <summary>
        /// Validates the XML file against schema and trows exceptions if it is invalid.
        /// </summary>
        /// <param name="xmlDocumentPath">The XML document path.</param>
        /// <param name="xmlSchemaPath">The XML schema path.</param>
        public static void ValidateXmlFile(string xmlDocumentPath, string xmlSchemaPath)
        {
            StreamReader file = new StreamReader(xmlDocumentPath);
            string xmlDocument = file.ReadToEnd();
            ValidateXml(xmlDocument, xmlSchemaPath);
            file.Close();
        }

        /// <summary>
        /// This method validates given XML against schema and throws exceptions if it is invalid.
        /// </summary>
        /// <param name="xml">XML document to be validated.</param>
        /// <param name="xmlSchemaPath">A filename of the XSD schema.</param>
        public static void ValidateXml(string xml, string xmlSchemaPath)
        {
            StringReader stringReader = null;
            XmlReader xmlReader = null;

            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, xmlSchemaPath);
                settings.XmlResolver = null;

                stringReader = new StringReader(xml);
                xmlReader = XmlReader.Create(stringReader, settings);

                while (xmlReader.Read())
                    ;
            }
            finally
            {
                if (stringReader != null)
                    stringReader.Close();

                if (xmlReader != null)
                    xmlReader.Close();
            }
        }
    }
}