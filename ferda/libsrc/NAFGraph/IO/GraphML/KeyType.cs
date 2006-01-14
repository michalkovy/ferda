
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
namespace Netron.GraphLib.IO.GraphML {    
    
    [XmlType(IncludeInSchema=true, TypeName="key.type")]
    [XmlRoot(ElementName="key", IsNullable=false, DataType="")]
    public class KeyType {
        private DefaultType _default;
        private string id;
        private string _desc;
        private KeyForType _for;
        
        [XmlElement(ElementName="desc")]
        public virtual string Desc {
            get {
                return this._desc;
            }
            set {
                this._desc = value;
            }
        }
        
        
        
        [XmlElement(ElementName="default")]
        public virtual DefaultType Default {
            get {
                return this._default;
            }
            set {
                this._default = value;
            }
        }
        
        
        
        [XmlAttribute(AttributeName="id")]
        public virtual string ID {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }
        
        
        
        [XmlAttribute(AttributeName="for")]
        public virtual KeyForType For {
            get {
                return this._for;
            }
            set {
                this._for = value;
            }
        }
    }
}
