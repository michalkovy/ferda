using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.Attributes;
namespace Netron.GraphLib
{
	/// <summary>
	/// An Entity is an element of the plex, can be either 
	/// <br>- a connection</br>
	///<br> - a connector</br>
	///<br> - a Shape (box, element)</br>
	/// </summary>
		

	[Serializable] public abstract class Entity 
	{	

		#region Events

		/// <summary>
		/// Occurs when the mouse is pressed on this shape
		/// </summary>
		public event MouseEventHandler OnMouseDown;

		/// <summary>
		/// Occurs when the mouse is released while above this shape
		/// </summary>
		public event MouseEventHandler OnMouseUp;

		public event MouseEventHandler OnMouseMove;
		#endregion


		#region Fields


		/// <summary>
		/// whether to recalculate the shape size, speed up the rendering
		/// </summary>
		protected bool recalculateSize ;

		[NonSerialized] protected internal PropertyBag bag;

		/// <summary>
		/// default blue pen, speeds up rendering
		/// Note that the Pen is not serialzable!
		/// </summary>
		[NonSerialized] protected internal Pen bluepen;
		/// <summary>
		/// default black pen, speeds up rendering
		/// </summary>
		[NonSerialized] protected internal Pen blackpen;

		protected internal float mPenWidth = 1F;
		/// <summary>
		/// default pen
		/// </summary>
		[NonSerialized] protected internal Pen pen;
		/// <summary>
		/// whether the entity is reset
		/// </summary>
		protected internal bool mIsReset=false;
		/// <summary>
		/// the font family
		/// </summary>
		protected string mFontFamily = "Verdana";
		/// <summary>
		/// whether the entity is selected
		/// </summary>
		protected Boolean mIsSelected = false;
		/// <summary>
		/// whether this entity is being hovered
		/// </summary>
		protected Boolean mHover = false;
		/// <summary>
		/// the unique identitfier
		/// </summary>
		protected Guid mUID ;
		/// <summary>
		/// mText or label
		/// </summary>
		protected string mText = "[Not set]";
		/// <summary>
		/// whether to show the mText label
		/// </summary>
		protected internal bool mShowLabel=true;

		
		/// <summary>
		/// the default mText color
		/// </summary>
		protected Color mTextColor=Color.Black;
		/// <summary>
		/// the default font size in points
		/// </summary>
		protected float mFontSize=7.8F;
		/// <summary>
		/// the default font for shapes
		/// </summary>
		protected Font mFont = null;
		/// <summary>
		/// the mSite of the entity
		/// </summary>
		[NonSerialized]  protected IGraphSite mSite;
			

		#endregion

		#region Properties

	
		/// <summary>
		/// Gets or sets the pen width
		/// </summary>
		[GraphMLData]public float PenWidth
		{
			get{return mPenWidth;}
			set{mPenWidth = value;}
		}

		/// <summary>
		/// Gets or sets whether the shape label should be shown.
		/// </summary>
		[GraphMLData]public virtual bool ShowLabel
		{
			get
			{
					
				return mShowLabel;
			}
			set
			{
				mShowLabel=value; this.Invalidate();
			}
		}
		/// <summary>
		/// Allows to view/change the properties of the shape, most probably on double-clicking it.
		/// </summary>			
		public virtual PropertyBag Properties
		{
			get{return bag;}

		}

		/// <summary>
		/// Gets or sets the entity label
		/// </summary>
		[GraphMLData]public virtual string Text
		{

			get
			{

				return mText;
			}
			set
			{
				if (value!=null)	mText=value;

			}
		}

		/// <summary>
		/// Gets or sets the mSite of the entity
		/// </summary>
		public IGraphSite Site
		{
			get{return mSite;}
			set{mSite = value;}
		}
		/// <summary>
		/// Tells wether the entity (shape) is selected
		/// </summary>
		protected internal virtual bool IsSelected
		{
			set { Invalidate(); mIsSelected = value; Invalidate(); }
			get { return mIsSelected; }
		}
		/// <summary>
		/// Gets or sets whether the entity is reset
		/// </summary>
		protected internal virtual bool IsReset
		{
			get{return mIsReset;}
			set{mIsReset = value;}
		}
		/// <summary>
		/// Gives true if the mouse is hovering over this entity
		/// </summary>
		protected internal virtual Boolean Hover
		{
			set { Invalidate(); mHover = value; Invalidate(); }
			get { return mHover; }
		}

		/// <summary>
		/// Gets or sets the mText color
		/// </summary>
		protected internal virtual Color TextColor
		{
			get
			{					
				return mTextColor;
			}
			set
			{
				mTextColor=value;
			}
		}

		/// <summary>
		/// Gets or sets the font size of the mText
		/// </summary>
		protected internal virtual float FontSize
		{
			get
			{
					
				return mFontSize;
			}
			set
			{
				mFontSize=value;
			}
		}
		/// <summary>
		/// Gets or sets the font family of the mText
		/// </summary>
		protected internal virtual string FontFamily
		{
			get{return mFontFamily;}
			set{mFontFamily = value;}
		}

		public Font Font
		{
			get{return this.mFont;}
			set
			{
				this.mFontFamily = value.FontFamily.ToString();
				this.mFontSize = value.Size;				
			}
		
		}
		#endregion
		
		#region Constructor
		/// <summary>
		/// Constructor for the entity class, initializes a new GUID for the entity
		/// </summary>
		public Entity()
		{
			InitEntity();
		}
		/// <summary>
		/// Creates a new entity, specifying the mSite 
		/// </summary>
		/// <param name="mSite"></param>
		public Entity(IGraphSite mSite)
		{
			this.mSite = mSite;
			InitEntity();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Determines which properties are accessible via the property grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			switch(e.Property.Name)
			{
				case "Text": e.Value=this.mText;break;
				
			}
		}


		/// <summary>
		/// Sets the values passed by the property grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{

			switch(e.Property.Name)
			{
				case "Text": 
					//use the logic and the constraint of the object that is being reflected
					if(e.Value.ToString() != null)					
					{
						this.mText=(string) e.Value;
						
					}
					else
						MessageBox.Show("Not a valid label","Invalid label",MessageBoxButtons.OK,MessageBoxIcon.Warning);					
					this.Invalidate();
					break;
			}
		}

		public virtual void AddProperties()
		{
			
		}
		/// <summary>
		/// Initializes the class. This method is necessary when deserializing since various elements like
		/// the Pen cannot be serialized to file and have to be, hence, set after deserialization.
		/// </summary>
		protected internal virtual void InitEntity()
		{
			bluepen= new Pen(Brushes.DarkSlateBlue,1F);
			blackpen = new Pen(Brushes.Black,1F);
			mUID = Guid.NewGuid();
			recalculateSize = false;
			mFont = new Font(mFontFamily,mFontSize,FontStyle.Regular,GraphicsUnit.Point);
			pen=new Pen(Brushes.Black, mPenWidth);
			bag=new PropertyBag(this);
			try
			{					
				bag.GetValue+=new PropertySpecEventHandler(GetPropertyBagValue);
				bag.SetValue+=new PropertySpecEventHandler(SetPropertyBagValue);
				bag.Properties.Add(new PropertySpec("Text",typeof(string),"Appearance","The text attached to the entity","[Not set]"));
				//					PropertySpec spec=new PropertySpec("MDI children",typeof(string[]));
				//					spec.Attributes=new Attribute[]{new System.ComponentModel.ReadOnlyAttribute(true)};
				//					bag.Properties.Add(spec);
				AddProperties(); //add the user defined shape properties					
					
			}
			catch(Exception exc)
			{				
				Debug.WriteLine(exc.Message);
			}

		
		}
		/// <summary>
		/// creates the actual visual element on screen
		/// </summary>
		/// <param name="g"></param>
		protected internal abstract void Paint(Graphics g);
		/// <summary>
		/// Gets the cursor for the current position of the mouse
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public abstract Cursor GetCursor(PointF p);

		/// <summary>
		/// GraphAbstract delete method; deletes the entity from the plex
		/// </summary>
		internal protected abstract void Delete();
		/// <summary>
		/// Says wether, for the given rectangle, the underlying shape is contained in it.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public abstract Boolean Hit(RectangleF r);
		/// <summary>
		/// Invalidating refreshes part or all of a control
		/// </summary>
		public abstract void Invalidate();
		/// <summary>
		/// Returns the unique identifier for the shape.
		/// </summary>
		public Guid UID
		{
			get
			{
				return mUID;
			}
			set{mUID = value;}
		}
		/// <summary>
		/// Regenerates a GUID for this entity
		/// </summary>
		internal void GenerateNewUID()
		{
			mUID=Guid.NewGuid();
			mIsReset=true;
		}


		/// <summary>
		/// Raises the mouse down event
		/// </summary>
		/// <param name="e"></param>
		internal void RaiseMouseDown(MouseEventArgs e)
		{
			if(OnMouseDown != null) OnMouseDown(this, e);
		}

		/// <summary>
		/// Raises the mouse up event
		/// </summary>
		/// <param name="e"></param>
		internal void RaiseMouseUp(MouseEventArgs e)
		{
			if(OnMouseUp != null) OnMouseUp(this, e);
		}

		internal void RaiseMouseMove(MouseEventArgs e)
		{
			if(OnMouseMove!=null) OnMouseMove(this,e);
		}
		
		#endregion
		
		
	}
}

