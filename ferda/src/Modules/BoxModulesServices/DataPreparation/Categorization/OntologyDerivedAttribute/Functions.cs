// Functions.cs - functionality for Ontology Derived Attribute box
//
// Author: Martin Zeman <martin.zeman@email.cz>
//
// Copyright (c) 2007 Martin Zeman
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
using System.Collections;
using System.Diagnostics;
using Ferda.Guha.Attribute;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor;
using Ferda.Modules.Helpers.Caching;
using Ice;
using Common = Ferda.Guha.Attribute.Common;
using Exception = System.Exception;
using System.Data;
using System.Data.Common;
using Ferda.Modules.Boxes.OntologyRelated;
using Ferda.OntologyRelated.generated.OntologyData;


namespace Ferda.Modules.Boxes.DataPreparation.Categorization.OntologyDerivedAttribute
{
    public class Functions : AttributeFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        #region Properties

        public const string PropNameInLiterals = "NameInLiterals";
        public const string PropCountOfCategories = "CountOfCategories";
        public const string PropClosedFrom = "ClosedFrom";
        public const string PropXCategory = "XCategory";
        public const string PropIncludeNullCategory = "IncludeNullCategory";
        public const string PropDomain = "Domain";
        public const string PropFrom = "From";
        public const string PropTo = "To";
        public const string PropCategories = "Categories";
        public const string SockColumn = "Column";
        public const string SockOntologyEnablingColumn = "OntologyEnablingColumn";
        //ontology derived properties
        public const string PropCardinality = "Cardinality";
        public const string PropDistinctValues = "DistinctValues";
        public const string PropDomainDividingValues = "DomainDividingValues";
        public const string PropMinimum = "Minimum";
        public const string PropMaximum = "Maximum";
        public const string Separator = ", ";

        /// <summary>
        /// Guid
        /// </summary>
        public GuidStruct Guid
        {
            get { return BoxInfoHelper.GetGuidStructFromProperty("Guid", _boxModule); }
        }

        /// <summary>
        /// Name in literals
        /// </summary>
        public string NameInLiterals
        {
            get { return _boxModule.GetPropertyString(PropNameInLiterals); }
        }

        /// <summary>
        /// Intervals closed from
        /// </summary>
        public Side ClosedFrom
        {
            get
            {
                if (_boxModule.GetPropertyString(PropClosedFrom).Equals("Left"))
                    return Side.Left;
                else
                    return Side.Right;
            }
        }

        /// <summary>
        /// Count of intervals
        /// </summary>
        public LongTI Count
        {
            get
            {
                return _boxModule.GetPropertyLong(PropCountOfCategories);
            }
        }

        /// <summary>
        /// Attribute domain
        /// </summary>
        public DomainEnum Domain
        {
            get
            {
                return (DomainEnum)Enum.Parse(
                                        typeof(DomainEnum),
                                        _boxModule.GetPropertyString(PropDomain)
                                        );
            }
        }

        /// <summary>
        /// Attribute cardinality
        /// </summary>
        public CardinalityEnum Cardinality
        {
            get
            {
                return (CardinalityEnum)Enum.Parse(
                                             typeof(CardinalityEnum),
                                             _boxModule.GetPropertyString(PropCardinality)
                                             );
            }
        }

        /// <summary>
        /// Domain dividing values
        /// </summary>
        public string DomainDividingValues
        {
            get { return _boxModule.GetPropertyString(PropDomainDividingValues); }
        }

        /// <summary>
        /// Distinct values
        /// </summary>
        public string DistinctValues
        {
            get { return _boxModule.GetPropertyString(PropDistinctValues); }
        }

        /// <summary>
        /// Minimum value
        /// </summary>
        public string Minimum
        {
            get
            {
                return _boxModule.GetPropertyString(PropMinimum);
            }
        }

        /// <summary>
        /// Maximum value
        /// </summary>
        public string Maximum
        {
            get
            {
                return _boxModule.GetPropertyString(PropMaximum);
            }
        }

        /// <summary>
        /// Xcategory
        /// </summary>
        public string XCategory
        {
            get { return _boxModule.GetPropertyString(PropXCategory); }
        }

        private string _nullCategoryName = null;

        /// <summary>
        /// Null category name
        /// </summary>
        public StringTI IncludeNullCategory
        {
            get { return _nullCategoryName; }
        }

        /// <summary>
        /// "From" restriction
        /// </summary>
        public string From
        {
            get { return _boxModule.GetPropertyString(PropFrom); }
        }

        /// <summary>
        /// "To" restriction
        /// </summary>
        public string To
        {
            get { return _boxModule.GetPropertyString(PropTo); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the properties of entity from the ontology, which is mapped to attribute (column) of this box
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public StrSeqMap getOntologyEntityProperties(bool fallOnError)
        {
            OntologyEnablingColumnFunctionsPrx prx = GetOntologyEnablingColumnFunctionsPrx(fallOnError);
            if (prx != null)
            {
                return prx.getOntologyEntityProperties();
            }
            return null;
        }

        /// <summary>
        /// Sets the properties of entity from the ontology, which is mapped to attribute (column) of this box
        /// </summary>
        /// <returns></returns>
        public void setOntologyEntityProperties()
        {
            /// clears previous properties if there were any
            string emptyString = "";
            _boxModule.setProperty(PropCardinality, new StringTI(CardinalityEnum.Nominal.ToString()));
            _boxModule.setProperty(PropMinimum, (StringTI)emptyString);
            _boxModule.setProperty(PropMaximum, (StringTI)emptyString);
            _boxModule.setProperty(PropDomainDividingValues, (StringTI)emptyString);
            _boxModule.setProperty(PropDistinctValues, (StringTI)emptyString);

            /// gets the data properties of ontology entity 
            /// which this box's column is mapped on
            StrSeqMap tmpMap = getOntologyEntityProperties(true);

            /// the entity is a class and have some data properties
            /// the null value means that either the column is not mapped...
            /// or the entity is an instance...
            /// or the entity is a class, but has no data properties
            if (tmpMap != null) 
            {
                foreach (string tmpStr in tmpMap.Keys)
                {
                    int i = 1;
                    if (tmpMap[tmpStr] != null)
                    {
                        string tmpAllValues = "";
                        foreach (string tmpValue in tmpMap[tmpStr])
                        {
                            //empty values are ignored
                            if (tmpValue != "")
                            {
                                tmpAllValues += tmpValue;
                                if (tmpMap[tmpStr].Length > i++)
                                    tmpAllValues += Separator;
                            }
                        }
                        /// removing unwanted separator at the end of the string
                        if (tmpAllValues != "")
                        {
                            tmpAllValues = tmpAllValues.Remove(tmpAllValues.Length - Separator.Length);
                        }
                        /// setting the property of the box with value from the ontology
                        try
                        {
                            _boxModule.setProperty(tmpStr, (StringTI)tmpAllValues);
                        }
                        catch { }
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Parses from and to values
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void parseFromTo(DbDataTypeEnum dataType, out IComparable from, out IComparable to)
        {
            try
            {
                GenericColumn.ParseValue(From, dataType, out from);
            }
            catch (Exception e)
            {
                throw Exceptions.BadValueError(
                    e,
                    _boxModule.StringIceIdentity,
                    null,
                    new string[] { PropFrom },
                    restrictionTypeEnum.BadFormat
                    );
            }
            try
            {
                GenericColumn.ParseValue(To, dataType, out to);
            }
            catch (Exception e)
            {
                throw Exceptions.BadValueError(
                    e,
                    _boxModule.StringIceIdentity,
                    null,
                    new string[] { PropTo },
                    restrictionTypeEnum.BadFormat
                    );
            }
        }

        private Guid _cachesReloadFlag = System.Guid.NewGuid();
        private Guid _lastReloadFlag = System.Guid.Empty;
        private Dictionary<string, BitStringIce> _cachedValueBitStrings = null;
        private long _lastBSQueryTicks = 0;

        /// <summary>
        /// Gets categories and frequencies
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public ValuesAndFrequencies GetCategoriesAndFrequencies(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<ValuesAndFrequencies>(
                fallOnError,
                delegate
                {
                    Attribute<IComparable> tmp = GetAttribute(fallOnError);
                    GenericColumn tmp2 = GetGenericColumn(fallOnError);
                    if (tmp != null && tmp2 != null)
                    {
                        Dictionary<string, int> categoriesFrequencies = tmp.GetFrequencies(
                            tmp2.GetDistinctsAndFrequencies(null)
                            );
                        return new ValuesAndFrequencies(
                            tmp2.Explain.dataType,
                            BoxInfoHelper.GetValueFrequencyPairArray(categoriesFrequencies)
                            );
                    }
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Gets proxy of the connected column box - OntologyEnablingColumn interface
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public OntologyEnablingColumnFunctionsPrx GetOntologyEnablingColumnFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<OntologyEnablingColumnFunctionsPrx>(
                _boxModule,
                SockOntologyEnablingColumn,
                OntologyEnablingColumnFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        //TODO smazat - ale možná to bude potøeba pro AddIn - zjistit a ošetøit

        /// <summary>
        /// Gets proxy of the connected column box - Column interface
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public ColumnFunctionsPrx GetColumnFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<ColumnFunctionsPrx>(
                _boxModule,
                SockColumn,
                ColumnFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        /// <summary>
        /// Gets categories names
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public string[] GetCategoriesNames(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    Attribute<IComparable> tmp = GetAttribute(fallOnError);
                    if (tmp != null)
                    {
                        bool dummy;
                        List<string> result = tmp.GetCategoriesIds(out dummy);
                        return result.ToArray();
                    }
                    return new string[0];
                },
                delegate
                {
                    return new string[0];
                },
                _boxModule.StringIceIdentity
                );
        }

        private CacheFlag _cacheFlagColumn = new CacheFlag();
        private GenericColumn _cachedValueColumn = null;

        /// <summary>
        /// Gets generic column connected to the attribute
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public GenericColumn GetGenericColumn(bool fallOnError)
        {
            OntologyEnablingColumnFunctionsPrx prx = GetOntologyEnablingColumnFunctionsPrx(fallOnError);
            if (prx == null)
                return null;
            ColumnInfo column = prx.getColumnInfo();

            DatabaseConnectionSettingHelper connSetting =
                new DatabaseConnectionSettingHelper(column.dataTable.databaseConnectionSetting);

            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
            cacheSetting.Add(
                Datasource.Database.BoxInfo.typeIdentifier + Datasource.Database.Functions.PropConnectionString,
                connSetting);
            cacheSetting.Add(Datasource.DataTable.BoxInfo.typeIdentifier + Datasource.DataTable.Functions.PropName,
                             column.dataTable.dataTableName);
            cacheSetting.Add(
                Datasource.Column.BoxInfo.typeIdentifier + Datasource.Column.Functions.PropSelectExpression,
                column.columnSelectExpression);

            if (_cacheFlagColumn.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValueColumn == null && fallOnError))
            {
                _cachesReloadFlag = System.Guid.NewGuid();
                _cachedValueColumn = ExceptionsHandler.GetResult<GenericColumn>(
                    fallOnError,
                    delegate
                    {
                        return
                            GenericDatabaseCache.GetGenericDatabase(connSetting)
                            [column.dataTable.dataTableName].GetGenericColumn(
                            column.columnSelectExpression, column);
                    },
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            }
            return _cachedValueColumn;
        }

        /// <summary>
        /// Converts string to float
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Float</returns>
        private float ConvertToFloat(string input)
        {
            return (float)Convert.ToDouble(input);
        }

        private CacheFlag _cacheFlag = new CacheFlag();
        private Attribute<IComparable> _cachedValue = null;


        /// <summary>
        /// Returns an attribute (created or from cache)
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>Attribute</returns>
        public Attribute<IComparable> GetAttribute(bool fallOnError)
        {
            //getting the proxy of a column
            OntologyEnablingColumnFunctionsPrx prx = GetOntologyEnablingColumnFunctionsPrx(fallOnError);
            if (prx == null)
                return null;

            //getting a info about a column
            ColumnInfo tmp =
                ExceptionsHandler.GetResult<ColumnInfo>(
                    fallOnError,
                    prx.getColumnInfo,
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );

            if (tmp == null)
                return null;

            //getting the connection setting
            DatabaseConnectionSettingHelper connSetting =
                new DatabaseConnectionSettingHelper(tmp.dataTable.databaseConnectionSetting);

            //creating a new cache entry
            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();

            //adding the information about the database to the cache
            cacheSetting.Add(
                Datasource.Database.BoxInfo.typeIdentifier + Datasource.Database.Functions.PropConnectionString,
                connSetting);

            //adding the information about the data table to the cache
            cacheSetting.Add(Datasource.DataTable.BoxInfo.typeIdentifier + Datasource.DataTable.Functions.PropName,
                             tmp.dataTable.dataTableName);
            //adding the information about the column select expression to the cache
            cacheSetting.Add(
                Datasource.Column.BoxInfo.typeIdentifier + Datasource.Column.Functions.PropSelectExpression,
                tmp.columnSelectExpression);
            //adding other attribute information to the cache
            cacheSetting.Add(BoxInfo.typeIdentifier + PropDomain, Domain.ToString());
            cacheSetting.Add(BoxInfo.typeIdentifier + PropFrom, From);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropTo, To);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropClosedFrom, ClosedFrom);

            //adding the information derived from ontology to the cache
            cacheSetting.Add(BoxInfo.typeIdentifier + PropCardinality, Cardinality);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropMinimum, Minimum);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropMaximum, Maximum);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropDomainDividingValues, DomainDividingValues);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropDistinctValues, PropDistinctValues);

            //Loading the values from cache or recounting them...
            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachesReloadFlag = System.Guid.NewGuid();
                _cachedValue = ExceptionsHandler.GetResult<Attribute<IComparable>>(
                    fallOnError,
                    delegate
                    {
                        _nullCategoryName = null;

                        //getting the column
                        GenericColumn column = GetGenericColumn(fallOnError);
                        if (column == null)
                            return null;

                        //creating an empty result
                        Attribute<IComparable> result =
                            (Attribute<IComparable>)Common.GetAttributeObject(column.DbSimpleDataType, true);

                        //string representation of max and min values of the column
                        System.Data.DataTable dt;
                        string _min;
                        string _max;

                        //getting the min and max values of the column from the user input
                        //also getting the values from the table for the purpose of counting
                        //their frequencies for the equifrequency intervals generation
                        if (Domain == DomainEnum.SubDomain)
                        {
                            IComparable from;
                            IComparable to;
                            parseFromTo(column.Explain.dataType, out from, out to);
                            _min = from.ToString();
                            _max = to.ToString();
                            string columnSelectExpression =
                                column.GetQuotedQueryIdentifier();

                            //strings must be placed between apostrophes in SQL QUERY
                            if (column.DbSimpleDataType == DbSimpleDataTypeEnum.StringSimpleType)
                            {

                                dt = column.GetDistinctsAndFrequencies(
                                    columnSelectExpression + ">= '" + from + "' AND " + columnSelectExpression + "<= '" + to + "'"
                                    );
                            }

                            else
                            {
                                dt = column.GetDistinctsAndFrequencies(
                                    columnSelectExpression + ">=" + from + " AND " + columnSelectExpression + "<=" + to
                                    );
                            }
                        }
                        else if (Domain == DomainEnum.WholeDomain)
                        {
                            //or from the column statistics, if no user input is given
                            dt = column.GetDistinctsAndFrequencies(String.Empty);
                            _min = column.Statistics.valueMin;
                            _max = column.Statistics.valueMax;
                        }
                        else
                            throw new NotImplementedException();
                        
                        IComparable __min = null;
                        IComparable __max = null;

                        _min = column.Statistics.valueMin;
                        _max = column.Statistics.valueMax;

                        /// if cardinality is Cardinal or Ordinal, then attribute
                        /// is created by dividing the domain with Domain Dividing Values
                        /// and separating of Distinct Values from the intervals and creating
                        /// one category for every Distinct Value
                        if (Cardinality == CardinalityEnum.Cardinal || Cardinality == CardinalityEnum.Ordinal)
                        {

                            //creating intervals of the type based on the connected column datatype
                            //switch branches differ only in the datatype, so only one branch will be explained
                            switch (column.DbSimpleDataType)
                            {
                                case DbSimpleDataTypeEnum.FloatSimpleType:
                                case DbSimpleDataTypeEnum.DoubleSimpleType:
                                    //for double or float type, the same algorithm is used
                                    //which generates division points of double types
                                    //for float, the division points must be retyped to float

                                    //converting min and max values for the attribute domain
                                    double _dmin = Convert.ToDouble(_min);
                                    double _dmax = Convert.ToDouble(_max);

                                    //here the common class for retyping is used
                                    //the division points are retrieved there and retyped to the correct type
                                    if (column.DbSimpleDataType == DbSimpleDataTypeEnum.FloatSimpleType)
                                    {
                                        __min = (float)_dmin;
                                        __max = (float)_dmax;

                                        //delegate method to retype values to float
                                        Categorization.Retyper<float>.ToTypeDelegate dg =
                                        new Categorization.Retyper<float>.ToTypeDelegate(ConvertToFloat);

                                        float[] _divisionPoints;

                                        try
                                        {
                                            //generation of the division points
                                            _divisionPoints = Categorization.Retyper<float>.ConvertDomainDividingValuesToColumnTypeArray(DomainDividingValues, ", ", dg);
                                        }
                                        catch
                                        {
                                            throw new ArgumentOutOfRangeException();
                                        }
                                        //creating intervals in the attribute
                                        result.CreateIntervals(
                                            BoundaryEnum.Closed, __min,
                                            Categorization.Retyper<float>.Retype(_divisionPoints),
                                            ClosedFrom, __max,
                                            BoundaryEnum.Closed, false
                                        );
                                    }
                                    else
                                    {
                                        __min = _dmin;
                                        __max = _dmax;
                                        Categorization.Retyper<double>.ToTypeDelegate dg =
                                        new Categorization.Retyper<double>.ToTypeDelegate(Convert.ToDouble);

                                        double[] _divisionPoints;
                                        try
                                        {
                                            _divisionPoints = Categorization.Retyper<double>.ConvertDomainDividingValuesToColumnTypeArray(DomainDividingValues, ", ", dg);
                                        }
                                        catch
                                        {
                                            throw new ArgumentOutOfRangeException();
                                        }

                                        result.CreateIntervals(
                                            BoundaryEnum.Closed, __min,
                                            Categorization.Retyper<double>.Retype(_divisionPoints),
                                            ClosedFrom, __max,
                                            BoundaryEnum.Closed, false
                                        );
                                    }
                                    break;


                                case DbSimpleDataTypeEnum.ShortSimpleType:
                                case DbSimpleDataTypeEnum.IntegerSimpleType:
                                case DbSimpleDataTypeEnum.LongSimpleType:
                                    long _lmin = Convert.ToInt64(_min);
                                    long _lmax = Convert.ToInt64(_max);

                                    if (column.DbSimpleDataType == DbSimpleDataTypeEnum.ShortSimpleType)
                                    {
                                        __min = (short)_lmin;
                                        __max = (short)_lmax;
                                        Categorization.Retyper<short>.ToTypeDelegate dg =
                                        new Categorization.Retyper<short>.ToTypeDelegate(Convert.ToInt16);

                                        short[] _divisionPoints = Categorization.Retyper<short>.ConvertDomainDividingValuesToColumnTypeArray(DomainDividingValues, ", ", dg);

                                        result.CreateIntervals(
                                            BoundaryEnum.Closed, __min,
                                            Categorization.Retyper<short>.Retype(_divisionPoints),
                                            ClosedFrom, __max,
                                            BoundaryEnum.Closed, false
                                        );
                                    }
                                    else
                                    {
                                        if (column.DbSimpleDataType == DbSimpleDataTypeEnum.IntegerSimpleType)
                                        {
                                            __min = (int)_lmin;
                                            __max = (int)_lmax;

                                            Categorization.Retyper<int>.ToTypeDelegate dg =
                                            new Categorization.Retyper<int>.ToTypeDelegate(Convert.ToInt32);

                                            int[] _divisionPoints = Categorization.Retyper<int>.ConvertDomainDividingValuesToColumnTypeArray(DomainDividingValues, ", ", dg);

                                            System.Windows.Forms.MessageBox.Show("jsem tu");
                                            int ontologyMin = new int();
                                            int dataTableMin = new int();
                                            int from = new int();

                                            System.Windows.Forms.MessageBox.Show("hodnota min je *" + ontologyMin.ToString() + "*");
                                            try
                                            {
                                                ontologyMin = Convert.ToInt32(Minimum);
                                            }
                                            catch { }
                                            try
                                            {
                                                dataTableMin = Convert.ToInt32(column.Statistics.valueMin);
                                            }
                                            catch { }
                                            try
                                            {
                                                from = Convert.ToInt32(From);
                                            }
                                            catch { }
                                            

                                            //int test = Convert.ToInt32("");
                                            //if (test. == null)
                                                System.Windows.Forms.MessageBox.Show("hodnota min je *" + ontologyMin.ToString() + "*");

                                            

                                            
                                            /// if true: properties from ontology are in conflict
                                            /// domain dividing values can't be lower than specified minimal value
                                            if (_divisionPoints != null && _divisionPoints[0] < ontologyMin)
                                            {
                                                throw new ArgumentException("Ontology derived properties are in conflict. Some of the domain dividing values is lower than allowed minimum. The problem may be in ontology, but maybe you have changed the values manually.");
                                            }
                                            /// if true: column contains unallowed values,
                                            /// values must be greater than specified minimal value
                                            else if (dataTableMin < ontologyMin)
                                            {
                                                throw new ArgumentException("In the database there are unallowed values in the column.");
                                            }
                                            /// if true: user defined range of attribute is smaller
                                            /// than it is allowed by ontology minimum property
                                            /// the value is set to ontologyMin (in the column there are none values lower than ontologyMin - 
                                            /// due one of the previous conditions)
                                            else if (from < ontologyMin)
                                            {
                                                __min = ontologyMin;
                                            }
                                            /// if true, from property is set by user and is greater or equal to ontologyMin
                                            else if (From != "")
                                            {
                                                __min = from;
                                            }
                                            /// ontology property minimum is set
                                            else if (Minimum != "")
                                            {
                                                __min = ontologyMin;
                                            }
                                            /// neither from nor minimum property is set
                                            /// mimimal value is the minimal value from the dataColumn
                                            {
                                                __min = dataTableMin;
                                            }
                                            
                                            result.CreateIntervals(
                                                BoundaryEnum.Closed, __min,
                                                Categorization.Retyper<int>.Retype(_divisionPoints),
                                                ClosedFrom, __max,
                                                BoundaryEnum.Closed, false
                                            );
                                        }
                                        else
                                        {
                                            __min = _lmin;
                                            __max = _lmax;
                                            Categorization.Retyper<long>.ToTypeDelegate dg =
                                            new Categorization.Retyper<long>.ToTypeDelegate(Convert.ToInt64);

                                            long[] _divisionPoints = Categorization.Retyper<long>.ConvertDomainDividingValuesToColumnTypeArray(DomainDividingValues, ", ", dg);

                                            result.CreateIntervals(
                                                BoundaryEnum.Closed, __min,
                                                Categorization.Retyper<long>.Retype(_divisionPoints),
                                                ClosedFrom, __max,
                                                BoundaryEnum.Closed, false
                                            );
                                        }
                                    }
                                    break;

                                case DbSimpleDataTypeEnum.DateTimeSimpleType:
                                    {
                                        __min = Convert.ToDateTime(_min);
                                        __max = Convert.ToDateTime(_max);

                                        Categorization.Retyper<DateTime>.ToTypeDelegate dg =
                                                new Categorization.Retyper<DateTime>.ToTypeDelegate(Convert.ToDateTime);

                                        DateTime[] _divisionPoints = Categorization.Retyper<DateTime>.ConvertDomainDividingValuesToColumnTypeArray(DomainDividingValues, ", ", dg);

                                        result.CreateIntervals(
                                            BoundaryEnum.Closed, __min,
                                            Categorization.Retyper<DateTime>.Retype(_divisionPoints),
                                            ClosedFrom, __max,
                                            BoundaryEnum.Closed, false
                                        );
                                    }
                                    break;
                                case DbSimpleDataTypeEnum.StringSimpleType:
                                    {
                                        __min = _min;
                                        __max = _max;

                                        Categorization.Retyper<string>.ToTypeDelegate dg =
                                            new Categorization.Retyper<string>.ToTypeDelegate(Convert.ToString);

                                        string[] _divisionPoints = Categorization.Retyper<string>.ConvertDomainDividingValuesToColumnTypeArray(DomainDividingValues, ", ", dg);

                                        result.CreateIntervals(
                                            BoundaryEnum.Closed, __min,
                                            Categorization.Retyper<string>.Retype(_divisionPoints),
                                            ClosedFrom, __max,
                                            BoundaryEnum.Closed, false
                                        );
                                    }
                                    break;

                                default:
                                    //    if (fallOnError)
                                    throw new ArgumentException("Type not supported");
                                //    else
                                //         break;

                            }

                            _nullCategoryName = result.NullContainingCategory;

                            //DistinctValues

                            result.Add("12541");
                            result["12541"].Enumeration.Add(12541, true);

                            //result.CreateEnums(enumeration.ToArray(), containsNull, true);

                            //TODO smazat
                            //foreach (int dp in _divisionPoints)
                                //System.Windows.Forms.MessageBox.Show("vse ok");

                            return result;
                        }
                        /// if cardinality is nominal or cyclic ordinal,
                        /// then attribute is created as Each Value One Category
                        /*else
                        {
                            bool containsNull = false;
                            if (dt.Rows[0][0] is DBNull)
                                containsNull = true;

                            List<object> enumeration = new List<object>(dt.Rows.Count);
                            foreach (System.Data.DataRow row in dt.Rows)
                            {
                                object v = row[0];
                                if (v == null || v is DBNull)
                                    continue;
                                enumeration.Add(row[0]);
                            }

                            result.CreateEnums(enumeration.ToArray(), containsNull, true);

                            _nullCategoryName = result.NullContainingCategory;

                            return result;
                        }*/
                        return null;
                    },
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            }
            return _cachedValue;
        }

        //       private Guid _cachesReloadFlag = System.Guid.NewGuid();
        //      private Guid _lastReloadFlag;
        //     private Dictionary<string, BitStringIce> _cachedValueBitStrings = null;
        //     private long _lastBSQueryTicks = 0;

        /// <summary>
        /// Gets bitstring of the attribute
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public Dictionary<string, BitStringIce> GetBitStrings(bool fallOnError)
        {
            lock (this)
            {
                return ExceptionsHandler.GetResult<Dictionary<string, BitStringIce>>(
                    fallOnError,
                    delegate
                    {
                        if (_cachedValueBitStrings == null
                            || _lastReloadFlag == System.Guid.Empty
                            || _lastReloadFlag != _cachesReloadFlag
                            //|| (DateTime.Now.Ticks - _lastBSQueryTicks) > 300000000
                            )
                        // ticks:
                        // 1 tick = 100-nanosecond 
                        // nano is 0.000 000 001
                        // mili is 0.001
                        // 0.3 sec is ticks * 3 000 000
                        {
                            // get primary key
                            OntologyEnablingColumnFunctionsPrx prx = GetOntologyEnablingColumnFunctionsPrx(fallOnError);
                            if (prx == null)
                                return null;
                            string[] pks = prx.getColumnInfo().dataTable.primaryKeyColumns;

                            GenericColumn gc = GetGenericColumn(fallOnError);
                            Attribute<IComparable> att = GetAttribute(true);
                            //if (String.IsNullOrEmpty(_lastReloadFlag.ToString()) || _lastReloadFlag != _cachesReloadFlag)
                            {
                                if (gc == null || att == null)
                                {
                                    _cachedValueBitStrings = null;
                                    return null;
                                }
                                _cachedValueBitStrings = att.GetBitStrings(gc.GetSelect(pks));
                            }

                            _lastBSQueryTicks = DateTime.Now.Ticks;
                            _lastReloadFlag = _cachesReloadFlag;
                        }
                        return _cachedValueBitStrings;
                    },
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            }
        }

        /// <summary>
        /// Gets bitstring with specified category name
        /// </summary>
        /// <param name="categoryName">Category name to get bitstring for</param>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public BitStringIce GetBitString(string categoryName, bool fallOnError)
        {
            // categoryName is "" if it should be null (throught middleware)
            if (String.IsNullOrEmpty(categoryName))
                if (fallOnError)
                    throw Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity,
                                                 "String.IsNullOrEmpty(categoryName) in public BitStringIce GetBitString(string categoryName, bool fallOnError)");
                else
                    return null;

            lock (this)
            {
                return ExceptionsHandler.GetResult<BitStringIce>(
                    fallOnError,
                    delegate
                    {
                        Dictionary<string, BitStringIce> cachedValueBitStrings = GetBitStrings(fallOnError);
                        if (cachedValueBitStrings == null)
                        {
                            if (fallOnError)
                                throw Exceptions.BoxRuntimeError(null, _boxModule.StringIceIdentity,
                                                                 "cachedValueBitStrings == null in public BitStringIce GetBitString(string categoryName, bool fallOnError)");
                            else
                                return null;
                        }
                        else
                        {
                            try
                            {
                                return cachedValueBitStrings[categoryName];
                            }
                            catch (KeyNotFoundException e)
                            {
                                throw Exceptions.BoxRuntimeError(e, _boxModule.StringIceIdentity,
                                                                 "Category named " + categoryName +
                                                                 " was not found in the attribute.");
                            }
                        }
                    },
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            }
        }

        /// <summary>
        /// Gets categories ids
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public string[] GetCategoriesIds(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    Attribute<IComparable> tmp = GetAttribute(fallOnError);
                    if (tmp != null)
                    {
                        List<string> result = tmp.GetNotMissingsCategorieIds(XCategory);
                        return result.ToArray();
                    }
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Returns categories numeric values
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public double[] GetCategoriesNumericValues(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<double[]>(
                fallOnError,
                delegate
                {
                    Attribute<IComparable> tmp = GetAttribute(fallOnError);
                    GenericColumn tmp2 = GetGenericColumn(fallOnError);
                    if (tmp != null && tmp2 != null)
                    {
                        if (!tmp2.IsNumericDataType)
                            return null;
                        List<IComparable> partialResult =
                            tmp.GetSingleValues(tmp.GetNotMissingsCategorieIds(XCategory));
                        if (partialResult == null)
                            return null;
                        List<double> result = new List<double>(partialResult.Count);
                        foreach (IComparable comparable in partialResult)
                        {
                            result.Add((double)comparable);
                        }
                        return result.ToArray();
                    }
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Counts potential cardinality
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public CardinalityEnum PotentiallyCardinality(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<CardinalityEnum>(
                fallOnError,
                delegate
                {
                    OntologyEnablingColumnFunctionsPrx prx = GetOntologyEnablingColumnFunctionsPrx(fallOnError);
                    Attribute<IComparable> tmp = GetAttribute(fallOnError);
                    GenericColumn tmp2 = GetGenericColumn(fallOnError);
                    if (tmp != null && tmp2 != null && prx != null)
                    {
                        CardinalityEnum columnCardinality = prx.getColumnInfo().cardinality;

                        bool ordered;
                        tmp.GetCategoriesIds(out ordered);

                        if (Guha.Data.Common.CompareCardinalityEnums(columnCardinality, CardinalityEnum.Nominal) <=
                            0)
                            return CardinalityEnum.Nominal;
                        else
                        {
                            if (!ordered)
                                return CardinalityEnum.Nominal;
                            else
                            {
                                if (
                                    Guha.Data.Common.CompareCardinalityEnums(columnCardinality,
                                                                             CardinalityEnum.Cardinal) >= 0)
                                {
                                    bool isSingleValueCategories = (GetCategoriesNumericValues(fallOnError) != null);
                                    if (isSingleValueCategories)
                                        return CardinalityEnum.Cardinal;
                                }
                                else
                                {
                                    return columnCardinality; // Ordinal / OrdinalCyclic
                                }
                            }
                        }
                    }
                    return CardinalityEnum.Nominal;
                },
                delegate
                {
                    return CardinalityEnum.Nominal;
                },
                _boxModule.StringIceIdentity
                );
        }

        #endregion

        /// <summary>
        /// Gets attribute cardinality
        /// </summary>
        /// <param name="current__"></param>
        /// <returns>Attribute cardinality</returns>
        public override CardinalityEnum GetAttributeCardinality(Current current__)
        {
            if (Guha.Data.Common.CompareCardinalityEnums(
                    Cardinality,
                    PotentiallyCardinality(true)
                    ) > 1)
            {
                throw Exceptions.BadValueError(
                    null,
                    _boxModule.StringIceIdentity,
                    "Unsupported cardinality type for current attribute setting.",
                    new string[] { PropCardinality },
                    restrictionTypeEnum.OtherReason
                    );
            }
            return Cardinality;
        }

        /// <summary>
        /// Gets attribute id
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override GuidStruct GetAttributeId(Current current__)
        {
            return Guid;
        }

        /// <summary>
        /// Gets attribute names
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override GuidAttributeNamePair[] GetAttributeNames(Current current__)
        {
            return new GuidAttributeNamePair[]
                {
                    new GuidAttributeNamePair(Guid, NameInLiterals),
                };
        }

        /// <summary>
        /// Gets bitstring for specified category id
        /// </summary>
        /// <param name="categoryId">Categoryid of the required bitstring</param>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override BitStringIce GetBitString(string categoryId, Current current__)
        {
            return GetBitString(categoryId, true);
        }

        /// <summary>
        /// Gets categories ids
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override string[] GetCategoriesIds(Current current__)
        {
            return GetCategoriesIds(true);
        }


        /// <summary>
        /// Gets categories numeric values
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override double[] GetCategoriesNumericValues(Current current__)
        {
            return GetCategoriesNumericValues(true);
        }


        /// <summary>
        /// Gets missing information category id
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override string[] GetMissingInformationCategoryId(Current current__)
        {
            if (String.IsNullOrEmpty(XCategory))
                return new string[0];
            else
                return new string[] { XCategory }; ;
        }

        /// <summary>
        /// Gets source datatable id
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override string GetSourceDataTableId(Current current__)
        {
            OntologyEnablingColumnFunctionsPrx prx = GetOntologyEnablingColumnFunctionsPrx(true);
            if (prx != null)
                return prx.GetSourceDataTableId();
            return null;
        }

        /// <summary>
        /// Gets serialized attribute
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override string getAttribute(Current current__)
        {
            return Guha.Attribute.Serializer.Serialize(GetAttribute(true).Export());
        }

        /// <summary>
        /// Gets categories and their frequencies in the attribute
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override ValuesAndFrequencies getCategoriesAndFrequencies(Current current__)
        {
            return GetCategoriesAndFrequencies(true);
        }

        /// <summary>
        /// Gets count vector for the relational DM purposes
        /// </summary>
        /// <param name="masterIdColumn">Master table id column</param>
        /// <param name="masterDatatableName">Master table name</param>
        /// <param name="detailIdColumn">Detail table id column</param>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override int[] GetCountVector(string masterIdColumn, string masterDatatableName, string detailIdColumn, Current current__)
        {
            string detailId = String.Empty;
            if (String.IsNullOrEmpty(detailIdColumn))
                detailId =
            GetOntologyEnablingColumnFunctionsPrx(true).getColumnInfo().dataTable.primaryKeyColumns[0];
            else
                detailId = detailIdColumn;

            GenericColumn _column = GetGenericColumn(true);
            DataTable _table = _column.GetCountVector(
                masterIdColumn, masterDatatableName, detailId);
            int[] result = new int[_table.Rows.Count];
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                result[i] = (int)_table.Rows[i][0];
            }
            return result;
        }

        /// <summary>
        /// Gets next bitstring for virtual column - not implemented here, as the attribute is not virtual
        /// </summary>
        /// <param name="skipFirstN"></param>
        /// <param name="bitString"></param>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override bool GetNextBitString(int skipFirstN, out BitStringIceWithCategoryId bitString, Current current__)
        {
            bitString = new BitStringIceWithCategoryId();
            return false;
        }

        /// <summary>
        /// Gets maximal bitstring count (for the relational DM purposes)
        /// </summary>
        /// <param name="current__"></param>
        /// <returns></returns>
        public override long GetMaxBitStringCount(Current current__)
        {
            return 0;
        }

        #region IFunctions Members

        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
        }

        #endregion

    }
}