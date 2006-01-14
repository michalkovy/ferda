
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
namespace Netron.GraphLib.IO.GraphML {    
    
    [XmlType(IncludeInSchema=true, TypeName="node.type")]
    [XmlRoot(ElementName="node", IsNullable=false, DataType="")]
    public class NodeType {
        private DataCollection _items = new DataCollection();
        private string _desc;
        private string id;
        
        [XmlElement(ElementName="desc")]
        public virtual string Desc {
            get {
                return this._desc;
            }
            set {
                this._desc = value;
            }
        }
        
        
        
        [XmlElement(ElementName="locator", Type=typeof(LocatorType))]
        [XmlElement(ElementName="graph", Type=typeof(GraphType))]
        [XmlElement(ElementName="data", Type=typeof(DataType))]
        [XmlElement(ElementName="port", Type=typeof(PortType))]
        public virtual DataCollection Items {
            get {
                return this._items;
            }
            set {
                this._items = value;
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
 
    }
}
