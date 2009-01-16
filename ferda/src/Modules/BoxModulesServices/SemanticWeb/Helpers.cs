// Helpers.cs - Helper classes for the SemanticWeb boxes
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
using System.Xml;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Results;

namespace Ferda.Modules.Boxes.SemanticWeb.Helpers
{
    /// <summary>
    /// A helping structure for storing information about Boolean attrbutes in PMML
    /// </summary>
    public struct PMMLBooleanAttribute
    {
        /// <summary>
        /// The Entity setting
        /// </summary>
        public IEntitySetting EntitySetting;

        /// <summary>
        /// Unique identifier of the Boolean attribute used for XML referencing
        /// </summary>
        public int XmlId;

        /// <summary>
        /// The text representatio of the Boolean attribute, which is the
        /// same as the default identifiers of the Boolean attribute boxes.
        /// </summary>
        public string TextRepresentation;

        /// <summary>
        /// XML ID's of the ancestors of this Boolean attribute
        /// </summary>
        public int[] Ancestors;

        /// <summary>
        /// Name of the attribute from which the Boolean attribute is created
        /// </summary>
        public string AttributeName;
    }

    /// <summary>
    /// Class for providing helping information for the PMMLBuilder box for
    /// providing Boolean attributes
    /// </summary>
    public class PMMLBooleanAttributeHelper
    {
        #region Private fields

        /// <summary>
        /// List of Boolean attribute helping structures.
        /// </summary>
        private List<PMMLBooleanAttribute> BAs = new List<PMMLBooleanAttribute>();

        /// <summary>
        /// The PMML builder functions object for retrieving unique ID's
        /// </summary>
        private PMMLBuilder.Functions functions;

        /// <summary>
        /// Identifier of the antecedent
        /// </summary>
        private int antecedentID;

        /// <summary>
        /// Identifier of the succedent
        /// </summary>
        private int succedentID;

        /// <summary>
        /// Identifier of the condition
        /// </summary>
        private int conditionID = -1;

        #endregion

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="functions">The PMML builder functions object</param>
        /// <param name="taskPrx">The 4FT task proxy</param>
        public PMMLBooleanAttributeHelper(PMMLBuilder.Functions functions,
            BoxModulePrx taskPrx)
        {
            this.functions = functions;

            BoxModulePrx antPrx = taskPrx.getConnections("Antecedent")[0];
            BoxModulePrx succPrx = taskPrx.getConnections("Succedent")[0];
            BoxModulePrx condPrx = null;
            try //in case that condition is not connected
            {
                condPrx = taskPrx.getConnections("Condition")[0];
            }
            catch { }

            antecedentID = CreatePMMLBA(antPrx);
            succedentID = CreatePMMLBA(succPrx);
            if (condPrx != null)
            {
                conditionID = CreatePMMLBA(condPrx);
            }
        }

        /// <summary>
        /// Writes basic Boolean attributes to the XML output
        /// </summary>
        /// <param name="output">The XML output</param>
        /// <returns>XML output with basic Boolean information</returns>
        public XmlTextWriter WriteBasicBooleanAttributes(XmlTextWriter output)
        {
            foreach (PMMLBooleanAttribute attr in BAs)
            {
                if (attr.EntitySetting is ILeafEntitySetting)
                {
                    output.WriteStartElement("BasicBooleanAttributeSetting");
                    output.WriteAttributeString("id", attr.XmlId.ToString());
                    output.WriteAttributeString("name", attr.TextRepresentation);
                    output.WriteElementString("Attribute", attr.AttributeName);

                    if (attr.EntitySetting is CoefficientSetting)
                    {
                        CoefficientSetting setting = attr.EntitySetting as CoefficientSetting;
                        output.WriteElementString("CoefficientType", 
                            GetPMMLCoefficientType(setting.coefficientType));
                        output.WriteElementString("MinimalLength",
                            setting.minLength.ToString());
                        output.WriteElementString("MaximalLength",
                            setting.maxLength.ToString());
                    }

                    if (attr.EntitySetting is CoefficientFixedSetSetting)
                    {
                        CoefficientFixedSetSetting setting = 
                            attr.EntitySetting as CoefficientFixedSetSetting;
                        output.WriteElementString("CoefficientType", "FixedSet");
                        foreach (string s in setting.categoriesIds)
                        {
                            output.WriteElementString("Category", s);
                        }
                    }

                    output.WriteEndElement();//BasicBooleanAttributeSetting
                }
            }

            return output;
        }

        /// <summary>
        /// Writes derived Boolean attributes to the XML output
        /// </summary>
        /// <param name="output">The XML output</param>
        /// <returns>XML output with derived Boolean information</returns>
        public XmlTextWriter WriteDerivedBooleanAttributes(XmlTextWriter output)
        {
            foreach (PMMLBooleanAttribute attr in BAs)
            {
                if (attr.EntitySetting is ISingleOperandEntitySetting)
                {
                    output.WriteStartElement("DerivedBooleanAttributeSetting");
                    output.WriteAttributeString("type", "Sign");
                    output.WriteAttributeString("id", attr.XmlId.ToString());
                    output.WriteAttributeString("name", attr.TextRepresentation);
                    output.WriteElementString("ConnectiveOrAtomId", 
                        attr.Ancestors[0].ToString());
                    if (attr.EntitySetting is NegationSetting)
                    {
                        output.WriteElementString("Type", "Negative");
                    }
                    if (attr.EntitySetting is BothSignsSetting)
                    {
                        output.WriteElementString("Type", "Both");
                    }
                    output.WriteEndElement(); //DerivedBoolenaAttributeSetting
                }

                if (attr.EntitySetting is IMultipleOperandEntitySetting)
                {
                    output.WriteStartElement("DerivedBooleanAttributeSetting");
                    if (attr.EntitySetting is ConjunctionSetting)
                    {
                        output.WriteAttributeString("type", "Conjunction");
                    }
                    else
                    {
                        output.WriteAttributeString("type", "Disjunction");
                    }
                    output.WriteAttributeString("id", attr.XmlId.ToString());
                    output.WriteAttributeString("name", attr.TextRepresentation);
                    foreach (int ancestor in attr.Ancestors)
                    {
                        output.WriteElementString("ConnectiveOrAtomId", ancestor.ToString());
                    }
                    IMultipleOperandEntitySetting setting =
                        attr.EntitySetting as IMultipleOperandEntitySetting;
                    output.WriteElementString("MinimalLength", setting.minLength.ToString());
                    output.WriteElementString("MaximaLength", setting.maxLength.ToString());
                    output.WriteEndElement(); //DerivedBoolenaAttributeSetting
                }
            }

            return output;
        }

        /// <summary>
        /// Gets XML identity of the antecedent
        /// </summary>
        /// <returns>Unique number representing antecedent BA</returns>
        public int GetAntecedentID()
        {
            return antecedentID;
        }

        /// <summary>
        /// GetsXML identity of the succedent
        /// </summary>
        /// <returns>Unique number representing succedent BA</returns>
        public int GetSuccedentID()
        {
            return succedentID;
        }

        /// <summary>
        /// Gets XML identity of the condition
        /// </summary>
        /// <returns>Unique number representing condition BA</returns>
        public int GetConditionID()
        {
            return conditionID;
        }

        #region Private methods

        /// <summary>
        /// Recursive procedure that creates the <see cref="PMMLBooleanAttribute"/>
        /// structures according to Boolean attributes (proxies).
        /// </summary>
        /// <param name="booleanAttribute">The Boolean attribute setting proxy</param>
        /// <returns>Unique XML identifier of the attribute</returns>
        private int CreatePMMLBA(BoxModulePrx booleanAttribute)
        {
            PMMLBooleanAttribute tmp = new PMMLBooleanAttribute();
            BooleanAttributeSettingFunctionsPrx BAFunctionsPrx =
                BooleanAttributeSettingFunctionsPrxHelper.checkedCast(booleanAttribute.getFunctions());

            IEntitySetting setting = BAFunctionsPrx.GetEntitySetting();
            tmp.EntitySetting = setting;

            if (setting is ILeafEntitySetting)
            {
                tmp.XmlId = functions.UniqueIdentifier;
                tmp.TextRepresentation = booleanAttribute.getDefaultUserLabel()[0];
                tmp.AttributeName =
                    functions.GetAttributeName(BAFunctionsPrx.GetBitStringGenerators()[0]);
                BAs.Add(tmp);
                return tmp.XmlId;
            }

            if (setting is IMultipleOperandEntitySetting)
            {
                IMultipleOperandEntitySetting mutliple = setting as IMultipleOperandEntitySetting;
                tmp.TextRepresentation = booleanAttribute.getDefaultUserLabel()[0];
                tmp.Ancestors = new int[mutliple.operands.Length];
                int i = 0;
                foreach (BoxModulePrx operand in booleanAttribute.getConnections("BooleanAttributeSetting"))
                {
                    tmp.Ancestors[i] = CreatePMMLBA(operand);
                    i++;
                }
                tmp.XmlId = functions.UniqueIdentifier;
                BAs.Add(tmp);
                return tmp.XmlId;
            }

            if (setting is ISingleOperandEntitySetting)
            {
                ISingleOperandEntitySetting single = setting as ISingleOperandEntitySetting;
                tmp.TextRepresentation = booleanAttribute.getDefaultUserLabel()[0];
                tmp.Ancestors = new int[1];
                tmp.Ancestors[0] =
                    CreatePMMLBA(booleanAttribute.getConnections("BooleanAttributeSetting")[0]);
                tmp.XmlId = functions.UniqueIdentifier;
                BAs.Add(tmp);
                return tmp.XmlId;
            }

            return -1;
        }

        /// <summary>
        /// Conversion between Ferda's enum for coefficients and string
        /// representation needed in PMML.
        /// </summary>
        /// <param name="type">Ferda's coefficient type</param>
        /// <returns>String PMML representation</returns>
        private string GetPMMLCoefficientType(CoefficientTypeEnum type)
        {
            switch (type)
            {
                case CoefficientTypeEnum.Cuts:
                    return "Cuts";
                case CoefficientTypeEnum.CyclicIntervals:
                    return "CyclicIntervals";
                case CoefficientTypeEnum.Intervals:
                    return "Intervals";
                case CoefficientTypeEnum.LeftCuts:
                    return "LeftCuts";
                case CoefficientTypeEnum.RightCuts:
                    return "RightCuts";
                case CoefficientTypeEnum.Subsets:
                case CoefficientTypeEnum.SubsetsOneOne:
                    return "Subsets";
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
