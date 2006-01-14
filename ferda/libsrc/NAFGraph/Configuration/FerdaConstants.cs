using System;

namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// FerdaConstants is a class, that defines the constants for
	/// drawing shapes and sockets in Ferda.
	/// </summary>
	public class FerdaConstants
	{
		#region Fields

		/// <summary>
		/// Heigth of a Ferda socket (connector)
		/// </summary>
		private float connectorSize;

		/// <summary>
		/// Vertical gap between 2 Ferda sockets (connectors)
		/// </summary>
		private float connectorGap;

		/// <summary>
		/// Offset for drawing socket besides the shape
		/// </summary>
		private float connectorLeftOffset;

		/// <summary>
		/// Heigth of Krabicka
		/// </summary>
		private float krabickaHeight;

		/// <summary>
		/// Width of Krabicka on canvas
		/// </summary>
		private float krabickaWidth;

		#endregion

		#region Properties

		/// <summary>
		/// Heigth of Krabicka on canvas
		/// </summary>
		public float KrabickaHeight
		{
			get
			{
				return krabickaHeight;
			}
		}

		/// <summary>
		/// Width of Krabicka on canvas
		/// </summary>
		public float KrabickaWidth
		{
			get
			{
				return krabickaWidth;
			}
		}

		/// <summary>
		/// Size of a Ferda socket (connector)
		/// </summary>
		public float ConnectorSize
		{
			get
			{
				return connectorSize;
			}
		}

		/// <summary>
		/// Vertical gap between 2 Ferda sockets (connectors)
		/// </summary>
		public float ConnectorGap
		{
			get
			{
				return connectorGap;
			}
		}

		/// <summary>
		/// Offset for drawing socket besides the shape
		/// </summary>
		public float ConnectorLeftOffset
		{
			get
			{
				return connectorLeftOffset;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Setting the default values for properties
		/// </summary>
		public FerdaConstants()
		{
			//
			// TODO: Add constructor logic here
			//
			connectorSize = 10;
			connectorGap = 5;
			connectorLeftOffset = 5;
			this.krabickaHeight = 32;
            this.krabickaWidth = 32;
			//this.krabickaWidth = 80;
		}

		#endregion
	}
}
