using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Netron.GraphLib;

using Ferda.FrontEnd.Desktop;
using Ferda.Modules;

namespace Ferda.FrontEnd.Desktop
{
    /// <summary>
    /// Defines a special Netron connector that uses a different Paint
    /// function, more suitable for the purposes of Ferda. This function
    /// draws a bitmap onto canvas.
    /// </summary>
    class FerdaConnector : Connector
    {
        #region Protected fields

        /// <summary>
        /// SVGManager that holds the bitmap of the socket
        /// </summary>
        SVGManager svgManager;

        /// <summary>
        /// A bitmap that will be drawn on the canvas in the paint method
        /// </summary>
        protected Bitmap bitmap;

        /// <summary>
        /// The socket this connector is representing
        /// </summary>
        protected SocketInfo socket;

        /// <summary>
        /// Determines if something is packed in the connector
        /// </summary>
        protected bool hasPacked;

        #endregion

        #region Properties

        /// <summary>
        /// The socket this connector is representing
        /// </summary>
        public SocketInfo Socket
        {
            get
            {
                return socket;
            }
        }

        /// <summary>
        /// Determines, if this socket packs something
        /// </summary>
        public bool HasPacked
        {
            get
            {
                return hasPacked;
            }
            set
            {
                hasPacked = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the FerdaConnector clss
        /// </summary>
        /// <param name="o">the underlying shape to which the connector belongs</param>
        /// <param name="socket">The connector will represent this socket</param>
        /// <param name="svgMan">SVGManager for drawing svg bitmaps</param>
        /// <param name="packed">If the connector is packed in the beginning</param>
        /// <remarks>
        /// Nevim jeste, jestli se oplati pro kazdy konektor drzet
        /// si svuj vlastni socket, zatim se to vyuziva jenom pri konstrukci k ziskani
        /// bitmapy
        /// </remarks>
        public FerdaConnector(Shape o, SVGManager svgMan, SocketInfo socket, bool packed)
            : base(o, socket.label, socket.moreThanOne)
        {
            //setting the svgManager and the Bitmap
            svgManager = svgMan;
            bitmap = svgManager.GetSocketBitmap(socket);

            this.socket = socket;
            hasPacked = packed;

            //this shlould eliminate the problems with connecting more than
            //one connection when the MoreThanOne property of ISocket is set to false
            this.AllowMultipleConnections = socket.moreThanOne;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Overrides the Paint of the control and paint a little connection point or a highlighted connecting widget to 
        /// show the user that a connection is possible.
        /// </summary>
        /// <remarks>
        /// The parent's Hover boolean can be used to check if the mouse is currently hovering over this object. This enables a status message or a different shape.
        /// </remarks>
        /// <param name="g">The Graphics or canvas onto which to paint.</param>
        protected override void Paint(System.Drawing.Graphics g)
        {
            Rectangle r = Rectangle.Round(ConnectionGrip());

            //Barva linky, ktera se kresli okolo konektoru
            Color Line = Color.Silver;

            if (Hover)
            {
                //stanovi se linka vyplne
                Line = Color.Red;

                //tady se testuje, jestli uz sem dosahl maximalniho poctu pripojeni
                //v pripade, ze budu moct zapojovat pouze jedno, potom to bude uz 
                //cervene a nebude se do toho moct nic zapojovat
                /*
                if ((mAllowMultipleConnections) || (_Connections.Count < 1))
                    Fill = Color.FromArgb(0, 192, 0); // medium green
                else
                    Fill = Color.FromArgb(255, 0, 0); // red
                */
            }

            //Barvime ctverecek, presne tak jak bude vypadat
            //g.FillRectangle(new SolidBrush(Fill), r);
            g.DrawImage(bitmap, r.X, r.Y, r.Width, r.Height);

            if (hasPacked)
            {
                g.DrawRectangle(new Pen(Line, 3),
                    new Rectangle(r.X - 2, r.Y - 2, r.Width + 3, r.Height + 3));
            }
            else
            {
                g.DrawRectangle(new Pen(Line, 1), r);
            }

            //Pise ctverecek s kecami okolo
            //da se tam neco napsat s poctem dalsich prvku zapojenych do tohoto konektoru
            if (Hover)
            {
                Font f = new Font("Tahoma", 8.25f);
                //Size s = g.MeasureString(Description + " " + this.Connections.Count , f).ToSize();
                Size s = g.MeasureString(mText, f).ToSize();
                Rectangle a =
                new Rectangle(r.X - (s.Width), r.Y, s.Width, s.Height + 1);
                Rectangle b = a;
                a.Inflate(+2, +1);

                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 231)), a);
                g.DrawRectangle(new Pen(Color.Black, 1), a);
                //g.DrawString(Description + " " + this.Connections.Count, f, new SolidBrush(Color.Black), b.Location);
                g.DrawString(mText, f, new SolidBrush(Color.Black), b.Location);
            }
        }

        #endregion
    }
}
