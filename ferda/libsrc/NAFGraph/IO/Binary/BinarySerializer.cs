using System;
using System.Diagnostics;
using System.IO;
using Netron.GraphLib.UI;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
namespace Netron.GraphLib
{
	/// <summary>
	/// Summary description for BinarySerializer.
	/// </summary>
	public class BinarySerializer
	{
		public BinarySerializer()
		{
			
		}
		public static  bool SaveAs(string fileName, GraphControl site)
		{

			FileStream fs = new FileStream(fileName, FileMode.Create);

			BinaryFormatter f = new BinaryFormatter();
			//XmlSerializer f = new XmlSerializer(typeof(GraphAbstract),"Netron.GraphLib");
			
			try
			{
				//Warning!: cleaning up, you need to unhook all events since unserializable classes hooked to events will give problems				
				f.Serialize(fs, site.extract);
				return true;
			}
			//catch(SerializationException exc)			
			catch(Exception exc)			
			{
				System.Windows.Forms.MessageBox.Show("The graph was not saved, because some graph events were attached to non-serializable classes.\r\n This is a known issue and will be resolved in a later stadium.");
				Debug.WriteLine(exc.Message);
				
				DumpInfo();
			}
			finally
			{
				fs.Close();
			}
			return false;
		}
		public static  void Open (string fileName, GraphControl site)
		{
			FileStream fs=null;
			
			try
			{
				fs= File.OpenRead(fileName);
			}
			catch (System.IO.DirectoryNotFoundException exc)
			{
				System.Windows.Forms.MessageBox.Show(exc.Message);
			}
			catch(System.IO.FileLoadException exc)
			{				
				System.Windows.Forms.MessageBox.Show(exc.Message);
			}
			catch (System.IO.FileNotFoundException exc)
			{
				System.Windows.Forms.MessageBox.Show(exc.Message);
			}
			//donnot open anything if filestream is not there
			if (fs==null) return;
			try
			{
				
				BinaryFormatter f = new BinaryFormatter();

				GraphAbstract tmp = (GraphAbstract) f.Deserialize(fs); //so simple, so powerful
				tmp.Site=site;

				if(tmp.Shapes.Count<1) return;
				// paint the connections
				foreach (Shape o in tmp.Shapes)
					foreach (Connector c in o.Connectors)
						foreach (Connection n in c.Connections)
						{
							
							n.Site = site;
							if(site.Abstract.mConnections.Contains(n)) continue;
							site.Abstract.mConnections.Add(n);//is not done automatically
							n.InitEntity();
						}
				// paint the shapes
				foreach (Shape o in tmp.Shapes)
				{
					o.Site = site;					
					o.InitEntity();
				}
				//paint the connector
				foreach (Shape o in tmp.Shapes)
					foreach (Connector c in o.Connectors)
						if ((o.Hover) || (c.Hover))
						{
							c.Site = site;
							c.InitEntity();
						}
				
				

				site.extract = tmp;

			}
			catch(SerializationException exc)			
			{
				System.Windows.Forms.MessageBox.Show(exc.Message);
			}
			finally
			{
				if(fs!=null)
					fs.Close();				
			}
		}
		public static void DumpInfo()
		{
			System.Reflection.MemberInfo[] mi;
			

			mi= System.Runtime.Serialization.FormatterServices.GetSerializableMembers(typeof(GraphAbstract));
			Debug.WriteLine(  Environment.NewLine + "________________________" + Environment.NewLine + "GraphAbstract" + Environment.NewLine + "________________________" + Environment.NewLine);
			DumpInfo(mi);

			mi= System.Runtime.Serialization.FormatterServices.GetSerializableMembers(typeof(Shape));
			Debug.WriteLine("Shape" + Environment.NewLine + Environment.NewLine);
			DumpInfo(mi);

		
		}
		private static void DumpInfo(System.Reflection.MemberInfo[] array)
		{
			for(int k=0; k<array.Length; k++)
			{
				Debug.WriteLine(array[k].Name + "   [" + array[k].ToString() + "]");
			}
		}
	}
}
