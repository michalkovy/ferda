using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;

using Netron.GraphLib.Attributes;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.Configuration;
using Netron.GraphLib;

using Ferda.FrontEnd.Desktop;
using Ferda.ProjectManager;

namespace Ferda.FrontEnd.Desktop
{
	/// <summary>
	/// Class that represents all Ferda nodes on the desktop
	/// </summary>
    /// <remarks>
    /// <para>
    /// Predavani nove polohy pro ProjectManager se deje malym trikem (duvody
    /// jsou popsane v eventech BoxNode_OnMouseDown a BoxNode_OnMouseUp). Spocita
    /// se offset polohy mysi od tlacitka pri MouseDown a potom se z polohy e.Location
    /// a offsetu spocita skutecna poloha mysi
    /// </para>
    /// <para>
    /// !!!V EVENTECH BoxNode_OnMouseDown A BoxNode_OnMouseUp 
    /// NENI IMPLEMENTOVANO ZOOMOVANI GRAPHCONTROLU!!!
    /// </para>
    /// <para>
    /// !!!IN EVENTS BoxNode_OnMouseDown AND BoxNode_OnMouseUp, 
    /// ZOOMING OF GRAPHCONTROL IS NOT TAKEN INTO ACCOUNT!!!
    /// </para>
    /// </remarks>
	[Serializable]
	[Description("Ferda graphical node")]
	[NetronGraphShape("BoxNode",
		 "a5deeaa1-9337-4b18-bfb4-5635de540b2a",
		 "Ferda Nodes",
		 "Netron.GraphLib.BasicShapes.BoxNode",
		 "Ferda basic node")]
	public class BoxNode : Shape
	{
		#region Fields

        //X and Y offsets of the mouse location during the MouseUp and MouseDown
        //events;
        private int xOffset = 0;
        private int yOffset = 0;
        private Point oldLocation;

        /// <summary>
        /// The current view for accessing the position and other functions
        /// </summary>
        protected ProjectManager.View view;

        /// <summary>
        /// A bitmap that will be drawn on the canvas in the paint method
        /// </summary>
        protected Bitmap bitmap;

		/// <summary>
		/// One input connector for every box
		/// </summary>
		protected Connector outputConnector;

		/// <summary>
		/// Collection of connectors that will be displayed on the left side
		/// </summary>
		protected ConnectorCollection inputConnectors;

        /// <summary>
        /// Box that is represented by this node
        /// </summary>
        protected ModulesManager.IBoxModule box;

        /// <summary>
        /// SVGManager that holds the bitmap of the box
        /// </summary>
        protected SVGManager svgManager;

        /// <summary>
        /// Resource Manager for localization purposes
        /// </summary>
        protected ResourceManager resManager;

		#endregion

		#region Properties

        /// <summary>
        /// Resource Manager for localization purposes
        /// </summary>
        public ResourceManager ResManager
        {
            set
            {
                resManager = value;
            }
            get
            {
                return resManager;
            }
        }

		/// <summary>
        /// One input connector for every box
		/// </summary>
		/// <remarks>
		/// Jeste nevim, jestli povolim i setovani
		/// </remarks>
		public virtual Connector OutputConnector
		{
			get
			{
				return outputConnector;
			}
			set
			{
				outputConnector = value;
			}
		}

		/// <summary>
        /// Collection of connectors that will be displayed on the left side
		/// </summary>
		/// <remarks>
		/// Jeste nevim, jestli povolim i setovani
		/// </remarks>
		public virtual ConnectorCollection InputConnectors
		{
			get
			{
				return inputConnectors;
			}
			set
			{
				inputConnectors = value;
			}
		}

        /// <summary>
        /// Box that is represented by this node
        /// </summary>
        public ModulesManager.IBoxModule Box
        {
            get
            {
                return box;
            }
        }

		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor for the class
		/// </summary>
		/// <remarks>
		///	Parametrem by se mohlo predavat jmeno vystupniho konektoru.
        /// Problem bude s resenim vystupniho konenktoru, pro ktery vlastne 
        /// neni socket ale pozaduje SVG (bud kreslit natvrdo, nebo udelat
        /// v IBoxModule pro to nejaky zvlastni socket
		/// </remarks>
        /// <param name="site">Interface of a graph site (control)
		/// </param>
        /// <param name="box">Box that is connected with this node</param>
        /// <param name="resM">Resource manager of the application</param>
        /// <param name="svgman">Provider of svg images</param>
        /// <param name="view">View where this box is located</param>
		public BoxNode(IGraphSite site, ModulesManager.IBoxModule box, 
            SVGManager svgman, ProjectManager.View view, ResourceManager resM)
            : base(site)
		{
            FerdaConstants constants = new FerdaConstants();

            //filling the node
            this.box = box;
            this.svgManager = svgman;
            this.ResManager = resM;

            //determining the size
            Rectangle = new RectangleF(0, 0, constants.KrabickaWidth, constants.KrabickaHeight);

            //adding the output connector
            //it is not a FerdaConnector, because the box has no socket defined
            outputConnector = new Connector(this, ResManager.GetString("PropertiesOutputConnector"), true);
            Connectors.Add(outputConnector);
            outputConnector.ConnectorLocation = ConnectorLocations.East;

            //adding the input connectors
            inputConnectors = new ConnectorCollection();
            for (int i = 0; i < box.Sockets.Length; i++)
            {
                bool packed = view.IsAnyBoxPackedIn(box, box.Sockets[i].name);

                inputConnectors.Add(
                    new FerdaConnector(this, svgManager, box.Sockets[i], packed));
                Connectors.Add(inputConnectors[i]);
                inputConnectors[i].ConnectorLocation = ConnectorLocations.West;
            }

            //getting the bitmap
            bitmap = svgManager.GetBoxBitmap(box);

            //setting the view
            this.view = view;

            //adding the moving handler
            OnMouseDown += new MouseEventHandler(BoxNode_OnMouseDown);
            OnMouseUp += new MouseEventHandler(BoxNode_OnMouseUp);

            Resizable = false;
            this.Text = box.UserName;
		}

		#endregion

		#region Methods

		/// <summary>
		/// The paint procedure of the node (will draw the SVG bitmap on canvas)
		/// </summary>
		/// <param name="g">GDI+ graphics handle</param>
		protected override void Paint(Graphics g)
		{
			if(recalculateSize)
			{
				Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y),
					g.MeasureString(this.mText,mFont));	
				recalculateSize = false; //very important!
			}

            //If the node is selected a red rectangle will be drawn around, otherwise
            //a white rectangle (same as the connectors)
            if (IsSelected)
            {
                g.DrawRectangle(new Pen(Color.Red), Rectangle.X - 1, Rectangle.Y - 1,
                    Rectangle.Width + 2, Rectangle.Height + 2);
            }
            else
            {
                //Michal si to tady nepreje
                //g.DrawRectangle(new Pen(Color.Silver), Rectangle.X - 1, Rectangle.Y - 1,
                //    Rectangle.Width + 2, Rectangle.Height + 2);
            }

            if (Hover)
            {
                Font creatorFont = new Font("Tahoma", 10);
                Size creatorSize = 
                    g.MeasureString(Box.MadeInCreator.Label, creatorFont).ToSize();
               
                Rectangle a
                    = new Rectangle((int)(Rectangle.X),
                        (int)(Rectangle.Y  - Rectangle.Height + 5),
                        creatorSize.Width, creatorSize.Height + 1);
                Rectangle b = a;
                a.Inflate(+2, +1);

                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 231)), a);
                g.DrawRectangle(new Pen(Color.Black, 1), a);
                g.DrawString(Box.MadeInCreator.Label, creatorFont, new SolidBrush(Color.Black), b.Location);
            }

            //Tady kreslim bitmapu
            g.DrawImage(bitmap, Rectangle.X, Rectangle.Y, Rectangle.Width + 1, Rectangle.Height + 1);
			if (ShowLabel && (mText != ""))
			{
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;
				//g.DrawString(mText, mFont, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + 3, sf);

                //determining the y coordinates of the text
                if (inputConnectors.Count > 0)
                {
                    //Determining the rectangle to show the text
                    int connectorLastY =
                        System.Drawing.Rectangle.Round(inputConnectors[inputConnectors.Count - 1].ConnectionGrip()).Bottom;

                    if (connectorLastY > Rectangle.Y + Rectangle.Height + 5)
                    {
                        g.DrawString(mText, mFont, TextBrush, Rectangle.X + (Rectangle.Width / 2), connectorLastY + 5, sf);
                    }
                    else
                    {
                        g.DrawString(mText, mFont, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + Rectangle.Height + 5, sf);
                    }
                }
                else
                {
                    g.DrawString(mText, mFont, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + Rectangle.Height + 5, sf);
                }
            }
			base.Paint(g);
		}

		/// <summary>
		/// Function retrieves the point around which the connector will be drawn. 
		/// </summary>
		/// <param name="c">Connector that wants to draw itself</param>
		/// <returns>Point where the connector should be drawn
        /// </returns>
		public override PointF ConnectionPoint(Connector c)
		{
			//counting the y coordinate of the middle of the box
			float middle = (Rectangle.Bottom - Rectangle.Top)/2 +
				Rectangle.Top;

			if (c == OutputConnector) //for the output connector
                //returns the middle of the box
				return new PointF(Rectangle.Right, middle);
			else
			{
				FerdaConstants constants = new FerdaConstants();
				PointF cPoint = new PointF(0,0);

				//checking which connector was calling the function
				int whichConnector = -1;

				for (int i = 0; i < InputConnectors.Count; i++ )
				{
					if ( c == InputConnectors[i] )
					{
						whichConnector = i;
						break;
					}
				}				

				//exception, if anything goes wrong
				if ( whichConnector == -1 )
				throw new ApplicationException("KrabickaNode.ConnectionPoint : no connector recognized");

				//counting which connector is in da middle
				//!!!! counting from 1 (not from 0)
				int middleConnector;

				if ( (InputConnectors.Count/2) == ( (float) InputConnectors.Count/2) )
					middleConnector = InputConnectors.Count/2;
				else
					middleConnector = InputConnectors.Count/2 + 1;

				//changing to count from 0
				middleConnector--;

				//setting the y coordinate
				cPoint.Y = middle + (whichConnector - middleConnector)*
					(constants.ConnectorGap + constants.ConnectorSize);

				//setting the x coordinate
				cPoint.X = Rectangle.Left - constants.ConnectorLeftOffset;

				return cPoint;
			}
		}

        /// <summary>
        /// Refreshes the text of the box
        /// </summary>
        public void RefreshText()
        {
            Text = Box.UserName;
        }

        /// <summary>
        /// The function selects or deselects the node (box) on the desktop
        /// </summary>
        /// <param name="select">To select or deselects</param>
        public void Select(bool select)
        {
            IsSelected = select;
        }

		#endregion

        #region Events

        /// <summary>
        /// This event is raised when the user presses a button on the mouse.
        /// It should remember the offset from the coordinates for usage in
        /// the BoxNode_OnMouseUp event, which will pass the change in 
        /// position to the ProjectManager. More in czech remarks.
        /// </summary>
        /// <remarks>
        /// <para>
        /// V Netronu provadi presun nodu GraphControl a dela to tak, ze nejdrive
        /// raisne event, az potom dal v kodu zahlasi zmenu polohy controlu. Tudiz
        /// kdyz zpracovavam event, vlastnosti X a Y jeste nevi, ze sou zmenene. 
        /// Proto v tady propocitavam offset od e.Location a potom to pocitam
        /// nejak zpatky.
        /// </para>
        /// <para>
        /// !!!V TETO PROCEDURE NENI IMPLEMENTOVANO ZOOMOVANI GRAPHCONTROLU!!!
        /// </para>
        /// <para>
        /// !!!IN THIS EVENT, ZOOMING OF GRAPHCONTROL IS NOT TAKEN INTO ACCOUNT!!!
        /// </para>
        /// </remarks>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void BoxNode_OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xOffset = e.X - (int) this.X;
                yOffset = e.Y - (int) this.Y;
                oldLocation = e.Location;
            }
        }

        /// <summary>
        /// This event is raised when the user releases a button on the mouse.
        /// It should calculate the movement of the node by recalculating the
        /// offset of the click and the new position of the click and pass the
        /// new position to the ProjectManager. More in czech remarks.
        /// </summary>
        /// <remarks>
        /// <para>
        /// V Netronu provadi presun nodu GraphControl a dela to tak, ze nejdrive
        /// raisne event, az potom dal v kodu zahlasi zmenu polohy controlu. Tudiz
        /// kdyz zpracovavam event, vlastnosti X a Y jeste nevi, ze sou zmenene. 
        /// Proto v tady z propocitaneho offsetu pocitam zpatky polohu krabicky, 
        /// podle toho urcuju jestli se hnula a hlasim ProjectManageru
        /// </para>
        /// <para>
        /// !!!V TETO PROCEDURE NENI IMPLEMENTOVANO ZOOMOVANI GRAPHCONTROLU!!!
        /// </para>
        /// <para>
        /// !!!IN THIS EVENT, ZOOMING OF GRAPHCONTROL IS NOT TAKEN INTO ACCOUNT!!!
        /// </para>
        /// </remarks>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void BoxNode_OnMouseUp(object sender, MouseEventArgs e)
        {
            PointF newPosition = new PointF();

            if (e.Button == MouseButtons.Left)
            {
                if (e.Location != oldLocation) //the mouse has moved
                {
                    newPosition.X = e.X - xOffset;
                    newPosition.Y = e.Y - yOffset;
                    view.SetPosition(box, newPosition);

                    //reseting the previous values for new usage
                    xOffset = 0;
                    yOffset = 0;
                }
            }
        }

        #endregion
    }
}
