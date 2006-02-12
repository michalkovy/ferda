using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.Modules.Helpers.Common
{
    public enum ParsedResultType
    {
        None,
        Enum,
        Value
    }

    public static class Parsing
    {
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
