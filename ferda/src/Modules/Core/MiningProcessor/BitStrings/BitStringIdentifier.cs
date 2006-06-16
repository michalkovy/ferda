using System;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public class BitStringIdentifier : IEquatable<BitStringIdentifier>
    {
        private readonly Guid _attributeId;

        public Guid AttributeId
        {
            get { return _attributeId; }
        }

        public readonly string _categoryId;

        public string CategoryId
        {
            get { return _categoryId; }
        }

        public BitStringIdentifier(Guid attributeId, string categoryId)
        {
            _attributeId = attributeId;
            _categoryId = categoryId;
        }


        public override int GetHashCode()
        {
            return _attributeId.GetHashCode() ^ _categoryId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BitStringIdentifier);
        }

        #region IEquatable<BitStringIdentifier> Members

        public bool Equals(BitStringIdentifier other)
        {
            if (other == null)
                return false;
            else
                return (_attributeId.Equals(other._attributeId)
                        && _categoryId.Equals(other._categoryId));
        }

        #endregion
    }
}