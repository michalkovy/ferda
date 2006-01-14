using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.LMTasks
{
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
			this.registerBox("FFTTaskFactoryCreator", new Boxes.FFTTask.FFTTaskBoxInfo());
			this.registerBox("AboveAverageImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.AboveAverageImplication.AboveAverageImplicationBoxInfo());
			this.registerBox("BaseCeilFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.BaseCeil.BaseCeilBoxInfo());
			this.registerBox("BelowAverageImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.BelowAverageImplication.BelowAverageImplicationBoxInfo());
			this.registerBox("ChiSquaredFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.ChiSquared.ChiSquaredBoxInfo());
			this.registerBox("DoubleFoundedImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.DoubleFoundedImplication.DoubleFoundedImplicationBoxInfo());
			this.registerBox("DoubleCriticalImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.DoubleCriticalImplication.DoubleCriticalImplicationBoxInfo());
			this.registerBox("FisherFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.Fisher.FisherBoxInfo());
			this.registerBox("FoundedEquivalenceFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.FoundedEquivalence.FoundedEquivalenceBoxInfo());
			this.registerBox("FoundedImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.FoundedImplication.FoundedImplicationBoxInfo());
			this.registerBox("SimpleDeviationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.SimpleDeviation.SimpleDeviationBoxInfo());
			this.registerBox("CriticalEquivalenceFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.CriticalEquivalence.CriticalEquivalenceBoxInfo());
			this.registerBox("CriticalImplicationFunctionalQuantifiersFFTTaskFactoryCreator", new Boxes.FFTTask.Quantifiers.Functional.CriticalImplication.CriticalImplicationBoxInfo());

			//SDFFT
			this.registerBox("SDFFTTaskFactoryCreator", new Boxes.SDFFTTask.SDFFTTaskBoxInfo());
			this.registerBox("AboveAverageImplicationFunctionalQuantifiersSDFFTTaskFactoryCreator", new Boxes.SDFFTTask.Quantifiers.Functional.AboveAverageImplication.AboveAverageImplicationBoxInfo());
			this.registerBox("DoubleFoundedImplicationFunctionalQuantifiersSDFFTTaskFactoryCreator", new Boxes.SDFFTTask.Quantifiers.Functional.DoubleFoundedImplication.DoubleFoundedImplicationBoxInfo());
			this.registerBox("FoundedEquivalenceFunctionalQuantifiersSDFFTTaskFactoryCreator", new Boxes.SDFFTTask.Quantifiers.Functional.FoundedEquivalence.FoundedEquivalenceBoxInfo());
			this.registerBox("FoundedImplicationFunctionalQuantifiersSDFFTTaskFactoryCreator", new Boxes.SDFFTTask.Quantifiers.Functional.FoundedImplication.FoundedImplicationBoxInfo());
			this.registerBox("BaseCeilAggregationQuantifiersSDFFTTaskFactoryCreator", new Boxes.SDFFTTask.Quantifiers.Aggregation.BaseCeil.BaseCeilBoxInfo());
			this.registerBox("MaxValueAggregationQuantifiersSDFFTTaskFactoryCreator", new Boxes.SDFFTTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo());
			this.registerBox("MinValueAggregationQuantifiersSDFFTTaskFactoryCreator", new Boxes.SDFFTTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo());
			this.registerBox("SumOfValuesAggregationQuantifiersSDFFTTaskFactoryCreator", new Boxes.SDFFTTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo());

			//KL
			this.registerBox("KLTaskFactoryCreator", new Boxes.KLTask.KLTaskBoxInfo());
			this.registerBox("FunctionSumOfRowsFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Functional.FunctionSumOfRows.FunctionSumOfRowsBoxInfo());
			this.registerBox("FunctionEachRowFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Functional.FunctionEachRow.FunctionEachRowBoxInfo());
			//this.registerBox("ChiSquareTestFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Functional.ChiSquareTest.ChiSquareTestBoxInfo());
			this.registerBox("ConditionalEntropyFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Functional.ConditionalEntropy.ConditionalEntropyBoxInfo());
			this.registerBox("MutualInformationNormalizedFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Functional.MutualInformationNormalized.MutualInformationNormalizedBoxInfo());
			this.registerBox("InformationDependencyFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Functional.InformationDependency.InformationDependencyBoxInfo());
			this.registerBox("KendalFunctionalQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Functional.Kendal.KendalBoxInfo());
			this.registerBox("SumOfValuesAggregationQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo());
			this.registerBox("MinValueAggregationQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo());
			this.registerBox("MaxValueAggregationQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo());
			this.registerBox("AverageValueAggregationQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo());
			this.registerBox("AnyValueAggregationQuantifiersKLTaskFactoryCreator", new Boxes.KLTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo());

			//SDKL
			this.registerBox("SDKLTaskFactoryCreator", new Boxes.SDKLTask.SDKLTaskBoxInfo());
			this.registerBox("SumOfValuesAggregationQuantifiersSDKLTaskFactoryCreator", new Boxes.SDKLTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo());
			this.registerBox("MinValueAggregationQuantifiersSDKLTaskFactoryCreator", new Boxes.SDKLTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo());
			this.registerBox("MaxValueAggregationQuantifiersSDKLTaskFactoryCreator", new Boxes.SDKLTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo());
			this.registerBox("AverageValueAggregationQuantifiersSDKLTaskFactoryCreator", new Boxes.SDKLTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo());
			this.registerBox("AnyValueAggregationQuantifiersSDKLTaskFactoryCreator", new Boxes.SDKLTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo());

			//CF
            this.registerBox("CFTaskFactoryCreator", new Boxes.CFTask.CFTaskBoxInfo());
			this.registerBox("AnyValueAggregationQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo());
			this.registerBox("AverageValueAggregationQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo());
			this.registerBox("MaxValueAggregationQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo());
			this.registerBox("MinValueAggregationQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo());
			this.registerBox("SumOfValuesAggregationQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo());
			this.registerBox("ArithmeticAverageFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Functional.ArithmeticAverage.ArithmeticAverageBoxInfo());
			this.registerBox("AsymetryFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Functional.Asymetry.AsymetryBoxInfo());
			this.registerBox("DiscreteOrdinaryVariationFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Functional.DiscreteOrdinaryVariation.DiscreteOrdinaryVariationBoxInfo());
			this.registerBox("GeometricAverageFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Functional.GeometricAverage.GeometricAverageBoxInfo());
			this.registerBox("NominalVariationFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Functional.NominalVariation.NominalVariationBoxInfo());
			this.registerBox("SkewnessFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Functional.Skewness.SkewnessBoxInfo());
			this.registerBox("StandardDeviationFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Functional.StandardDeviation.StandardDeviationBoxInfo());
			this.registerBox("VarianceFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Functional.Variance.VarianceBoxInfo());
			this.registerBox("VariationRatioFunctionalQuantifiersCFTaskFactoryCreator", new Boxes.CFTask.Quantifiers.Functional.VariationRatio.VariationRatioBoxInfo());

			//SDCF
            this.registerBox("SDCFTaskFactoryCreator", new Boxes.SDCFTask.SDCFTaskBoxInfo());
			this.registerBox("AnyValueAggregationQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo());
			this.registerBox("AverageValueAggregationQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo());
			this.registerBox("MaxValueAggregationQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo());
			this.registerBox("MinValueAggregationQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo());
			this.registerBox("SumOfValuesAggregationQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo());
			this.registerBox("ArithmeticAverageFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Functional.ArithmeticAverage.ArithmeticAverageBoxInfo());
			this.registerBox("AsymetryFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Functional.Asymetry.AsymetryBoxInfo());
			this.registerBox("DiscreteOrdinaryVariationFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Functional.DiscreteOrdinaryVariation.DiscreteOrdinaryVariationBoxInfo());
			this.registerBox("GeometricAverageFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Functional.GeometricAverage.GeometricAverageBoxInfo());
			this.registerBox("NominalVariationFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Functional.NominalVariation.NominalVariationBoxInfo());
			this.registerBox("SkewnessFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Functional.Skewness.SkewnessBoxInfo());
			this.registerBox("StandardDeviationFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Functional.StandardDeviation.StandardDeviationBoxInfo());
			this.registerBox("VarianceFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Functional.Variance.VarianceBoxInfo());
			this.registerBox("VariationRatioFunctionalQuantifiersSDCFTaskFactoryCreator", new Boxes.SDCFTask.Quantifiers.Functional.VariationRatio.VariationRatioBoxInfo());

			//this.registerBox("...FactoryCreator", new Boxes...BoxInfo());
		}
	}
}
