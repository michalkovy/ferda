namespace Design.Miners
{
    /*
    interface ICategorialCedent
    {
        IBitString this[int index]
        {
            get;
            set;
        }

        int Length { get; set; }
        Guid AttributeId { get; set; }
        object Cardinality { get; set; } //enumeration

        #region Methods
        ICategorialCedent And(IBitString bitString); //po slozkach udela conjunkci a zachova ostatni parmetry (cardinality, ...)
        ICategorialCedent Or(IBitString bitString);
        #endregion
    }

    interface ICategorialCedentSetting
    {
        //TODO
    }

    interface ICategorialCedentEnumerator : IEnumerable<ICategorialCedent>
    {

    }
    /*
    class Pseudocode
    {
        public void CfMiner(ICategorialCedentEnumerator rowAttributes, IEntityEnumerator conditions)
        {
            foreach (ICategorialCedent rowAttribute in rowAttributes)
            {
                foreach (IBitString condition in conditions)
                {

                }
            }
        }


        public IEnumerator<IBitString> GenerovaniPodmnozin(List<IBitString> bitStrings, int minLen, int maxLen)
        {
            //pokud minLen == 1
            IBitString cache;
            Stack<IBitString> resultsStack = new Stack<IBitString>(maxLen - 1);
            int actLen;
            int bitStingsCount = bitStrings.Count;
            for (int i = 0; i < bitStingsCount; i++)
            {
                // initialize
                cache = bitStrings[i];
                actLen = 1;
                yield return cache;


            nextSubset:
                if (actLen < maxLen) // subset should be prolonged
                {
                    if ((actLen + i) < bitStingsCount) // next item to subset can be added
                    {
                        // prolong subset
                        resultsStack.Push(cache);
                        cache = cache.or(bitStrings[actLen + i]);
                        actLen++;
                        yield return cache;
                        goto nextSubset;
                    }
                    else
                    {
                        // next subsets will be shorted.
                        maxLen--;
                        goto nextSubset;
                    }
                }
                else // actLen == maxLen
                {
                    // last item added to subset will be changed/removed
                    if ((actLen + i) < bitStingsCount) // next item to make a change is available
                    {
                        // change last item in subset
                        cache = resultsStack.Peek(); //TODO s touhle operaci by se dalo setrit
                        cache = cache.or(bitStrings[actLen + i]);
                        yield return cache;
                        goto nextSubset;
                    }
                    else // last item should be removed, next
                    {
                        // next subsets will be shorted.
                        maxLen--;
                        goto nextSubset;
                    }
                }

            }

        }

    }
    */
}