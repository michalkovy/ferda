
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
namespace Netron.GraphLib.IO.GraphML {    
    
    public enum KeyForType {
        
        
        
        [XmlEnum(Name="all")]
        All,
        
        
        
        [XmlEnum(Name="graph")]
        Graph,
        
        
        
        [XmlEnum(Name="node")]
        Node,
        
        
        
        [XmlEnum(Name="edge")]
        Edge,
        
        
        
        [XmlEnum(Name="hyperedge")]
        HyperEdge,
        
        
        
        [XmlEnum(Name="port")]
        Port,
        
        
        
        [XmlEnum(Name="endpoint")]
        EndPoint,
    }
}
