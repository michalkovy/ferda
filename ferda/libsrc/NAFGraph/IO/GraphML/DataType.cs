
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    
namespace Netron.GraphLib.IO.GraphML {    
    
    
    [XmlType(IncludeInSchema=true, TypeName="data.type")]
    [XmlRoot(ElementName="data", IsNullable=false, DataType="")]
    public class DataType {

		#region Fields
        private DataCollection _text = new DataCollection();

        private string _key;

        private string id;

		#endregion

		#region Properties
        [XmlAttribute(AttributeName="key")]
        public virtual string Key {
            get {
                return this._key;
            }
            set {
                this._key = value;
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
        
        
        
        [XmlText(Type=typeof(string))]
        public virtual DataCollection Text {
            get {
                return this._text;
            }
            set {
                this._text = value;
            }
        }
		#endregion        

    }
}
