using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers
{
	public abstract class AbstractFFTTaskQuantifierFunctions : AbstractFFTQuantifierFunctionsDisp_, IFunctions
	{
		protected BoxModuleI boxModule;
		//protected IBoxInfo boxInfo;

		#region IFunctions Members
		public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
		{
			this.boxModule = boxModule;
			//this.boxInfo = boxInfo;
		}
		#endregion

		#region Functions
		public override string QuantifierIdentifier(Ice.Current __current)
		{
			return boxModule.BoxInfo.Identifier;
		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Property K is floating number (float) geater than 0.
	/// </remarks>
	public abstract class AbstractFFTTaskQuantifierFunctionsWithParamsK : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
		protected internal float K
		{
			get
			{
				float value = this.boxModule.GetPropertyFloat("ParamK");
				if (value <= 0.0f)
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter k must greather than 0!", new string[] { "ParamK" }, restrictionTypeEnum.Minimum);
				}
				return value;
			}
		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Property Alpha is floating number from interval &lt;0, 0.5&gt;.
	/// </remarks>
	public abstract class AbstractFFTTaskQuantifierFunctionsWithParamsAlpha : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
		protected internal double Alpha
		{
			get
			{
				double value = this.boxModule.GetPropertyDouble("ParamAlpha");
				if (value <= 0.0f)
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter alpha must greather than 0!", new string[] { "ParamAlpha" }, restrictionTypeEnum.Minimum);
				}
				else if (value > 0.5f)
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter alpha must less than or equal 0.5!", new string[] { "ParamAlpha" }, restrictionTypeEnum.Maximum);
				}
				return value;
			}
		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Property P is floating number from interval &lt;0, 1&gt;.
	/// </remarks>
	public abstract class AbstractFFTTaskQuantifierFunctionsWithParamsP : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
		protected internal double P
		{
			get
			{
				double value = this.boxModule.GetPropertyDouble("ParamP");
				if (value < 0.0f)
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter alpha must greather than 0!", new string[] { "ParamP" }, restrictionTypeEnum.Minimum);
				}
				else if (value > 1.0f)
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter alpha must less than 1!", new string[] { "ParamP" }, restrictionTypeEnum.Maximum);
				}
				return value;
			}
		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Property P is floating number geater than 0.
	/// </remarks>
	public abstract class AbstractFFTTaskQuantifierFunctionsWithParamsP2 : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
		protected internal double P
		{
			get
			{
				double value = this.boxModule.GetPropertyDouble("ParamP");
				if (value <= 0.0f)
				{
					throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter alpha must greather than 0!", new string[] { "ParamP" }, restrictionTypeEnum.Minimum);
				}
				return value;
			}
		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// <para>Property Alpha is floating number from interval &lt;0, 0.5&gt;.</para>
	/// <para>Property P is floating number from interval &lt;0, 1&gt;.</para>
	/// </remarks>
	public abstract class AbstractFFTTaskQuantifierFunctionsWithParamsRelationAlphaP : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
		protected CoreRelationEnum Relation
		{
			get
			{
				return (CoreRelationEnum)Enum.Parse(typeof(CoreRelationEnum), this.boxModule.GetPropertyString("Relation"));
			}
		}

		protected internal double Alpha
		{
			get
			{
				double value = this.boxModule.GetPropertyDouble("ParamAlpha");
				if (value <= 0.0f)
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter alpha must greather than 0!", new string[] { "ParamAlpha" }, restrictionTypeEnum.Minimum);
				}
				else if (value > 0.5f)
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter alpha must less than or equal 0.5!", new string[] { "ParamAlpha" }, restrictionTypeEnum.Maximum);
				}
				return value;
			}
		}

		protected internal double P
		{
			get
			{
				double value = this.boxModule.GetPropertyDouble("ParamP");
				if (value < 0.0f)
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter alpha must greather than 0!", new string[] { "ParamP" }, restrictionTypeEnum.Minimum);
				}
				else if (value > 1.0f)
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter alpha must less than 1!", new string[] { "ParamP" }, restrictionTypeEnum.Maximum);
				}
				return value;
			}
		}
		#endregion
	}
}
