using System;

namespace Netron.GraphLib
{
	/// <summary>
	/// The types of backgrounds the control can have
	/// </summary>
	public enum CanvasBackgroundTypes
	{
			
		/// <summary>
		/// Uniform flat colored
		/// </summary>
		FlatColor,
		/// <summary>
		/// Two-color gradient
		/// </summary>
		Gradient,
		/// <summary>
		/// A user defined image
		/// </summary>
		Image

	}
	/// <summary>
	/// The types of graph layouts
	/// </summary>
	public enum GraphLayoutAlgorithms
	{
		/// <summary>
		/// The spring embedder algorithm
		/// </summary>
		SpringEmbedder,
		/// <summary>
		/// The rectangular algorithm
		/// </summary>
		Rectangular
	}

	/// <summary>
	/// The basic types of shapes
	/// </summary>
	public enum BasicShapeTypes
	{
		/// <summary>
		/// A resizable node with four connectors
		/// </summary>
		BasicNode,
		/// <summary>
		/// A non-resiable node with one connector
		/// </summary>
		SimpelNode,
		/// <summary>
		/// A resizable text label node with no connectors
		/// </summary>
		TextLabel

	}
	/// <summary>
	/// The possible weights of connections
	/// </summary>
	public enum ConnectionWeight
	{
		/// <summary>
		/// Thin weight
		/// </summary>
		Thin,
		/// <summary>
		/// Medium weight
		/// </summary>
		Medium,
		/// <summary>
		/// Fat weight
		/// </summary>
		Fat
	}

	/// <summary>
	/// The types of connection ends
	/// </summary>
	public enum ConnectionEnds
	{
		LeftFilledArrow,
		RightFilledArrow,
		BothFilledArrow,
		LeftOpenArrow,
		RightOpenArrow,
		BothOpenArrow,
		NoEnds
		
	}
	/// <summary>
	/// Connector locations
	/// </summary>
	public enum ConnectorLocations
	{
		North,
		East,
		South,
		West,
		Omni,
		Unknown
	}

	

	/// <summary>
	/// The various ways you can display the shapes in the viewer
	/// </summary>
	public enum ShapesView
	{
		/// <summary>
		/// Display as a tree
		/// </summary>
		Tree,
		/// <summary>
		/// Display as large icons
		/// </summary>
		Icons
	}
	/// <summary>
	/// The types of handles of a Bezier curve
	/// </summary>
	public enum HandleTypes
	{
		/// <summary>
		/// Only one tangent
		/// </summary>
		Single,
		/// <summary>
		/// Two independent tangents
		/// </summary>
		Free,
		/// <summary>
		/// Two tangent symmetric on both sides of the handle
		/// </summary>
		Symmetric
	}
}
