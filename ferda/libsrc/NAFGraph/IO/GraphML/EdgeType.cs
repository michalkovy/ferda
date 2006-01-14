
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
namespace Netron.GraphLib.IO.GraphML {    
    
    [XmlType(IncludeInSchema=true, TypeName="edge.type")]
    [XmlRoot(ElementName="edge", IsNullable=false, DataType="")]
    public class EdgeType {
		#region Fields
        private bool _directed;
        private DataCollection _data = new DataCollection();
        private string _source;
        private string id;
        private string _sourceport;
        private string _targetport;
        private bool _directedspecified;
        private string _target;
        private GraphType _graph;
        private string _desc;
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
        
        
        
        [XmlElement(ElementName="data", Type=typeof(DataType))]
        public virtual DataCollection Data {
            get {
                return this._data;
            }
            set {
                this._data = value;
            }
        }
        
        
        
        [XmlElement(ElementName="graph")]
        public virtual GraphType Graph {
            get {
                return this._graph;
            }
            set {
                this._graph = value;
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
        
        
        
        [XmlAttribute(AttributeName="directed")]
        public virtual bool Directed {
            get {
                return this._directed;
            }
            set {
                this._directed = value;
            }
        }
        
        
        
        [XmlElement(ElementName="directedSpecified")]
        public virtual bool Directedspecified {
            get {
                return this._directedspecified;
            }
            set {
                this._directedspecified = value;
            }
        }
        
        
        
        [XmlAttribute(AttributeName="source")]
        public virtual string Source {
            get {
                return this._source;
            }
            set {
                this._source = value;
            }
        }
        
        
        
        [XmlAttribute(AttributeName="target")]
        public virtual string Target {
            get {
                return this._target;
            }
            set {
                this._target = value;
            }
        }
        
        
        
        [XmlAttribute(AttributeName="sourceport")]
        public virtual string Sourceport {
            get {
                return this._sourceport;
            }
            set {
                this._sourceport = value;
            }
        }
        
        
        
        [XmlAttribute(AttributeName="targetport")]
        public virtual string Targetport {
            get {
                return this._targetport;
            }
            set {
                this._targetport = value;
            }
        }
		#endregion  
   
    }
}
