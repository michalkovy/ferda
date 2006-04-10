// NineFoldContingencyTable.cs - 9 fold contingency table and its quantifiers
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Tomáš Kuchař
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


using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Quantifiers
{
    /// <summary>
    /// Represents <see cref="T:Ferda.Modules.Quantifiers.FourFoldContingencyTable"/>
    /// extended by missing information. 9ft-table looks:
    /// <code>
    /// Mx     |   SUCC    SUCCx   notSUCC  
    /// ------------------------------------
    /// ANT    |   f1,1    f1,x    f1,0
    /// ANTx   |   fx,1    fx,x    fx,0
    /// notANT |   f0,1    f0,x    f0,0
    /// </code>
    /// See lecture notes doc. Rauch ("Dobývání znalostí z databází" 2005-2006)
    /// DBI_022_04_4_listopadu/DBI_022_neuplna_inf.ppt
    /// </summary>
    public class NineFoldContingencyTable : ContingencyTable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NineFoldContingencyTable"/> class.
        /// </summary>
        public NineFoldContingencyTable()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NineFoldContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
        public NineFoldContingencyTable(int[][] contingencyTable)
            : base(contingencyTable)
        {
            if (!this.IsBasicNineFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NineFoldContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
        public NineFoldContingencyTable(long[][] contingencyTable)
            : base(contingencyTable)
        {
            if (!this.IsBasicNineFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NineFoldContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
        public NineFoldContingencyTable(long[,] contingencyTable)
            : base(contingencyTable)
        {
            if (!this.IsBasicNineFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NineFoldContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <param name="denominator">The denominator.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
        public NineFoldContingencyTable(long[,] contingencyTable, long denominator)
            : base(contingencyTable, denominator)
        {
            if (!this.IsBasicNineFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NineFoldContingencyTable"/> class.
        /// </summary>
        /// <param name="f11">The F11 frequency.</param>
        /// <param name="f1x">The F1X frequency.</param>
        /// <param name="f10">The F10 frequency.</param>
        /// <param name="fx1">The FX1 frequency.</param>
        /// <param name="fxx">The FXX frequency.</param>
        /// <param name="fx0">The FX0 frequency.</param>
        /// <param name="f01">The F01 frequency.</param>
        /// <param name="f0x">The F0X frequency.</param>
        /// <param name="f00">The F00 frequency.</param>
        public NineFoldContingencyTable(long f11, long f1x, long f10,
                                        long fx1, long fxx, long fx0,
                                        long f01, long f0x, long f00)
            : base(new long[3, 3] { { f11, f1x, f10 }, { fx1, fxx, fx0 }, { f01, f0x, f00 } })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NineFoldContingencyTable"/> class.
        /// </summary>
        /// <param name="f11">The F11 frequency.</param>
        /// <param name="f1x">The F1X frequency.</param>
        /// <param name="f10">The F10 frequency.</param>
        /// <param name="fx1">The FX1 frequency.</param>
        /// <param name="fxx">The FXX frequency.</param>
        /// <param name="fx0">The FX0 frequency.</param>
        /// <param name="f01">The F01 frequency.</param>
        /// <param name="f0x">The F0X frequency.</param>
        /// <param name="f00">The F00 frequency.</param>
        /// <param name="denominator">The denominator.</param>
        public NineFoldContingencyTable(long f11, long f1x, long f10,
                                        long fx1, long fxx, long fx0,
                                        long f01, long f0x, long f00,
                                        long denominator)
            : base(new long[3, 3] { { f11, f1x, f10 }, { fx1, fxx, fx0 }, { f01, f0x, f00 } }, denominator)
        {
        }
        #endregion

        #region Fields (F11, F1x, F10, Fx1, Fxx, Fx0, F01, F0x, F00)

        /// <summary>
        /// Gets the <c>f1,1</c> (Antecedent &amp; Succedent) frequency of the 9f-table
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>f1,1</c> frequency of the 9f-table (nine fold table).</result>
        private long f11
        {
            get { return Table[0, 0]; }
        }

        /// <summary>
        /// Gets the <c>F1,1</c> (Antecedent &amp; Succedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <result>The <c>F1,1</c> frequency of the 9f-table (nine fold table).</result>
        public double F11
        {
            get { return f11 / (double)Denominator; }
        }

        /// <summary>
        /// Gets the <c>f1,x</c> (Antecedent &amp; SuccedentX) frequency of the 
        /// 9f-table (nine fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>f1,x</c> frequency of the 9f-table (nine fold table).</result>
        private long f1x
        {
            get { return Table[0, 1]; }
        }

        /// <summary>
        /// Gets the <c>F1,x</c> (Antecedent &amp; SuccedentX) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <result>The <c>F1,x</c> frequency of the 9f-table (nine fold table).</result>
        public double F1x
        {
            get { return f1x / (double)Denominator; }
        }

        /// <summary>
        /// Gets the <c>f1,0</c> (Antecedent &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>f1,0</c> frequency of the 9f-table (nine fold table).</result>
        private long f10
        {
            get { return Table[0, 2]; }
        }

        /// <summary>
        /// Gets the <c>F1,0</c> (Antecedent &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <result>The <c>F1,0</c> frequency of the 9f-table (nine fold table).</result>
        public double F10
        {
            get { return f10 / (double)Denominator; }
        }

        /// <summary>
        /// Gets the <c>fx,1</c> (AntecedentX &amp; Succedent) frequency of the 9f-table
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>fx,1</c> frequency of the 9f-table (nine fold table).</result>
        private long fx1
        {
            get { return Table[1, 0]; }
        }

        /// <summary>
        /// Gets the <c>Fx,1</c> (AntecedentX &amp; Succedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <result>The <c>Fx,1</c> frequency of the 9f-table (nine fold table).</result>
        public double Fx1
        {
            get { return fx1 / (double)Denominator; }
        }

        /// <summary>
        /// Gets the <c>fx,x</c> (AntecedentX &amp; SuccedentX) frequency of the 
        /// 9f-table (nine fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>fx,x</c> frequency of the 9f-table (nine fold table).</result>
        private long fxx
        {
            get { return Table[1, 1]; }
        }

        /// <summary>
        /// Gets the <c>Fx,x</c> (AntecedentX &amp; SuccedentX) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <result>The <c>Fx,x</c> frequency of the 9f-table (nine fold table).</result>
        public double Fxx
        {
            get { return fxx / (double)Denominator; }
        }

        /// <summary>
        /// Gets the <c>fx,0</c> (AntecedentX &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>fx,0</c> frequency of the 9f-table (nine fold table).</result>
        private long fx0
        {
            get { return Table[1, 2]; }
        }

        /// <summary>
        /// Gets the <c>Fx,0</c> (AntecedentX &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <result>The <c>Fx,0</c> frequency of the 9f-table (nine fold table).</result>
        public double Fx0
        {
            get { return fx0 / (double)Denominator; }
        }

        /// <summary>
        /// Gets the <c>f0,1</c> (notAntecedent &amp; Succedent) frequency of the 9f-table
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>f0,1</c> frequency of the 9f-table (nine fold table).</result>
        private long f01
        {
            get { return Table[1, 0]; }
        }

        /// <summary>
        /// Gets the <c>F0,1</c> (notAntecedent &amp; Succedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <result>The <c>F0,1</c> frequency of the 9f-table (nine fold table).</result>
        public double F01
        {
            get { return f01 / (double)Denominator; }
        }

        /// <summary>
        /// Gets the <c>f0,x</c> (notAntecedent &amp; SuccedentX) frequency of the 
        /// 9f-table (nine fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>f0,x</c> frequency of the 9f-table (nine fold table).</result>
        private long f0x
        {
            get { return Table[1, 1]; }
        }

        /// <summary>
        /// Gets the <c>F0,x</c> (notAntecedent &amp; SuccedentX) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <result>The <c>F0,x</c> frequency of the 9f-table (nine fold table).</result>
        public double F0x
        {
            get { return f0x / (double)Denominator; }
        }

        /// <summary>
        /// Gets the <c>f0,0</c> (notAntecedent &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>f0,0</c> frequency of the 9f-table (nine fold table).</result>
        private long f00
        {
            get { return Table[1, 2]; }
        }

        /// <summary>
        /// Gets the <c>F0,0</c> (notAntecedent &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <result>The <c>F0,0</c> frequency of the 9f-table (nine fold table).</result>
        public double F00
        {
            get { return f00 / (double)Denominator; }
        }

        #endregion

        #region Secured FourFoldTables
        /*
         * Definice: 
         * Zabezpečená čtyřpolní tabulka < as, bs, cs, ds > pro matici MX 
         * s neúplnou informací a pro asociační pravidlo Ant ~ Succ je čtyřpolní 
         * tabulka splňující podmínku : 
         *  Val(Ant ~ Succ, M) = 1 v každém doplnění M matice MX 
         *  právě když ~(as, bs, cs, ds ) = 1.
         * 
         */

        /// <summary>
        /// Definition of classes of associetion rules.
        /// </summary>
        public enum AssocQuantifierClass
        {
            /// <summary>
            /// Secured table will be: 
            /// a = f1,1; 
            /// b = f1,0 + f1,X + fX,X + fX,0; 
            /// c = 0; 
            /// d = 0;
            /// </summary>
            Implicational,

            /// <summary>
            /// Secured table will be the same as for <c>SumDoubleImplicational</c>
            /// </summary>
            DoubleImplicational,

            /// <summary>
            /// Secured table will be: 
            /// a = f1,1;
            /// b = f1,0 + f1,X + fX,X + fX,0;
            /// c = f0,1 + fX,1 + f0,X;
            /// d = 0;
            /// </summary>
            SumDoubleImplicational,

            /// <summary>
            /// Equivalency (sometimes associational)
            /// Secured table will be the same as for <c>SumEquivalency</c>
            /// </summary>
            Equivalency,

            /// <summary>
            /// Secured table will be: 
            /// a = f1,1;
            /// b = f1,0 + f1,X + fX,X + fX,0;
            /// c = f0,1 + fX,1 + f0,X;
            /// d = f0,0;
            /// </summary>
            SumEquivalency
        }

        /// <summary>
        /// Gets the secured four fold contingency table.
        /// </summary>
        /// <param name="quantifierClass">The quantifier class.</param>
        /// <returns>Secured 4ft contingecy table.</returns>
        /// <remarks>
        /// Secured completion depends on 
        /// <see cref="T:Ferda.Modules.Quantifiers.NineFoldContingencyTable.AssocQuantifierClass">
        /// class of associational rule (quantifier)</see>.
        /// </remarks>
        public FourFoldContingencyTable GetSecuredFourFoldContingencyTable(AssocQuantifierClass quantifierClass)
        {
            switch (quantifierClass)
            {
                case AssocQuantifierClass.Implicational:
                    return new FourFoldContingencyTable(f11, f10 + f1x + fxx + fx0, 0, 0, Denominator);
                case AssocQuantifierClass.SumDoubleImplicational:
                case AssocQuantifierClass.DoubleImplicational:
                    return new FourFoldContingencyTable(f11, f10 + f1x + fxx + fx0, f01 + fx1 + f0x, 0, Denominator);
                case AssocQuantifierClass.SumEquivalency:
                case AssocQuantifierClass.Equivalency:
                    return new FourFoldContingencyTable(f11, f10 + f1x + fxx + fx0, f01 + fx1 + f0x, f00, Denominator);
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(quantifierClass);
            }
        }
        #endregion

        #region Optimistical FourFoldTables
        /// <summary>
        /// Gets the optimistical four fold contingency table.
        /// </summary>
        /// <returns>Optimistacal 4ft-table.</returns>
        /// <remarks>
        /// <b>Definition in czech:</b>
        /// Optimistická čtyřpolní tabulka [ ao, bo, co, do ] pro matici MX
        /// s neúplnou informací a pro asociační pravidlo Ant ~ Succ je čtyřpolní 
        /// tabulka splňující podmínku : 
        /// Existuje doplnění M matice MX tak že Val(Ant ~ Succ, M) = 1 
        /// právě když ~(ao, bo, co, do ) = 1.
        /// i.e. [ f1,1 + f1,X + fX,1 + fX,X; f0,1; 0; 0 ]
        /// </remarks>
        public FourFoldContingencyTable GetOptimisticalFourFoldContingencyTable()
        {
            return new FourFoldContingencyTable(f11 + f1x + fx1 + fxx, f01, 0, 0, Denominator);
        }
        #endregion

        #region Deleting missing information -> FourFoldTables
        /// <summary>
        /// Gets the deleting (missing information) four fold contingency table.
        /// </summary>
        /// <returns>Deleting 4ft contingecy table.</returns>
        /// <remarks>
        /// [ f1,1; f1,0; f0,1; f0,0 ]
        /// </remarks>
        public FourFoldContingencyTable GetDeletingFourFoldContingencyTable()
        {
            return new FourFoldContingencyTable(f11, f10, f01, f00, Denominator);
        }
        #endregion
    }
}
