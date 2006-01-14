using System;
namespace Netron.GraphLib
{
	/// <summary>
	/// The signature of a show-properties event
	/// </summary>
	public delegate void ShowPropsDelegate(object sender, PropertyBag props);

	/// <summary>
	/// The signature of the show-description-on-hover event
	/// </summary>
	public delegate void ItemDescription(string message);

	/// <summary>
	/// when a new connection is added event 
	/// </summary>
	public delegate bool NewConnection(object sender, ConnectionEventArgs e);
	/// <summary>
	/// The general purpose output delegate 
	/// </summary>
	public delegate void InfoDelegate(object obj);

	/// <summary>
	/// when a new shape is added
	/// </summary>
	public delegate void NewShape(object sender, Shape shape);

    /// <summary>
    /// When a connection is deleted
    /// </summary>
    public delegate void FerdaConnection(object sender, Connection con);
}