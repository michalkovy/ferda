// EquidistantAlgorithm.cs - equidistant algorithm
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


using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using System.Data;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.EquidistantIntervalsAttribute
{
    /// <summary>
    /// Algorithm for equidistant attribute.
    /// </summary>
    public static class EquidistantAlgorithm
    {
        private static GeneratedAttribute generateFloating(float from, float to, SidesEnum closedFrom, float length, ColumnInfo column)
        {
            FloatIntervalStruct intervalTemplate = new FloatIntervalStruct();
            char leftBound, rightBound;
            switch (closedFrom)
            {
                case SidesEnum.Left:
                    intervalTemplate.leftBoundType = BoundaryEnum.Sharp;
                    intervalTemplate.rightBoundType = BoundaryEnum.Round;
                    leftBound = Constants.LeftClosedInterval;
                    rightBound = Constants.RightOpenedInterval;
                    break;
                case SidesEnum.Right:
                    intervalTemplate.leftBoundType = BoundaryEnum.Round;
                    intervalTemplate.rightBoundType = BoundaryEnum.Sharp;
                    leftBound = Constants.LeftOpenedInterval;
                    rightBound = Constants.RightClosedInterval;
                    break;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(closedFrom);
            }
            CategoriesStruct result = new CategoriesStruct();
            result.floatIntervals = new FloatIntervalCategorySeq();
            float fromTmp = from;
            float toTmp = ((fromTmp + length) > to) ? to : fromTmp + length;
            for (; ; )
            {
                FloatIntervalStruct interval = intervalTemplate;
                interval.leftBound = fromTmp;
                interval.rightBound = toTmp;
                result.floatIntervals.Add(
                    leftBound + fromTmp.ToString() + Constants.SeparatorInterval + toTmp.ToString() + rightBound,
                    new FloatIntervalStruct[] { interval });
                if (toTmp == to)
                    break;
                fromTmp = toTmp;
                toTmp = ((toTmp + length) > to) ? to : toTmp + length;
            }
            return new GeneratedAttribute(
                result,
                null,
                result.floatIntervals.Count);
        }

        private static GeneratedAttribute generateIntegral(long from, long to, SidesEnum closedFrom, long length, ColumnInfo column)
        {
            LongIntervalStruct intervalTemplate = new LongIntervalStruct();
            char leftBound, rightBound;
            switch (closedFrom)
            {
                case SidesEnum.Left:
                    intervalTemplate.leftBoundType = BoundaryEnum.Sharp;
                    intervalTemplate.rightBoundType = BoundaryEnum.Round;
                    leftBound = Constants.LeftClosedInterval;
                    rightBound = Constants.RightOpenedInterval;
                    break;
                case SidesEnum.Right:
                    intervalTemplate.leftBoundType = BoundaryEnum.Round;
                    intervalTemplate.rightBoundType = BoundaryEnum.Sharp;
                    leftBound = Constants.LeftOpenedInterval;
                    rightBound = Constants.RightClosedInterval;
                    break;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(closedFrom);
            }
            CategoriesStruct result = new CategoriesStruct();
            result.longIntervals = new LongIntervalCategorySeq();
            long fromTmp = from;
            long toTmp = ((fromTmp + length) > to) ? to : fromTmp + length;
            for (; ; )
            {
                LongIntervalStruct interval = intervalTemplate;
                interval.leftBound = fromTmp;
                interval.rightBound = toTmp;
                result.longIntervals.Add(
                    leftBound + fromTmp.ToString() + Constants.SeparatorInterval + toTmp.ToString() + rightBound,
                    new LongIntervalStruct[] { interval });
                if (toTmp == to)
                    break;
                fromTmp = toTmp;
                toTmp = ((toTmp + length) > to) ? to : toTmp + length;
            }
            return new GeneratedAttribute(
                result,
                null,
                result.longIntervals.Count);
        }
        /*
                private static Output generateDateTime(DateTime from, DateTime to, SidesEnum closedFrom, long length, ColumnInfo column)
                {
                    //TODO
                    Output output = new Output();
                    return output;
                }
        */
        private static GeneratedAttribute generateString(string from, string to, long length, DataTable distincts)
        {
            CategoriesStruct result = new CategoriesStruct();
            result.enums = new EnumCategorySeq();
            bool finish = false;
            string value;
            long currentLength = 0;
            string categoryName = null;
            string includeNullCategoryName = null;
            bool includeNullCategory = false;
            String valueName;
            List<string> currentEnum = new List<string>();
            foreach (DataRow row in distincts.Rows)
            {
                value = row.ItemArray[0].ToString();
                if (String.Compare(from, value) > 0)
                    continue;
                if (String.Compare(value, to) >= 0)
                    finish = true;
                currentEnum.Add(value);
                if (String.IsNullOrEmpty(value))
                {
                    valueName = Constants.DbNullCategoryName;
                    includeNullCategory = true;
                }
                else
                {
                    valueName = value;
                }
                categoryName = (String.IsNullOrEmpty(categoryName)) ? valueName : categoryName + Constants.SeparatorEnum + valueName;
                currentLength++;
                if (finish || currentLength == length)
                {
                    categoryName = Constants.LeftEnum + categoryName + Constants.RightEnum;
                    result.enums.Add(
                        categoryName,
                        currentEnum.ToArray());
                    if (includeNullCategory)
                    {
                        includeNullCategory = false;
                        includeNullCategoryName = categoryName;
                    }
                    currentEnum.Clear();
                    currentLength = 0;
                    categoryName = null;
                }
                if (finish)
                    break;
            }
            return new GeneratedAttribute(
                result,
                includeNullCategoryName,
                result.enums.Count);
        }
        /*
                private static Output generateBoolean(bool from, bool to, SidesEnum closedFrom, long length, ColumnInfo column)
                {
                    //TODO
                    Output output = new Output();
                    return output;
                }
        */
        private static Ferda.Modules.BadParamsError badParamsError(string boxIdentity, string[] socketNames, Exception ex)
        {
            if (socketNames == null)
                return Ferda.Modules.Exceptions.BadParamsError(ex, boxIdentity, null, restrictionTypeEnum.BadFormat);
            else
                return Ferda.Modules.Exceptions.BadValueError(ex, boxIdentity, null, socketNames, restrictionTypeEnum.BadFormat);
        }

        /// <summary>
        /// Generates the attribute.
        /// </summary>
        /// <param name="domainType">Type of the domain.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="closedFrom">The closed from.</param>
        /// <param name="length">The length.</param>
        /// <param name="column">The column.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>Generated attribute.</returns>
        public static GeneratedAttribute Generate(
            AttributeDomainEnum domainType,
            string from,
			string to,
			SidesEnum closedFrom,
			double length,
			ColumnInfo column,
			string boxIdentity)
        {
            if (length <= 0)
                throw Ferda.Modules.Exceptions.BadParamsError(null, boxIdentity, "Length have to be greater than 0!", restrictionTypeEnum.Minimum);

            DataTable distincts = null;

            //get from, to values
            switch (domainType)
            {
                case AttributeDomainEnum.WholeDomain:
                    from = column.statistics.ValueMin;
                    to = column.statistics.ValueMax;
                    break;
                case AttributeDomainEnum.SubDomainValueBounds:
                    break;
                case AttributeDomainEnum.SubDomainNumberOfValuesBounds:
                    int fromTmp, toTmp;
                    try
                    {
                        fromTmp = (String.IsNullOrEmpty(from)) ? 0 : Convert.ToInt32(from);
                    }
                    catch (System.ArgumentException ex) { throw badParamsError(boxIdentity, new string[] { "From" }, ex); }
                    catch (System.OverflowException ex) { throw badParamsError(boxIdentity, new string[] { "From" }, ex); }
                    catch (System.FormatException ex) { throw badParamsError(boxIdentity, new string[] { "From" }, ex); }
                    try
                    {
                        toTmp = (String.IsNullOrEmpty(to)) ? 0 : Convert.ToInt32(to);
                    }
                    catch (System.ArgumentException ex) { throw badParamsError(boxIdentity, new string[] { "To" }, ex); }
                    catch (System.OverflowException ex) { throw badParamsError(boxIdentity, new string[] { "To" }, ex); }
                    catch (System.FormatException ex) { throw badParamsError(boxIdentity, new string[] { "To" }, ex); }
                    distincts = Ferda.Modules.Helpers.Data.Column.GetDistincts(column.dataMatrix.database.odbcConnectionString, column.dataMatrix.dataMatrixName, column.columnSelectExpression, boxIdentity);
                    if (distincts.Rows.Count > fromTmp + toTmp)
                    {
                        from = distincts.Rows[fromTmp].ItemArray[0].ToString();
                        to = distincts.Rows[distincts.Rows.Count - toTmp - 1].ItemArray[0].ToString();
                    }
                    else
                    {
                        return new GeneratedAttribute();
                        //TODO ???
                    }
                    break;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(domainType);
            }
            if (String.IsNullOrEmpty(from))
                from = column.statistics.ValueMin;
            if (String.IsNullOrEmpty(to))
                to = column.statistics.ValueMax;

            switch (Ferda.Modules.Helpers.Data.Column.GetColumnValueTypeByValueSubType(column.columnSubType))
            {
                case Ferda.Modules.Helpers.Data.Column.ValueType.Floating:
                    float fromTmpFl, toTmpFl;
                    try
                    {
                        fromTmpFl = (String.IsNullOrEmpty(from)) ? 0 : Convert.ToSingle(from);
                    }
                    catch (System.InvalidCastException ex) { throw badParamsError(boxIdentity, new string[] { "From" }, ex); }
                    try
                    {
                        toTmpFl = (String.IsNullOrEmpty(to)) ? 0 : Convert.ToSingle(to);
                    }
                    catch (System.InvalidCastException ex) { throw badParamsError(boxIdentity, new string[] { "To" }, ex); }
                    return generateFloating(fromTmpFl, toTmpFl, closedFrom, (float)length, column);
                case Ferda.Modules.Helpers.Data.Column.ValueType.Integral:
                    long fromTmpLn, toTmpLn;
                    try
                    {
                        fromTmpLn = (String.IsNullOrEmpty(from)) ? 0 : Convert.ToInt64(from);
                    }
                    catch (System.ArgumentException ex) { throw badParamsError(boxIdentity, new string[] { "From" }, ex); }
                    catch (System.OverflowException ex) { throw badParamsError(boxIdentity, new string[] { "From" }, ex); }
                    catch (System.FormatException ex) { throw badParamsError(boxIdentity, new string[] { "From" }, ex); }
                    try
                    {
                        toTmpLn = (String.IsNullOrEmpty(to)) ? 0 : Convert.ToInt64(to);
                    }
                    catch (System.ArgumentException ex) { throw badParamsError(boxIdentity, new string[] { "To" }, ex); }
                    catch (System.OverflowException ex) { throw badParamsError(boxIdentity, new string[] { "To" }, ex); }
                    catch (System.FormatException ex) { throw badParamsError(boxIdentity, new string[] { "To" }, ex); }
                    return generateIntegral(fromTmpLn, toTmpLn, closedFrom, (long)length, column);
                case Ferda.Modules.Helpers.Data.Column.ValueType.DateTime:
                    throw Ferda.Modules.Exceptions.BadParamsError(null, boxIdentity, null, restrictionTypeEnum.DbColumnDataType);
                //TODO
                /*
                DateTime fromTmpDt, toTmpDt;
                try
                {
                    fromTmpDt = Convert.ToDateTime(from);
                }
                catch (System.InvalidCastException ex) { throw badParamsError(boxIdentity, new string[] { "From" }, ex); }
                try
                {
                    toTmpDt = Convert.ToDateTime(to);
                }
                catch (System.InvalidCastException ex) { throw badParamsError(boxIdentity, new string[] { "To" }, ex); }
                return generateDateTime(fromTmpDt, toTmpDt, closedFrom, (long)length, column);
                 */
                case Ferda.Modules.Helpers.Data.Column.ValueType.String:
                    if (distincts == null)
                        distincts = Ferda.Modules.Helpers.Data.Column.GetDistincts(column.dataMatrix.database.odbcConnectionString, column.dataMatrix.dataMatrixName, column.columnSelectExpression, boxIdentity);
                    //TODO lepe distincts (dle from a to)
                    return generateString(from, to, (long)length, distincts);
                case Ferda.Modules.Helpers.Data.Column.ValueType.Boolean:
                    throw Ferda.Modules.Exceptions.BadParamsError(null, boxIdentity, null, restrictionTypeEnum.DbColumnDataType);
                //TODO
                //return generateBoolean(Convert.ToBoolean(from), Convert.ToBoolean(to), closedFrom, (long)length, column);
                case Ferda.Modules.Helpers.Data.Column.ValueType.Unknown:
                    //TODO
                    throw Ferda.Modules.Exceptions.BadParamsError(null, boxIdentity, null, restrictionTypeEnum.DbColumnDataType);
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(Ferda.Modules.Helpers.Data.Column.GetColumnValueTypeByValueSubType(column.columnSubType));
            }
        }
    }
}
