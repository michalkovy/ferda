using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.LISpMinerTasks
{
	/// <summary>
	/// TODO Michal
	/// </summary>
    public class Service : Ferda.Modules.FerdaServiceI
	{

		/// <summary>
		/// Register box modules to Ice.ObjectAdapter.
		/// </summary>
		/// <remarks>
		/// Remember if you are adding registering of new box module,
		/// you must also change application.xml filePath in config directory.
		/// </remarks>
		protected override void registerBoxes()
		{
			//FFT
            this.registerBox("FFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.FFTTaskBoxInfo());
			this.registerBox("AboveAverageImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.AboveAverageImplication.AboveAverageImplicationBoxInfo());
			this.registerBox("BaseCeilFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.BaseCeil.BaseCeilBoxInfo());
			this.registerBox("BelowAverageImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.BelowAverageImplication.BelowAverageImplicationBoxInfo());
			this.registerBox("ChiSquaredFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.ChiSquared.ChiSquaredBoxInfo());
			this.registerBox("DoubleFoundedImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.DoubleFoundedImplication.DoubleFoundedImplicationBoxInfo());
			this.registerBox("DoubleCriticalImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.DoubleCriticalImplication.DoubleCriticalImplicationBoxInfo());
			this.registerBox("FisherFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.Fisher.FisherBoxInfo());
			this.registerBox("FoundedEquivalenceFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.FoundedEquivalence.FoundedEquivalenceBoxInfo());
			this.registerBox("FoundedImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.FoundedImplication.FoundedImplicationBoxInfo());
			this.registerBox("SimpleDeviationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.SimpleDeviation.SimpleDeviationBoxInfo());
			this.registerBox("CriticalEquivalenceFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.CriticalEquivalence.CriticalEquivalenceBoxInfo());
			this.registerBox("CriticalImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.CriticalImplication.CriticalImplicationBoxInfo());
            this.registerBox("EFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.E.EBoxInfo());

			//SDFFT
			this.registerBox("SDFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.SDFFTTask.SDFFTTaskBoxInfo());
			this.registerBox("AboveAverageImplicationFunctionalQuantifiersSDFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Functional.AboveAverageImplication.AboveAverageImplicationBoxInfo());
			this.registerBox("DoubleFoundedImplicationFunctionalQuantifiersSDFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Functional.DoubleFoundedImplication.DoubleFoundedImplicationBoxInfo());
			this.registerBox("FoundedEquivalenceFunctionalQuantifiersSDFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Functional.FoundedEquivalence.FoundedEquivalenceBoxInfo());
			this.registerBox("FoundedImplicationFunctionalQuantifiersSDFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Functional.FoundedImplication.FoundedImplicationBoxInfo());
			this.registerBox("BaseCeilAggregationQuantifiersSDFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Aggregation.BaseCeil.BaseCeilBoxInfo());
			this.registerBox("MaxValueAggregationQuantifiersSDFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo());
			this.registerBox("MinValueAggregationQuantifiersSDFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo());
			this.registerBox("SumOfValuesAggregationQuantifiersSDFFTTaskFactoryCreator", new Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo());

			//KL
			this.registerBox("KLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.KLTaskBoxInfo());
			this.registerBox("FunctionSumOfRowsFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.FunctionSumOfRows.FunctionSumOfRowsBoxInfo());
			this.registerBox("FunctionEachRowFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.FunctionEachRow.FunctionEachRowBoxInfo());
			//this.registerBox("ChiSquareTestFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.ChiSquareTest.ChiSquareTestBoxInfo());
			this.registerBox("ConditionalEntropyFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.ConditionalEntropy.ConditionalEntropyBoxInfo());
			this.registerBox("MutualInformationNormalizedFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.MutualInformationNormalized.MutualInformationNormalizedBoxInfo());
			this.registerBox("InformationDependencyFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.InformationDependency.InformationDependencyBoxInfo());
			this.registerBox("KendalFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.Kendal.KendalBoxInfo());
			this.registerBox("SumOfValuesAggregationQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo());
			this.registerBox("MinValueAggregationQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo());
			this.registerBox("MaxValueAggregationQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo());
			this.registerBox("AverageValueAggregationQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo());
			this.registerBox("AnyValueAggregationQuantifiersKLTaskFactoryCreator", new Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo());

			//SDKL
			this.registerBox("SDKLTaskFactoryCreator", new Boxes.LISpMinerTasks.SDKLTask.SDKLTaskBoxInfo());
			this.registerBox("SumOfValuesAggregationQuantifiersSDKLTaskFactoryCreator", new Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo());
			this.registerBox("MinValueAggregationQuantifiersSDKLTaskFactoryCreator", new Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo());
			this.registerBox("MaxValueAggregationQuantifiersSDKLTaskFactoryCreator", new Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo());
			this.registerBox("AverageValueAggregationQuantifiersSDKLTaskFactoryCreator", new Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo());
			this.registerBox("AnyValueAggregationQuantifiersSDKLTaskFactoryCreator", new Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo());

			//CF
            this.registerBox("CFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.CFTaskBoxInfo());
			this.registerBox("AnyValueAggregationQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo());
			this.registerBox("AverageValueAggregationQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo());
			this.registerBox("MaxValueAggregationQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo());
			this.registerBox("MinValueAggregationQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo());
			this.registerBox("SumOfValuesAggregationQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo());
			this.registerBox("ArithmeticAverageFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.ArithmeticAverage.ArithmeticAverageBoxInfo());
			this.registerBox("AsymetryFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.Asymetry.AsymetryBoxInfo());
			this.registerBox("DiscreteOrdinaryVariationFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.DiscreteOrdinaryVariation.DiscreteOrdinaryVariationBoxInfo());
			this.registerBox("GeometricAverageFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.GeometricAverage.GeometricAverageBoxInfo());
			this.registerBox("NominalVariationFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.NominalVariation.NominalVariationBoxInfo());
			this.registerBox("SkewnessFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.Skewness.SkewnessBoxInfo());
			this.registerBox("StandardDeviationFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.StandardDeviation.StandardDeviationBoxInfo());
			this.registerBox("VarianceFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.Variance.VarianceBoxInfo());
			this.registerBox("VariationRatioFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.VariationRatio.VariationRatioBoxInfo());

			//SDCF
            this.registerBox("SDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.SDCFTaskBoxInfo());
			this.registerBox("AnyValueAggregationQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo());
			this.registerBox("AverageValueAggregationQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo());
			this.registerBox("MaxValueAggregationQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo());
			this.registerBox("MinValueAggregationQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo());
			this.registerBox("SumOfValuesAggregationQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo());
			this.registerBox("ArithmeticAverageFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.ArithmeticAverage.ArithmeticAverageBoxInfo());
			this.registerBox("AsymetryFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.Asymetry.AsymetryBoxInfo());
			this.registerBox("DiscreteOrdinaryVariationFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.DiscreteOrdinaryVariation.DiscreteOrdinaryVariationBoxInfo());
			this.registerBox("GeometricAverageFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.GeometricAverage.GeometricAverageBoxInfo());
			this.registerBox("NominalVariationFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.NominalVariation.NominalVariationBoxInfo());
			this.registerBox("SkewnessFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.Skewness.SkewnessBoxInfo());
			this.registerBox("StandardDeviationFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.StandardDeviation.StandardDeviationBoxInfo());
			this.registerBox("VarianceFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.Variance.VarianceBoxInfo());
			this.registerBox("VariationRatioFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.VariationRatio.VariationRatioBoxInfo());

			//this.registerBox("...FactoryCreator", new Boxes.LISpMinerTasks...BoxInfo());
		}
	}
}
