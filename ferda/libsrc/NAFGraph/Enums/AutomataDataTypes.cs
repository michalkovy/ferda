using System;

namespace Netron.GraphLib
{
	[Serializable]
	public enum AutomataDataTypes
	{
		Integer, Double, Color, Vector, Degree, Radians, String, Bool, Object,  DateTime
	}
	[Serializable]
	public enum VisualizationTypes
	{
		Chernoff,
		Color,
		Value,
		Pie,
		Gauge

	}
	[Serializable]
	public enum AutomataInitialStates
	{
		SingleDot, Alternate, Black, White, External
	}
}
