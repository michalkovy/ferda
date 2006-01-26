using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Quantifiers;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.CriticalEquivalence
{
	/// <summary>
	/// Upper critical equivalence quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Upper critical equivalence quantifier is defined as a condition
	/// Sum[i = 0..a] n! / (i! * (n - i)!) * p^i * (1 - p)^(n - i) is in <c>relation</c> to alpha.</para>
	/// </remarks>
	class CriticalEquivalenceFunctionsI : AbstractFFTTaskQuantifierFunctionsWithParamsRelationAlphaP
	{
		#region Functions
		/// <summary>
		/// Returns <c>true</c> if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).
		/// </summary>
		/// <returns><c>true</c> if if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
		/// <remarks>
		/// <para>It computes the following condition:</para>
		/// <para>Sum[i = 0..a] n! / (i! * (n - i)!) * p^i * (1 - p)^(n - i) is in <c>relation</c> to alpha.</para>
		/// </remarks>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.CriticalEquivalenceValidity(P, Alpha, Relation);
		}

		/// <summary>
		/// Computes the statistical strength value at the specified significance (alpha).
		/// </summary>
		/// <returns>Statistical strength value at the specified significance (alpha).</returns>
		/// <remarks>
		/// <para>Computes the numerical solution of the following equation (for variable p):</para>
		/// <para><c>Sum[i = 0..a] n! / (i! * (n - i)!) * p^i * (1.0 - p)^(n - i) - alpha = 0.0</c>.</para>
		/// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
		/// </remarks>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.CriticalEquivalenceValue(Alpha);
		}
		#endregion
	}
}