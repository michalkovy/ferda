using System;
using System.Collections;
namespace Netron.GraphLib.Attributes
{
	/// <summary>
	/// Attribute to tag a class as a Netron graph shape
	/// </summary>
	[Serializable] public  class NetronGraphShapeAttribute : System.Attribute
	{
		#region Fields
		/// <summary>
		/// the key of the shape, usually a GUID
		/// </summary>
		protected string mShapeKey;
		/// <summary>
		/// the name of the shape
		/// </summary>
		protected string mShapeName;
		/// <summary>
		/// the cateogry under which it will stay
		/// </summary>
		protected string mShapeCategory;
		/// <summary>
		/// the full name of the shape to reflect
		/// </summary>
		/// 
		protected string shapeReflectionName;
		/// <summary>
		/// a description
		/// </summary>
		protected string mDescription = "No mDescription available.";

		#endregion

		#region Properties
		
		/// <summary>
		/// Gets or sets the full name to reflect the shape
		/// </summary>
		public string ReflectionName
		{
			get{return shapeReflectionName;}
			set{shapeReflectionName = value;}
		}
		/// <summary>
		/// Gets the category of the shape under which it will reside in a viewer
		/// </summary>
		public string ShapeCategory
		{
			get{return mShapeCategory;}
			
		}
		/// <summary>
		/// Gets a mDescription of the shape
		/// </summary>
		public string Description
		{
			get{return mDescription;}
		}
		/// <summary>
		/// Gets the unique identifier of the shape
		/// </summary>
		public string ShapeKey
		{
			get{return mShapeKey;}
		}
		/// <summary>
		/// Gets the shape name
		/// </summary>
		public string ShapeName
		{
			get{return mShapeName;}
		}

		#endregion

		#region Constructor
		
		public NetronGraphShapeAttribute(string mShapeName, string mShapeKey, string mShapeCategory, string reflectionName)
		{
			this.mShapeName = mShapeName;
			this.mShapeKey = mShapeKey;
			this.mShapeCategory=mShapeCategory;
			this.shapeReflectionName = reflectionName;
		}
		public NetronGraphShapeAttribute(string mShapeName, string mShapeKey, string mShapeCategory, string reflectionName, string mDescription)
		{
			this.mShapeName = mShapeName;
			this.mShapeKey = mShapeKey;
			this.mShapeCategory=mShapeCategory;
			this.shapeReflectionName = reflectionName;
			this.mDescription = mDescription;
		}
		#endregion

		#region Methods

		
		#endregion
	}

	/// <summary>
	/// Attribute to tag a class as a Netron graph connection
	/// </summary>
	[Serializable] public  class NetronGraphConnectionAttribute : System.Attribute
	{
		#region Fields
		
		/// <summary>
		/// the name of the connection
		/// </summary>
		protected string mConnectionName;		
		/// <summary>
		/// the full name of the shape to reflect
		/// </summary>
		/// 
		protected string mReflectionName;
		/// <summary>
		/// a description
		/// </summary>
		protected string mDescription = "No description available.";

		#endregion

		#region Properties
		
		/// <summary>
		/// Gets or sets the full name to reflect the shape
		/// </summary>
		public string ReflectionName
		{
			get{return mReflectionName;}
			set{mReflectionName = value;}
		}
		/// <summary>
		/// Gets a mDescription of the shape
		/// </summary>
		public string Description
		{
			get{return mDescription;}
		}		
		/// <summary>
		/// Gets the shape name
		/// </summary>
		public string ConnectionName
		{
			get{return mConnectionName;}
		}

		#endregion

		#region Constructor
		
		public NetronGraphConnectionAttribute(  string connectionName,  string reflectionName)
		{
					
			this.mConnectionName = connectionName;			
			this.mReflectionName = reflectionName;
		}
		
		#endregion

		#region Methods

		
		#endregion
	}

	/// <summary>
	/// Attribute to tag a class as a Netron graph connection
	/// </summary>
	[Serializable] public  class ConnectionStyleAttribute : System.Attribute
	{
		protected ArrayList mExtraStyles;
		public ConnectionStyleAttribute(ArrayList extra)
		{
			mExtraStyles = extra;
		}
		public ConnectionStyleAttribute()
		{
			
		}

		public ArrayList ExtraStyles
		{
			get{return mExtraStyles;}
			set{mExtraStyles = value;}
		}


	}

}
