using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.AboveAverageImplication
{
	/// <summary>
	/// Above average quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Defined as a condition <c>a / (a + b) &gt;= k * (a + c) / (a + b + c + d)</c>.</para>
	/// </remarks>
	class AboveAverageImplicationFunctionsI : AbstractFFTTaskQuantifierFunctionsWithParamsK
	{
		#region Functions
        /// <summary>
        /// Returns <c>true</c> if the above average strength is greater than or equal to the strength parameter.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns>
        /// 	<c>true</c> if <c>a &gt; 0</c> and the strength defined as <c>(a / (a + b)) * ((a + b + c + d) / (a + c))</c> is greater than or equal to the strength parameter.
        /// </returns>
        /// <remarks>If a = 0, returns false.</remarks>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.AboveAverageImplicationValidity(K);
		}

		/// <summary>
		/// Computes the above average strength value.
		/// </summary>
		/// <returns>Above average strength value defined as <c>(a / (a + b)) * ((a + b + c + d) / (a + c))</c> if <c>a &gt; 0</c>; otherwise it returns zero.</returns>
		/// <remarks>
		/// <para>If a = 0, returns 0.</para>
		/// </remarks>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			 return FourFoldContingencyTable.AboveAverageImplicationValue(table);
		}
		#endregion
	}
}