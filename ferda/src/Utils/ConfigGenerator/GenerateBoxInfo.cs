using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Ferda.Utils.ConfigGenerator
{
	public class GenerateBoxInfo
	{
		public void CreateBoxInfoFile(string directoryPath, string identifier)
		{
			string className;
			string fileContent = this.getFileContent(identifier, out className);
			string outputFilePath = Path.Combine(directoryPath, className + ".cs");
			if (!File.Exists(outputFilePath))
				File.Create(outputFilePath).Close();
			File.WriteAllText(outputFilePath, fileContent, new UTF8Encoding(false, false));
		}

		private string getFileContent(string identifier, out string className)
		{
			if (identifier.Contains("."))
				className = identifier.Substring(identifier.LastIndexOf('.') + 1);
			else
				className = identifier;
			string functionsName = className + "FunctionsI";
			className += "BoxInfo";
			string baseClassName;
			if (identifier.Contains("SDFFTTask.Quantifiers"))
				baseClassName = "Ferda.Modules.Boxes.SDFFTTask.Quantifiers.AbstractSDFFTTaskQuantifierBoxInfo";
			else if (identifier.Contains("FFTTask.Quantifiers"))
				baseClassName = "Ferda.Modules.Boxes.FFTTask.Quantifiers.AbstractQuantifierBoxInfo";
			else if (identifier.Contains("SDCFTask.Quantifiers"))
				baseClassName = "Ferda.Modules.Boxes.SDCFTask.Quantifiers.AbstractSDCFTaskQuantifierBoxInfo";
			else if (identifier.Contains("CFTask.Quantifiers"))
				baseClassName = "Ferda.Modules.Boxes.CFTask.Quantifiers.AbstractCFTaskQuantifierBoxInfo";
			else if (identifier.Contains("SDKLTask.Quantifiers"))
				baseClassName = "Ferda.Modules.Boxes.SDKLTask.Quantifiers.AbstractSDKLTaskQuantifierBoxInfo";
			else if (identifier.Contains("KLTask.Quantifiers"))
				baseClassName = "Ferda.Modules.Boxes.KLTask.Quantifiers.AbstractKLTaskQuantifierBoxInfo";
			else
				baseClassName = "Ferda.Modules.Boxes.BoxInfo";
			string result = ""
	+ "using System;"
	+ "\n" + "using System.Collections.Generic;"
	+ "\n" + ""
	+ "\n" + "namespace Ferda.Modules.Boxes." + identifier
	+ "\n" + "{"
	+ "\n" + "	class " + className + " : " + baseClassName
	+ "\n" + "	{"
	+ "\n" + "		protected override string identifier"
	+ "\n" + "		{"
	+ "\n" + "			get { return \"" + identifier + "\"; }"
	+ "\n" + "		}"
	+ "\n" + ""
	+ "\n" + "		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)"
	+ "\n" + "		{"
	+ "\n" + "			" + functionsName + " result = new " + functionsName + "();"
	+ "\n" + "			iceObject = (Ice.Object)result;"
	+ "\n" + "			functions = (IFunctions)result;"
	+ "\n" + "		}"
	+ "\n" + ""
	+ "\n" + "		public override string[] GetBoxModuleFunctionsIceIds()"
	+ "\n" + "		{"
	+ "\n" + "			return " + functionsName + ".ids__;"
	+ "\n" + "		}"
	+ "\n" + "	}"
	+ "\n" + "}";
			return result;
		}
	}
}
