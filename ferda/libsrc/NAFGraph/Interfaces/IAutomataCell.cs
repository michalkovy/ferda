using System;

namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Required signature for being part of a cellular automata network
	/// </summary>
	public interface IAutomataCell
	{
		/// <summary>
		/// Initialization or reset method
		/// </summary>
		void InitAutomata();
		/// <summary>
		/// Elementary step or update of the cell's state
		/// </summary>
		void Update();

		/// <summary>
		/// Transmits data between connections
		/// </summary>
		void Transmit();
	}

	public interface IScript:IDisposable
	{
		void Initialize(IHost Host);

		void Method1();
		void Method2();
		void Method3();
		void Compute();
		
	}

	public interface IHost
	{
		
		void ShowMessage(string Message);
		Connector Out {get;}
		Connector XIn {get;}
		Connector YIn {get;}
	}
}
