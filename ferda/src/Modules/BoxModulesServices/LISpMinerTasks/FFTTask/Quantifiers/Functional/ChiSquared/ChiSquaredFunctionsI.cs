using System;
using Ferda.Modules.Boxes.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.ChiSquared
{
	/// <summary>
	/// Chi-square quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Chi-square quantifier is defined as a condition <c>(a * d) &gt; (b * c)  &amp;&amp;  n * (a * d - b * c) ^ 2 &gt;= chisq * x * s * k * l</c>,
	/// where chisq is (1-alpha) quantile of the chi-square distribution function (with 1 degree of freedom, because this is a four-fold contingency table).</para>
	/// <para>See chapter 4.4.23 in GUHA-book.</para>
	/// </remarks>
	class ChiSquaredFunctionsI : AbstractFFTTaskQuantifierFunctionsWithParamsAlpha
	{
		#region Functions
		/// <summary>
		/// The 4ft-quantifier chi-square is valid, if the null hypothesis of independence between antecedent and succedent is rejected,
		/// thus we say that antecedent and succedent are not independent.
		/// </summary>
		/// <returns><c>true</c> if the null hypothesis of independence is rejected.</returns>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.ChiSquareValidity(Alpha);
		}

		/// <summary>
		/// Returns the value of alpha that would be neccessary to reject a null hypothesis.
		/// </summary>
		/// <returns>The minimum value of alpha that is neccessary to reject a null hypothesis.</returns>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.ChiSquareValue();
		}
		#endregion
	}
}