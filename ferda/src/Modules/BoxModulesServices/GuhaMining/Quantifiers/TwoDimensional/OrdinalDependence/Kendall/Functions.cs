using System;
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.TwoDimensional.OrdinalDependence.Kendall
{
    /// <summary>
    /// <para>
    /// Kendall quantifier value (from interval [0,1]) is defined as
    /// <c>Abs(TauB)</c> where 
    /// <c>TauB = 2(P-Q) / Sqrt((n^2 * Sum(r)((Freq r,*)^2)) * (n^2 * Sum(c)((Freq *,c)^2)))</c>
    /// where <c>n = Sum(r,c)(Freq r,c)</c>; <c>Freq r,* = Sum(c)(Freq r,c)</c>;
    /// <c>P = Sum(r,c)(Freq r,c * Sum(i&gt;r,j&gt;c)(Freq i,j))</c> and
    /// <c>Q = Sum(r,c)(Freq r,c * Sum(i&gt;r,j&lt;c)(Freq i,j))</c>.
    /// </para>
    /// <para>
    /// In statistics, rank correlation is the study of relationships 
    /// between different rankings on the same set of items. It deals 
    /// with measuring correspondence between two rankings, and assessing 
    /// the significance of this correspondence. The statistic described 
    /// here is also known as Kendall's Tau, which is different from 
    /// Spearman's rank correlation coefficient.
    /// </para>
    /// </summary>
    /// <remarks>
    /// See [053 Definice KL-kvantifikátorù.pdf] chapter 5.4.
    /// See [052 Two-dimensional Contingency Tables.pdf] chapter 4.1.5.
    /// See [Mining for Patterns Based on Contingency Tables by KL–Miner - First Experience]
    /// </remarks>
    /// <seealso href="http://en.wikipedia.org/wiki/Kendall's_tau"/>
    public class Functions : TwoDimensionsValueDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        //protected IBoxInfo _boxInfo;

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
            //_boxInfo = boxInfo;
        }

        #endregion

        public OperationModeEnum OperationMode
        {
            get
            {
                return (OperationModeEnum) Enum.Parse(
                                               typeof (OperationModeEnum),
                                               _boxModule.GetPropertyString(Common.PropOperationMode)
                                               );
            }
        }

        public MissingInformationHandlingEnum MissingInformationHandling
        {
            get { return MissingInformationHandlingEnum.Deleting; }
        }

        public RelationEnum Relation
        {
            get
            {
                return (RelationEnum) Enum.Parse(
                                          typeof (RelationEnum),
                                          _boxModule.GetPropertyString(Common.PropRelation)
                                          );
            }
        }

        private bool _running = false;
        private double _treshold;

        public override void BeginOfUse(Current current__)
        {
            _treshold = this.Treshold;
            _running = true;
        }

        public override void EndOfUse(Current current__)
        {
            _running = false;
        }

        public double Treshold
        {
            get { return (_running) ? _treshold : _boxModule.GetPropertyDouble(Common.PropTreshold); }
        }

        public BoundTypeEnum FromRowBoundary
        {
            get
            {
                return (BoundTypeEnum) Enum.Parse(
                                           typeof (BoundTypeEnum),
                                           _boxModule.GetPropertyString(Common.PropFromRowBoundary)
                                           );
            }
        }

        public int FromRowBoundaryIndex
        {
            get { return _boxModule.GetPropertyInt(Common.PropFromRowBoundaryIndex); }
        }

        public BoundTypeEnum ToRowBoundary
        {
            get
            {
                return (BoundTypeEnum) Enum.Parse(
                                           typeof (BoundTypeEnum),
                                           _boxModule.GetPropertyString(Common.PropToRowBoundary)
                                           );
            }
        }

        public int ToRowBoundaryIndex
        {
            get { return _boxModule.GetPropertyInt(Common.PropToRowBoundaryIndex); }
        }

        public BoundTypeEnum FromColumnBoundary
        {
            get
            {
                return (BoundTypeEnum) Enum.Parse(
                                           typeof (BoundTypeEnum),
                                           _boxModule.GetPropertyString(Common.PropFromColumnBoundary)
                                           );
            }
        }

        public int FromColumnBoundaryIndex
        {
            get { return _boxModule.GetPropertyInt(Common.PropFromColumnBoundaryIndex); }
        }

        public BoundTypeEnum ToColumnBoundary
        {
            get
            {
                return (BoundTypeEnum) Enum.Parse(
                                           typeof (BoundTypeEnum),
                                           _boxModule.GetPropertyString(Common.PropToColumnBoundary)
                                           );
            }
        }

        public int ToColumnBoundaryIndex
        {
            get { return _boxModule.GetPropertyInt(Common.PropToColumnBoundaryIndex); }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] {}; }
        }

        public PerformanceDifficultyEnum PerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.QuiteDifficult; }
        }

        public bool NeedsNumericValues
        {
            get { return false; }
        }

        public CardinalityEnum SupportedData
        {
            get { return CardinalityEnum.Ordinal; }
        }

        public UnitsEnum Units
        {
            get { return UnitsEnum.Irrelevant; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        public override bool Compute(QuantifierEvaluateSetting param, Current current__)
        {
            double value = ComputeValue(param);
            return Guha.Math.Common.Compare(Relation, value, Treshold);
        }

        public override bool ComputeValidValue(QuantifierEvaluateSetting param, out double value, Current current__)
        {
            value = ComputeValue(param);
            return Guha.Math.Common.Compare(Relation, value, Treshold);
        }

        public override double ComputeValue(QuantifierEvaluateSetting param, Current current__)
        {
            ContingencyTable table = new ContingencyTable(param);
            // qTmp[r,c] = Sum(i>=r,j<=c)(Freq i,j)

            #region Initialize qTmp (Q table, P table can be computed from qTmp)

            // qTmp[r,c] = Sum(i>=r,j<=c)(Freq i,j)
            double[,] qTmp = new double[table.NumberOfRows,table.NumberOfColumns];
            qTmp.Initialize();
            for (int r = table.NumberOfRows - 1; r >= 0; r--)
            {
                for (int c = 0; c < table.NumberOfColumns; c++)
                {
                    if (c > 0)
                    {
                        // gets left rectangle
                        double leftRectangle = qTmp[r, c - 1];

                        // gets lower rectangle
                        double lowerRectangle;
                        // gets left lower rectangle
                        double leftLowerRectangle;

                        if (r == table.NumberOfRows - 1)
                        {
                            lowerRectangle = 0;
                            leftLowerRectangle = 0;
                        }
                        else
                        {
                            lowerRectangle = qTmp[r + 1, c];
                            leftLowerRectangle = qTmp[r + 1, c - 1];
                        }

                        qTmp[r, c] =
                            leftRectangle
                            + lowerRectangle
                            - leftLowerRectangle
                            + table[r, c];
                    }
                    else // i.e. (columnIndex == 0)
                    {
                        if (r == table.NumberOfRows - 1)
                            qTmp[r, c] = table[r, c];
                        else
                            qTmp[r, c] = qTmp[r + 1, c] + table[r, c];
                    }
                }
            }

            #endregion

            double p = 0;
            //P = Sum(r,c)(Freq r,c * Sum(i>r,j>c)(Freq i,j))
            double q = 0;
            //Q = Sum(r,c)(Freq r,c * Sum(i>r,j<c)(Freq i,j))

            double sumRowSum_2 = 0;
            //Sum(r)((Freq r,*)^2)
            double sumColSum_2 = 0;
            //Sum(c)((Freq *,c)^2)

            for (int r = 0; r < table.NumberOfRows; r++)
            {
                for (int c = 0; c < table.NumberOfColumns; c++)
                {
                    double item = table[r, c];

                    /* please note that
                     * qTmp[r,c] = Sum(i>=r,j<=c)(Freq i,j)
                     * */

                    // pSum = Sum(i>r,j>c)(Freq i,j)
                    // => pSum[r,c] = qTmp[r+1,*] - qTmp[r+1,c] (! be careful to overflow)
                    if (r < table.NumberOfRows - 1)
                        p += item*qTmp[r + 1, table.NumberOfColumns - 1] - qTmp[r + 1, c];

                    // qSum = Sum(i>r,j<c)(Freq i,j)
                    // => qSum[r,c] = qTmp[r+1,c-1] (! be careful to overflow)
                    if (c > 0 && r < table.NumberOfRows - 1)
                        q += item*qTmp[r + 1, c - 1];
                }
                sumRowSum_2 += Math.Pow(table.RowSums[r], 2);
            }
            for (int c = 0; c < table.NumberOfColumns; c++)
            {
                sumColSum_2 += Math.Pow(table.ColumnSums[c], 2);
            }

            double n_2 = Math.Pow(table.Sum, 2);
            //n^2 where n = Sum(r,c)(Freq r,c)

            return
                2*Math.Abs(p - q)
                /
                Math.Sqrt((n_2 - sumRowSum_2)*(n_2 - sumColSum_2));
        }

        public override string GetLocalizedBoxLabel(string[] localePrefs, Current current__)
        {
            return _boxModule.BoxInfo.GetLabel(localePrefs);
        }

        public override string GetLocalizedUserBoxLabel(string[] localePrefs, Current current__)
        {
            return _boxModule.Manager.getProjectInformation().getUserLabel(_boxModule.StringIceIdentity);
        }

        public override QuantifierSetting GetQuantifierSetting(Current current__)
        {
            return new QuantifierSetting(
                _boxModule.StringIceIdentity,
                _boxModule.BoxInfo.Identifier,
                MissingInformationHandling,
                OperationMode,
                Relation,
                Treshold,
                new Bound(FromRowBoundary, FromRowBoundaryIndex),
                new Bound(ToRowBoundary, ToRowBoundaryIndex),
                new Bound(FromColumnBoundary, FromColumnBoundaryIndex),
                new Bound(ToColumnBoundary, ToColumnBoundaryIndex),
                QuantifierClasses,
                PerformanceDifficulty,
                NeedsNumericValues,
                SupportedData,
                Units,
                SupportsFloatContingencyTable
                );
        }

        public override bool[] ComputeBatch(QuantifierEvaluateSetting[] param, Current current__)
        {
            return Common.ComputeBatch(param, this.Compute);
        }
    }
}