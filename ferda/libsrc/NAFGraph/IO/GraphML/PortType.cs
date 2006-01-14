
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
namespace Netron.GraphLib.IO.GraphML {    
    
    [XmlType(IncludeInSchema=true, TypeName="port.type")]
    [XmlRoot(ElementName="port", IsNullable=false, DataType="")]
    public class PortType {
        private DataCollection _items = new DataCollection();
        private string _name;
        private string _desc;
        
        
        
        [XmlElement(ElementName="desc")]
        public virtual string Desc {
            get {
                return this._desc;
            }
            set {
                this._desc = value;
            }
        }
        
        
        
        [XmlElement(ElementName="port", Type=typeof(PortType))]
        [XmlElement(ElementName="data", Type=typeof(DataType))]
        public virtual DataCollection Items {
            get {
                return this._items;
            }
            set {
                this._items = value;
            }
        }
        
        
        
        [XmlAttribute(AttributeName="name")]
        public virtual string Name {
            get {
                return this._name;
            }
            set {
                this._name = value;
            }
        }
   
    }
}
