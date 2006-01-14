using System;
using Ferda.Modules;
using Ferda.ModulesManager;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Ferda {
    namespace ProjectManager {

        /// <summary>
        /// Archive of boxes in project
        /// </summary>
        public class Archive {
			private int lastBoxModuleProjectIdentifier = 0;
			private List<IBoxModule> boxes = new List<IBoxModule>();
			private StringCollection boxCategories =
				new StringCollection();
			private Dictionary<string,List<IBoxModule>> boxesInCategory =
				new Dictionary<string,List<IBoxModule>>();
			private StringCollection boxLabels =
				new StringCollection();
			private Dictionary<string,List<IBoxModule>> boxesWithLabel =
				new Dictionary<string,List<IBoxModule>>();
			private List<View> views;
            private Dictionary<int, IBoxModule> boxesByProjectIdentifier =
                new Dictionary<int, IBoxModule>();
			
			/// <summary>
			/// Constructs archive
			/// </summary>
			/// <param name="views">A dictionary of
			/// <see cref="T:Ferda.ProjectManager.View"/> in project
			/// </param>
			protected internal Archive(List<View> views)
			{
				this.views = views;
			}
			
			private void addBoxCategories(IBoxModule box)
			{
				IBoxModuleFactoryCreator creator = box.MadeInCreator;
				foreach(string category in creator.BoxCategories)
				{
					if(!boxCategories.Contains(category))
					{
						boxCategories.Add(category);
						boxesInCategory[category] = new List<IBoxModule>();
					}
					boxesInCategory[category].Add(box);
				}
				string label = creator.Label;
				if(!boxLabels.Contains(label))
				{
					boxLabels.Add(label);
					boxesWithLabel[label] = new List<IBoxModule>();
				}
				boxesWithLabel[label].Add(box);
			}
			
			/// <summary>
			/// Adds box to archive
			/// </summary>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representation of box</param>
			/// <seealso cref="M:Ferda.ProjectManager.Archive.Remove(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="P:Ferda.ProjectManager.Archive.Boxes"/>
            public void Add(IBoxModule box) {
				if(!boxes.Contains(box))
				{
					boxes.Add(box);
					box.ProjectIdentifier = lastBoxModuleProjectIdentifier++;
					addBoxCategories(box);
                    boxesByProjectIdentifier.Add(box.ProjectIdentifier,box);
				}
            }
			
			private void removeBoxCategories(IBoxModule box)
			{
				IBoxModuleFactoryCreator creator = box.MadeInCreator;
				foreach(string category in creator.BoxCategories)
				{
					boxesInCategory[category].Remove(box);
					if(boxesInCategory[category].Count == 0)
					{
						boxCategories.Remove(category);
					}
				}
				string label = creator.Label;
				boxesWithLabel[label].Remove(box);
				if(boxesWithLabel[label].Count == 0)
				{
					boxCategories.Remove(label);
				}
			}
			
			protected internal void AddWithIdentifier(IBoxModule box, int projectIdentifier)
			{
				if(!boxes.Contains(box))
				{
					boxes.Add(box);
					box.ProjectIdentifier = projectIdentifier;
					lastBoxModuleProjectIdentifier =
						lastBoxModuleProjectIdentifier <= projectIdentifier ?
						projectIdentifier+1 : lastBoxModuleProjectIdentifier;
					addBoxCategories(box);
                    boxesByProjectIdentifier.Add(projectIdentifier, box);
				}
			}
			
			/// <summary>
			/// Removes box from archive.
			/// </summary>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representation of box</param>
			/// <seealso cref="M:Ferda.ProjectManager.Archive.Add(Ferda.ModulesManager.IBoxModule)"/>
			/// <seealso cref="P:Ferda.ProjectManager.Archive.Boxes"/>
            public void Remove(IBoxModule box) {
				foreach(IBoxModule otherBox in box.ConnectedTo())
				{
					foreach(SocketInfo info in otherBox.Sockets)
					{
						foreach(IBoxModule thirdBox in otherBox.GetConnections(info.name))
						{
							if(thirdBox == box)
							{
								otherBox.RemoveConnection(info.name,box);
								break;
							}
						}
					}
				}
				foreach(SocketInfo info in box.Sockets)
				{
					foreach(IBoxModule otherBox in box.GetConnections(info.name))
					{
						box.RemoveConnection(info.name, otherBox);
					}
				}
				foreach(View view in views)
				{
					if(view.ContainsBox(box)) view.Remove(box);
				}
				if(boxes.Remove(box))
				{
					removeBoxCategories(box);
				}
                boxesByProjectIdentifier.Remove(box.ProjectIdentifier);
				box.destroy();
            }

			/// <summary>
			/// Clones box, adds that clon to archive and returns it
			/// </summary>
			/// <returns>An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representating clone of box <paramref name="box"/></returns>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representation of box</param>
            public IBoxModule Clone(IBoxModule box) {
				IBoxModule result = box.Clone();
				Add(result);
				return result;
            }

			/// <summary>
			/// Lists boxes with type <paramref name="archiveBoxType"/>
			/// </summary>
			/// <returns>An array of <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing boxes with type <paramref name="archiveBoxType"/>
			/// </returns>
			/// <param name="archiveBoxType">A string representation
			/// of box type</param>
            public IBoxModule[] ListBoxesWithType(string archiveBoxType) {
				//TODO co pokud box label stejny jako kategorie
				if(boxCategories.Contains(archiveBoxType))
				{
					IBoxModule[] resultArray = boxesInCategory[archiveBoxType].ToArray();
					Array.Sort<IBoxModule>(resultArray);
					return resultArray;
				}
				if(boxLabels.Contains(archiveBoxType))
				{
					IBoxModule[] resultArray = boxesWithLabel[archiveBoxType].ToArray();
					Array.Sort<IBoxModule>(resultArray);
					return resultArray;
				}
				return new IBoxModule[0];
            }

			/// <summary>
			/// Gets boxes connected to some socket of box <paramref name="box"/>.
			/// </summary>
			/// <returns>An IBoxModule array of boxes connected to box
			/// <paramref name="box"/>.</returns>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representation of box</param>
			/// <seealso cref="M:Ferda.ProjectManager.Archive.ConnectionsFrom(Ferda.ModulesManager.IBoxModule)"/>
            public IBoxModule[] ConnectedTo(IBoxModule box) {
				IBoxModule[] resultArray = box.ConnectedTo();
				Array.Sort<IBoxModule>(resultArray);
				return resultArray;
            }

			/// <summary>
			/// Gets boxes to which is connected box <paramref name="box"/>.
			/// </summary>
			/// <returns>An IBoxModule array of boxes to which is connected box
			/// <paramref name="box"/>.</returns>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representation of box</param>
			/// <seealso cref="M:Ferda.ProjectManager.Archive.ConnectedTo(Ferda.ModulesManager.IBoxModule)"/>
            public IBoxModule[] ConnectionsFrom(IBoxModule box)
			{
				List<IBoxModule> result = new List<IBoxModule>();
				foreach (SocketInfo socket in box.Sockets)
				{
					foreach (IBoxModule otherBox in box.GetConnections(socket.name))
					{
						if (!result.Contains(otherBox)) result.Add(otherBox);
					}
				}
				IBoxModule[] resultArray = result.ToArray();
				Array.Sort<IBoxModule>(resultArray);
				return resultArray;
            }

			/// <summary>
			/// Looks if archive contains box <paramref name="box"/>
			/// </summary>
			/// <returns>true if archive contains box <paramref name="box"/>,
			/// otherwise false</returns>
			/// <param name="box">An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representation of box</param>
            public bool ContainsBox(IBoxModule box) {
				return boxes.Contains(box);
            }

			/// <summary>
			/// Types of boxes in archive
			/// </summary>
			/// <value>
			/// An array of string representing box types
			/// </value>
            public string[] ArchiveBoxTypes {

                get {
					int bc = boxCategories.Count;
					int bl = boxLabels.Count;
                    string[] result = new string[bc + boxLabels.Count];
					if(bc > 0)
						boxCategories.CopyTo(result, 0);
					if(bl > 0)
						boxLabels.CopyTo(result, bc);
					Array.Sort<string>(result, 0, bc);
					Array.Sort<string>(result, bc, bl);
					return result;
                }
            }
			
			/// <summary>
			/// Boxes in archive
			/// </summary>
			/// <value>
			/// An array of <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing boxes in archive
			/// </value>
            public ModulesManager.IBoxModule[] Boxes {
				
                get {
                    return boxes.ToArray();
                }
            }

            /// <summary>
            /// Gets box with specified project identifier
            /// </summary>
            /// <returns>box with specified project identifier or null box with specified identifier does not exist</returns>
            /// <param name="projectIdentifier">An int representing project identifier</param>
            public ModulesManager.IBoxModule GetBoxByProjectIdentifier(int projectIdentifier)
            {
                IBoxModule result = null;
                boxesByProjectIdentifier.TryGetValue(projectIdentifier, out result);
                return result;
            }
			
			/// <summary>
			/// Boxes in archive
			/// </summary>
			/// <value>
			/// An sorted array of <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// representing boxes in archive
			/// </value>
            public ModulesManager.IBoxModule[] SortedBoxes {
				
                get {
					ModulesManager.IBoxModule[] result = boxes.ToArray();
					Array.Sort<IBoxModule>(result);
                    return result;
                }
            }

            /// <summary>
            /// Refreshes information about order of boxes. Use before
            /// <see cref="M:Ferda.ProjectManager.Archive.ConnectedTo(Ferda.ModulesManager.IBoxModule)"/>,
            /// <see cref="M:Ferda.ProjectManager.Archive.ConnectionsFrom(Ferda.ModulesManager.IBoxModule)"/>
            /// and
            /// <see cref="P:Ferda.ProjectManager.Archive.SortedBoxes"/>
            /// </summary>
            public void RefreshOrder()
            {
                foreach(ModulesManager.IBoxModule box in Boxes)
                    box.RefreshOrder();
            }
			
			/// <summary>
			/// Destroys archive
			/// </summary>
			/// <remarks>
			/// Removes all boxes from archive
			/// </remarks>
			protected internal void Destroy()
			{
				foreach(IBoxModule box in boxes)
				{
					box.destroy();
				}
				boxes.Clear();
			}
			
			
        }
    }
}
