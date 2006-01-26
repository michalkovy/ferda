using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.SimpleDeviation
{
	/// <summary>
	/// Simple deviation quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Simple deviation quantifier is defined as the condition <c>(a * d) >= 2^k * (b * c)</c>, where <c>k</c> is a strength parameter.</para>
	/// <para>See chapter 2.2.4 (e) in GUHA-book (simple association, which is a simpler form without parameter).</para>
	/// <para>This quantifier behaves strangely when there are some zeros in 4ft-table. For example, for the table (a = 1000, b = 1, c = 1, d = 0),
	/// we can see strong association between antecedent and succedent and thus strong deviation from their independency; having d = 0 means that the quantifier fails completely.
	/// Similarly, very small numbers in contrast with large numbers in one 4ft-table are not quite testifying as well. The best case is when all four values are "in balance".</para>
	/// </remarks>
	class SimpleDeviationFunctionsI : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
		protected internal float K
		{
			get
			{
				return this.boxModule.GetPropertyFloat("ParamK");
			}
		}
		#endregion

		#region Functions
		/// <summary>
		/// Returns <c>true</c> if the simple deviation strength is greater than or equal to the strength parameter.
		/// </summary>
		/// <returns><c>true</c> iff <c>(a * d) &gt;= 2^k * (b * c)</c>.</returns>
		/// <remarks>
		/// <para>There are special cases defined explicitly:</para>
		/// <para>If both <c>(a * d) = 0</c> and <c>(b * c) = 0</c>, return true if <c>k &lt;= 0</c>.</para>
		/// <para>If only <c>(a * d) = 0</c>, return <c>false</c>.</para>
		/// <para>If only <c>(b * c) = 0</c>, return <c>true</c>.</para>
		/// </remarks>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.SimpleDeviationValidity(K);
		}

		/// <summary>
		/// Computes the simple deviation strength value.
		/// </summary>
		/// <returns>Simple deviation strength value defined as <c>ln(ad/bc) / ln(2)</c>.</returns>
		/// <remarks>
		/// <para>There are special cases defined explicitly:</para>
		/// <para>If both <c>(a * d) = 0</c> and <c>(b * c) = 0</c>, return 0.</para>
		/// <para>If only <c>(a * d) = 0</c>, return -INF.</para>
		/// <para>If only <c>(b * c) = 0</c>, return +INF.</para>
		/// </remarks>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			FourFoldContingencyTable table = new FourFoldContingencyTable(setting.firstContingencyTableRows);
			return table.SimpleDeviationValue();
		}
		#endregion
	}
}