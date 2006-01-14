
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
    
namespace Netron.GraphLib.IO.GraphML {    
    [XmlType(IncludeInSchema=true, TypeName="locator.type")]
    [XmlRoot(ElementName="locator", IsNullable=false, DataType="")]
    public class LocatorType {
    }
}
