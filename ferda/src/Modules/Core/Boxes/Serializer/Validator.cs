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