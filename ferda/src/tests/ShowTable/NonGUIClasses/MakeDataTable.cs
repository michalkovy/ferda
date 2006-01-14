using System;
using System.Collections;
using System.Text;
using System.Data;

namespace Ferda
{
    namespace ShowTable.NonGUIClasses
    {
        public class MakeDataTable
        {
            private String [,] data;
            public MakeDataTable(String[,] data)
            {
                this.data = data;
            }

            public DataTable GetDataTable()
            {
                DataTable returnTable = new DataTable("Table");
                DataColumn myDataColumn;
                DataRow newRow;

                for(int i = 0; i < this.data.GetUpperBound(0);i++)
                {
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = System.Type.GetType("System.String");
                    myDataColumn.ColumnName = "Column" + i.ToString();
                    myDataColumn.ReadOnly = true;
                    returnTable.Columns.Add(myDataColumn);
                }

                for (int i = 0; i < this.data.GetUpperBound(0); i++)
                {
                    newRow = returnTable.NewRow();
                    for (int j = 0; j < this.data.GetUpperBound(1); j++)
                    {
                        newRow["Column"+j.ToString()] = this.data[i, j];
                    }
                    returnTable.Rows.Add(newRow);
                }
                return returnTable;
            }
        }
    }

}