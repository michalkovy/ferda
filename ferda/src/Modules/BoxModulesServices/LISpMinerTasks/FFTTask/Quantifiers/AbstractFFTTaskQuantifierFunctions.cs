using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers
{
    /// <summary>
    /// Base for FFT quantifiers.
    /// </summary>
	public abstract class AbstractFFTTaskQuantifierFunctions : AbstractFFTQuantifierFunctionsDisp_, IFunctions
	{
        /// <summary>
        /// The box module.
        /// </summary>
		protected BoxModuleI boxModule;
		//protected IBoxInfo boxInfo;

		#region IFunctions Members
        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
		{
			this.boxModule = boxModule;
			//this.boxInfo = boxInfo;
		}
		#endregion

		#region Functions
        /// <summary>
        /// Gets the quantifier box identifier.
        /// </summary>
        /// <param name="__current">The Ice __current.</param>
        /// <returns>Box type identifier.</returns>
		public override string QuantifierIdentifier(Ice.Current __current)
		{
			return boxModule.BoxInfo.Identifier;
		}
		#endregion
	}

	/// <summary>
    /// Base for FFT quantifiers.
	/// </summary>
	/// <remarks>
	/// Property K is floating number (float) geater than 0.
	/// </remarks>
	public abstract class AbstractFFTTaskQuantifierFunctionsWithParamsK : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
        /// <summary>
        /// Gets the K.
        /// </summary>
        /// <value>The K.</value>
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
    /// Base for FFT quantifiers.
	/// </summary>
	/// <remarks>
    /// Property Alpha is floating number from interval &lt;0, 0.5&gt;.
	/// </remarks>
	public abstract class AbstractFFTTaskQuantifierFunctionsWithParamsAlpha : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
        /// <summary>
        /// Gets the alpha.
        /// </summary>
        /// <value>The alpha from interval &lt;0, 0.5&gt;.</value>
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
    /// Base for FFT quantifiers.
	/// </summary>
	/// <remarks>
	/// Property P is floating number from interval &lt;0, 1&gt;.
	/// </remarks>
	public abstract class AbstractFFTTaskQuantifierFunctionsWithParamsP : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
        /// <summary>
        /// Gets the P.
        /// </summary>
        /// <value>The P from interval &lt;0, 1&gt;.</value>
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
    /// Base for FFT quantifiers.
	/// </summary>
	/// <remarks>
	/// Property P is floating number geater than 0.
	/// </remarks>
	public abstract class AbstractFFTTaskQuantifierFunctionsWithParamsP2 : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
        /// <summary>
        /// Gets the P.
        /// </summary>
        /// <value>The P geater than 0.</value>
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
    /// Base for FFT quantifiers.
	/// </summary>
	/// <remarks>
	/// <para>Property Alpha is floating number from interval &lt;0, 0.5&gt;.</para>
	/// <para>Property P is floating number from interval &lt;0, 1&gt;.</para>
    /// <para>Relation</para>
	/// </remarks>
	public abstract class AbstractFFTTaskQuantifierFunctionsWithParamsRelationAlphaP : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
        /// <summary>
        /// Gets the relation.
        /// </summary>
        /// <value>The relation.</value>
		protected RelationEnum Relation
		{
			get
			{
				return (RelationEnum)Enum.Parse(typeof(RelationEnum), this.boxModule.GetPropertyString("Relation"));
			}
		}

        /// <summary>
        /// Gets the alpha.
        /// </summary>
        /// <value>The alpha from interval &lt;0, 0.5&gt;.</value>
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

        /// <summary>
        /// Gets the P.
        /// </summary>
        /// <value>The P from interval &lt;0, 1&gt;.</value>
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
