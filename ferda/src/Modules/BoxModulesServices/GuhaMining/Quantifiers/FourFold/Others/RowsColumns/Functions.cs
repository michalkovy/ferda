﻿// Functions.cs - Function objects for the RowsColumns quantifier box module
//
// Authora: Martin Ralbovský <martin.ralbovsky@gmail.com>
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
using Ferda.Guha.Data;
using Ferda.Guha.Math;
using Ferda.Guha.Math.Quantifiers;
using System.Collections.Generic;
using System.Text;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.Quantifiers.FourFold.Others.RowsColumns
{
    /// <summary>
    /// Computes the RowsAndColumns quantifier
    /// </summary>
    /// <remarks>
    /// The quantifier is valid, when all the rows and columns of the four-fold
    /// contingency table (a+b,a+c,b+d,c+d) are above given threshold.
    /// </remarks>
    public class Functions : FourFoldValidDisp_, IFunctions
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

        #region Properties
        /*
        THE DOCUMENTATION FOR MAJORITY OF STRUCTURES USED IN THIS REGION CAN BE 
        FOUND IN src\Modules\Core\Math\Quantifiers\IceDesignDocumentation        
        */

        public OperationModeEnum OperationMode
        {
            get
            {
                return (OperationModeEnum)Enum.Parse(
                                               typeof(OperationModeEnum),
                                               _boxModule.GetPropertyString(Common.PropOperationMode)
                                               );
            }
        }

        public MissingInformationHandlingEnum MissingInformationHandling
        {
            get
            {
                return (MissingInformationHandlingEnum)Enum.Parse(
                                                            typeof(MissingInformationHandlingEnum),
                                                            _boxModule.GetPropertyString(
                                                                Common.PropMissingInformationHandling)
                                                            );
            }
        }

        public RelationEnum Relation
        {
            get
            {
                return (RelationEnum)Enum.Parse(
                                          typeof(RelationEnum),
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
            get { return BoundTypeEnum.All; }
        }

        public int FromRowBoundaryIndex
        {
            get { return 0; }
        }

        public BoundTypeEnum ToRowBoundary
        {
            get { return BoundTypeEnum.All; }
        }

        public int ToRowBoundaryIndex
        {
            get { return 1; }
        }

        public BoundTypeEnum FromColumnBoundary
        {
            get { return BoundTypeEnum.All; }
        }

        public int FromColumnBoundaryIndex
        {
            get { return 0; }
        }

        public BoundTypeEnum ToColumnBoundary
        {
            get { return BoundTypeEnum.All; }
        }

        public int ToColumnBoundaryIndex
        {
            get { return 1; }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public PerformanceDifficultyEnum PerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.Easy; }
        }

        public bool NeedsNumericValues
        {
            get { return false; }
        }

        public CardinalityEnum SupportedData
        {
            get { return CardinalityEnum.Nominal; }
        }

        public UnitsEnum Units
        {
            get { return UnitsEnum.Irrelevant; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion

        #region QuantifierBaseFunctions implementation

        /// <summary>
        /// Returns the localized label of the box (for showing in result browser...)
        /// </summary>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Localized label of the box</returns>
        public override string GetLocalizedBoxLabel(string[] localePrefs, Current current__)
        {
            return _boxModule.BoxInfo.GetLabel(localePrefs);
        }

        /// <summary>
        /// Returns the user label of the box
        /// </summary>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns></returns>
        public override string GetLocalizedUserBoxLabel(string[] localePrefs, Current current__)
        {
            return _boxModule.Manager.getProjectInformation().getUserLabel(_boxModule.StringIceIdentity);
        }

        /// <summary>
        /// Determines if the contingency table determined by
        /// <paramref name="param"/> satisfies the quantifier
        /// </summary>
        /// <param name="param">
        /// The quantifier setting includes the
        /// contingency table and other information
        /// </param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>If the settings satisfies the quantifier</returns>
        public override bool Compute(QuantifierEvaluateSetting param, Current current__)
        {
            return ExceptionsHandler.TryCatchMethodThrow<bool>(
                delegate
                {
                    FourFoldContingencyTable table = new FourFoldContingencyTable(param);
                    if (!Guha.Math.Common.Compare(Relation, table.A + table.B, Treshold))
                    {
                        return false;
                    }
                    if (!Guha.Math.Common.Compare(Relation, table.A + table.C, Treshold))
                    {
                        return false;
                    }
                    if (!Guha.Math.Common.Compare(Relation, table.C + table.D, Treshold))
                    {
                        return false;
                    }
                    if (!Guha.Math.Common.Compare(Relation, table.B + table.D, Treshold))
                    {
                        return false;
                    }

                    return true;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Returns a structure giving information about the quantifier
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Quantifier setting</returns>
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

        /// <summary>
        /// Method to compute more quantifier testing
        /// </summary>
        /// <param name="param">More quantifier settings</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Result of more quantifiers testing</returns>
        public override bool[] ComputeBatch(QuantifierEvaluateSetting[] param, Current current__)
        {
            return Common.ComputeBatch(param, this.Compute);
        }

        #endregion
    }
}