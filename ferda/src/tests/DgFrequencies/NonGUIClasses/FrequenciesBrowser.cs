using System;
using System.Collections;
using System.Text;

namespace Ferda
{
    namespace DgFrequencies.NonGUIClasses
    {
        public struct Frequency
        {
            public String AttributeName;
            public long Freq;
        }

        public class FrequenciesBrowser
        {
            private ArrayList frequencies = new ArrayList();

            public FrequenciesBrowser(Frequency [] frequencies)
            {
                foreach (Frequency frequency in frequencies)
                {
                    this.frequencies.Add(frequency);
                }
            }

            public IEnumerator GetEnumerator()
            {
                foreach (Frequency frequency in this.frequencies)
                {
                    yield return frequency;
                }
            }
        }
    }

}