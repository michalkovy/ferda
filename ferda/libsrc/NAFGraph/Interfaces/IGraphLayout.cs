namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Required layout methods
	/// </summary>
	public interface IGraphLayout
	{
		/// <summary>
		/// This is the layout method running on a separate thread.
		/// </summary>
		Task StartLayout(CancellationToken cancellation);
		/// <summary>
		/// Stops the layout process
		/// </summary>
		void StopLayout();
	}
}