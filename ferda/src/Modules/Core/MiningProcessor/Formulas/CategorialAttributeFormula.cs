namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class CategorialAttributeFormula : Formula
    {
        private string _attributeGuid;

        public string AttributeGuid
        {
            get { return _attributeGuid; }
            set { _attributeGuid = value; }
        }

        public CategorialAttributeFormula()
        {
        }

        public CategorialAttributeFormula(string attributeGuid)
        {
            _attributeGuid = attributeGuid;
        }

        public override string ToString()
        {
            return AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(_attributeGuid);
        }
    }
}