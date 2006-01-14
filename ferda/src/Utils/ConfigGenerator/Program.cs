using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Schema;
using System.Xml.XPath;
using Ferda.Modules.Boxes.Serializer;

namespace Ferda.Utils.ConfigGenerator
{
	class Program
	{
		public class ConfigGenerator
		{

			private bool generateBoxInfo = false;

			public string InputFileName;

			public string OutputFileName;

			public string OutputBaseDirectory;

			public string XsdFileName;

			public TaskTypeEnum TaskType;
			private bool taskTypeIsSet = false;

			public enum TaskTypeEnum
			{
				BoxConfig,
				Localization
			}

			private string[] args;
			public string Args
			{
				get
				{
					string result = "";
					foreach (string arg in args)
					{
						result += " " + arg;
					}
					return result;
				}
			}

			public bool TryGetParams(string[] args)
			{
				if (args.Length < 8)
				{
					return false;
				}

				this.args = args;
				try
				{
					bool paramsOK = true;
					for (int i = 0; i < args.Length; i++)
					{
						string arg = args[i];
						switch (arg)
						{
							case "-inputFileName":
								InputFileName = args[++i];
								Console.WriteLine("Input file name is: " + InputFileName);
								break;
							case "-outputFileName":
								OutputFileName = args[++i];
								Console.WriteLine("Output file name is: " + OutputFileName);
								break;
							case "-taskType":
								TaskType = (TaskTypeEnum)Enum.Parse(typeof(TaskTypeEnum), args[++i]);
								taskTypeIsSet = true;
								Console.WriteLine("Task type is: " + TaskType.ToString());
								break;
							case "-outputBaseDirectory":
								OutputBaseDirectory = args[++i];
								Console.WriteLine("Output base directory is: " + OutputBaseDirectory);
								break;
							case "-xsdFileName":
								XsdFileName = args[++i];
								Console.WriteLine("XSD file is: " + XsdFileName);
								break;
							case "-boxInfo":
								generateBoxInfo = true;
								break;
						}
					}
					if (String.IsNullOrEmpty(InputFileName))
					{
						paramsOK = false;
						Console.WriteLine("Parameter \"-inputFileName\" is missing.");
					}
					if (String.IsNullOrEmpty(OutputFileName))
					{
						paramsOK = false;
						Console.WriteLine("Parameter \"-outputFileName\" is missing.");
					}
					if (!taskTypeIsSet)
					{
						paramsOK = false;
						Console.WriteLine("Parameter \"-taskType\" is missing.");
					}
					if (String.IsNullOrEmpty(OutputBaseDirectory))
					{
						paramsOK = false;
						Console.WriteLine("Parameter \"-outputBaseDirectory\" is missing.");
					}
					if (!paramsOK)
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
				return true;
			}

			public void Generate()
			{
				int filesGenerated = 0;
				string myNamespace = "http://ferda.is-a-geek.net";
				string xmlItem;
				string xmlItemName;
				string outputFilePath = "";

				string xpath;
				string newNamespace;
				string consoleSubString;
				string outputDirectory;
				GenerateBoxInfo generateBoxInfo = new GenerateBoxInfo();

				if (TaskType == TaskTypeEnum.BoxConfig)
				{
					xpath = "/boxes:Boxes/boxes:Box";
					newNamespace = " xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://ferda.is-a-geek.net\"";
					consoleSubString = "box config";
				}
				else //(TaskType == TaskTypeEnum.Localization)
				{
					xpath = "/boxes:BoxesLocalization/boxes:BoxLocalization";
					newNamespace = " xmlns=\"http://ferda.is-a-geek.net\"";
					consoleSubString = "localization";
				}

				//try
				{
					//Try xsd

					XPathDocument document = new XPathDocument(InputFileName);
					XPathNavigator navigator = document.CreateNavigator();
					XPathExpression query = navigator.Compile((xpath));
					XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
					manager.AddNamespace("boxes", myNamespace);
					query.SetContext(manager);
					XPathNodeIterator nodes = navigator.Select(query);

					while (nodes.MoveNext())
					{
						filesGenerated++;
						xmlItem = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
							+ nodes.Current.OuterXml.Replace(" xmlns=\"http://ferda.is-a-geek.net\"", newNamespace);

						nodes.Current.MoveToFirstChild();
						xmlItemName = nodes.Current.InnerXml;
						nodes.Current.MoveToParent();

						string subDirectory = xmlItemName.Replace('.', System.IO.Path.DirectorySeparatorChar);
						outputDirectory = Path.Combine(OutputBaseDirectory, subDirectory);

						if (!System.IO.Directory.Exists(outputDirectory))
						{
							Console.WriteLine("_Warning: Directory " + subDirectory + " not exists");
							continue;
						}
						try
						{
							outputFilePath = Path.Combine(outputDirectory, OutputFileName);
							if (!File.Exists(outputFilePath))
								File.Create(outputFilePath).Close();
							File.WriteAllText(outputFilePath, xmlItem, new UTF8Encoding(false, false));
						}
						catch
						{
							Console.WriteLine("_Warning: Exporting " + consoleSubString + " for: " + xmlItemName + " FAILED");
						}
						if (this.generateBoxInfo)
							generateBoxInfo.CreateBoxInfoFile(outputDirectory, xmlItemName);
						try
						{
							if (TaskType == TaskTypeEnum.BoxConfig)
							{
								Reader.ReadBox(outputFilePath);
							}
							else //(TaskType == TaskTypeEnum.Localization)
							{
								Reader.ReadBoxLocalization(outputFilePath);
							}
						}
						catch
						{
							Console.WriteLine("_Warning: Deserializing " + consoleSubString + " for: " + xmlItemName + " FAILED");
						}
					}
					Console.WriteLine(filesGenerated + " files generated.");
					return;
				}
				//catch (Exception ex)
				{
					//Console.WriteLine("_Error: Config Generation FAILED! " + Args);
					//Console.WriteLine(ex.Message);
					//Environment.Exit(1);
				}
			}
		}

		static int Main(string[] args)
		{
			ConfigGenerator configGenerator = new ConfigGenerator();
			if (!configGenerator.TryGetParams(args))
			{
				Console.WriteLine("Bad parameters");
				Console.WriteLine("USAGE:");
				Console.WriteLine("\t -inputFileName <name of input file> (REQUIRED)");
				Console.WriteLine("\t -outputFileName <name of output file> (REQUIRED)");
				Console.WriteLine("\t -taskType <BoxConfig|Localization> (REQUIRED)");
				Console.WriteLine("\t -outputBaseDirectory <name of output base directory> (REQUIRED)");
				Console.WriteLine("\t -xsdFileName <name of xsd file> (OPTIONAL) - NOT IMPLEMETED JET");
				Environment.Exit(1);
			}
			try
			{
				configGenerator.Generate();
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.FileName);
				Environment.Exit(1);
			}
			return 0;
		}
		/*
 * USAGE
 * -inputFileName boxes.xml -outputFileName BoxConfig.xml -taskType BoxConfig -outputBaseDirectory ./Boxes
 * -inputFileName boxesLocalization.cs-CZ.xml -outputFileName Localization.cs-CZ.xml -taskType Localization -outputBaseDirectory ./Boxes
 * -inputFileName boxesLocalization.en-US.xml -outputFileName Localization.en-US.xml -taskType Localization -outputBaseDirectory ./Boxes
 * -inputFileName boxesLocalization.en-US.xml -outputFileName Localization.xml -taskType Localization -outputBaseDirectory ./Boxes
	 */
	}
}