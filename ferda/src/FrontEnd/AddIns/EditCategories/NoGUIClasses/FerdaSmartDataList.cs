using System;
using System.Text;
using System.Collections;
using Ferda;
using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;

namespace Ferda
{
    namespace FrontEnd.AddIns.EditCategories.NoGUIclasses
    {
        /// <summary>
        /// Class to operate with categories and their values.
        /// </summary>
        public class FerdaSmartDataList : System.Exception, DataStructureChange
        {
            #region Private variables



            /// <summary>

            /// List of multisets to operate with.

            /// </summary>

            private ArrayList multiSets = new ArrayList();



            public ArrayList MultiSets

            {

                get

                {

                    return this.multiSets;

                }

            }



            /// <summary>

            /// All the possible values for the category.

            /// </summary>

            private ArrayList allValues = new ArrayList();



            public ArrayList AllValues

            {

                get

                {

                    return this.allValues;

                }

                set

                {

                    this.allValues.Add(value);

                }

            }



            #endregion


            #region Constructor



            /// <summary>

            /// Constructor initializes the array of possible values

            /// and fills the array of multisets with existing ones.

            /// </summary>

            /// <param name="allValues">All of the values for current category</param>

            /// <param name="existingValues">Existing categories</param>

            public FerdaSmartDataList(ArrayList allValues, ArrayList existingValues)

            {

                existingValues.Sort();

                this.allValues = allValues;

                this.allValues.Sort();

                this.multiSets = existingValues;

            }

            #endregion


            public event FerdaEvent StructureChange;
            public void OnDataStructureChange()
            {
                if (StructureChange != null)
                {
                    StructureChange();
                }
            }


            /// <summary>
            /// Adds a new multiset.
            /// </summary>
            /// <param name="multiset">A multiset to add</param>
            /// <returns></returns>
            public bool AddNewMultiSetDirect(Category multiset)
            {
                this.multiSets.Add(multiset);
                this.OnDataStructureChange();
                return true;
            }


            /// <summary>
            /// Removes multiset at the given index
            /// </summary>
            /// <param name="index">Index as an integer</param>
            public void RemoveMultiSet(int index)
            {
                this.multiSets.RemoveAt(index);
                this.OnDataStructureChange();
            }

            /// <summary>
            /// Returns index for a searched multiset
            /// </summary>
            /// <param name="multiset">Searched multiset</param>
            /// <returns>Index of a searched multiset</returns>
            public int GetIndex(Category multiset)
            {
                return multiSets.IndexOf(multiset);
            }

            /// <summary>
            /// Finds out values that are not covered by sets or intervals.
            /// </summary>
            /// <returns>Arraylist of unused values</returns>
            public ArrayList GetAvailableValues()
            {
                ArrayList availableValues = new ArrayList();

                //in the beginning, all values are available
                availableValues.AddRange(this.allValues);

                foreach (object value in this.allValues)
                {
                    //we need to go through every multiset and find out used values
                    foreach (Category multiset in this.multiSets)
                    {
                        //if the value is contained in sets of a multiset, it is not available
                        if (multiset.IsInSet(value))
                        {
                            availableValues.Remove(value);
                        }


                        //if the value is contained in intervals of a multiset, it is not available

                        try

                        {

                            if (multiset.IsInInterval(Convert.ToDouble(value)))

                            {

                                availableValues.Remove(value);

                            }

                        }

                        catch

                        {

                            //nothing to do here, as the value is not double,

                            //so cannot be contained in any of the intervals

                            //they are implemented only for integers

                        }

                    }

                }

                return availableValues;

            }





            /// <summary>

            /// Checks whether the interval is disjunct with already existing intervals.

            /// </summary>

            /// <param name="interval">Interval to check</param>

            /// <returns>Whether the interval is disjunct</returns>

            public bool IntervalIsDisjunct(Interval interval)

            {

                foreach (Category multiSet in this.multiSets)

                {

                    if (multiSet.CatType == CategoryType.Interval)

                    {

                        if (!multiSet.IntervalIsDisjunct(interval))

                        {

                            return false;

                        }

                    }

                }

                return true;

            }



            /// <summary>

            /// Checks whether the interval is disjunct with values contained in all of the enums

            /// in the current SmartDataList.

            /// </summary>

            /// <param name="interval">Interval to check</param>

            /// <returns>Whether the interval is disjunct</returns>

            public bool IntervalDisjunctWithCurrentEnums(Interval interval)

            {

                foreach (Category multiSet in this.multiSets)

                {

                    if (multiSet.CatType == CategoryType.Enumeration)

                    {

                        if (!multiSet.IntervalDisjunctWithCurrentEnums(interval))

                        {

                            return false;

                        }

                    }

                }

                return true;

            }



            /// <summary>

            /// Method to join together selected multisets in the listview.

            /// </summary>

            /// <param name="categoriesIndices">Indices of selected categories.</param>

            public void JoinCategories(ArrayList categoriesIndices)

            {

                categoriesIndices.Sort();

                CategoryType firstType = new CategoryType();

                String newName;



                newName = "Join < ";

                if (categoriesIndices.Count > 0)

                {

                    firstType = this.GetMultiSet(Convert.ToInt32(categoriesIndices[0])).CatType;

                }



                else

                {

                    return;

                }



                Category newMultiSet = new Category();

                newMultiSet.CatType = firstType;



                bool firstRun = true;



                foreach (int index1 in categoriesIndices)

                {

                    //cannot join multisets of different types

                    if (this.GetMultiSet(index1).CatType != firstType)

                    {

                        return;

                    }



                    //if the category is of interval type, add all of the intervals to the new category

                    if (newMultiSet.CatType == CategoryType.Interval)

                    {

                        foreach (Interval tempInterval in this.GetMultiSet(index1).GetIntervals())

                        {

                            newMultiSet.AddInterval(tempInterval);

                        }

                    }



                        //add all of the enum values otherwise

                    else

                    {

                        newMultiSet.AddSingleSet(this.GetMultiSet(index1).Set);

                    }



                    if (firstRun)

                    {

                        newName = newName + this.GetMultiSet(index1).Name;

                        firstRun = false;

                    }

                    else

                    {

                        newName = newName + "; " + this.GetMultiSet(index1).Name;

                    }

                }



                newName = newName + " >";

                newMultiSet.Name = newName;





                int offset = 0;

                foreach (object index1 in categoriesIndices)

                {

                    int index = Convert.ToInt32(index1.ToString());

                    this.RemoveMultiSet(index - offset);

                    offset++;

                }

                this.AddNewMultiSetDirect(newMultiSet);

                this.OnDataStructureChange();

            }



            /// <summary>

            /// Gets the multiset from the FerdaDataList at the specified index.

            /// </summary>

            /// <param name="index">Index of the required multiset</param>

            /// <returns>A multiset at the requred index</returns>

            public Category GetMultiSet(int index)

            {

                if ((index >= 0) && (this.multiSets.Count > index))

                {

                    return (Category)this.multiSets.GetRange(index, 1)[0];

                }

                else

                {

                    return null;

                }

            }



            /// <summary>

            /// Method to set multiset name at the required index

            /// </summary>

            /// <param name="index">Index of the multiset being changed</param>

            public void SetName(int index,string name)

            {

                try

                {

                    Category tempSet = (Category)this.multiSets[index];

                    tempSet.Name = name;

                    this.OnDataStructureChange();

                }

                catch

                {

                }

            }



            /// <summary>

            /// Checks if the supplied array of multisets have the same type

            /// </summary>

            /// <param name="indexes">indexes of multisets</param>

            /// <returns></returns>

            public bool SameTypeMultiSets(int [] indexes)

            {

                CategoryType type = CategoryType.Interval;

                bool first = true;

                for (int i = 0; i < indexes.Length; i++)

                {

                    try

                    {

                        if (first)

                        {

                            //setting the type of the first multiset in the list

                            type = this.GetMultiSet(indexes[i]).CatType;

                            first = false;

                        }



                        else

                        {

                            //if any other multiset is of different type

                            if (type != this.GetMultiSet(indexes[i]).CatType)

                            {

                                return false;

                            }

                        }

                    }

                    catch

                    {

                        //other problem happened

                        return false;

                    }

                }

                //no problems, same type

                return true;

            }

        }

    }

}