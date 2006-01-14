
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    namespace Netron.GraphLib.IO.GraphML {
    
    
    
    [XmlType(IncludeInSchema=true, TypeName="graphml.type")]
    [XmlRoot(ElementName="graphml", IsNullable=false, DataType="")]
    public class GraphMLType {
        
		#region Fields
        
        
        private DataCollection _key = new DataCollection();
        
        
        
        private string _desc;
        
        
        
        private DataCollection _items = new DataCollection();
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
        
        
        
        [XmlElement(ElementName="key", Type=typeof(KeyType))]
        public virtual DataCollection Key {
            get {
                return this._key;
            }
            set {
                this._key = value;
            }
        }
        
        
        
        [XmlElement(ElementName="graph", Type=typeof(GraphType))]
        [XmlElement(ElementName="data", Type=typeof(DataType))]
        public virtual DataCollection Items {
            get {
                return this._items;
            }
            set {
                this._items = value;
            }
        }
		#endregion

		
    
    
    }
}
