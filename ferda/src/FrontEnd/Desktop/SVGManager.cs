// SVGManager.cs - class that controls the svg rendering on the desktop
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Martin Ralbovský
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Ferda.ModulesManager;
using Ferda.Modules;
using SharpVectors.Dom.Svg;
using SharpVectors.Renderer.Gdi;


namespace Ferda.FrontEnd.Desktop 
{
    /// <summary>
	/// Repository of all svg strings for all boxmodules in the system.
    /// Class will provide additional functionality for extracting GDI+
    /// code from svg
	/// </summary>
    /// <remarks>
    /// The recent implementation is without svg support, although the 
    /// code that turns svg into gdi+ is already written. It is because
    /// the external SVG library that we use does not support all the
    /// SVG versions and moreover, it slows the program a lot.
    /// </remarks>
	///<stereotype>container</stereotype>
    public class SVGManager
	{
        /// <summary>
        /// Default constructor - for now loads the socket and box bitmaps
        /// </summary>
        public SVGManager(Control control)
        {
            socket = new Bitmap("socket.bmp");
            box = new Bitmap("box.bmp");

            boxBitmaps = new Dictionary<string, Bitmap>();
            myControl = control;
        }

        ///<summary>
        ///HashTable for storing the bitmaps of svgs of boxes
        ///</summary>
        protected Dictionary<string, Bitmap> boxBitmaps;
        
        /// <summary>
        /// The socket image when there is no socket svg (there can be one)
        /// </summary>
        protected Bitmap socket;
        /// <summary>
        /// The box image when there is no boxDesign.svg file of the box
        /// </summary>
        protected Bitmap box;

        /// <summary>
        /// Required by SharpVectorLibrary to paint on some control
        /// </summary>
        protected Control myControl;

        /// <summary>
        /// Function returns the bitmap of the BoxModule. It creates the bitmap
        /// from the SVG if the bitmap is not created
        /// </summary>
        /// <param name="boxModule">Box that wants the bitmap</param>
        /// <returns>Bitmap of the box</returns>
        public Bitmap GetBoxBitmap(ModulesManager.IBoxModule boxModule)
        {
            IBoxModuleFactoryCreator creator = boxModule.MadeInCreator;

            //prozatim je to takhle
            if (creator.Icon.Length != 0)
            {
                MemoryStream stream = new MemoryStream(boxModule.MadeInCreator.Icon);
                Icon icon = new Icon(stream);
                return icon.ToBitmap();
            }
            else
            {
                return box;
            }

            //bude to ale takhle
            //if (boxBitmaps.ContainsKey(creator.Identifier))
            //{
            //    return boxBitmaps[creator.Identifier];
            //}
            //else
            //{
            //    there is a svg design file
            //    if (creator.Design != string.Empty)
            //    {
            //        SvgWindow window;
            //        GdiRenderer renderer;

            //        setting up the renderer and the svgWindow
            //        renderer = new GdiRenderer();
            //        window = new SvgWindow(myControl, renderer);
            //        SvgDocument document = new SvgDocument(window);
            //        document.LoadXml(creator.Design);

            //        renderer.Render(window.Document as SvgDocument);
            //        Bitmap image = renderer.RasterImage;

            //        boxBitmaps.Add(creator.Identifier, image);
            //        return image;
            //    }
            //    there is no svg design file
            //    else
            //    {
            //        return box;
            //    }
            //}
        }

        /// <summary>
        /// Function returns the bitmap of the associated Socket. It creates the bitmap
        /// from the SVG if the bitmap is not created
        /// </summary>
        /// <param name="socket">Socket that wants the bitmap</param>
        /// <returns>bitmap of the socket</returns>
        public Bitmap GetSocketBitmap(SocketInfo socket)
        {
            //Postup obdobny jako u GetBox Bitmap

            return this.socket;
        }
	}
}
