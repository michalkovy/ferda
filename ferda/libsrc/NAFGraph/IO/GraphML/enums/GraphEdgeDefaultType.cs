
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
namespace Netron.GraphLib.IO.GraphML {    
    
    public enum GraphEdgeDefaultType {
        
        
        
        [XmlEnum(Name="directed")]
        Directed,
        
        
        
        [XmlEnum(Name="undirected")]
        Undirected,
    }
}
