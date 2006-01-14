
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
    
namespace Netron.GraphLib.IO.GraphML {    
    
    [XmlType(IncludeInSchema=true, TypeName="default.type")]
    [XmlRoot(ElementName="default", IsNullable=false, DataType="")]
    public class DefaultType {        
        
        private DataCollection _text = new DataCollection();
        
        [XmlText(Type=typeof(string))]
        public virtual DataCollection Text {
            get {
                return this._text;
            }
            set {
                this._text = value;
            }
        }
        

    }
}
