using System;

namespace Netron.GraphLib.Interfaces
{
	public interface ILayoutNode
	{
		float X {get;set;}
		float Y {get;set;}
		double dx {get;set;}
		double dy {get;set;}
		bool Fixed {get;set;}
	}
}
