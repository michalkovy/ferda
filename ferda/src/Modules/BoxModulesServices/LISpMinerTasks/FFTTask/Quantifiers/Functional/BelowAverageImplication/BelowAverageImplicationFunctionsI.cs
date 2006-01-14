using System;
using Ferda.Modules.Boxes.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.BelowAverageImplication
{
	/// <summary>
	/// Below average quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Defined as a condition <c>a / (a + b) &lt;= (1 / k) * (a + c) / (a + b + c + d)</c>.</para>
	/// </remarks>
	class BelowAverageImplicationFunctionsI : AbstractFFTTaskQuantifierFunctionsWithParamsK
	{
		#region Functions
		/// <summary>
		/// Returns <c>true</c> if the below average strength is greater than or equal to the strength parameter.
		/// </summary>
		/// <returns><c>true</c> if the strength defined as <c>((a + b) / a) * ((a + c) / (a + b + c + d))</c> is greater than or equal to the strength parameter.</returns>
		/// <remarks>
		/// <para>If <c>(a + c) = 0</c>, return false.</para>
		/// <para>If <c>a = 0</c>, returns true.</para>
		/// </remarks>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.BelowAverageImplicationValidity(K);
		}

		/// <summary>
		/// Computes the below average strength value.
		/// </summary>
		/// <returns>Below average strength value defined as <c>((a + b) / a) * ((a + c) / (a + b + c + d))</c> with two exceptions: returns zero if <c>(a + c) = 0</c> and returns +INF if <c>a = 0</c>.</returns>
		/// <remarks>
		/// <para>The below average quantifier value must be explicitly defined for <c>a = 0</c> as +INF to prevent division by zero.</para>
		/// <para>Furthermore, if <c>(a + c) = 0</c>, the sumOfRowMax +INF (= extremely strong hypothesis) would not make sense, so it is defined as zero (= no hypothesis) instead.</para>
		/// </remarks>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.BelowAverageImplicationValue();
		}
		#endregion
	}
}