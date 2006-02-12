using System;
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
        public NineFoldContingencyTable()
            : base()
        { }
        public NineFoldContingencyTable(int[][] contingencyTable)
            : base(contingencyTable)
        {
            if (!this.IsBasicNineFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        public NineFoldContingencyTable(long[][] contingencyTable)
            : base(contingencyTable)
        {
            if (!this.IsBasicNineFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        public NineFoldContingencyTable(long[,] contingencyTable)
            : base(contingencyTable)
        {
            if (!this.IsBasicNineFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        public NineFoldContingencyTable(long[,] contingencyTable, long denominator)
            : base(contingencyTable, denominator)
        {
            if (!this.IsBasicNineFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        public NineFoldContingencyTable(long f11, long f1x, long f10,
                                        long fx1, long fxx, long fx0,
                                        long f01, long f0x, long f00)
            : base(new long[3, 3] { { f11, f1x, f10 }, { fx1, fxx, fx0 }, { f01, f0x, f00 } })
        {
        }

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
        /// <value>The <c>f1,1</c> frequency of the 9f-table (nine fold table).</value>
        private long f11
        {
            get { return table[0, 0]; }
        }

        /// <summary>
        /// Gets the <c>F1,1</c> (Antecedent &amp; Succedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>F1,1</c> frequency of the 9f-table (nine fold table).</value>
        public double F11
        {
            get { return f11 / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>f1,x</c> (Antecedent &amp; SuccedentX) frequency of the 
        /// 9f-table (nine fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>f1,x</c> frequency of the 9f-table (nine fold table).</value>
        private long f1x
        {
            get { return table[0, 1]; }
        }

        /// <summary>
        /// Gets the <c>F1,x</c> (Antecedent &amp; SuccedentX) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>F1,x</c> frequency of the 9f-table (nine fold table).</value>
        public double F1x
        {
            get { return f1x / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>f1,0</c> (Antecedent &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>f1,0</c> frequency of the 9f-table (nine fold table).</value>
        private long f10
        {
            get { return table[0, 2]; }
        }

        /// <summary>
        /// Gets the <c>F1,0</c> (Antecedent &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>F1,0</c> frequency of the 9f-table (nine fold table).</value>
        public double F10
        {
            get { return f10 / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>fx,1</c> (AntecedentX &amp; Succedent) frequency of the 9f-table
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>fx,1</c> frequency of the 9f-table (nine fold table).</value>
        private long fx1
        {
            get { return table[1, 0]; }
        }

        /// <summary>
        /// Gets the <c>Fx,1</c> (AntecedentX &amp; Succedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>Fx,1</c> frequency of the 9f-table (nine fold table).</value>
        public double Fx1
        {
            get { return fx1 / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>fx,x</c> (AntecedentX &amp; SuccedentX) frequency of the 
        /// 9f-table (nine fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>fx,x</c> frequency of the 9f-table (nine fold table).</value>
        private long fxx
        {
            get { return table[1, 1]; }
        }

        /// <summary>
        /// Gets the <c>Fx,x</c> (AntecedentX &amp; SuccedentX) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>Fx,x</c> frequency of the 9f-table (nine fold table).</value>
        public double Fxx
        {
            get { return fxx / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>fx,0</c> (AntecedentX &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>fx,0</c> frequency of the 9f-table (nine fold table).</value>
        private long fx0
        {
            get { return table[1, 2]; }
        }

        /// <summary>
        /// Gets the <c>Fx,0</c> (AntecedentX &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>Fx,0</c> frequency of the 9f-table (nine fold table).</value>
        public double Fx0
        {
            get { return fx0 / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>f0,1</c> (notAntecedent &amp; Succedent) frequency of the 9f-table
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>f0,1</c> frequency of the 9f-table (nine fold table).</value>
        private long f01
        {
            get { return table[1, 0]; }
        }

        /// <summary>
        /// Gets the <c>F0,1</c> (notAntecedent &amp; Succedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>F0,1</c> frequency of the 9f-table (nine fold table).</value>
        public double F01
        {
            get { return f01 / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>f0,x</c> (notAntecedent &amp; SuccedentX) frequency of the 
        /// 9f-table (nine fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>f0,x</c> frequency of the 9f-table (nine fold table).</value>
        private long f0x
        {
            get { return table[1, 1]; }
        }

        /// <summary>
        /// Gets the <c>F0,x</c> (notAntecedent &amp; SuccedentX) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>F0,x</c> frequency of the 9f-table (nine fold table).</value>
        public double F0x
        {
            get { return f0x / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>f0,0</c> (notAntecedent &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>f0,0</c> frequency of the 9f-table (nine fold table).</value>
        private long f00
        {
            get { return table[1, 2]; }
        }

        /// <summary>
        /// Gets the <c>F0,0</c> (notAntecedent &amp; notSuccedent) frequency of the 9f-table 
        /// (nine fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>F0,0</c> frequency of the 9f-table (nine fold table).</value>
        public double F00
        {
            get { return f00 / (double)denominator; }
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

        public enum AssocQuantifierClass
        {
            //The class                     //secured 4ft-table
            //-------------------------------------------------------------------------------------------
            Implicational,                  //<f1,1; f1,0 + f1,X + fX,X + fX,0; 0; 0>
            Equivalency, //(asociational)   //TODO
            DoubleImplicational,            //TODO
            SumDoubleImplicational,         //<f1,1; f1,0 + f1,X + fX,X + fX,0; f0,1 + fX,1 + f0,X; 0>
            SumEquivalency                 //<f1,1; f1,0 + f1,X + fX,X + fX,0; f0,1 + fX,1 + f0,X; f0,0>
        }

        public FourFoldContingencyTable GetSecuredFourFoldContingencyTable(AssocQuantifierClass quantifierClass)
        {
            switch (quantifierClass)
            {
                case AssocQuantifierClass.Implicational:
                    return new FourFoldContingencyTable(f11, f10 + f1x + fxx + fx0, 0, 0, denominator);
                case AssocQuantifierClass.SumDoubleImplicational:
                    return new FourFoldContingencyTable(f11, f10 + f1x + fxx + fx0, f01 + fx1 + f0x, 0, denominator);
                case AssocQuantifierClass.SumEquivalency:
                    return new FourFoldContingencyTable(f11, f10 + f1x + fxx + fx0, f01 + fx1 + f0x, f00, denominator);
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(quantifierClass);
            }
        }
        #endregion

        #region Optimistical FourFoldTables
        /*
         * Definice: 
         * Optimistická čtyřpolní tabulka < ao, bo, co, do > pro matici MX
         * s neúplnou informací a pro asociační pravidlo Ant ~ Succ je čtyřpolní 
         * tabulka splňující podmínku : 
         *  Existuje doplnění M matice MX tak že Val(Ant ~ Succ, M) = 1 
         *  právě když    ~(ao, bo, co, do ) = 1.
         * 
         */
        //<f1,1 + f1,X + fX,1 + fX,X; f0,1; 0; 0>

        public FourFoldContingencyTable GetOptimisticalFourFoldContingencyTable()
        {
            return new FourFoldContingencyTable(f11 + f1x + fx1 + fxx, f01, 0, 0, denominator);
        }
        #endregion

        #region Deleting missing information -> FourFoldTables
        //<f1,1; f1,0; f0,1; f0,0>

        public FourFoldContingencyTable GetDeletingFourFoldContingencyTable()
        {
            return new FourFoldContingencyTable(f11, f10, f01, f00, denominator);
        }
        #endregion
    }
}
