using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.FoundedImplication
{
	/// <summary>
	/// Founded implication quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Defined as a condition a / (a + b) &gt;= p.</para>
	/// </remarks>
	class FoundedImplicationFunctionsI : AbstractFFTTaskQuantifierFunctionsWithParamsP
	{
		#region Functions
		/// <summary>
		/// Returns <c>true</c> iff the <c>confidence</c> is greater than or equal to the confidence parameter.
		/// </summary>
		/// <returns><c>true</c> iff the <c>confidence</c> defined as <c>a / (a + b)</c> is greater than or equal to the confidence parameter.</returns>
		/// <remarks>
		/// <para>If (a + b) = 0, returns false.</para>
		/// </remarks>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.FoundedImplicationValidity(P);
		}

		/// <summary>
		/// Computes the <c>confidence</c>.
		/// </summary>
		/// <returns>The confidence defined as  <c>a / (a + b)</c>.</returns>
		/// <remarks>
		/// <para>If (a + b) = 0, returns 0.</para>
		/// </remarks>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return FourFoldContingencyTable.FoundedImplicationValue(table);
		}
		#endregion
	}
}