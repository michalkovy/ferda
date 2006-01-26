using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.FoundedEquivalence
{
	/// <summary>
	/// Founded equivalence quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Defined as a condition (a + d) / (a + b + c + d) &gt;= p.</para>
	/// </remarks>
	class FoundedEquivalenceFunctionsI : AbstractFFTTaskQuantifierFunctionsWithParamsP
	{
		#region Functions
		/// <summary>
		/// Returns <c>true</c> iff the strength is greater than or equal to the strength parameter.
		/// </summary>
		/// <returns><c>true</c> iff the strength defined as <c>(a + d) / (a + b + c + d)</c> is greater than or equal to the strength parameter.</returns>
		/// <remarks>
		/// <para>If (a + b + c + d) = 0, returns false.</para>
		/// </remarks>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.FoundedEquivalenceValidity(P);
		}

		/// <summary>
		/// Computes the <c>strength</c> of founded equivalence.
		/// </summary>
		/// <returns>The strength defined as  <c>(a + d) / (a + b + c + d)</c>.</returns>
		/// <remarks>
		/// <para>If (a + b + c + d) = 0, returns 0.</para>
		/// </remarks>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return FourFoldContingencyTable.FoundedEquivalenceValue(table);
		}
		#endregion
	}
}