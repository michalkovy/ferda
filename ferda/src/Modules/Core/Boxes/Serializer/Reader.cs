// Reader.cs - Functions for deserialization of configuration files
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchař
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
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Ferda.Modules.Boxes.Serializer.Configuration;
using Ferda.Modules.Boxes.Serializer.Localization;

namespace Ferda.Modules.Boxes.Serializer
{
    /// <summary>
    /// Provides functions for deserealization of configuration files.
    /// </summary>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>
    public static class Reader
    {
        /// <summary>
        /// Deserealizes the specified type of serialized object 
        /// (<c>typeOfSerializedObject</c>) from specified XML file
        /// (<c>fileName</c>).
        /// </summary>
        /// <param name="typeOfSerializedObject">The type of serialized object.</param>
        /// <param name="xmlRootAttribute">Root element specification of the XML file.</param>
        /// <param name="fileName">Name of the XML file.</param>
        /// <returns>Deserealized specified object.</returns>
        public static object Deserealize(Type typeOfSerializedObject, XmlRootAttribute xmlRootAttribute, string fileName)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.

            // There can be some problems with startup performance
            // please see more information about 
            //  - XML Boxes.Serializer Generator Tool (Sgen.exe) 
            //  - Generate serialization assembly
            XmlSerializer serializer = new XmlSerializer(typeOfSerializedObject);
            //XmlSerializer serializer = new XmlSerializer(typeOfSerializedObject, xmlRootAttribute);

            // If the XML document has been altered with unknown
            // nodes, elements or attributesAndEquivalenceClasses, handle them with the
            // UnknownNode, UnknownElement and UnknownAttribute events.
            serializer.UnknownNode += new
                XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
                XmlAttributeEventHandler(serializer_UnknownAttribute);
            serializer.UnknownElement += new
                XmlElementEventHandler(serializer_UnknownElement);
            serializer.UnreferencedObject += new
                UnreferencedObjectEventHandler(serializer_UnreferencedObject);

            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            // Use the Deserialize method to restore the object's state with
            // data from the XML document.
            object result = serializer.Deserialize(fs);
            fs.Close();
            return result;
        }

        /// <summary>
        /// Reads the <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box">box`s configuration</see>.
        /// </summary>
        /// <param name="fileName">Name of the box`s configuration file.</param>
        /// <returns>Deserealized box`s configuration.</returns>
        /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>
        public static Box ReadBox(string fileName)
        {
#if DEBUG
#if MONO
            // validation does not work correctly
#else
            XmlValidator.ValidateXmlFile(fileName, "box.xsd");
#endif    
#endif
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute();
            xmlRootAttribute.Namespace = "http://ferda.is-a-geek.net";
            xmlRootAttribute.ElementName = "Box";
            return (Box) Deserealize(
                             typeof (Box),
                             xmlRootAttribute,
                             fileName);
        }

        /// <summary>
        /// Reads the <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization">box`s localization</see>.
        /// </summary>
        /// <param name="fileName">Name of the box`s localization file.</param>
        /// <returns>Deserealized box`s localization.</returns>
        /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization"/>
        public static BoxLocalization ReadBoxLocalization(string fileName)
        {
#if DEBUG
#if MONO
            // validation does not work correctly
#else
            XmlValidator.ValidateXmlFile(fileName, "boxLocalization.xsd");
#endif
#endif
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute();
            xmlRootAttribute.Namespace = "http://ferda.is-a-geek.net";
            xmlRootAttribute.ElementName = "BoxLocalization";
            try
            {
                return (BoxLocalization) Deserealize(
                                             typeof (BoxLocalization),
                                             xmlRootAttribute,
                                             fileName);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Handles the UnknownNode event of the serializer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Xml.Serialization.XmlNodeEventArgs"/> instance containing the event data.</param>
        private static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            object o = e.ObjectBeingDeserialized;
            Debug.WriteLine(
                "UnknownNode Name: " + e.Name
                + "\n" + "UnknownNode LocalName: " + e.LocalName
                + "\n" + "UnknownNode Namespace URI: " + e.NamespaceURI
                + "\n" + "UnknownNode Text: " + e.Text
                + "\n" + "Node Type: " + e.NodeType
                + "\n" + "Object being deserialized " + ((o == null) ? "Null" : o.ToString())
                );
        }

        /// <summary>
        /// Handles the UnknownAttribute event of the serializer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Xml.Serialization.XmlAttributeEventArgs"/> instance containing the event data.</param>
        private static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            Debug.WriteLine(
                "Unknown Attribute"
                + "\t" + e.Attr.Name + " " + e.Attr.InnerXml
                + "\t LineNumber: " + e.LineNumber
                + "\t LinePosition: " + e.LinePosition
                );
        }

        /// <summary>
        /// Handles the UnknownElement event of the serializer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Xml.Serialization.XmlElementEventArgs"/> instance containing the event data.</param>
        private static void serializer_UnknownElement(object sender, XmlElementEventArgs e)
        {
            Debug.WriteLine(
                "Unknown Element"
                + "\t" + e.Element.Name + " " + e.Element.InnerXml
                + "\t LineNumber: " + e.LineNumber
                + "\t LinePosition: " + e.LinePosition
                );
        }

        /// <summary>
        /// Handles the UnreferencedObject event of the serializer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Xml.Serialization.UnreferencedObjectEventArgs"/> instance containing the event data.</param>
        private static void serializer_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            Debug.WriteLine(
                "UnreferencedObject"
                + "\t" + "ID: " + e.UnreferencedId
                + "\t" + "UnreferencedObject: " + e.UnreferencedObject
                );
        }
    }
}
