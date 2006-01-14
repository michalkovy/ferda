using System;
using Ferda.Modules.Boxes.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.DoubleFoundedImplication
{
	/// <summary>
	/// Double founded implication quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Defined as a condition a / (a + b + c) &gt;= p.</para>
	/// </remarks>
	class DoubleFoundedImplicationFunctionsI : AbstractFFTTaskQuantifierFunctionsWithParamsP
	{
		#region Functions
		/// <summary>
		/// Returns <c>true</c> iff the strength is greater than or equal to the strength parameter.
		/// </summary>
		/// <returns><c>true</c> iff the strength defined as <c>a / (a + b + c)</c> is greater than or equal to the strength parameter.</returns>
		/// <remarks>
		/// <para>If (a + b + c) = 0, returns false.</para>
		/// </remarks>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.DoubleFoundedImplicationValidity(P);
		}

		/// <summary>
		/// Computes the <c>strength</c>.
		/// </summary>
		/// <returns>The strength defined as  <c>a / (a + b + c)</c>.</returns>
		/// <remarks>
		/// <para>If (a + b + c) = 0, returns 0.</para>
		/// </remarks>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return FourFoldContingencyTable.DoubleFoundedImplicationValue(table);
		}
		#endregion
	}
}