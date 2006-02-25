using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.Modules.Helpers.Common
{
    /// <summary>
    /// Used as result of some parsing methods. See
    /// <see cref="T:Ferda.Modules.Helpers.Common.Parsing"/>.
    /// </summary>
    public enum ParsedResultType
    {
        /// <summary>
        /// Used when parsing wasn`t successful.
        /// </summary>
        None,

        /// <summary>
        /// Used when result of parsing is one 
        /// item of specified enumeration (enum).
        /// </summary>
        Enum,

        /// <summary>
        /// Used when parsing didn`t lead to parse some item
        /// of specified enumeration (enum) but to some number 
        /// (usually long).
        /// </summary>
        Value
    }

    /// <summary>
    /// This static class provides some static 
    /// methods useful for parsing enums, ingrals, ...
    /// </summary>
    public static class Parsing
    {
        /// <summary>
        /// Tries the parse enum or integral value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="enumValue">The out enum value.</param>
        /// <param name="integralValue">The out integral value.</param>
        /// <returns>Result type.</returns>
        public static ParsedResultType TryParseEnumOrIntegral(string input, Type enumType, out object enumValue, out long integralValue)
        {
            integralValue = 0;

            // try parse by enum
            enumValue = null;
            foreach (string enumItemName in Enum.GetNames(enumType))
            {
                if (String.Equals(enumItemName, input, StringComparison.OrdinalIgnoreCase))
                {
                    enumValue = Enum.Parse(enumType, input);
                    return ParsedResultType.Enum;
                }
            }

            // try parse long integer
            if (Int64.TryParse(input, out integralValue))
                return ParsedResultType.Value;

            // parsing was unsuccsessful
            return ParsedResultType.None;
        }

        /// <summary>
        /// Parses the enum or integral value. If parsing is not successful
        /// some exception is thrown.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="enumValue">The out enum value.</param>
        /// <param name="integralValue">The out integral value.</param>
        /// <returns>Result type.</returns>
        public static ParsedResultType ParseEnumOrIntegral(string input, Type enumType, out object enumValue, out long integralValue)
        {
            integralValue = 0;

            // try parse by enum
            enumValue = null;
            foreach (string enumItemName in Enum.GetNames(enumType))
            {
                if (String.Equals(enumItemName, input, StringComparison.OrdinalIgnoreCase))
                {
                    enumValue = Enum.Parse(enumType, input);
                    return ParsedResultType.Enum;
                }
            }

            // parse long integer
            integralValue = Int64.Parse(input);
            return ParsedResultType.Value;

            // if unsuccessful some exception si thrown
        }

        /// <summary>
        /// Tries to parse <see cref="T:Ferda.Modules.RangeEnum"/> item 
        /// or integer result from <c>oneBasedBound</c>. First item has 
        /// bound equal to 1!
        /// </summary>
        /// <param name="oneBasedBound">
        /// String representing <see cref="T:Ferda.Modules.RangeEnum"/> 
        /// or integer &lt; 0. Bound is counted from 1.
        /// </param>
        /// <returns>
        /// <remarks>
        /// Input bound is from range 1 .. infinite but output bound 
        /// is from range 0 .. infinite (input is decreased by 1).
        /// </remarks>
        /// <para>Returns -1 iff RangeEnum.All was entered.</para>
        /// <para>Returns -2 iff RangeEnum.Half was entered.</para>
        /// <para>Otherwise returns parsed integer decreased by 1 .</para>.</returns>
        /// <exception cref="T:Ferda.Modules.Exceptions.BadParamsError">
        /// If parsing integer or RangeEnum from parameter <c>oneBasedBound</c>
        /// was unsuccesfull or parsed integer isn`t &lt; 0.
        /// </exception>
        public static int ZeroBasedBoundFromOneBasedString(string oneBasedBound)
        {
            object enumResult;
            long numberResult;
            ParsedResultType parsedResult;
            try
            {
                parsedResult = ParseEnumOrIntegral(oneBasedBound, typeof(RangeEnum), out enumResult, out numberResult);
            }
            catch (Exception ex)
            {
                throw Ferda.Modules.Exceptions.BadParamsError(ex, null, "Error while parsing input: '" + oneBasedBound + "'", restrictionTypeEnum.BadFormat);
            }

            if (parsedResult == ParsedResultType.Enum)
            {
                RangeEnum rangeEnum = (RangeEnum)enumResult;
                switch (rangeEnum)
                {
                    case RangeEnum.All:
                        return -1;
                    case RangeEnum.Half:
                        return -2;
                    default:
                        throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(rangeEnum);
                }
            }
            else //if (parsedResult == ParsedResultType.Value)
            {
                if (numberResult <= 0)
                    throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Bound have to greater than 0! Current bound is: " + oneBasedBound, restrictionTypeEnum.Minimum);
                return (int)numberResult - 1;
            }
        }
    }
}
