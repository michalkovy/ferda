using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.Guha.MiningProcessor
{
    public class Exceptions
    {
        public static BoxRuntimeError BitStringLengthError()
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "The length of a BitString must be a positive integer.";

            return result;
        }

        public static BoxRuntimeError BitStringsLengtsAreNotEqualError()
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "BitString sizes do not match.";
            return result;
        }

        public static BoxRuntimeError MaxLengthIsLessThanMinLengthError()
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "MinLength is greather than MaxLength";
            return result;
        }

        public static BoxRuntimeError EmptyCedentIsNotAllowedError(MarkEnum cedentType)
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "Empty cedent (or with MinLength equal to zero) is not allowed for cedent type: " + cedentType.ToString();
            return result;
        }
    }
}
