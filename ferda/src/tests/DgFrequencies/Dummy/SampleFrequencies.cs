using System;
using System.Collections.Generic;
using System.Text;
using Ferda.DgFrequencies.NonGUIClasses;

namespace Ferda
{
    namespace DgFrequencies.Dummy
    {
        /// <summary>
        /// Sample class for constructing some frequencies and values;
        /// </summary>
        public class SampleFrequencies
        {
            /// <summary>
            /// Method for retrieving sample data;
            /// </summary>
            /// <returns>Frequency array</returns>
            public Frequency[] GetFrequencies()
            {
                Random myRandom = new Random();
                Frequency [] returnValue = new Frequency[20];
                int sum = 0;
                int i = 0;
                while (i < 20)
                {
                    int number = myRandom.Next(5);
                    if ((sum + number) < 100)
                    {
                        returnValue[i] = new Frequency();
                        returnValue[i].AttributeName = "Attribute nr." + i;
                        returnValue[i].Freq = number;
                        sum += number;
                        i++;
                    }
                }

                return returnValue;
            }
        }
    }
}