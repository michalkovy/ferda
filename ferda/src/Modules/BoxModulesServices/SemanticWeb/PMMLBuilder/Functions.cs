﻿// Functions.cs - Function objects for the PMML Builder box
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.cz>
//
// Copyright (c) 2007 Martin Ralbovský
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
using Ice;
using System.IO;
using System.Reflection;
using System.Xml;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Guha.Data;
using Ferda.Guha.Attribute;

namespace Ferda.Modules.Boxes.SemanticWeb.PMMLBuilder
{
    /// <summary>
    /// Class is providing ICE functionality of the PMMLBuilder
    /// box module
    /// </summary>
    class Functions : PMMLBuilderFunctionsDisp_, Ferda.Modules.IFunctions
    {
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
        /// The PMML string
        /// </summary>
        protected string PMML = null;

        /// <summary>
        /// The cached bit string generators of the connected 4FT task
        /// </summary>
        private BitStringGeneratorPrx[] _bsg = null;

        #endregion

        #region Properties

        /// <summary>
        /// The path where the PMML file should be saved
        /// </summary>
        public string PMMLFile
        {
            get
            {
                return boxModule.GetPropertyString(SockPMMLFile);
            }
        }

        /// <summary>
        /// Author of the PMML report
        /// </summary>
        public string Author
        {
            get
            {
                return boxModule.GetPropertyString(SockAuthor);
            }
        }

        #endregion

        #region Sockets

        /// <summary>
        /// Name of the socket defining the location of PMML file for export
        /// </summary>
        public const string SockPMMLFile = "PMMLFile";

        /// <summary>
        /// Name of the socket determining the 4FT task
        /// </summary>
        public const string Sock4FTTask = "4FTTask";

        /// <summary>
        /// Name of the socket determining the author of the PMML report
        /// </summary>
        public const string SockAuthor = "Author";

        #endregion

        #region ICE functions

        /// <summary>
        /// Builds (if not built previously) and returns a PMML document
        /// </summary>
        /// <param name="__current">Ice stuff</param>
        /// <returns>A PMML document</returns>
        public override string getPMML(Ice.Current __current)
        {
            if (PMML == null)
            {
                PMML = BuildPMML();
                return PMML;
            }
            else
            {
                return PMML;
            }
        }

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
        /// Saves the PMML to file specified by the property "PMMLFile".
        /// </summary>
        public void SavePMMLToFile()
        {
            if (PMML == null)
            {
                PMML = BuildPMML();
            }

            //deleting the file if it exists
            if (File.Exists(PMMLFile))
            {
                File.Delete(PMMLFile);
            }

            using (FileStream fs = File.Create(PMMLFile))
            {
                UnicodeEncoding uniEncoding = new UnicodeEncoding();

                fs.Write(uniEncoding.GetBytes(PMML), 
                    0, uniEncoding.GetByteCount(PMML));
            }
        }

        /// <summary>
        /// Gets the result from the 4FT task
        /// </summary>
        /// <returns>Result from the 4FT task</returns>
        public Result GetResult()
        {
            string statistics = String.Empty;

            MiningTaskFunctionsPrx taskPrx =
                SocketConnections.GetPrx<MiningTaskFunctionsPrx>(
                    boxModule,
                    Sock4FTTask,
                    MiningTaskFunctionsPrxHelper.checkedCast,
                    true);

            string serializedResult = taskPrx.GetResult(out statistics);
            return SerializableResult.Deserialize(serializedResult);
        }

        /// <summary>
        /// Gets the result information from the 4FT task
        /// </summary>
        /// <returns>Result informationfrom the 4FT task</returns>
        public SerializableResultInfo GetResultInfo()
        {
            string statistics = String.Empty;

            MiningTaskFunctionsPrx taskPrx =
                SocketConnections.GetPrx<MiningTaskFunctionsPrx>(
                    boxModule,
                    Sock4FTTask,
                    MiningTaskFunctionsPrxHelper.checkedCast,
                    true);

            string serializedResult = taskPrx.GetResult(out statistics);
            return SerializableResultInfo.Deserialize(statistics);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Builds and returns the PMML
        /// </summary>
        /// <returns>PMML</returns>
        protected string BuildPMML()
        {
            StringWriter strWriter = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(strWriter);

            //Use automatic indentation for readability.
            xmlWriter.Formatting = Formatting.Indented;
            //Write the start of the document
            xmlWriter.WriteStartDocument();

            //The PMML element
            xmlWriter.WriteStartElement("PMML");
            xmlWriter.WriteAttributeString("version", "3.0");
            xmlWriter.WriteAttributeString("xmlns", "http://www.dmg.org/PMML-3_2");
            xmlWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xmlWriter.WriteAttributeString("xsi:schemaLocation", "http://www.dmg.org/PMML-3_2 http://www.dmg.org/v3-2/pmml-3-2.xsd");

            //The Header element
            xmlWriter.WriteStartElement("Header");
            xmlWriter.WriteAttributeString("copyright", "Copyright (c) KIZI UEP");
            xmlWriter.WriteStartElement("Extension");
            xmlWriter.WriteAttributeString("name", "author");
            xmlWriter.WriteAttributeString("value", Author);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Application");
            xmlWriter.WriteAttributeString("name", "Ferda");
            //The version of the Ferda system in the PMML corresponds to the 
            //FerdaSemanticWebBoxes.dll assembly.
            Assembly ass = Assembly.GetExecutingAssembly();
            xmlWriter.WriteAttributeString("version", ass.GetName().Version.ToString());
            xmlWriter.WriteEndElement(); //Application
            xmlWriter.WriteElementString("Annotation", "Exported to PMML using the Ferda software");
            xmlWriter.WriteEndElement(); //Header

            xmlWriter = CreateDataDictionary(xmlWriter);
            xmlWriter = CreateTransformationDictionary(xmlWriter);
            xmlWriter = CreateAssociationModel(xmlWriter);

            xmlWriter.WriteEndElement(); //PMML

            xmlWriter.Close();
            return strWriter.ToString();
        }

        /// <summary>
        /// Creates and fills the AssociationModel element of the PMML
        /// </summary>
        /// <param name="xmlWriter">Here the content is written</param>
        /// <returns>Writer with added content association model</returns>
        protected XmlTextWriter CreateAssociationModel(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("AssociationModel");


            xmlWriter.WriteEndElement();
            return xmlWriter;
        }
        
        /// <summary>
        /// Creates and fills the TransformationDictionary element of the PMML
        /// </summary>
        /// <param name="xmlWriter">Here the content is written</param>
        /// <returns>Writer with added content transformation dictionary</returns>
        protected XmlTextWriter CreateTransformationDictionary(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("TransformationDictionary");

            //assuming _bsg is not null (was initialized)
            foreach (BitStringGeneratorPrx generator in _bsg)
            {
                xmlWriter.WriteStartElement("DerivedField");
                xmlWriter.WriteAttributeString("optype", GetPMMLOptype(generator));
                ValuesAndFrequencies vaf = generator.GetColumnValuesAndFrequencies();
                xmlWriter.WriteAttributeString("dataType",
                    DBDataTypeToPMMLDataType(vaf.dataType));

                //deserializing attribute
                Attribute<IComparable> attr =
                    Ferda.Guha.Attribute.Serializer.RetypeAttributeSerializable(
                        generator.getAttribute(), vaf.dataType);
                if (attr.ContainsIntervals())
                {
                    xmlWriter.WriteStartElement("Discretize");
                    xmlWriter.WriteAttributeString("field", GetAttributeName(generator));

                    foreach (Category<IComparable> category in attr.Values)
                    {
                        xmlWriter.WriteStartElement("DiscretizeBin");
                        xmlWriter.WriteEndElement();//DiscretizeBin
                    }

                    xmlWriter.WriteEndElement();//Discretize
                }
                else
                {
                    xmlWriter.WriteStartElement("MapValues");
                    xmlWriter.WriteEndElement();//MapValues
                }

                xmlWriter.WriteEndElement();//DerivedField
            }

            xmlWriter.WriteEndElement();
            return xmlWriter;
        }

        /// <summary>
        /// Creates and fills the DataDictionary element of the PMML
        /// </summary>
        /// <param name="xmlWriter">Here the content is written</param>
        /// <returns>Writer with added content data dictionary`</returns>
        protected XmlTextWriter CreateDataDictionary(XmlTextWriter xmlWriter)
        {
            MiningTaskFunctionsPrx taskPrx =
                SocketConnections.GetPrx<MiningTaskFunctionsPrx>(
                    boxModule,
                    Sock4FTTask,
                    MiningTaskFunctionsPrxHelper.checkedCast,
                    true);
            _bsg = taskPrx.GetBitStringGenerators();

            xmlWriter.WriteStartElement("DataDictionary");

            //Number of fields attribute
            xmlWriter.WriteAttributeString("numberOfFields", _bsg.Length.ToString());

            /// !!! Tady bude mozna jeste casem nutne zavest kontrolu disjunktnosti proxin,
            /// ale zatim bych to neresil

            foreach (BitStringGeneratorPrx generator in _bsg)
            {
                xmlWriter.WriteStartElement("DataField");
                xmlWriter.WriteAttributeString("name",GetAttributeName(generator));
                xmlWriter.WriteAttributeString("optype",GetPMMLOptype(generator));
                ValuesAndFrequencies vaf = generator.GetColumnValuesAndFrequencies();
                xmlWriter.WriteAttributeString("dataType", 
                    DBDataTypeToPMMLDataType(vaf.dataType));

                foreach (ValueFrequencyPair pair in vaf.data)
                {
                    xmlWriter.WriteStartElement("Value");
                    xmlWriter.WriteAttributeString("value", pair.value);
                    xmlWriter.WriteStartElement("Extension");
                    xmlWriter.WriteAttributeString("name", "Frequency");
                    xmlWriter.WriteAttributeString("value", pair.frequency.ToString());
                    xmlWriter.WriteAttributeString("extender", pair.value);
                    xmlWriter.WriteEndElement();//Extension
                    xmlWriter.WriteEndElement();//Value
                }

                xmlWriter.WriteEndElement();//DataField
            }

            xmlWriter.WriteEndElement(); //DataDictionary
            return xmlWriter;
        }

        /// <summary>
        /// Returns name of an attribute depending on its GUID
        /// </summary>
        /// <param name="prx">Bit string generator proxy (attribute name
        /// provider)</param>
        /// <returns>Name of the attribute</returns>
        protected string GetAttributeName(BitStringGeneratorPrx prx)
        {
            GuidAttributeNamePair[] listOfPairs = prx.GetAttributeNames();
            foreach (GuidAttributeNamePair pair in listOfPairs)
            {
                if (pair.id == prx.GetAttributeId())
                {
                    return pair.attributeName;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a PMML OPTYPE (type of the data) from Ferda's
        /// cardinality information
        /// </summary>
        /// <param name="prx">Bit string generator proxy</param>
        /// <returns>PMML type of the data</returns>
        protected string GetPMMLOptype(BitStringGeneratorPrx prx)
        {
            CardinalityEnum card = prx.GetAttributeCardinality();
            switch (card)
            {
                case CardinalityEnum.Cardinal:
                    return "continuous";
                case CardinalityEnum.Nominal:
                    return "categorical";
                case CardinalityEnum.Ordinal:
                    return "ordinal";
                case CardinalityEnum.OrdinalCyclic:
                    return "ordinal";
            }

            return string.Empty;
        }

        /// <summary>
        /// Switches the Ferda.Data database type to PMML one
        /// </summary>
        /// <param name="datatype">Ferda.Data database type</param>
        /// <returns>PMML data type</returns>
        protected string DBDataTypeToPMMLDataType(DbDataTypeEnum datatype)
        {
            switch (datatype)
            {
                case DbDataTypeEnum.BooleanType:
                    return "integer";
                case DbDataTypeEnum.DateTimeType:
                    return "dateTime";
                case DbDataTypeEnum.DecimalType:
                    return "float";
                case DbDataTypeEnum.DoubleType:
                    return "double";
                case DbDataTypeEnum.FloatType:
                    return "float";
                case DbDataTypeEnum.IntegerType:
                    return "integer";
                case DbDataTypeEnum.LongIntegerType:
                    return "integer";
                case DbDataTypeEnum.ShortIntegerType:
                    return "integer";
                case DbDataTypeEnum.StringType:
                    return "string";
                case DbDataTypeEnum.TimeType:
                    return "time";
                case DbDataTypeEnum.UnknownType:
                    return "string";
                case DbDataTypeEnum.UnsignedIntegerType:
                    return "integer";
                case DbDataTypeEnum.UnsignedLongIntegerType:
                    return "integer";
                case DbDataTypeEnum.UnsignedShortIntegerType:
                    return "integer";
                default:
                    return null;
            }
        }

        #endregion
    }
}
