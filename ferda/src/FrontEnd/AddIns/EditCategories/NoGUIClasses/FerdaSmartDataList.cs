// FerdaSmartDataList.cs - class for working with categories list
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Text;
using System.Collections;
using Ferda;
using Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses;

namespace Ferda.FrontEnd.AddIns.EditCategories.NoGUIclasses
{
    /// <summary>
    /// Class to operate with categories and their values.
    /// </summary>
    public class FerdaSmartDataList : System.Exception, DataStructureChange
    {
        #region Private variables

        /// <summary>
        /// List of categories to operate with.
        /// </summary>
        private ArrayList categories = new ArrayList();

        /// <summary>
        /// Categories getter
        /// </summary>
        public ArrayList Categories
        {
            get
            {
                return this.categories;
            }
        }

        /// <summary>
        /// All the possible values for the category.
        /// </summary>
        private ArrayList allValues = new ArrayList();

        /// <summary>
        /// All values getter / setter
        /// </summary>
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
        /// and fills the array of categories with existing ones.
        /// </summary>
        /// <param name="allValues">All of the values for current category</param>
        /// <param name="existingValues">Existing categories</param>
        public FerdaSmartDataList(ArrayList allValues, ArrayList existingValues)
        {
            existingValues.Sort();
            this.allValues = allValues;
            this.allValues.Sort();
            this.categories = existingValues;
        }

        #endregion


        #region Events

        public event FerdaEvent StructureChange;
        public void OnDataStructureChange()
        {
            if (StructureChange != null)
            {
                StructureChange();
            }
        }

        #endregion


        #region Categories methods

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="category">A category to add</param>
        /// <returns></returns>
        public bool AddNewCategoryDirect(Category category)
        {
            this.categories.Add(category);
            this.OnDataStructureChange();
            return true;
        }


        /// <summary>
        /// Removes category at the given index
        /// </summary>
        /// <param name="index">Index as an integer</param>
        public void RemoveCategory(int index)
        {
            this.categories.RemoveAt(index);
            this.OnDataStructureChange();
        }

        /// <summary>
        /// Returns index for a searched category
        /// </summary>
        /// <param name="category">Searched category</param>
        /// <returns>Index of a searched category</returns>
        public int GetIndex(Category category)
        {
            return categories.IndexOf(category);
        }

        #endregion


        #region Intervals methods

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
                foreach (Category multiset in this.categories)
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
                        //nothing to do here, as the value is unique,
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
        /// <returns>True if the interval is disjunct</returns>
        public bool IntervalIsDisjunct(Interval interval)
        {
            foreach (Category multiSet in this.categories)
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
            foreach (Category multiSet in this.categories)
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

        #endregion


        #region Other categories methods

        /// <summary>
        /// Method to join together selected categories in the listview.
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
                firstType = this.GetCategory(Convert.ToInt32(categoriesIndices[0])).CatType;
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
                if (this.GetCategory(index1).CatType != firstType)
                {
                    return;
                }

                //if the category is of interval type, add all of the intervals to the new category
                if (newMultiSet.CatType == CategoryType.Interval)
                {
                    foreach (Interval tempInterval in this.GetCategory(index1).GetIntervals())
                    {
                        newMultiSet.AddInterval(tempInterval);
                    }
                }
                   
                //add all of the enum values otherwise
                else
                {
                    newMultiSet.AddSingleSet(this.GetCategory(index1).Set);
                }
                if (firstRun)
                {
                    newName = newName + this.GetCategory(index1).Name;
                    firstRun = false;
                }
                else
                {
                    newName = newName + "; " + this.GetCategory(index1).Name;
                }
            }
            newName = newName + " >";
            newMultiSet.Name = newName;
            int offset = 0;
            foreach (object index1 in categoriesIndices)
            {
                int index = Convert.ToInt32(index1.ToString());
                this.RemoveCategory(index - offset);
                offset++;
            }
            this.AddNewCategoryDirect(newMultiSet);
            this.OnDataStructureChange();
        }


        /// <summary>
        /// Gets the category from the FerdaDataList at the specified index.
        /// </summary>
        /// <param name="index">Index of the required category</param>
        /// <returns>Category at the requred index</returns>
        public Category GetCategory(int index)
        {
            if ((index >= 0) && (this.categories.Count > index))
            {
                return (Category)this.categories.GetRange(index, 1)[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method to set category name at the required index
        /// </summary>
        /// <param name="index">Index of the category being changed</param>
        public void SetName(int index, string name)
        {
            try
            {
                Category tempSet = (Category)this.categories[index];
                tempSet.Name = name;
                this.OnDataStructureChange();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Checks if the supplied array of categories have the same type
        /// </summary>
        /// <param name="indexes">indexes of multisets</param>
        /// <returns></returns>
        public bool SameTypeCategories(int[] indexes)
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
                        type = this.GetCategory(indexes[i]).CatType;
                        first = false;
                    }
                    else
                    {
                        //if any other multiset is of different type
                        if (type != this.GetCategory(indexes[i]).CatType)
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

        #endregion
    }
}