using System;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    [Serializable()]
    public class BitStringIdentifier : IEquatable<BitStringIdentifier>
    {
        private readonly string _attributeGuid;

        public string AttributeGuid
        {
            get { return _attributeGuid; }
        }

        private readonly string _categoryId;

        public string CategoryId
        {
            get { return _categoryId; }
        }

        public BitStringIdentifier(string attributeGuid, string categoryId)
        {
            _attributeGuid = attributeGuid;
            _categoryId = categoryId;
        }


        public override int GetHashCode()
        {
            return _attributeGuid.GetHashCode() ^ _categoryId.GetHashCode();
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
                return (_attributeGuid.Equals(other._attributeGuid)
                        && _categoryId.Equals(other._categoryId));
        }

        #endregion
    }
}