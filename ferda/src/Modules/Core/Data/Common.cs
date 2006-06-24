using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Guha.Data
{
    public static class Common
    {
        private static int assignOrdNumber(CardinalityEnum op)
        {
            switch (op)
            {
                case CardinalityEnum.Nominal:
                    return 0;
                case CardinalityEnum.Ordinal:
                    return 1;
                case CardinalityEnum.OrdinalCyclic:
                    return 2;
                case CardinalityEnum.Cardinal:
                    return 3;
                default:
                    throw new NotImplementedException();
            }            
        }
        
        public static int CompareCardinalityEnums(CardinalityEnum op1, CardinalityEnum op2)
        {
            return assignOrdNumber(op1).CompareTo(assignOrdNumber(op2));
        }
        
        public static CardinalityEnum GreaterCardinalityEnums(CardinalityEnum op1, CardinalityEnum op2)
        {
            int comparationResult = CompareCardinalityEnums(op1, op2);
            if (comparationResult >= 0)
                return op1;
            else
                return op2;
        }

        public static CardinalityEnum LesserCardinalityEnums(CardinalityEnum op1, CardinalityEnum op2)
        {
            int comparationResult = CompareCardinalityEnums(op1, op2);
            if (comparationResult >= 0)
                return op2;
            else
                return op1;
        }
    }
}
