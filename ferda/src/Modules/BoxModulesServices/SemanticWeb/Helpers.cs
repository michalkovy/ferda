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
using Ferda.Guha.MiningProcessor.Formulas;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;

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
    /// A helping structure for storing information about PMML item
    /// </summary>
    public struct PMMLItem
    {
        /// <summary>
        /// Unique identifier of the item used for XML referencing
        /// </summary>
        public int XmlId;

        /// <summary>
        /// The name of the attribute
        /// </summary>
        public string AttributeName;

        /// <summary>
        /// The name of the category in the attribute
        /// </summary>
        public string CategoryName;

        /// <summary>
        /// The text representation of the PMMLItem
        /// </summary>
        public string Text;
    }

    /// <summary>
    /// A helping structure for storing information about PMML itemset
    /// </summary>
    public struct PMMLItemset
    {
        /// <summary>
        /// Unique identifier of the itemset used for XML referencing
        /// </summary>
        public int XmlId;

        /// <summary>
        /// XML ID's of the ancestors. 
        /// </summary>
        public int[] Ancestors;

        /// <summary>
        /// The text representation of the PMMLItem
        /// </summary>
        public string Text;

        /// <summary>
        /// Conjunction, Disjunction or Negation
        /// </summary>
        public string Connective;
    }

    /// <summary>
    /// A helping structure for storing information about PMML itemset
    /// </summary>
    public struct PMMLAssociationRule
    {
        /// <summary>
        /// The XML ID of the antecedent (item or itemset)
        /// </summary>
        public int antecedent;

        /// <summary>
        /// The XML ID of the consequent(item or itemset)
        /// </summary>
        public int consequent;

        /// <summary>
        /// The XML ID of the condition (item or itemset)
        /// </summary>
        public int condition;

        /// <summary>
        /// Values of the connected quantifiers. Key means identifier
        /// of the quantifier and value means the quantifier value
        /// </summary>
        public Dictionary<string, double> quantifiers;

        /// <summary>
        /// The contingency table of thy hypothesis
        /// </summary>
        public double[][] contingencyTable; 

        /// <summary>
        /// Computes values of quantifiers for given hypothesis
        /// </summary>
        /// <param name="hyp">The hypothesis</param>
        /// <param name="allObjectsCount">count of all objects 
        /// for quantifier counting</param>
        public void ComputeQuantifiers(Hypothesis hyp, Quantifiers quant,
            long allObjectsCount)
        {
            quantifiers = new Dictionary<string, double>();
            foreach (string name in quant.Quantifeirs.Keys)
            {
                quantifiers.Add(quant.Quantifeirs[name].LocalizedLabel.Replace(" ", string.Empty),
                    quant.Quantifeirs[name].Value(hyp, allObjectsCount));
            }
        }
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
                    output.WriteElementString("MaximalLength", setting.maxLength.ToString());
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

    /// <summary>
    /// Class for providing helping information for the PMMLBuilder box
    /// for providing association rules
    /// </summary>
    public class PMMLAssociationRulesHelper
    {
        #region Privage fields

        /// <summary>
        /// The list of all PMML items
        /// </summary>
        private List<PMMLItem> items = new List<PMMLItem>();

        /// <summary>
        /// The list of all PMML itemsets
        /// </summary>
        private List<PMMLItemset> itemsets = new List<PMMLItemset>();

        /// <summary>
        /// The list of all PMML association rules
        /// </summary>
        private List<PMMLAssociationRule> rules = new List<PMMLAssociationRule>();

        /// <summary>
        /// The PMML builder functions object for retrieving unique ID's
        /// </summary>
        private PMMLBuilder.Functions functions;

        private List<string> miningFields = new List<string>();

        /// <summary>
        /// The bit string generator provider needed for PMML Mining Fields
        /// </summary>
        private MiningTaskFunctionsPrx bitStringGeneratorProvider;

        #endregion

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="functions">The PMML builder functions object</param>
        /// <param name="result">The Ferda result</param>
        /// <param name="taskPrx">Bit string generator provider</param>
        public PMMLAssociationRulesHelper(PMMLBuilder.Functions functions,
            Result result, MiningTaskFunctionsPrx taskPrx, 
            BitStringGeneratorProviderPrx bsgp)
        {
            this.functions = functions;
            this.bitStringGeneratorProvider = taskPrx;

            Quantifiers quantifiers = 
                new Quantifiers(taskPrx.GetQuantifiers(), bsgp, new string[1] {"en-US"});

            foreach (Hypothesis hyp in result.Hypotheses)
            {
                PMMLAssociationRule rule = new PMMLAssociationRule();
                rule.antecedent = ConstructPMMLItemset(hyp.GetFormula(MarkEnum.Antecedent));
                rule.consequent = ConstructPMMLItemset(hyp.GetFormula(MarkEnum.Succedent));
                rule.condition = ConstructPMMLItemset(hyp.GetFormula(MarkEnum.Condition));
                rule.contingencyTable = hyp.ContingencyTableA;
                rule.ComputeQuantifiers(hyp, quantifiers, result.AllObjectsCount);
                rules.Add(rule);
            }
        }

        /// <summary>
        /// Writes the PMML association rules to the XML output
        /// </summary>
        /// <param name="output">The XML output</param>
        /// <returns>XML output with PMML mining fields information</returns>
        public XmlTextWriter WriteAssociationRules(XmlTextWriter writer)
        {
            foreach (PMMLAssociationRule ar in rules)
            {
                writer.WriteStartElement("AssociationRule");
                writer.WriteAttributeString("antecedent", ar.antecedent.ToString());
                writer.WriteAttributeString("consequent", ar.consequent.ToString());

                //confidence
                if (ar.quantifiers.ContainsKey("FoundedImplication"))
                {
                    writer.WriteAttributeString("confidence",
                        ar.quantifiers["FoundedImplication"].ToString().Replace(',', '.'));
                }
                else
                {
                    writer.WriteAttributeString("confidence", "-1");
                }
                //support
                if (ar.quantifiers.ContainsKey("Base"))
                {
                    writer.WriteAttributeString("support",
                        ar.quantifiers["Base"].ToString().Replace(',', '.'));
                }
                else
                {
                    writer.WriteAttributeString("support", "-1");
                }

                writer.WriteStartElement("Extension");
                writer.WriteAttributeString("name", "Condition");
                writer.WriteAttributeString("value", ar.condition.ToString());
                writer.WriteEndElement(); //Extension

                //other quantifiers
                foreach (string s in ar.quantifiers.Keys)
                {
                    if (s == "FoundedImplication" || s == "Base")
                    {
                        continue;
                    }

                    writer.WriteStartElement("Extension");
                    writer.WriteAttributeString("name", "Quantifier");
                    writer.WriteAttributeString("value", ar.quantifiers[s].ToString().Replace(',', '.'));
                    writer.WriteAttributeString("extender", s);
                    writer.WriteEndElement(); //Extension
                }

                //Writing the contingency table
                writer.WriteStartElement("Extension");
                writer.WriteAttributeString("name", "4ftFrequency");
                writer.WriteAttributeString("value", ar.contingencyTable[0][0].ToString());
                writer.WriteAttributeString("extender", "a");
                writer.WriteEndElement(); //Extension
                writer.WriteStartElement("Extension");
                writer.WriteAttributeString("name", "4ftFrequency");
                writer.WriteAttributeString("value", ar.contingencyTable[0][2].ToString());
                writer.WriteAttributeString("extender", "b");
                writer.WriteEndElement(); //Extension
                writer.WriteStartElement("Extension");
                writer.WriteAttributeString("name", "4ftFrequency");
                writer.WriteAttributeString("value", ar.contingencyTable[2][0].ToString());
                writer.WriteAttributeString("extender", "c");
                writer.WriteEndElement(); //Extension
                writer.WriteStartElement("Extension");
                writer.WriteAttributeString("name", "4ftFrequency");
                writer.WriteAttributeString("value", ar.contingencyTable[2][2].ToString());
                writer.WriteAttributeString("extender", "d");
                writer.WriteEndElement(); //Extension

                writer.WriteEndElement(); //AssociationRule
            }
            return writer;
        }

        /// <summary>
        /// Writes the PMML mining fields to the XML output
        /// </summary>
        /// <param name="output">The XML output</param>
        /// <returns>XML output with PMML mining fields information</returns>
        public XmlTextWriter WriteMiningFields(XmlTextWriter writer)
        {
            writer.WriteStartElement("MiningSchema");
            foreach (string s in miningFields)
            {
                writer.WriteStartElement("MiningField");
                writer.WriteAttributeString("name", s);
                writer.WriteEndElement();//MiningField
            }
            writer.WriteEndElement(); //MiningSchema
            return writer;
        }

        /// <summary>
        /// Writes items and itemsets to the XML output
        /// </summary>
        /// <param name="output">The XML output</param>
        /// <returns>XML output with items and itemsets information</returns>
        public XmlTextWriter WriteItemsItemsets(XmlTextWriter writer)
        {
            foreach (PMMLItem item in items)
            {
                writer.WriteStartElement("Item");
                writer.WriteAttributeString("id", item.XmlId.ToString());
                writer.WriteAttributeString("value", item.Text);
                writer.WriteStartElement("Extension");
                writer.WriteAttributeString("name", "attribute");
                writer.WriteAttributeString("value", item.AttributeName);
                writer.WriteEndElement(); //Extension
                writer.WriteStartElement("Extension");
                writer.WriteAttributeString("name", "value");
                writer.WriteAttributeString("value", item.CategoryName);
                writer.WriteEndElement(); //Extension
                writer.WriteEndElement(); //Item
            }
            foreach (PMMLItemset itemset in itemsets)
            {
                writer.WriteStartElement("Itemset");
                writer.WriteAttributeString("id", itemset.XmlId.ToString());
                writer.WriteStartElement("Extension");
                writer.WriteAttributeString("name", "Connective");
                writer.WriteAttributeString("value", itemset.Connective);
                writer.WriteEndElement(); //Extenstion
                foreach (int i in itemset.Ancestors)
                {
                    writer.WriteStartElement("ItemRef");
                    writer.WriteAttributeString("itemRef", i.ToString());
                    writer.WriteEndElement(); //ItemRef
                }
                writer.WriteEndElement(); //Itemset
            }
            return writer;
        }

        /// <summary>
        /// Recursive function constructing the PMML items and PMML itemsets
        /// </summary>
        /// <param name="formula">Formula, accoridng to which the item or 
        /// itemset is beeing constructed</param>
        /// <returns>XML identifier of the formula</returns>
        private int ConstructPMMLItemset(Formula formula)
        {
            if (formula is AtomFormula)
            {
                PMMLItem item = new PMMLItem();
                AtomFormula atom = formula as AtomFormula;
                
                //case that i.e. condition is not connected
                if (atom.BitStringIdentifier.AttributeGuid == null)
                {
                    return -1;
                }
                //filling the mining fields
                else
                {
                    GuidStruct guid = new GuidStruct(atom.BitStringIdentifier.AttributeGuid);
                    BitStringGeneratorPrx[] generators =
                        bitStringGeneratorProvider.GetBitStringGenerators();
                    foreach (BitStringGeneratorPrx generator in generators)
                    {
                        if (generator.GetAttributeId() == guid)
                        {
                            string columnName = generator.GetColumnName();
                            if (!miningFields.Contains(columnName))
                            {
                                miningFields.Add(columnName);
                            }
                        }
                    }
                }
                
                item.AttributeName =
                    AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(atom.BitStringIdentifier.AttributeGuid);
                item.CategoryName = atom.BitStringIdentifier.CategoryId;
                item.Text = atom.ToString();
                
                //checks if the PMMLItem was already created
                int result = ContainsPMMLItem(item);
                if (result == -1)
                {
                    item.XmlId = functions.UniqueIdentifier;
                    items.Add(item);
                    return item.XmlId;
                }
                else
                {
                    return result;
                }
            }

            PMMLItemset itemset = new PMMLItemset();
            if (formula is NegationFormula)
            {
                NegationFormula negation = formula as NegationFormula;
                int operand = ConstructPMMLItemset(negation.Operand);
                itemset.Ancestors = new int[1];
                itemset.Ancestors[0] = operand;
                itemset.Text = negation.ToString();
                itemset.Connective = "Negation";

                int result = ContainsPMMLItemSet(itemset);
                if (result == -1)
                {
                    itemset.XmlId = functions.UniqueIdentifier;
                    itemsets.Add(itemset);
                    return itemset.XmlId;
                }
                else
                {
                    return result;
                }
            }

            if (formula is ConjunctionFormula)
            {
                ConjunctionFormula conjunction = formula as ConjunctionFormula;
                itemset.Ancestors = new int[conjunction.Operands.Count];
                itemset.Text = conjunction.ToString();
                itemset.Connective = "Conjunction";

                //checking if there is other same conjunction (texts should be unique)
                int result = ContainsPMMLItemSet(itemset);
                if (result == -1)
                {
                    for (int i = 0; i < conjunction.Operands.Count; i++)
                    {
                        itemset.Ancestors[i] =
                            ConstructPMMLItemset(conjunction.Operands[i]);
                    }
                    itemset.XmlId = functions.UniqueIdentifier;
                    itemsets.Add(itemset);
                    return itemset.XmlId;
                }
                else
                {
                    return result;
                }
            }

            if (formula is DisjunctionFormula)
            {
                DisjunctionFormula disjunction = formula as DisjunctionFormula;
                itemset.Ancestors = new int[disjunction.Operands.Count];
                itemset.Text = disjunction.ToString();
                itemset.Connective = "Disjunction";

                //checking if there is other same conjunction (texts should be unique)
                int result = ContainsPMMLItemSet(itemset);
                if (result == -1)
                {
                    for (int i = 0; i < disjunction.Operands.Count; i++)
                    {
                        itemset.Ancestors[i] =
                            ConstructPMMLItemset(disjunction.Operands[i]);
                    }
                    itemset.XmlId = functions.UniqueIdentifier;
                    itemsets.Add(itemset);
                    return itemset.XmlId;
                }
                else
                {
                    return result;
                }
            }
            return -1;
        }

        /// <summary>
        /// The method checks, if the PMMLItemset in the parameter was already
        /// created previously. The method checks the equality of 
        /// </summary>
        /// <param name="itemset"></param>
        /// <returns></returns>
        private int ContainsPMMLItemSet(PMMLItemset itemset)
        {
            foreach (PMMLItemset itSet in itemsets)
            {
                //text should be unique
                if (itemset.Text == itSet.Text)
                {
                    return itSet.XmlId;
                }
            }
            return -1;
        }

        /// <summary>
        /// The method checks, if the PMMLItem in parameter was already created 
        /// previously. The method checks the equality of the attribute name and
        /// category name. 
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>XML identifier of the equal item iff exists, otherwise -1
        /// </returns>
        private int ContainsPMMLItem(PMMLItem item)
        {
            foreach (PMMLItem i in items)
            {
                //text should be unique
                if (i.Text == item.Text)
                {
                    return i.XmlId;
                }
            }
            return -1;
        }
    }
}
