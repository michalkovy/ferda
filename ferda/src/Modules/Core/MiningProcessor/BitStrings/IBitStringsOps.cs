using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public abstract class IBitStringsOps
    {
        protected abstract IBitString operation(IBitString operand1, IBitString operand2);

        public IBitString[][] Operation(IBitString[] rowOperand, IBitString[] columnOperand)
        {
            if (rowOperand == null)
                throw new ArgumentNullException("rowOperand");
            if (columnOperand == null)
                throw new ArgumentNullException("columnOperand");

            int rNum = rowOperand.GetLength(0);
            int cNum = columnOperand.GetLength(0);

            IBitString[][] result = new IBitString[rNum][];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = new IBitString[cNum];
                for (int c = 0; c < cNum; c++)
                {
                    result[r][c] = operation(rowOperand[r], columnOperand[c]);
                }
            }
            return result;
        }
        
        public IBitString[][] Operation(IBitString[][] twoDimTable, IBitString operand)
        {
            if (twoDimTable == null)
                throw new ArgumentNullException("twoDimTable");
            if (operand == null)
                throw new ArgumentNullException("operand");

            int rNum = twoDimTable.GetLength(0);
            int cNum = twoDimTable.GetLength(1);
            
            IBitString[][] result = new IBitString[rNum][];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = new IBitString[cNum];
                for (int c = 0; c < cNum; c++)
                {
                    result[r][c] = operation(twoDimTable[r][c], operand);
                }
            }
            return result;
        }

        public IBitString[] Operation(IBitString[] rowOperand, IBitString operand)
        {
            if (rowOperand == null)
                throw new ArgumentNullException("rowOperand");
            if (operand == null)
                throw new ArgumentNullException("operand");

            int rNum = rowOperand.GetLength(0);
            
            IBitString[] result = new IBitString[rNum];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = operation(rowOperand[r], operand);
            }
            return result;
        }
    }
}
