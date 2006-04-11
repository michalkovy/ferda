// GeneratedAttribute.cs - class for holding results of [dynamic] attributes
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Tomáš Kuchař
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
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes
{
	/// <summary>
    /// Class that helps to hold (cache) useful information about 
    /// <see cref="T:Ferda.Modules.CategoriesStruct">categories</see>.
	/// </summary>
    public class GeneratedAttribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedAttribute"/> class.
        /// </summary>
        public GeneratedAttribute()
        {
            this.categoriesStruct = new CategoriesStruct();
            this.categoriesCount = 0;
            this.includeNullCategoryName = String.Empty;
            this.categoriesNames = new SelectString[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedAttribute"/> class.
        /// </summary>
        /// <param name="categoriesStruct">The categories struct.</param>
        /// <param name="includeNullCategoryName">Name of the include null category.</param>
        /// <param name="categoriesCount">The categories count.</param>
		public GeneratedAttribute(CategoriesStruct categoriesStruct, string includeNullCategoryName, long categoriesCount)
		{
			this.categoriesStruct = categoriesStruct;
			this.categoriesCount = categoriesCount;
			this.includeNullCategoryName = includeNullCategoryName;
			this.categoriesNames = Ferda.Modules.Boxes.BoxInfoHelper.StringArrayToSelectStringArray(
                Ferda.Modules.Helpers.Data.Attribute.GetCategoriesNames(
				    categoriesStruct,
				    Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeConstants.MaxLengthOfCategoriesNamesSelectStringArray
                    )
                );
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedAttribute"/> class.
        /// </summary>
        /// <param name="categoriesStruct">The categories struct.</param>
        /// <param name="includeNullCategoryName">Name of the include null category.</param>
		public GeneratedAttribute(CategoriesStruct categoriesStruct, string includeNullCategoryName)
		{
			this.categoriesStruct = categoriesStruct;
			this.categoriesCount = Ferda.Modules.Helpers.Data.Attribute.GetCategoriesCount(categoriesStruct);
			this.includeNullCategoryName = includeNullCategoryName;
			this.categoriesNames = Ferda.Modules.Boxes.BoxInfoHelper.StringArrayToSelectStringArray(
                Ferda.Modules.Helpers.Data.Attribute.GetCategoriesNames(
				    categoriesStruct,
				    Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeConstants.MaxLengthOfCategoriesNamesSelectStringArray
                    )
                );
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedAttribute"/> class.
        /// </summary>
        /// <param name="categoriesStruct">The categories struct.</param>
        /// <param name="includeNullCategoryName">Name of the include null category.</param>
        /// <param name="categoriesCount">The categories count.</param>
        /// <param name="categoriesNames">The categories names.</param>
		public GeneratedAttribute(CategoriesStruct categoriesStruct, string includeNullCategoryName, long categoriesCount, SelectString[] categoriesNames)
		{
			this.categoriesStruct = categoriesStruct;
			this.categoriesCount = categoriesCount;
			this.includeNullCategoryName = includeNullCategoryName;
			this.categoriesNames = categoriesNames;
		}

		private CategoriesStruct categoriesStruct;
        /// <summary>
        /// Gets or sets the categories struct.
        /// </summary>
        /// <value>The categories struct.</value>
		public CategoriesStruct CategoriesStruct
		{
			get { return categoriesStruct; }
			set { categoriesStruct = value; }
		}

		private long categoriesCount;
        /// <summary>
        /// Gets or sets the categories count.
        /// </summary>
        /// <value>The categories count.</value>
		public long CategoriesCount
		{
			get { return categoriesCount; }
			set { categoriesCount = value; }
		}
		
		private string includeNullCategoryName;
        /// <summary>
        /// Gets or sets the name of the include null category.
        /// </summary>
        /// <value>The name of the include null category.</value>
		public string IncludeNullCategoryName
		{
			get { return includeNullCategoryName; }
			set { includeNullCategoryName = value; }
		}

		private SelectString[] categoriesNames;
        /// <summary>
        /// Gets or sets the categories names.
        /// </summary>
        /// <value>The categories names.</value>
		public SelectString[] CategoriesNames
		{
			get { return categoriesNames; }
			set { categoriesNames = value; }
		}
	}
}
