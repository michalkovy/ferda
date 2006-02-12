// View.cs - Representation of desktop for frontends
//
// Author: Michal Kováč <michal.kovac.develop@centrum.cz>
//
// Copyright (c) 2005 Michal Kováč 
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
using System.Collections.Generic;
using Ferda;
using Ferda.ModulesManager;
using Ferda.Modules;
using System.Drawing;

namespace Ferda {
    namespace ProjectManager {

		/// <summary>
		/// View in project.
		/// </summary>
		/// <remarks>
		/// View is collection of boxes with positions.
		/// </remarks>
		public class View {
			private string name;
			private Dictionary<int,PointF> positions =
				new Dictionary<int,PointF>();
			private Dictionary<int,PointF> oldPositions =
				new Dictionary<int,PointF>();
			//private const int minLenghtFromBox = 20;
			private const int defaultPositionX = 40;
			private const int defaultPositionY = 20;
			private const int defaultShiftPositionX = 80;
            private const int defaultShiftPositionY = 70;
            private const int boxShiftHeight = 60;
            private const int boxShiftWidth = 60;
			private Archive archive;
			private ModulesManager.ModulesManager modulesManager;

			/// <summary>
			/// Constructs new view
			/// </summary>
			/// <param name="archive">An <see cref="T:Ferda.ProjectManager.Archive"/>
			/// with boxes in project</param>
			/// <param name="modulesManager">A <see cref="T:Ferda.ModulesManager.ModulesManager"/>
            /// representing Modules manager</param>
			/// <param name="name">A string representing name of view</param>
			protected internal View(Archive archive, ModulesManager.ModulesManager modulesManager, string name)
			{
				this.archive = archive;
				this.modulesManager = modulesManager;
				this.name = name;
			}
			
			/// <summary>
			/// Adds box to view on specified position. Is equivalent vith showing
			/// of box on specified position.
			/// </summary>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/> representing box from archive</param>
			/// <param name="position">A <see cref="T:System.Drawing.PointF"/> representing
			/// place on view</param>
			/// <seealso cref="M:Ferda.ProjectManager.View.AddRange(Ferda.ModulesManager.IBoxModule[])"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.Add(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.Remove(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="P:Ferda.ProjectManager.View.Boxes"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.GetPosition(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.SetPosition(Ferda.ModulesManager.IBoxModule,System.Drawing.PointF)"/>
            public void Add(IBoxModule box, PointF position) {
				positions[box.ProjectIdentifier] = position;
            }

            /// <summary>
            /// This algorithm move box up or down in view to not be in colision with other box
            /// </summary>
            /// <param name="x">Position from left of box</param>
            /// <param name="y">Position from top of box</param>
            /// <param name="boxProjectIdentifier">project identifier of box</param>
            private void moveBoxUpDown(float x, ref float y, int boxProjectIdentifier)
            {
                float minUp = y;
                float minDown = y;
                bool somethingDone = true;
                while (somethingDone)
                {
                    somethingDone = false;
                    foreach (KeyValuePair<int, PointF> position in positions)
                    {
                        if (position.Key != boxProjectIdentifier && position.Value.X > x - boxShiftWidth && position.Value.X < x + boxShiftWidth)
                        {
                            if (position.Value.Y < minUp + boxShiftHeight && position.Value.Y >= minUp - boxShiftHeight)
                            {
                                somethingDone = true;
                                minUp = position.Value.Y - boxShiftHeight;
                            }
                            if (position.Value.Y > minDown - boxShiftHeight && position.Value.Y <= minDown + boxShiftHeight)
                            {
                                somethingDone = true;
                                minDown = position.Value.Y + boxShiftHeight;
                            }
                        }
                    }
                }
                if ((minDown - y) <= (y - minUp) || minUp < 0)
                {
                    y = minDown;
                }
                else
                {
                    y = minUp;
                }
            }
			
			/// <summary>
			/// Adds box to view. Is equivalent with showing of box.
			/// </summary>
			/// <remarks>
			/// It will count position where to show. If you would like to
			/// add more boxes in one time use
			/// <see cref="M:Ferda.ProjectManager.View.AddRange(Ferda.ModulesManager.IBoxModule[])"/>
			/// instead of this method.
			/// </remarks>
			/// <note type="implementnotes">implemented very badly in this time</note>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from archive</param>
			/// <seealso cref="M:Ferda.ProjectManager.View.Add(Ferda.ModulesManager.IBoxModule,System.Drawing.PointF)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.AddRange(Ferda.ModulesManager.IBoxModule[])"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.Remove(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="P:Ferda.ProjectManager.View.Boxes"/>
            public void Add(IBoxModule box) {
				PointF positionOld;
				if(this.oldPositions.TryGetValue(box.ProjectIdentifier,out positionOld))
				{
					positions[box.ProjectIdentifier] = positionOld;
					oldPositions.Remove(box.ProjectIdentifier);
					return;
				}
				int countt = 0;
				int countf = 0;
				float sumty = 0;
				float sumtx = 0;
				float sumfy = 0;
				float sumfx = 0;
				foreach(IBoxModule otherBox in archive.ConnectedTo(box))
				{
					PointF position;
					if(this.positions.TryGetValue(otherBox.ProjectIdentifier,out position))
					{
						sumty += position.Y;
						sumtx += position.X;
						countt++;
					}
				}
				foreach(IBoxModule otherBox in archive.ConnectionsFrom(box))
				{
					PointF position;
					if(this.positions.TryGetValue(otherBox.ProjectIdentifier,out position))
					{
						sumfy += position.Y;
						sumfx += position.X;
						countf++;
					}
				}
				float xt,yt;
				float xf,yf;
				float x,y;
				if(countt>0)
				{
					yt = (sumty / countt);
					xt = (sumtx / countt);
					if(countf>0)
					{
						yf = (sumfy / countf);
						xf = (sumfx / countf);
						x = (xt + xf) / 2;
						y = (yt + yf) / 2;
					}
					else
					{
						x = xt - defaultShiftPositionX;
						if(x < 0) x = 0;
						y = yt;
					}
				}
				else
				{
					if(countf>0)
					{
						yf = (sumfy / countf);
						xf = (sumfx / countf);
						x = xf + defaultShiftPositionX;
						y = yf;
					}
					else
					{
						x = defaultPositionX;
						y = defaultPositionY;
					}
				}
                moveBoxUpDown(x, ref y, box.ProjectIdentifier);
				positions[box.ProjectIdentifier] = new PointF(x,y);
            }
			
			/// <summary>
			/// Adds boxes to view. Is equivalent vith showing of boxes.
			/// </summary>
			/// <remarks>
			/// It will count positions where to show.
			/// </remarks>
			/// <note type="implementnotes">implemented very badly in this time</note>
			/// <param name="boxes">An array of
			/// <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing boxes from archive</param>
			/// <seealso cref="M:Ferda.ProjectManager.View.Add(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.Add(Ferda.ModulesManager.IBoxModule,System.Drawing.PointF)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.Remove(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="P:Ferda.ProjectManager.View.Boxes"/>
			public void AddRange(IBoxModule[] boxes)
			{
                //TODO can be made better
				foreach(IBoxModule box in boxes)
				{
					this.Add(box);
				}
			}

			/// <summary>
			/// Removes box from view. Is equivalent with hiding of box.
			/// </summary>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
			/// <seealso cref="M:Ferda.ProjectManager.View.Add(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.Add(Ferda.ModulesManager.IBoxModule,System.Drawing.PointF)"/>
			/// <seealso cref="P:Ferda.ProjectManager.View.Boxes"/>
			public void Remove(IBoxModule box) {
				//TODO can be made better
				PointF position;
				if(this.positions.TryGetValue(box.ProjectIdentifier,out position))
				{
					oldPositions[box.ProjectIdentifier] = position;
				}
				positions.Remove(box.ProjectIdentifier);
            }

			/// <summary>
			/// Gets a position of box in this view.
			/// </summary>
			/// <returns>A <see cref="T:System.Drawing.PointF"/> representing
			/// place on view</returns>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
			/// <seealso cref="M:Ferda.ProjectManager.View.SetPosition(Ferda.ModulesManager.IBoxModule,System.Drawing.PointF)"/>
            public PointF GetPosition(IBoxModule box) {
				return positions[box.ProjectIdentifier];
            }

			/// <summary>
			/// Sets position of box in this view.
			/// </summary>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
			/// <param name="position">A <see cref="T:System.Drawing.PointF"/> representing
			/// place on view</param>
			/// <seealso cref="M:Ferda.ProjectManager.View.GetPosition(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.Add(Ferda.ModulesManager.IBoxModule,System.Drawing.PointF)"/>
            public void SetPosition(IBoxModule box, PointF position) {
				positions[box.ProjectIdentifier] = position;
            }

			/// <summary>
			/// Looks if box is in this view visible.
			/// </summary>
			/// <returns>A Boolean value saying if <paramref name="box"/> is in view.
			/// </returns>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
            public bool ContainsBox(IBoxModule box) {
				return positions.ContainsKey(box.ProjectIdentifier);
            }

			/// <summary>
			/// User name of view
			/// </summary>
			/// <value>
			/// String representing name of view
			/// </value>
            public string Name {

                get {
                    return name;
                }

                set {
					name = value;
                }
            }
			
			/// <summary>
			/// Boxes in view
			/// </summary>
			/// <value>
			/// An array of <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing boxes in view
			/// </value>
            public IBoxModule[] Boxes {

                get {
					List<IBoxModule> result = new List<IBoxModule>();
					foreach(IBoxModule box in archive.Boxes)
					{
						if(positions.ContainsKey(box.ProjectIdentifier))
							result.Add(box);
					}
					return result.ToArray();
                }
            }
			
			/// <summary>
			/// Creates boxes from structure
			/// <see cref="T:Ferda.Modules.ModulesAskingForCreation"/>
			/// and add it to view.
			/// </summary>
			/// <remarks>
			/// You can get <see cref="T:Ferda.Modules.ModulesAskingForCreation"/>
			/// structure from property
			/// <see cref="P:Ferda.ModulesManager.IBoxModule.ModulesAskingForCreation"/>
			/// of <see cref="T:Ferda.ModulesManager.IBoxModule"/>.
			/// </remarks>
			/// <param name="info">A  ModulesAskingForCreation</param>
			/// <seealso cref="P:Ferda.ModulesManager.IBoxModule.ModulesAskingForCreation"/>
			public void CreateBoxesAskingForCreation(ModulesAskingForCreation info)
			{
				IBoxModule[] boxes = modulesManager.CreateBoxesAskingForCreation(info);
				foreach (IBoxModule box in boxes)
				{
					archive.Add(box);
					this.Add(box);
				}
			}
			
			/// <summary>
			/// Connections in this view
			/// </summary>
			/// <value>
			/// An list of <see cref="T:Ferda.ProjectManager.Connection"/>
			/// representing connections in this view
			/// </value>
			public List<Connection> Connections {
				get {
					List<Connection> result = new List<Connection>();
					foreach(int boxProjectIdentifier in positions.Keys)
					{
						foreach(IBoxModule box in archive.Boxes)
						{
							if(box.ProjectIdentifier == boxProjectIdentifier)
							{
								foreach (SocketInfo socket in box.Sockets)
								{
									foreach(IBoxModule fromBox in box.GetConnections(socket.name))
									{
										if(this.ContainsBox(fromBox))
											result.Add(
												new Connection(fromBox,box,socket.name));
									}
								}
							}
						}
					}
					return result;
				}
			}
			
            /// <summary>
            /// Recursive method for packing of a socket
            /// </summary>
            /// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
            /// representing box from view which socket has to be packed</param>
            /// <param name="socketName">A string representing name
            /// of socket of box <paramref name="box"/></param>
            /// <param name="firstUse">If this method was called first time or in recursive propagation</param>
            /// <seealso cref="M:Ferda.ProjectManager.View.PackAllSockets(Ferda.ModulesManager.IBoxModule,System.Boolean)"/>
            /// <seealso cref="M:Ferda.ProjectManager.View.PackSocket(Ferda.ModulesManager.IBoxModule,System.String)"/>
            /// <seealso cref="M:Ferda.ProjectManager.View.PackAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			private void PackSocket(IBoxModule box, string socketName, bool firstUse)
			{
				foreach(IBoxModule otherBox in box.GetConnections(socketName))
				{
					if(this.ContainsBox(otherBox))
					{
						bool noOther = true;
						if(!firstUse)
						{
							foreach(IBoxModule secBox in otherBox.ConnectedTo())
							{
								if(this.ContainsBox(secBox) &&
								   secBox != box &&
								   secBox != otherBox)
								{
									noOther=false;
									break;
								}
							}
						}
						if(noOther)
						{
							this.Remove(otherBox);
							this.PackAllSockets(otherBox,false);
						}
					}
				}
			}
			
			/// <summary>
			/// Packs socket
			/// </summary>
			/// <remarks>
			/// Hides all boxes connected to socket <paramref name="socketName"/>
			/// of box <paramref name="box"/> which are not used by other box
			/// and if it hides some box, hides also boxes connected to that
			/// box which are not used by other box.
			/// </remarks>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
			/// <param name="socketName">A string representing name
			/// of socket of box <paramref name="box"/></param>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.IsAnyBoxPackedIn(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayer(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayerAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayers(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayersAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			public void PackSocket(IBoxModule box, string socketName)
			{
				PackSocket(box, socketName, true);
			}

            /// <summary>
            /// Method for packing all sockets of box
            /// </summary>
            /// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
            /// representing box from view which sockets have to be packed</param>
            /// <param name="firstUse">If this method was called first time or in recursive propagation</param>
            /// <seealso cref="M:Ferda.ProjectManager.View.PackSocket(Ferda.ModulesManager.IBoxModule,System.Boolean)"/>
            /// <seealso cref="M:Ferda.ProjectManager.View.PackAllSockets(Ferda.ModulesManager.IBoxModule)"/>
            /// <seealso cref="M:Ferda.ProjectManager.View.PackSocket(Ferda.ModulesManager.IBoxModule,System.String)"/>
			private void PackAllSockets(IBoxModule box, bool firstUse)
			{
				foreach(SocketInfo socket in box.Sockets)
				{
					this.PackSocket(box,socket.name,firstUse);
				}
			}
			
			/// <summary>
			/// Packs all sockets of box
			/// </summary>
			/// <remarks>
			/// Hides all boxes connected to box <paramref name="box"/> which
			/// are not used by other box
			/// and if it hides some box, hides also boxes connected to that
			/// box which are not used by other box.
			/// </remarks>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackSocket(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.IsAnyBoxPackedIn(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayer(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayerAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayers(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayersAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			public void PackAllSockets(IBoxModule box)
			{
				PackAllSockets(box, true);
			}
			
			/// <summary>
			/// Looks if is some box is packed in socket <paramref name="socketName"/>
			/// of box <paramref name="box"/>.
			/// </summary>
			/// <remarks>
			/// Looks if
			/// <see cref="M:Ferda.ProjectManager.View.UnpackOneLayer(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// will unpack something when called with box <paramref name="box"/>
			/// and socket <paramref name="socketName"/>.
			/// </remarks>
            /// <returns>True if some box is packed in socket <paramref name="socketName"/>
            /// of box <paramref name="box"/></returns>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
			/// <param name="socketName">A string representing name
			/// of socket of box <paramref name="box"/></param>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackSocket(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayer(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayerAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayers(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayersAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			public bool IsAnyBoxPackedIn(IBoxModule box, string socketName)
			{
				foreach(IBoxModule otherBox in box.GetConnections(socketName))
				{
					if(!this.ContainsBox(otherBox))
					{
						return true;
					}
				}
				return false;
			}
			
			/// <summary>
			/// Unpacks one layer of boxes connected to <paramref name="socketName"/>
			/// of box <paramref name="box"/>.
			/// </summary>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
			/// <param name="socketName">A string representing name
			/// of socket of box <paramref name="box"/></param>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackSocket(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.IsAnyBoxPackedIn(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayerAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayers(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayersAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			public void UnpackOneLayer(IBoxModule box, string socketName)
			{
				foreach(IBoxModule otherBox in box.GetConnections(socketName))
				{
					if(!this.ContainsBox(otherBox))
					{
						this.Add(otherBox);
					}
				}
			}
			
			/// <summary>
			/// Unpacks one layer of boxes connected to box <paramref name="box"/>.
			/// </summary>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackSocket(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.IsAnyBoxPackedIn(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayer(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayers(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayersAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			public void UnpackOneLayerAllSockets(IBoxModule box)
			{
				foreach(SocketInfo socket in box.Sockets)
				{
					this.UnpackOneLayer(box,socket.name);
				}
			}
			
			/// <summary>
			/// Unpacks all layers of boxes connected to <paramref name="socketName"/>
			/// of box <paramref name="box"/>.
			/// </summary>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
			/// <param name="socketName">A string representing name
			/// of socket of box <paramref name="box"/></param>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackSocket(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.IsAnyBoxPackedIn(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayer(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayerAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayersAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			public void UnpackAllLayers(IBoxModule box, string socketName)
			{
				foreach(IBoxModule otherBox in box.GetConnections(socketName))
				{
					if(!this.ContainsBox(otherBox))
					{
						this.Add(otherBox);
						UnpackAllLayersAllSockets(otherBox);
					}
				}
			}
			
			/// <summary>
			/// Unpacks all layers of boxes connected to box <paramref name="box"/>.
			/// </summary>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing box from view</param>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackSocket(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.PackAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.IsAnyBoxPackedIn(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayer(Ferda.ModulesManager.IBoxModule,System.String)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackOneLayerAllSockets(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="M:Ferda.ProjectManager.View.UnpackAllLayers(Ferda.ModulesManager.IBoxModule,System.String)"/>
			public void UnpackAllLayersAllSockets(IBoxModule box)
			{
				foreach(SocketInfo socket in box.Sockets)
				{
					this.UnpackAllLayers(box,socket.name);
				}
			}

            /// <summary>
            /// Looks for boxes transitively connected to boxes <paramref name="boxes"/>
            /// but not containing <paramref name="oldBoxes"/>.
            /// </summary>
            /// <remarks>
            /// This is recursive
            /// function, first called witht clean list <paramref name="oldBoxes"/> and with
            /// one box in <paramref name="boxes"/>. This function is used for finding
            /// components of boxes connected together in view.
            /// </remarks>
            /// <param name="boxes">A list of <see cref="T:Ferda.ModulesManager.IBoxModule"/>
            /// representing some boxes in view. We are finding boxes around these boxes.</param>
            /// <param name="oldBoxes">A list of <see cref="T:Ferda.ModulesManager.IBoxModule"/>
            /// representing some boxes in view. We do not want to see these boxes in result</param>
            /// <returns>An list of <see cref="T:Ferda.ModulesManager.IBoxModule"/> which contains
            /// boxes around boxes in <paramref name="boxes"/> but does not contain
            /// boxes in <paramref name="oldBoxes"/></returns>
            private List<IBoxModule> boxesAround(List<IBoxModule> boxes, List<IBoxModule> oldBoxes)
            {
                List<IBoxModule> result = new List<IBoxModule>(boxes);
                List<IBoxModule> newBoxes = new List<IBoxModule>();
                foreach (IBoxModule box in boxes)
                {
                    foreach (IBoxModule otherBox in box.ConnectionsFrom())
                    {
                        if (this.ContainsBox(otherBox) && 
                            !oldBoxes.Contains(otherBox) && 
                            !result.Contains(otherBox))
                        {
                            result.Add(otherBox);
                            newBoxes.Add(otherBox);
                        }
                    }
                    foreach (IBoxModule otherBox in box.ConnectedTo())
                    {
                        if (this.ContainsBox(otherBox) &&
                            !oldBoxes.Contains(otherBox) &&
                            !result.Contains(otherBox))
                        {
                            result.Add(otherBox);
                            newBoxes.Add(otherBox);
                        }
                    }
                }
                oldBoxes.AddRange(boxes);
                if (newBoxes.Count>0)
                    result.AddRange(boxesAround(newBoxes, oldBoxes));
                return result;
            }

            /// <summary>
            /// Counts components of boxes connected together in view.
            /// </summary>
            /// <returns>An list of components, where component is list of
            /// <see cref="T:Ferda.ModulesManager.IBoxModule"/> representation
            /// of box</returns>
            private List<List<IBoxModule>> getComponents()
            {
                List<List<IBoxModule>> components = new List<List<IBoxModule>>();
                IBoxModule[] boxesInView = this.Boxes;
                if (boxesInView.Length == 0) return new List<List<IBoxModule>>();
                List<IBoxModule> firstItem = new List<IBoxModule>();
                firstItem.Add(boxesInView[0]);
                int i = 0;
                bool finded;
                do
                {
                    components.Add(boxesAround(firstItem, new List<IBoxModule>()));
                    List<IBoxModule> boxesInViewCopy = new List<IBoxModule>(boxesInView);
                    foreach (List<IBoxModule> component in components)
                    {
                        foreach (IBoxModule box in component)
                        {
                            boxesInViewCopy.Remove(box);
                        }
                    }
                    i++;
                    finded = (boxesInViewCopy.Count > 0);
                    if (finded)
                    {
                        firstItem = new List<IBoxModule>();
                        firstItem.Add(boxesInViewCopy[0]);
                    }
                } while (finded);
                return components;
            }

            /// <summary>
            /// Counts position of boxes in easier representation of positions (topology)
            /// of boxes.
            /// This representation is made by two integers whith meens on which place 
            /// is box from left an on whitch place from top.
            /// </summary>
            /// <remarks>
            /// This algorithm first counts components <see cref="M:getComponents()"/>, than
            /// it try to find on which place is box from left than from top.
            /// </remarks>
            /// <param name="fromTop">Where are boxes positioned from top</param>
            /// <returns>Where are boxes positioned from left</returns>
            private Dictionary<IBoxModule, int> getTopology(out Dictionary<IBoxModule, int> fromTop)
            {
                fromTop = new Dictionary<IBoxModule, int>();

                // Get components of connected boxes in view
                List<List<IBoxModule>> components = getComponents();
                IBoxModule[] boxesInView = this.Boxes;
                if (boxesInView.Length == 0) return new Dictionary<IBoxModule, int>();

                // Creating connections from relation in view from which 
                // we will later remove (like topology sort algorithm)
                Dictionary<IBoxModule, List<IBoxModule>> connectionsFrom = new Dictionary<IBoxModule, List<IBoxModule>>();
                foreach (IBoxModule box in boxesInView)
                {
                    List<IBoxModule> otherBoxes = new List<IBoxModule>();
                    foreach (IBoxModule otherBox in box.ConnectionsFrom())
                    {
                        if (this.ContainsBox(otherBox))
                            otherBoxes.Add(otherBox);
                    }
                    connectionsFrom[box] = otherBoxes;
                }

                // Topology from left
                Dictionary<IBoxModule, int> topology = new Dictionary<IBoxModule, int>();
                foreach (List<IBoxModule> component in components)
                {
                    // This list will represent boxes which does not have topology set yet
                    // We will later remove boxes from this list like topology sort algorithm
                    // but we will remove sometimes more boxes together
                    List<IBoxModule> boxesToGoThrough = new List<IBoxModule>(component);

                    for (int i = 0; boxesToGoThrough.Count > 0; i++)
                    {
                        // List of boxes that does not have something in connection from
                        // relation (of special if there is cycle). Boxes in this list
                        // will have topology i.
                        List<IBoxModule> findedBoxes = new List<IBoxModule>();

                        // Find boxes that dont have connections from boxes which dont have
                        // topology set
                        foreach (IBoxModule box in boxesToGoThrough)
                        {
                            if (connectionsFrom[box].Count == 0)
                            {
                                topology[box] = i;
                                findedBoxes.Add(box);
                            }
                        }

                        // If there are non box like that, there have to be some cycle. So
                        // take some box with minimum connections form other boxes with not set
                        // topology
                        if (findedBoxes.Count == 0)
                        {
                            int minimum = Int32.MaxValue;
                            IBoxModule minimumBox = null;
                            foreach (IBoxModule box in component)
                            {
                                if (connectionsFrom[box].Count < minimum)
                                {
                                    minimum = connectionsFrom[box].Count;
                                    minimumBox = box;
                                }
                            }
                            connectionsFrom[minimumBox].Clear();
                            topology[minimumBox] = i;
                            findedBoxes.Add(minimumBox);
                        }
                        
                        // Remove boxes which have topology set
                        foreach (IBoxModule box in findedBoxes)
                            boxesToGoThrough.Remove(box);

                        // Create connections from relation actual
                        foreach (IBoxModule box in boxesToGoThrough)
                        {
                            foreach (IBoxModule findedBox in findedBoxes)
                            {
                                if (connectionsFrom[box].Contains(findedBox))
                                    connectionsFrom[box].Remove(findedBox);
                            }
                        }
                    }
                }

                // Now topology from left is mostly OK, but there can be something like:
                // X-X-X
                // X--/
                // where X is a box and - or --/ a connection
                // better way:
                // X-X-X
                //   X/
                // So we will move boxes to right to boxes on right how much it is possible

                // if there was some movement to right
                bool somethingDone;
                do
                {
                    somethingDone = false;
                    foreach (IBoxModule box in boxesInView)
                    {
                        int boxTopology = topology[box];
                        int minimum = Int32.MaxValue;
                        bool somethingAfter = false;
                        foreach (IBoxModule otherBox in box.ConnectedTo())
                        {
                            if (this.ContainsBox(otherBox))
                            {
                                somethingAfter = true;
                                if (topology[otherBox] < minimum)
                                    minimum = topology[otherBox];
                            }
                        }
                        minimum--;
                        if (somethingAfter && minimum > boxTopology)
                        {
                            somethingDone = true;
                            topology[box] = minimum;
                        }
                    }
                }
                while (somethingDone);

                //tops = topology from top
                Dictionary<int, List<int>>topologiesTops = new Dictionary<int, List<int>>();
                int topsMinimum = 0;
                foreach (List<IBoxModule> component in components)
                {
                    // temporary topology from top
                    Dictionary<IBoxModule, int> temporaryFromTop = new Dictionary<IBoxModule, int>();
                    
                    // temporary maximum height of tops in columns representet by topology from left
                    Dictionary<int, int> temporaryMaxTop = new Dictionary<int, int>();

                    // Now we will recursively go from boxes on right (which dont have connections
                    // to other boxes) to boxes on left and set their temporary topologies from top
                    IBoxModule lastBox = findSomethingNewLast(temporaryFromTop, component);
                    while (lastBox != null)
                    {
                        recurseTopsDown(lastBox, temporaryFromTop, temporaryMaxTop, component, topology);
                        lastBox = findSomethingNewLast(temporaryFromTop, component);
                    }

                    // We know in this if some box have to be more up or down than other box in
                    // one column. So we will count on which place exactly it has to be.
                    // So from:
                    // X-X X
                    //  \X/
                    // We will do:
                    // X-X
                    //  \X-X
                    createBetterTops(temporaryFromTop, temporaryMaxTop, component, topology);

                    // Easy but not best implementation of how to put components together
                    // on ane view
                    int nextTopsMinimum = topsMinimum;
                    foreach (KeyValuePair<IBoxModule, int> oneTop in temporaryFromTop)
                    {
                        int newValue = oneTop.Value + topsMinimum;
                        fromTop[oneTop.Key] = newValue;
                        if (newValue + 1 > nextTopsMinimum)
                        {
                            nextTopsMinimum = newValue + 1;
                        }
                    }
                    topsMinimum = nextTopsMinimum;
                }
                
                return topology;
            }

            /// <summary>
            /// Finds some box which is not connected to other box in view and
            /// does not have temporary topology from top set
            /// </summary>
            /// <param name="temporaryFromTop">Temporary topology from top</param>
            /// <param name="component">Component of boxes in which we are trying to find
            /// a box</param>
            /// <returns>An <see cref="T:Ferda.ModulesManager.IBoxModule"/> from component
            /// <paramref name="component"/> which is not connected
            /// to other box in view and
            /// does not have temporary topology from top set</returns>
            private IBoxModule findSomethingNewLast(Dictionary<IBoxModule, int> temporaryFromTop, List<IBoxModule> component)
            {
                foreach (IBoxModule box in component)
                {
                    if (!temporaryFromTop.ContainsKey(box))
                    {
                        bool somethingAfter = false;
                        foreach (IBoxModule otherBox in box.ConnectedTo())
                        {
                            if (component.Contains(otherBox))
                            {
                                somethingAfter = true;
                                break;
                            }
                        }
                        if (!somethingAfter) return box;
                    }
                }
                return null;
            }

            /// <summary>
            /// Recursively set topology from top of boxes transitively connected to box
            /// <paramref name="lastBox"/>
            /// </summary>
            /// <param name="lastBox">Box most on right to which topology from right will be set</param>
            /// <param name="temporaryFromTop">Topology from top</param>
            /// <param name="temporaryMaxTop">Maximums of topology from top in topologies from left</param>
            /// <param name="component">Component of boxes</param>
            /// <param name="topology">Topology from left</param>
            private void recurseTopsDown(IBoxModule lastBox, Dictionary<IBoxModule, int> temporaryFromTop, Dictionary<int, int> temporaryMaxTop, List<IBoxModule> component, Dictionary<IBoxModule, int> topology)
            {
                if (!temporaryFromTop.ContainsKey(lastBox))
                {
                    int fromLeft = topology[lastBox];
                    int top;
                    if (!temporaryMaxTop.ContainsKey(fromLeft))
                    {
                        temporaryMaxTop[fromLeft] = 0;
                        top = 0;
                    }
                    else
                    {
                        top = temporaryMaxTop[fromLeft] + 1;
                        temporaryMaxTop[fromLeft] = top;
                    }
                    temporaryFromTop[lastBox] = top;

                    foreach (SocketInfo socket in lastBox.Sockets)
                    {
                        foreach (IBoxModule box in lastBox.GetConnections(socket.name))
                        {
                            if (component.Contains(box))
                                recurseTopsDown(box, temporaryFromTop, temporaryMaxTop, component, topology);
                        }
                    }
                }
            }

            /// <summary>
            /// Make topology from top better than only to know what is more on top than other
            /// </summary>
            /// <param name="temporaryFromTop">Topology from top</param>
            /// <param name="temporaryMaxTop">Maximums of topology from top in topologies from left</param>
            /// <param name="component">Component of boxes</param>
            /// <param name="topology">Topology from left</param>
            private void createBetterTops(Dictionary<IBoxModule, int> temporaryFromTop, Dictionary<int, int> temporaryMaxTop, List<IBoxModule> component, Dictionary<IBoxModule, int> topology)
            {
                int maxTop = 0;
                int maxTopId = 0;
                foreach (KeyValuePair<int,int> maxTopPair in temporaryMaxTop)
                {
                    if (maxTopPair.Value > maxTop)
                    {
                        maxTop = maxTopPair.Value;
                        maxTopId = maxTopPair.Key;
                    }
                }
                for (int i = maxTopId + 1; i < temporaryMaxTop.Keys.Count; i++)
                {
                    createBetterTopsOnColumn(temporaryFromTop, component, topology, i, true);
                }
                for (int i = maxTopId - 1; i >= 0; i--)
                {
                    createBetterTopsOnColumn(temporaryFromTop, component, topology, i, false);
                }

            }

            /// <summary>
            /// Make topology from top better on specified column than only to know what is more on top than other.
            /// It knows that topology from top is correct on one side of column.
            /// </summary>
            /// <param name="temporaryFromTop">Topology from top</param>
            /// <param name="component">Component of boxes</param>
            /// <param name="topology">Topology from left</param>
            /// <param name="columnId">Id of column in topology from left in which to make
            /// topology from top better</param>
            /// <param name="left">If looks for connection from other boxes or to boxes in which
            /// are boxes connected</param>
            private void createBetterTopsOnColumn(Dictionary<IBoxModule, int> temporaryFromTop, List<IBoxModule> component, Dictionary<IBoxModule, int> topology, int columnId, bool left)
            {
                SortedList<int, IBoxModule> column = new SortedList<int, IBoxModule>();
                foreach (KeyValuePair<IBoxModule, int> topologyPairs in topology)
                {
                    if (component.Contains(topologyPairs.Key) && topologyPairs.Value == columnId)
                        column[temporaryFromTop[topologyPairs.Key]] = topologyPairs.Key;
                }

                // Count averages from boxes which have connection together with these boxes
                for (int i = 0; i < column.Count; i++)
                {
                    IBoxModule[] connections;
                    if (left)
                        connections = column[i].ConnectionsFrom().ToArray();
                    else
                        connections = column[i].ConnectedTo();
                    int j = 0;
                    int sum = 0;
                    foreach (IBoxModule connectedBox in connections)
                    {
                        if (component.Contains(connectedBox))
                        {
                            sum += temporaryFromTop[connectedBox];
                            j++;
                        }
                    }
                    if (j > 0)
                    {
                        temporaryFromTop[column[i]] = sum / j;
                    }
                    else
                    {
                        if (i > 0)
                            temporaryFromTop[column[i]] = temporaryFromTop[column[i - 1]] + 1;
                    }
                }

                // Look on problem if box which hase to be more top than other is less or equal
                bool somethingDone = true;
                while (somethingDone)
                {
                    somethingDone = false;
                    for (int i = 0; i < column.Count - 1; i++)
                    {
                        IBoxModule firstBox = column[i];
                        IBoxModule secondBox = column[i+1];
                        int firstTop = temporaryFromTop[firstBox];
                        int secondTop = temporaryFromTop[secondBox];
                        if (firstTop >= secondTop)
                        {
                            somethingDone = true;
                            temporaryFromTop[firstBox] = (firstTop + secondTop) / 2;
                            temporaryFromTop[secondBox] = temporaryFromTop[firstBox] + 1;
                        }
                    }
                }
            }

            /// <summary>
            /// Changes positions of boxes to nice configuration
            /// </summary>
            public void Relayout()
            {
                IBoxModule[] boxesInView = this.Boxes;
                if (boxesInView.Length == 0) return;
                Dictionary<IBoxModule, int> tops;
                Dictionary<IBoxModule, int> topology = getTopology(out tops);
                foreach (IBoxModule box in boxesInView)
                {
                    this.SetPosition(box, new PointF(topology[box] * defaultShiftPositionX + defaultPositionX, tops[box] * defaultShiftPositionY + defaultPositionY));
                }
                oldPositions.Clear();
            }
		}
    }
}
