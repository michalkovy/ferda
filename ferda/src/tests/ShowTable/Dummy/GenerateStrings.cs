using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda
{
    namespace ShowTable.Dummy
    {
        public class GenerateStrings
        {
            private int ii;
            private int jj;
            
            public GenerateStrings(int i, int j)
            {
                this.ii = i;
                this.jj = j;
            }

            public String[,] GetStrings()
            {
                String[,] returnArray = new string[this.ii, this.jj];

                for(int i = 0;i<this.ii;i++)
                {
                    for (int j = 0;j< this.jj;j++)
                    {
                        returnArray[i,j] = "String at " + i + "," + j;
                    }
                }
                return returnArray;
            }
        }
    }
}
