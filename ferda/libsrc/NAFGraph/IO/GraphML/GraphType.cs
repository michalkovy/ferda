
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
namespace Netron.GraphLib.IO.GraphML {    
    
    [XmlType(IncludeInSchema=true, TypeName="graph.type")]
    [XmlRoot(ElementName="graph", IsNullable=false, DataType="")]
    public class GraphType {
		#region Fields
        private DataCollection _items = new DataCollection();
        private GraphEdgeDefaultType _edgeDefault;
        private string _desc;
        private string id;
		#endregion

		#region Properties
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
        [XmlElement(ElementName="edge", Type=typeof(EdgeType))]
        [XmlElement(ElementName="node", Type=typeof(NodeType))]
        [XmlElement(ElementName="data", Type=typeof(DataType))]
        [XmlElement(ElementName="hyperedge", Type=typeof(HyperEdgeType))]
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
        
        
        
        [XmlAttribute(AttributeName="edgedefault")]
        public virtual GraphEdgeDefaultType EdgeDefault {
            get {
                return this._edgeDefault;
            }
            set {
                this._edgeDefault = value;
            }
        }
  
		#endregion
    }
}
