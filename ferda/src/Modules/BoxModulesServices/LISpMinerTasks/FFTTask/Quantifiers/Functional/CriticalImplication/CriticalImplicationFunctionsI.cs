using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Quantifiers;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.CriticalImplication
{
	/// <summary>
	/// Upper critical implication quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Upper critical implication quantifier is defined as a condition Sum[i = 0..a] x! / (i! * (x - i)!) * p^i * (1 - p)^(x - i) is in <c>relation</c> to alpha.</para>
	/// <para>See chapter 4.4.9 and 4.4.12 in GUHA-book (suspicious p-implication quantifier).</para>
	/// </remarks>
	class CriticalImplicationFunctionsI : AbstractFFTTaskQuantifierFunctionsWithParamsRelationAlphaP
	{
		#region Functions
		/// <summary>
		/// Returns <c>true</c> if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).
		/// </summary>
		/// <returns><c>true</c> if if the statistical confidence value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
		/// <remarks>
		/// <para>It computes the following condition:</para>
		/// <para>Sum[i = 0..a] x! / (i! * (x - i)!) * p^i * (1 - p)^(x - i) is in <c>relation</c> to alpha.</para>
		/// </remarks>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.CriticalImplicationValidity(P, Alpha, Relation);
		}

		/// <summary>
		/// Computes the statistical strength value at the specified significance (alpha).
		/// </summary>
		/// <returns>Statistical strength value at the specified significance (alpha).</returns>
		/// <remarks>
		/// <para>Computes the numerical solution of the following equation (for variable p):</para>
		/// <para><c>Sum[i = 0..a] x! / (i! * (x - i)!) * p^i * (1.0 - p)^(x - i) - alpha = 0.0</c></para>
		/// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
		/// </remarks>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.CriticalImplicationValue(Alpha);
		}
		#endregion
	}
}