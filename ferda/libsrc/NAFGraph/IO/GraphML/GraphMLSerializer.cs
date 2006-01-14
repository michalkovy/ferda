using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Xml.Schema;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.IO.GraphML;

namespace Netron.GraphLib.IO
{
	/// <summary>
	/// GraphMLSerializer serializes a graph to GraphML (see http://www.graphdrawing.org/) 
	/// </summary>
	public class GraphMLSerializer
	{
		#region Fields
		private string dtdPath = "http://graphml.graphdrawing.org/dtds/1.0rc/graphml.dtd";
		private String nodeShapeTypeKeyName = @"ShapeType";
		private String edgeConnectionTypeKeyName = @"EdgeType";

		private Hashtable KeyList = new Hashtable();

		#endregion

		#region Constructor
		public GraphMLSerializer()
		{
		}

		public GraphMLSerializer(string dtdPath)
		{
			this.dtdPath = dtdPath;
		}
		#endregion

		#region Methods

		public static void Validate(XmlReader reader)
		{
			XmlValidatingReader vr = new XmlValidatingReader(reader);

			vr.ValidationType = ValidationType.Auto;
			vr.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

			while (vr.Read()){};
		}

		private static void ValidationHandler(object sender, ValidationEventArgs args)
		{
			Debug.WriteLine(args.ToString());
		}

		public void Serialize(XmlWriter writer, GraphAbstract g	)
		{

			GraphMLType graphml = new GraphMLType();

			graphml.Key.Add(BuildKeyType(nodeShapeTypeKeyName));
			graphml.Key.Add(BuildKeyType(edgeConnectionTypeKeyName));

			GraphType graph = new GraphType();			
			graphml.Items.Add(graph);

			foreach ( Shape s in g.Shapes )
			{
				graph.Items.Add(SerializeNode(s));
			}

			foreach(Connection c in g.Connections)
			{
				graph.Items.Add(SerializeEdge(c));
			}

			foreach(DictionaryEntry de in KeyList)
			{
				graphml.Key.Add(BuildKeyType((String)de.Key));
			}

			// serialize
			XmlSerializer ser = new XmlSerializer(typeof(GraphMLType));
			ser.Serialize(writer,graphml);
		}

		private NodeType SerializeNode(Shape s)
		{
			Hashtable attributes = GraphMLDataAttribute.GetValuesOfTaggedFields(s);

			NodeType node = new NodeType();
			node.ID = FormatID(s);
			
			node.Items.Add(DataTypeFromEntity(s));

			foreach(DataType data in DataTypesFromAttributes(attributes))
			{
				node.Items.Add(data);
			}

			return node;
		}

		private EdgeType SerializeEdge(Connection c)
		{
			Hashtable attributes = GraphMLDataAttribute.GetValuesOfTaggedFields(c);

			EdgeType edge = new EdgeType();
			edge.ID = FormatID(c);
			
			/* Save the connectors the Connection is connected to */
			edge.Source = FormatID(c.From.BelongsTo);
			edge.Target = FormatID(c.To.BelongsTo);
			/* Save the connectors the Connection is connected to */
			edge.Sourceport = FormatID(c.From);
			edge.Targetport = FormatID(c.To);
			edge.Directed = true;

			edge.Data.Add(DataTypeFromEntity(c));

			foreach(DataType dt in DataTypesFromAttributes(attributes))
			{
				edge.Data.Add(dt);
			}

			return edge;
		}
		#endregion

		#region Helper Functions
		private string FormatID(Entity e)
		{
			return String.Format("e{0}",e.UID.ToString());
		}

		private KeyType BuildKeyType(String s)
		{
			KeyType kt = new KeyType();
			kt.ID = s;
			kt.For = KeyForType.All;
			
			return kt;
		}

		private DataType DataTypeFromEntity(Entity e)
		{
			DataType dt = new DataType();
			if ( e.GetType() == typeof(Shape))
				dt.ID = nodeShapeTypeKeyName;
			else
				dt.ID = edgeConnectionTypeKeyName;
			dt.Text.Add(GetTypeQualifiedName(e));
			
			return dt;
		}


		private DataType[] DataTypesFromAttributes(Hashtable attributes)
		{
			DataType[] dts = new DataType[attributes.Count];
			
			int i = 0;
			foreach ( DictionaryEntry de in attributes )
			{
				dts[i] = new DataType();
				dts[i].Key = de.Key.ToString();
				if (de.Value != null)
					dts[i].Text.Add(de.Value.ToString());
				if ( !KeyList.Contains(de.Key.ToString()))
					KeyList.Add(de.Key.ToString(),de.Value);
				++i;
			}
			return dts;

		}

		/// <summary>
		/// Returns qualified type name of o
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		private string GetTypeQualifiedName(Object o)
		{
			if (o==null)
				throw new ArgumentNullException("o");
			return this.GetTypeQualifiedName(o.GetType());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		private string GetTypeQualifiedName(Type t)
		{
			return Assembly.CreateQualifiedName(
				t.Assembly.FullName,
				t.FullName
				);
		}

		private Type ToType(string text)
		{
			return Type.GetType(text,true);
		}

		private bool ToBool(string text)
		{
			return bool.Parse(text);
		}
		#endregion
	}
}
