using System;

namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// Collects info about a shape from the class attributes
	/// </summary>
	public class ShapeSummary
	{
		#region Fields
		protected string mShapeKey;
		protected string mShapeName;
		protected string mShapeCategory;
		protected string mLibPath;
		protected string shapeReflectionName;
		protected string mDescription = "No description available.";


		#endregion

		#region Properties
		public string ReflectionName
		{
			get{return shapeReflectionName;}
			set{shapeReflectionName = value;}
		}

		public string LibPath
		{
			get{return mLibPath;}
			set{mLibPath = value;}
		}
		public string Description
		{
			get{return mDescription;}
			set{mDescription = value;}
		}

		public string ShapeKey
		{
			get{return mShapeKey;}
			set{mShapeKey = value;}
		}
		public string ShapeName
		{
			get{return mShapeName;}
			set{mShapeName = value;}
		}

		public string ShapeCategory
		{
			get{return mShapeCategory;}
			set{mShapeCategory = value;}
		}
		#endregion

		#region Constructors
		public ShapeSummary()
		{
			
		}

		public ShapeSummary(string libraryPath, string mShapeKey, string mShapeName, string mShapeCategory, string reflectionName)
		{
			this.mLibPath=libraryPath;
			this.mShapeKey = mShapeKey;
			this.mShapeName = mShapeName;
			this.mShapeCategory = mShapeCategory;
			this.shapeReflectionName = reflectionName;
		}
		public ShapeSummary(string libraryPath, string mShapeKey, string mShapeName, string mShapeCategory, string reflectionName, string mDescription)
		{
			this.mLibPath=libraryPath;
			this.mShapeKey = mShapeKey;
			this.mShapeName = mShapeName;
			this.mShapeCategory = mShapeCategory;
			this.shapeReflectionName = reflectionName;
			this.mDescription = mDescription;
		}

		#endregion
	}
}
