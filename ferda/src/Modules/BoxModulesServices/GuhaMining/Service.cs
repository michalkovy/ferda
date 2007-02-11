//#define CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
using Ice;
using ObjectFactory=Ferda.Guha.MiningProcessor.ObjectFactory;

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
            registerBox(VirtualAttributes.VirtualFFTBooleanAttribute.BoxInfo.typeIdentifier, new VirtualAttributes.VirtualFFTBooleanAttribute.BoxInfo());
          //  registerBox(VirtualAttributes.VirtualSDFFTBooleanAttribute.BoxInfo.typeIdentifier, new VirtualAttributes.VirtualSDFFTBooleanAttribute.BoxInfo());

            // guha task boxes
            registerBox(Tasks.FourFold.BoxInfo.typeIdentifier, new Tasks.FourFold.BoxInfo());
            registerBox(Tasks.SDFourFold.BoxInfo.typeIdentifier, new Tasks.SDFourFold.BoxInfo());

            registerBox(Tasks.SingleDimensional.BoxInfo.typeIdentifier, new Tasks.SingleDimensional.BoxInfo());
            registerBox(Tasks.SDSingleDimensional.BoxInfo.typeIdentifier, new Tasks.SDSingleDimensional.BoxInfo());

            registerBox(Tasks.TwoDimensional.BoxInfo.typeIdentifier, new Tasks.TwoDimensional.BoxInfo());
            registerBox(Tasks.SDTwoDimensional.BoxInfo.typeIdentifier, new Tasks.SDTwoDimensional.BoxInfo());

            // guha quantifiers boxes
            registerBox(Quantifiers.FourFold.DoubleImplicational.DoubleFoundedImplication.BoxInfo.typeIdentifier,
                        new Quantifiers.FourFold.DoubleImplicational.DoubleFoundedImplication.BoxInfo());
            registerBox(Quantifiers.FourFold.Equivalence.FoundedEquivalence.BoxInfo.typeIdentifier,
                        new Quantifiers.FourFold.Equivalence.FoundedEquivalence.BoxInfo());
            registerBox(Quantifiers.FourFold.Equivalence.AboveNegation.BoxInfo.typeIdentifier,
                        new Quantifiers.FourFold.Equivalence.AboveNegation.BoxInfo());
            registerBox(Quantifiers.FourFold.Implicational.FoundedImplication.BoxInfo.typeIdentifier,
                        new Quantifiers.FourFold.Implicational.FoundedImplication.BoxInfo());
            registerBox(Quantifiers.FourFold.Others.AboveAverageDependence.BoxInfo.typeIdentifier,
                        new Quantifiers.FourFold.Others.AboveAverageDependence.BoxInfo());
            registerBox(Quantifiers.FourFold.Others.BelowAverageDependence.BoxInfo.typeIdentifier,
                        new Quantifiers.FourFold.Others.BelowAverageDependence.BoxInfo());
            registerBox(Quantifiers.FourFold.Others.Pairing.BoxInfo.typeIdentifier,
                        new Quantifiers.FourFold.Others.Pairing.BoxInfo());
            registerBox(Quantifiers.FourFold.Others.Base.BoxInfo.typeIdentifier,
                        new Quantifiers.FourFold.Others.Base.BoxInfo());
            registerBox(Quantifiers.FourFold.Others.E.BoxInfo.typeIdentifier,
                        new Quantifiers.FourFold.Others.E.BoxInfo());
            registerBox(
                Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.AritmeticAverage.BoxInfo.
                    typeIdentifier,
                new Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.AritmeticAverage.BoxInfo());
            registerBox(
                Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Asymetry.BoxInfo.typeIdentifier,
                new Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Asymetry.BoxInfo());
            registerBox(
                Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.GeometricAverage.BoxInfo.
                    typeIdentifier,
                new Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.GeometricAverage.BoxInfo());
            registerBox(
                Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Skewness.BoxInfo.typeIdentifier,
                new Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Skewness.BoxInfo());
            registerBox(
                Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.StandardDeviation.BoxInfo.
                    typeIdentifier,
                new Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.StandardDeviation.BoxInfo());
            registerBox(
                Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Variance.BoxInfo.typeIdentifier,
                new Quantifiers.OneDimensional.CardinalVariableDistributionCharacteristics.Variance.BoxInfo());
            registerBox(
                Quantifiers.OneDimensional.NominalVariableDistributionCharacteristics.NominalVariationNormalized.BoxInfo
                    .typeIdentifier,
                new
                    Quantifiers.OneDimensional.NominalVariableDistributionCharacteristics.NominalVariationNormalized.
                    BoxInfo());
            registerBox(
                Quantifiers.OneDimensional.NominalVariableDistributionCharacteristics.VariationRatio.BoxInfo.
                    typeIdentifier,
                new Quantifiers.OneDimensional.NominalVariableDistributionCharacteristics.VariationRatio.BoxInfo());
            registerBox(
                Quantifiers.OneDimensional.OrdinalVariableDistributionCharacteristics.
                    DiscreteOrdinaryVariationNormalized.BoxInfo.typeIdentifier,
                new
                    Quantifiers.OneDimensional.OrdinalVariableDistributionCharacteristics.
                    DiscreteOrdinaryVariationNormalized.BoxInfo());
            registerBox(Quantifiers.OneOrTwoDimensional.Aggregation.AnyValue.BoxInfo.typeIdentifier,
                        new Quantifiers.OneOrTwoDimensional.Aggregation.AnyValue.BoxInfo());
            registerBox(Quantifiers.OneOrTwoDimensional.Aggregation.Average.BoxInfo.typeIdentifier,
                        new Quantifiers.OneOrTwoDimensional.Aggregation.Average.BoxInfo());
            registerBox(Quantifiers.OneOrTwoDimensional.Aggregation.Maximum.BoxInfo.typeIdentifier,
                        new Quantifiers.OneOrTwoDimensional.Aggregation.Maximum.BoxInfo());
            registerBox(Quantifiers.OneOrTwoDimensional.Aggregation.Minimum.BoxInfo.typeIdentifier,
                        new Quantifiers.OneOrTwoDimensional.Aggregation.Minimum.BoxInfo());
            registerBox(Quantifiers.OneOrTwoDimensional.Aggregation.Sum.BoxInfo.typeIdentifier,
                        new Quantifiers.OneOrTwoDimensional.Aggregation.Sum.BoxInfo());
            registerBox(Quantifiers.TwoDimensional.FunctionalDependence.FunctionOfRow.BoxInfo.typeIdentifier,
                        new Quantifiers.TwoDimensional.FunctionalDependence.FunctionOfRow.BoxInfo());
            registerBox(Quantifiers.TwoDimensional.FunctionalDependence.FunctionOfRowEachRow.BoxInfo.typeIdentifier,
                        new Quantifiers.TwoDimensional.FunctionalDependence.FunctionOfRowEachRow.BoxInfo());
            registerBox(Quantifiers.TwoDimensional.InformationTheory.ConditionalEntropy.BoxInfo.typeIdentifier,
                        new Quantifiers.TwoDimensional.InformationTheory.ConditionalEntropy.BoxInfo());
            registerBox(Quantifiers.TwoDimensional.InformationTheory.InformationDependence.BoxInfo.typeIdentifier,
                        new Quantifiers.TwoDimensional.InformationTheory.InformationDependence.BoxInfo());
            registerBox(
                Quantifiers.TwoDimensional.InformationTheory.MutualInformationNormalized.BoxInfo.typeIdentifier,
                new Quantifiers.TwoDimensional.InformationTheory.MutualInformationNormalized.BoxInfo());
            registerBox(Quantifiers.TwoDimensional.OrdinalDependence.Kendall.BoxInfo.typeIdentifier,
                        new Quantifiers.TwoDimensional.OrdinalDependence.Kendall.BoxInfo());


        }

        public override void AddCustomFactoriesToComunicator(Communicator communicator)
        {
            ObjectFactory factory = new ObjectFactory();
            ObjectFactory.addFactoryToCommunicator(communicator, factory);
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