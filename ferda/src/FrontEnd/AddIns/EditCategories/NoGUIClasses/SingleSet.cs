using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Ferda;

namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories
    {
        /// <summary>
        /// Class for a single set of values.
        /// </summary>
        public class SingleSet
        {
            /// <summary>
            /// Constructor which fills in the values for a set.
            /// </summary>
            /// <param name="Values">Values to be added</param>
            public SingleSet(ArrayList Values)
            {
                this.values = Values;
                this.values.Sort();
            }

            /// <summary>
            /// Values in the set.
            /// </summary>
            private ArrayList values = new ArrayList();

            public ArrayList Values
            {
                get
                {
                    return this.values;
                }
            }

            /// <summary>
            /// Method to detect whether set contains the required value
            /// </summary>
            /// <param name="value">Value to check for</param>
            /// <returns></returns>
            public bool IsInSet(int value)
            {
                if (this.values.Contains(value))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Method to detect whether set contains the required value
            /// </summary>
            /// <param name="value">Value to check for</param>
            /// <returns></returns>
            public bool IsInSet(object value)
            {
                if (this.values.Contains(value))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Method to add the value to set
            /// </summary>
            /// <param name="value">Value to add</param>
            public void AddValue(object value)
            {
                this.values.Add(value);
            }
        }
    }
}