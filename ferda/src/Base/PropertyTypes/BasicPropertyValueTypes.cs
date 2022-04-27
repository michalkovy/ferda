using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules
{
    public static class BasicPropertyValueTypes
    {
        public const string BoolT = "::Ferda::Modules::BoolT";

        public const string ShortT = "::Ferda::Modules::ShortT";
        public const string IntT = "::Ferda::Modules::IntT";
        public const string LongT = "::Ferda::Modules::LongT";

        public const string FloatT = "::Ferda::Modules::FloatT";
        public const string DoubleT = "::Ferda::Modules::DoubleT";

        public const string StringT = "::Ferda::Modules::StringT";
        
        public const string DateT = "::Ferda::Modules::DateT";
        public const string DateTimeT = "::Ferda::Modules::DateTimeT";

        public const string TimeT = "::Ferda::Modules::TimeT";

        /// <summary>
        /// Determines whether the <c>propertyValueType</c> (i.e. ICE id of type of the PropertyValue)
        /// is "basic property value type".
        /// </summary>
        /// <param name="propertyValueType">ICE id of type of the property value.</param>
        /// <returns>
        /// 	<c>true</c> if <c>propertyValueType</c> is "basic"; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Basic types could be determined by <c>const string</c> fields of this static class.
        /// </remarks>
        public static bool IsInBasicPropertyValueTypes(string propertyValueType)
        {
            return
                (
                (propertyValueType == BoolT)
                || (propertyValueType == ShortT)
                || (propertyValueType == IntT)
                || (propertyValueType == LongT)
                || (propertyValueType == FloatT)
                || (propertyValueType == DoubleT)
                || (propertyValueType == StringT)
                || (propertyValueType == DateT)
                || (propertyValueType == DateTimeT)
                || (propertyValueType == TimeT)
                );
        }

        public static string GetPropertyValueTypeIceId(Type type)
        {
            if (ReferenceEquals(typeof(BoolTI), type))
                return BoolT;
            else if (ReferenceEquals(typeof(ShortTI), type))
                return ShortT;
            else if (ReferenceEquals(typeof(IntTI), type))
                return IntT;
            else if (ReferenceEquals(typeof(LongTI), type))
                return LongT;
            else if (ReferenceEquals(typeof(FloatTI), type))
                return FloatT;
            else if (ReferenceEquals(typeof(DoubleTI), type))
                return DoubleT;
            else if (ReferenceEquals(typeof(StringTI), type))
                return StringT;
            else if (ReferenceEquals(typeof(DateTI), type))
                return DateT;
            else if (ReferenceEquals(typeof(DateTimeTI), type))
                return DateTimeT;
            else if (ReferenceEquals(typeof(TimeTI), type))
                return TimeT;
            else
                throw new ArgumentOutOfRangeException("type", type, "Specified type is not basic.");
        }

        /// <summary>
        /// Gets the property value (returned object is of corresponding CLR subtype of the PropertyValue).
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <returns>
        /// The CLR subtype value representation of the PropertyValue. E.g. for 
        /// <c>Ferda.Modules.BoolT</c> or <c>Ferda.Modules.BoolTI</c> the <c>bool</c> value
        /// is returned.
        /// </returns>
        public static object GetPropertyValue(Ferda.Modules.PropertyValue propertyValue)
        {
            string propertyValueTypeIceId = propertyValue.ice_id();
            switch (propertyValueTypeIceId)
            {
                case Ferda.Modules.BasicPropertyValueTypes.BoolT:
                    return ((Ferda.Modules.BoolT)propertyValue).boolValue;
                case Ferda.Modules.BasicPropertyValueTypes.ShortT:
                    return ((Ferda.Modules.ShortT)propertyValue).shortValue;
                case Ferda.Modules.BasicPropertyValueTypes.IntT:
                    return ((Ferda.Modules.IntT)propertyValue).intValue;
                case Ferda.Modules.BasicPropertyValueTypes.LongT:
                    return ((Ferda.Modules.LongT)propertyValue).longValue;
                case Ferda.Modules.BasicPropertyValueTypes.FloatT:
                    return ((Ferda.Modules.FloatT)propertyValue).floatValue;
                case Ferda.Modules.BasicPropertyValueTypes.DoubleT:
                    return ((Ferda.Modules.DoubleT)propertyValue).doubleValue;
                case Ferda.Modules.BasicPropertyValueTypes.StringT:
                    return ((Ferda.Modules.StringT)propertyValue).stringValue;
                case Ferda.Modules.BasicPropertyValueTypes.DateT:
                    {
                        Ferda.Modules.DateT value = ((Ferda.Modules.DateT)propertyValue);
                        return new System.DateTime(value.year, value.month, value.day);
                    }
                case Ferda.Modules.BasicPropertyValueTypes.DateTimeT:
                    {
                        Ferda.Modules.DateTimeT value = ((Ferda.Modules.DateTimeT)propertyValue);
                        return new System.DateTime(value.year, value.month, value.day, value.hour, value.minute, value.second);
                    }
                case Ferda.Modules.BasicPropertyValueTypes.TimeT:
                    {
                        Ferda.Modules.TimeT value = ((Ferda.Modules.TimeT)propertyValue);
                        return new System.TimeSpan(value.hour, value.minute, value.second);
                    }
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Determines the type. E.g. if specified <c>propertyValue</c> is <c>Ferda.Modules.BoolT</c>
        /// or <c>Ferda.Modules.BoolTI</c> than <c>typeof(bool)</c> is returned.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="propertyValueTypeIceId">The property value type ICE id.</param>
        /// <returns>System.Type of CLR subtype of the Ferda.Modules.PropertyValue type.</returns>
        public static Type DetermineType(Ferda.Modules.PropertyValue propertyValue, out string propertyValueTypeIceId)
        {
            propertyValueTypeIceId = propertyValue.ice_id();
            switch (propertyValueTypeIceId)
            {
                case Ferda.Modules.BasicPropertyValueTypes.BoolT:
                    return typeof(bool);
                case Ferda.Modules.BasicPropertyValueTypes.ShortT:
                    return typeof(short);
                case Ferda.Modules.BasicPropertyValueTypes.IntT:
                    return typeof(int);
                case Ferda.Modules.BasicPropertyValueTypes.LongT:
                    return typeof(long);
                case Ferda.Modules.BasicPropertyValueTypes.FloatT:
                    return typeof(float);
                case Ferda.Modules.BasicPropertyValueTypes.DoubleT:
                    return typeof(double);
                case Ferda.Modules.BasicPropertyValueTypes.StringT:
                    return typeof(string);
                case Ferda.Modules.BasicPropertyValueTypes.DateT:
                    return typeof(System.DateTime);
                case Ferda.Modules.BasicPropertyValueTypes.DateTimeT:
                    return typeof(System.DateTime);
                case Ferda.Modules.BasicPropertyValueTypes.TimeT:
                    return typeof(System.TimeSpan);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
