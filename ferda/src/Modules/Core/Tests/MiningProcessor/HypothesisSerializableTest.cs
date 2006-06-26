﻿// The following code was generated by Microsoft Visual Studio 2005.
// The test owner should check each test for validity.
using System;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    /// <summary>
    ///This is a test class for Ferda.Guha.MiningProcessor.Formulas.Result and is intended
    ///to contain all Ferda.Guha.MiningProcessor.Formulas.Result Unit Tests
    ///</summary>
    [TestClass()]
    public class ResultTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        /// <summary>
        ///A test for Serialize (Result)
        ///</summary>
        [TestMethod()]
        public void SerializeTest()
        {
            BitStringIdentifier bsiA = new BitStringIdentifier(Guid.NewGuid(), "category A");
            BitStringIdentifier bsiB = new BitStringIdentifier(Guid.NewGuid(), "category B");
            BitStringIdentifier bsiC = new BitStringIdentifier(Guid.NewGuid(), "category C");
            BitStringIdentifier bsiD = new BitStringIdentifier(Guid.NewGuid(), "category D");

            Result r = new Result();
            r.AllObjectsCount = 510345;
            Hypothesis h = new Hypothesis();
            h.ContingencyTableA = new double[2][]
                {
                    new double[2] {35489, 354},
                    new double[2] {13, 27}
                };
            h.SetFormula(MarkEnum.Antecedent,
                new AtomFormula(bsiA)
                );
            h.SetFormula(MarkEnum.Succedent,
                new NegationFormula(
                    new ConjunctionFormula(
                        new BooleanAttributeFormula[]
                            {
                                new NegationFormula(
                                    new AtomFormula(bsiB)
                                    ),
                                new AtomFormula(bsiC)
                            }
                        )
                    ));
            h.SetFormula(MarkEnum.Condition,
                new DisjunctionFormula(
                    new BooleanAttributeFormula[]
                        {
                            new NegationFormula(
                                new ConjunctionFormula(
                                    new BooleanAttributeFormula[]
                                        {
                                            new NegationFormula(
                                                new AtomFormula(bsiD)
                                                )
                                        }
                                    )
                                )
                        }
                    ));
            h.SetFormula(MarkEnum.Attribute,
                new CategorialAttributeFormula(
                    bsiD.AttributeId
                    ));

            r.Hypotheses.Add(h);

            string actual;

            actual = SerializableResult.Serialize(r);

            Result rOut = SerializableResult.DeSerialize(actual);

            Formula af = rOut.Hypotheses[0].GetFormula(MarkEnum.Antecedent);
            Formula nf = rOut.Hypotheses[0].GetFormula(MarkEnum.Succedent);
            Formula df = rOut.Hypotheses[0].GetFormula(MarkEnum.Condition);
            Formula cf = rOut.Hypotheses[0].GetFormula(MarkEnum.Attribute); 
            
            ;
        }
    }
}