using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.E
{
	/// <summary>
	/// E-quantifier.
	/// </summary>
	/// <remarks>
	/// <para>(<c>a</c> + <c>d</c>) / (<c>a</c> + <c>b</c> + <c>c</c> + <c>d</c>) &gt;= p &amp; <c>a</c> &gt; Base.</para>
	/// </remarks>
	class EFunctionsI : AbstractFFTTaskQuantifierFunctionsWithParamsP2
	{
		#region Functions
		/// <summary>
		/// See [054 Redundance vystupu 4ftMiner.doc]
		/// Returns the validity of E-quantifier, i.e. true iff the (<c>a</c> + <c>d</c>) / (<c>a</c> + <c>b</c> + <c>c</c> + <c>d</c>) from 4ft-table is greater than or equal to the specified param <c>p</c>.
		/// </summary>
		/// <returns><c>true</c> iff the value of E-quantifier is greater than or equal to the specified param <c>p</c>.</returns>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.EValidity(P);
		}

		/// <summary>
		/// Returns value of expression (<c>a</c> + <c>d</c>) / (<c>a</c> + <c>b</c> + <c>c</c> + <c>d</c>) from 4ft-table.
		/// </summary>
		/// <returns>The value of expression (<c>a</c> + <c>d</c>) / (<c>a</c> + <c>b</c> + <c>c</c> + <c>d</c>) 4ft-table.</returns>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.EValue();
		}
		#endregion
	}
}