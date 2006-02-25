using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes
{
	/// <summary>
	/// Some constant for usage by attribute box modules.
	/// </summary>
    public static class AbstractAttributeConstants
	{
		private static int maxLengthOfCategoriesNamesSelectStringArray = 255;
		/// <summary>
		/// Maximal number of values for categories names SelectString array
		/// </summary>
		public static int MaxLengthOfCategoriesNamesSelectStringArray
		{
			get { return AbstractAttributeConstants.maxLengthOfCategoriesNamesSelectStringArray; }
		}
	}
}
