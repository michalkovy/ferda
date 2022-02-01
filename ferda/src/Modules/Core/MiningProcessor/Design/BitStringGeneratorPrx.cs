// BitStringGeneratorPrx.cs - Functional output of a bit string generator
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
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

using System;
using Ferda.Modules;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Design
{
    /// <summary>
    /// Commented slice design of the bit string generator proxy. Bit string generator
    /// is service providing information about bit strings of a attribute, therefore
    /// attributes in Ferda provide bit strings. 
    /// </summary>
    /// <remarks>
    /// The original slice design can be found in 
    /// <c>slice/Modules/Guha.MiningProcessor.ice</c>.
    /// The class diagram representing dependencies of the designed entities
    /// can be found in
    /// <c>slice/Modules/GUha.MiningProcessor.png(csd)</c>.
    /// The csd file stands for class diagram, that can be edited with the 
    /// <c>NClass</c> tool, see <see cref="http://nclass.sourceforge.net"/>.
    /// </remarks>
    public class BitStringGeneratorPrx : AttributeNameProvider
    {
        /// <summary>
        /// Gets the name of the column from which the attribute and bit string generator
        /// is created. Added for PMML purposes. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns></returns>
        public override string GetColumnName(Current current__)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets categories and frequencies of the underlying attribute. The function
        /// was taken from AttributeFunctions for purposes of PMML generation. 
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Values and frequencies of the attribute categories</returns>
        public override ValuesAndFrequencies getCategoriesAndFrequencies(Current current__)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns serialized attribute <see cref="Ferda.Guha.Attribute"/>.
        /// This fucntion was added to the Slice desing for
        /// the PMML support - it was removed from 
        /// <see cref="Ferda.Modules.Boxes.DataPreparation.AttributeFunctions"/>
        /// </summary>
        /// <param name="current__">ICE stuff</param>
        /// <returns>Serialized attribute</returns>
        public override string getAttribute(Current current__)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns information from the column about the values and frequencies
        /// of the column. This fucntion was added to the Slice desing for
        /// the PMML support.
        /// </summary>
        /// <returns>ValuesAndFrequencies structure</returns>
        public ValuesAndFrequencies GetColumnValuesAndFrequencies()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the identification of the attribute.
        /// </summary>
        public Guid AttributeGuid
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Returns cardinality of the attribute (nominal/ordinal/cyclic ordinal/cardinal).
        /// </summary>
        public CardinalityEnum AttributeCardinality
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets identificators of categories. Names of the categories are used
        /// as their identificators. The <see cref="Ferda.Guha.Attribute"/> class
        /// ensures, that the names of categories are unique.
        /// </summary>
        /// <returns>Identificators of categories</returns>
        public string[] GetCategoriesIds()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns numerical values of the categories. These numerical
        /// values can be returned only for the <c>ordinal, cyclic ordinal
        /// and cardinal</c> attributes. Otherwise, <c>null</c> or
        /// <c>double[0]</c> is returned. The value is returned, only
        /// if the category contains no intervals and only one enumeration.
        /// Otherwise, null is returned.
        /// </summary>
        /// <returns>
        /// Numerical values of the categories. These numerical
        /// values can be returned only for the <c>ordinal, cyclic ordinal
        /// and cardinal</c> attributes. Otherwise, <c>null</c> or
        /// <c>double[0]</c> is returned
        /// </returns>
        public double[] GetCategoriesNumericValues()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a bit string for category in the 
        /// <paramref name="categoryId"/> parameter.
        /// </summary>
        /// <param name="categoryId">Category identification
        /// (name of the category)</param>
        /// <returns>BitString</returns>
        public BitStringIce GetBitString(string categoryId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns identification (category name) of a
        /// category that contains missing information.
        /// </summary>
        /// <returns>Missing information category name
        /// </returns>
        public string GetMissingInformationCategoryId()
        {
            throw new NotImplementedException();
        }

        #region Multirelational DM

        /// <summary>
        /// Returns a count vector for this attribute, given the master data table name,
        /// master and detial key columns. It is used for virtual hypotheses attributes.
        /// The count vector is an array of integers 
        /// representing for each item in the master data table how many records are
        /// in the detail data table corresponding to the item. 
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.
        /// </summary>
        /// <param name="masterIdColumn">ID of the master data table</param>
        /// <param name="masterDataTableName">Name of the master data table</param>
        /// <param name="detailIdColumn">Detail data table ID column</param>
        /// <returns>a count vector</returns>
        public int[] GetCountVector(string masterIdColumn, string masterDataTableName, string detailIdColumn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets next bit string of the virtual hypotheses attribute. The virtual
        /// hypotheses attribute does not know the exact number of bit strings
        /// to be generated by the miner. Therefore it returns only the next
        /// bit strings.
        /// More information can
        /// be found in <c>svnroot/publications/diplomky/Kuzmos/diplomka.pdf</c> or
        /// in <c>svnroot/publications/Icde08/ICDE.pdf</c>.
        /// </summary>
        /// <param name="skipFirstN"></param>
        /// <param name="bitString">Bit string to be returned</param>
        /// <returns>True iff there is a next bit string in the output
        /// <paramref name="bitString"/></returns>
        public bool GetNextBitString(int skipFirstN, out BitStringWithCategoryId bitString)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns maximal number of bit strings (verfications) that a 
        /// virtual hypotheses attribute can generate. The number is usually
        /// set via a property in the corresponding virtual attribute box.
        /// </summary>
        /// <returns>Maximal number of bit strings</returns>
        public long GetMaxBitStringCount()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}