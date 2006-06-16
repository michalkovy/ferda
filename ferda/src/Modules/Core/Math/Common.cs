using System;
using Ferda.Modules;

namespace Ferda.Guha.Math
{
    public static class Common
    {
        const int defaultPrecisionTestForEquality = 10;

        public static bool CloseEnough(int precision, double x, double y)
        {
            if (precision < 1)
                throw new ArgumentOutOfRangeException("precision", precision, "The precision must be at least 1.");
            if ((x == 0.0) && (y == 0.0))
                return true;

            double level = (System.Math.Abs(x) > System.Math.Abs(y))
                               ? System.Math.Log10(System.Math.Abs(x))
                               : System.Math.Log10(System.Math.Abs(y));

            return (System.Math.Abs(x - y) < System.Math.Pow(10, level - precision));
        }

        public static bool Compare(RelationEnum relation, double firstOperand, double secondOperand)
        {
            switch (relation)
            {
                case RelationEnum.Equal:
                    return CloseEnough(defaultPrecisionTestForEquality, firstOperand, secondOperand);
                case RelationEnum.Greater:
                    return (firstOperand > secondOperand);
                case RelationEnum.GreaterOrEqual:
                    return (firstOperand >= secondOperand);
                case RelationEnum.Less:
                    return (firstOperand < secondOperand);
                case RelationEnum.LessOrEqual:
                    return (firstOperand <= secondOperand);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get orientation of specified relation. I.e.
        /// for orientation to growth (&gt; or &gt;=) returns 1;
        /// for request for equality returns 0;
        /// otherwise returns -1.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <returns></returns>
        public static int GetRelationOrientation(RelationEnum relation)
        {
            switch (relation)
            {
                case RelationEnum.Equal:
                    return 0;
                case RelationEnum.Greater:
                case RelationEnum.GreaterOrEqual:
                    return 1;
                case RelationEnum.Less:
                case RelationEnum.LessOrEqual:
                    return -1;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the orientation start value for specified orientation. 
        /// I.e. NaN for Equality, -INF for growth, +INF lowering.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <returns></returns>
        public static double GetOrientationStartValue(RelationEnum relation)
        {
            switch (relation)
            {
                case RelationEnum.Equal:
                    return Double.NaN;
                case RelationEnum.Greater:
                case RelationEnum.GreaterOrEqual:
                    return Double.NegativeInfinity;
                case RelationEnum.Less:
                case RelationEnum.LessOrEqual:
                    return Double.PositiveInfinity;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the orientation better value for specified relation.
        /// I.e. greater value for growth, lower value for lowering.
        /// Exception is thrown if relation is request for equality.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static double GetOrientationBetterValue(RelationEnum relation, double x, double y)
        {
            switch (relation)
            {
                case RelationEnum.Equal:
                    throw new InvalidOperationException();
                case RelationEnum.Greater:
                case RelationEnum.GreaterOrEqual:
                    if (x == Double.NaN)
                        return y;
                    if (y == Double.NaN)
                        return x;
                    return System.Math.Max(x, y);
                case RelationEnum.Less:
                case RelationEnum.LessOrEqual:
                    if (x == Double.NaN)
                        return y;
                    if (y == Double.NaN)
                        return x;
                    return System.Math.Min(x, y);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}