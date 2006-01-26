using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Quantifiers;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.Fisher
{
	/// <summary>
	/// Fisher quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Fisher quantifier is a statistical test of independence between antecedent and succedent (null hypothesis) against positive dependence (alternative hypothesis) on the level alpha.</para>
	/// <para>It is defined as the condition <c>(a * d) &gt; (b * c)  &amp;  Sum[i = a..x] (x! * s! * k! * l!) / (n! * i! * (x-i)! * (k-i)! * (n+i-x-k)!) &lt;= alpha</c>, where <c>x = min(x, k)</c>.</para>
	/// <para>See chapter 4.4.20 in GUHA-book.</para>
	/// </remarks>
	class FisherFunctionsI : AbstractFFTTaskQuantifierFunctionsWithParamsAlpha
	{
		#region Functions
		/// <summary>
		/// Returns <c>true</c> if the antecedent and succedent are positively associated (in terms of Fisher quantifier).
		/// </summary>
		/// <returns><c>true</c> iff <c>(a * d) &gt; (b * c)</c> and <c>Sum[i = a..x] (x! * s! * k! * l!) / (n! * i! * (x-i)! * (k-i)! * (n+i-x-k)!) &lt;= alpha</c>, where <c>x = min(x, k)</c>.</returns>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.FisherValidity(Alpha);
		}

		/// <summary>
		/// Computes the value of Fisher quantifier.
		/// </summary>
		/// <returns>The value <c>Sum[i = a..x] (x! * s! * k! * l!) / (n! * i! * (x-i)! * (k-i)! * (n+i-x-k)!)</c>, where <c>x = min(x, k)</c>.</returns>
		/// <remarks>
		/// <para>There is a special case defined explicitly:</para>
		/// <para>If <c>(a * d) &lt;= (b * c)</c>, return 0.</para>
		/// </remarks>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.FisherValue();
		}
		#endregion
	}
}