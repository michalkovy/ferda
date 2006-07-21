//#define CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining
{
    /// <summary>
    /// Represents a IceBox service for common boxes for data mining
    /// </summary>
    public class Service : FerdaServiceI
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
            // guha mining setting
            registerBox(AtomSetting.BoxInfo.typeIdentifier, new AtomSetting.BoxInfo());
            registerBox(ClassOfEquivalence.BoxInfo.typeIdentifier, new ClassOfEquivalence.BoxInfo());
            registerBox(ConjunctionSetting.BoxInfo.typeIdentifier, new ConjunctionSetting.BoxInfo());
            registerBox(DisjunctionSetting.BoxInfo.typeIdentifier, new DisjunctionSetting.BoxInfo());
            registerBox(FixedAtom.BoxInfo.typeIdentifier, new FixedAtom.BoxInfo());
            registerBox(Sign.BoxInfo.typeIdentifier, new Sign.BoxInfo());

            // guha task boxes
            registerBox(Tasks.FourFold.BoxInfo.typeIdentifier, new Tasks.FourFold.BoxInfo());
            registerBox(Tasks.SDFourFold.BoxInfo.typeIdentifier, new Tasks.SDFourFold.BoxInfo());

            registerBox(Tasks.SingleDimensional.BoxInfo.typeIdentifier, new Tasks.SingleDimensional.BoxInfo());
            registerBox(Tasks.SDSingleDimensional.BoxInfo.typeIdentifier, new Tasks.SDSingleDimensional.BoxInfo());

            registerBox(Tasks.TwoDimensional.BoxInfo.typeIdentifier, new Tasks.TwoDimensional.BoxInfo());
            registerBox(Tasks.SDTwoDimensional.BoxInfo.typeIdentifier, new Tasks.SDTwoDimensional.BoxInfo());

            // guha quantifiers boxes
            registerBox(GuhaMining.Quantifiers.FourFold.DoubleImplicational.DoubleFoundedImplication.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.FourFold.DoubleImplicational.DoubleFoundedImplication.BoxInfo());
            registerBox(GuhaMining.Quantifiers.FourFold.Equivalence.FoundedEquivalence.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.FourFold.Equivalence.FoundedEquivalence.BoxInfo());
            registerBox(GuhaMining.Quantifiers.FourFold.Implicational.FoundedImplication.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.FourFold.Implicational.FoundedImplication.BoxInfo());
            registerBox(GuhaMining.Quantifiers.FourFold.Others.AboveBelowAverageImplication.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.FourFold.Others.AboveBelowAverageImplication.BoxInfo());
            registerBox(GuhaMining.Quantifiers.FourFold.Others.Base.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.FourFold.Others.Base.BoxInfo());
            registerBox(GuhaMining.Quantifiers.FourFold.Others.E.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.FourFold.Others.E.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.AritmeticAverage.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.AritmeticAverage.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Asymetry.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Asymetry.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.GeometricAverage.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.GeometricAverage.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Skewness.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Skewness.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.StandardDeviation.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.StandardDeviation.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Variance.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Variance.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneDimensional.NominalVariableDistributionCharacteristics.NominalVariationNormalized.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneDimensional.NominalVariableDistributionCharacteristics.NominalVariationNormalized.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneDimensional.NominalVariableDistributionCharacteristics.VariationRatio.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneDimensional.NominalVariableDistributionCharacteristics.VariationRatio.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneDimensional.OrdinalVariableDistributionCharacteristics.DiscreteOrdinaryVariationNormalized.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneDimensional.OrdinalVariableDistributionCharacteristics.DiscreteOrdinaryVariationNormalized.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.AnyValue.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.AnyValue.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.Average.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.Average.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.Maximum.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.Maximum.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.Minimum.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.Minimum.BoxInfo());
            registerBox(GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.Sum.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.OneOrTwoDimensional.Aggregation.Sum.BoxInfo());
            registerBox(GuhaMining.Quantifiers.TwoDimensional.FunctionalDependence.FunctionOfRow.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.TwoDimensional.FunctionalDependence.FunctionOfRow.BoxInfo());
            registerBox(GuhaMining.Quantifiers.TwoDimensional.FunctionalDependence.FunctionOfRowEachRow.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.TwoDimensional.FunctionalDependence.FunctionOfRowEachRow.BoxInfo());
            registerBox(GuhaMining.Quantifiers.TwoDimensional.InformationTheory.ConditionalEntropy.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.TwoDimensional.InformationTheory.ConditionalEntropy.BoxInfo());
            registerBox(GuhaMining.Quantifiers.TwoDimensional.InformationTheory.InformationDependence.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.TwoDimensional.InformationTheory.InformationDependence.BoxInfo());
            registerBox(GuhaMining.Quantifiers.TwoDimensional.InformationTheory.MutualInformationNormalized.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.TwoDimensional.InformationTheory.MutualInformationNormalized.BoxInfo());
            registerBox(GuhaMining.Quantifiers.TwoDimensional.OrdinalDependence.Kendall.BoxInfo.typeIdentifier, new GuhaMining.Quantifiers.TwoDimensional.OrdinalDependence.Kendall.BoxInfo());
        }

        public override void AddCustomFactoriesToComunicator(Communicator communicator)
        {
            Ferda.Guha.MiningProcessor.ObjectFactory factory = new Ferda.Guha.MiningProcessor.ObjectFactory();
            Ferda.Guha.MiningProcessor.ObjectFactory.addFactoryToCommunicator(communicator, factory);
        }


        /// <summary>
        /// Says that this service has property boxes
        /// </summary>
        protected override bool havePropertyBoxes
        {
            get { return true; }
        }

#if CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
        private void registerBox(string factoryIdentifier, BoxInfo boxInfo)
        {
            BoxInfoLoadTesting.Test(boxInfo, new string[] { "en-US" });
        }

        public static void Main(string[] args)
        {
            Service service = new Service();
            service.registerBoxes();
        }
#endif
    }
}