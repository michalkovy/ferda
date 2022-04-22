// IBitString.cs - Basic interface for the bit strings in Ferda
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
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

using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// Defines an empty bit string
    /// </summary>
    public interface IEmptyBitString : IBitString
    {
    }

    /// <summary>
    /// Basic interface for bit strings. Defines operations that can 
    /// be done with the bit strings.
    /// </summary>
    public interface IBitString : IBitStringBase
    {
        /// <summary>
        /// Identifier of the bit string (each bit string should be
        /// identified by a boolean attribute formula representing the
        /// bit string. 
        /// </summary>
        BooleanAttributeFormula Identifier { get; }

        /// <summary>
        /// Performs the bitwise AND operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <returns>Result of the AND operation</returns>
        IBitString And(IBitString source);

        /// <summary>
        /// Performs the bitwise AND operation on current BitString against the specified BitString. It can change current instance.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <returns>Result of the AND operation</returns>
        IBitString AndInPlace(IBitString source);

        /// <summary>
        /// Performs the bitwise OR operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <returns>Result of the OR operation</returns>
        IBitString Or(IBitString source);

        /// <summary>
        /// Performs the bitwise OR operation on current BitString against the specified BitString. It can change current instance.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <returns>Result of the OR operation</returns>
        IBitString OrInPlace(IBitString source);

        /// <summary>
        /// Performs the bitwise NOT on current BitString.
        /// </summary>
        IBitString Not();

        /// <summary>
        /// Number of bits in the current bit string, that are not equal to zero.
        /// This property came with introduction of fuzzy bit strings. In boolean
        /// bit strings, the Sum operation determines both the number of non-zero
        /// bits and the sum of the bit string. In the fuzzy case these two numbers
        /// are different. The function is needed for determining frequencies in ETrees
        /// and number of all items belonging to a condition in a 4FT.
        /// </summary>
        long NonZeroBitsCount { get; }

        /// <summary>
        /// Performs the bitwise SUM operation on current BitString.
        /// </summary>
        float Sum { get; set; }
    }

    /// <summary>
    /// Defines a bit string capable of some advanced functionality
    /// </summary>
    public interface IBitStringCreate : IBitStringBase
    {
        /// <summary>
        /// Fills the whole BitString with the specified value. In case of 
        /// crisp bit strings, the value is 1 or 0, in case of fuzzy bit strings,
        /// the value is a float [0,1].
        /// </summary>
        /// <param name="value">Value to be filled into every "bit" of the BitString.</param>
        /// <remarks>
        /// <para>BitString are filled with zeroes when created, so there is no need to call Fill(false) after create() method.</para>
        /// </remarks>
        void Fill(float value);

        /// <summary>
        /// Gets a value of the specified bit from the BitString.
        /// </summary>
        /// <param name="index">Index of the bit to be retrieved.</param>
        /// <returns>Value of the specified bit from the BitString.</returns>
        float GetBit(int index);

        /// <summary>
        /// Sets a specified bit in the BitString.
        /// </summary>
        /// <param name="index">Index of the bit to be set.</param>
        /// <param name="value">New value of the bit.</param>
        void SetBit(int index, float value);
    }

    /// <summary>
    /// Defines a bit string base.
    /// </summary>
    public interface IBitStringBase
    {
        /// <summary>
        /// Length of a bit string
        /// </summary>
        int Length { get; }
        /// <summary>
        /// String (human readable) form of the bit string
        /// </summary>
        /// <returns>String representation of the bit string</returns>
        string ToString();
    }
}

/*
IBitString.Sum vs. IBitString.NonZeroBitsCount

Node.Frequency - u uzlu rozhodovacino stromu se urcuje, kolik prikladu se bude 
 * pomoci tohoto uzlu klasifikovat. Float hodnota by nedavala smysl. 

Node.CategoryFrequency(..) - pouziva se NonZeroBitsCount, protoze frekvence u 
 * kategorie udava pocet prikladu, ktere se klasifikuji podle teto kategorie - 
 * cele cislo

Node.InitNodeClassification(..) - tady zustava pouziti Sum. Zde je potreba 
 * priradit kategoriim uzlu kategorie klasifikacniho atributu (pomoci AND 
 * operatoru). Ve fuzzy pripade je dulezite, jestli stupen prislusnosti je 0.1 
 * nebo 0.9.

NodeClassification.noItemsInCategory, NodeClassification.noErrors - zde je 
 * zrejme, ze by to mel byt long (pouziti NonZeroBitsCount), protoze opet pocet 
 * prikladu, ktere se klasifikovali podle jiste tridy musi byt cele cislo. 
 * Nejsem si vsak jisty, zda bitove operace v Node.InitNodeClassification(..), 
 * kde se to nastavuje budou pocitat spravne.   

Tree.ConfusionMatrix(...) - jednotlive policka matice zamen odpovidaji poctum 
 * objektu, ktere se nejak klasifikovali, proto je pouzity NonZeroBitsCount. 
 * Opet nevim, jestli pro fuzzy pripad to bude fungovat uplne spravne. 

ETreeMiningProcessor.DeleteOneClassificationCategoryOnly(...) - tady se pocita, 
 * jestli kategorie klasifikuje pouze do jedne tridy, tedy vsechny prvky, ktere 
 * do teto kategorie patri jsou klasifikovany pouze do jedne tridy. Pro crisp 
 * pripad je to jedno, pro fuzzy pripad tato procedura zrejme vubec nema smysl, 
 * musi se to poradne promyslet.

ETreeMiningProcessor.SelectAttributesForBranching(...) - vybira se atribut, 
 * podle ktereho se bude vetvit. Kriteriem je chi-kvadrat. Zde myslim, ze bude 
 * pouziti fuzzy bitovych retizku povede k vetsi presnosti pocitani. 

ETreeMiningProcessor.FillChiSqData, (Guha.Math.)DecisionTrees.ChiSquared - zde 
 * opet kvuli vetsi presnosti se pocita se sumama. Zbyva overit, jestli se to 
 * takto pocitat (teoreticky) muze, tedy jestli muzeme mit racionalni 
 * kontingencni tabulku pro vypocet chi-kvadrat. 

HOTOVE ZMENY DO ETREE

MiningProcessor.Masks(...) - metoda pocita masky virtualniho atributu. Je to 
 * zmena pouze pro funkci setBit, kde se da 1f misto true

MiningProcessor.ActConditionCountOfObjects, 
 * FourFoldMiningProcessor.SetActConditionCountOfObjects - zde se pocita pocet 
 * radku databaze, ke kterym se vztahuje dana podminka - melo byt to opet byt cele cislo.  

*/