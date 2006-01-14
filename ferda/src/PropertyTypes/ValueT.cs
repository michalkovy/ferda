using System.Xml;
using System.Xml.Serialization;

namespace Ferda.Modules
{
	[XmlInclude(typeof(BoolValueT)),
	XmlInclude(typeof(ShortValueT)),
	XmlInclude(typeof(IntValueT)),
	XmlInclude(typeof(LongValueT)),
	XmlInclude(typeof(FloatValueT)),
	XmlInclude(typeof(DoubleValueT)),
	XmlInclude(typeof(StringValueT)),
	XmlInclude(typeof(StringSeqValueT)),
	XmlInclude(typeof(DateValueT)),
	XmlInclude(typeof(DateTimeValueT)),
	XmlInclude(typeof(TimeValueT)),
	XmlInclude(typeof(CategoriesValueT)),
	XmlInclude(typeof(GenerationInfoValueT)),
	XmlInclude(typeof(HypothesesValueT))]
	public class ValueT
	{
		public virtual PropertyValue GetPropertyValue()
		{
			return null;
		}
	}

	public class BoolValueT : ValueT
	{
		public Ferda.Modules.BoolTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class ShortValueT : ValueT
	{
		public Ferda.Modules.ShortTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class IntValueT : ValueT
	{
		public Ferda.Modules.IntTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class LongValueT : ValueT
	{
		public Ferda.Modules.LongTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class FloatValueT : ValueT
	{
		public Ferda.Modules.FloatTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class DoubleValueT : ValueT
	{
		public Ferda.Modules.DoubleTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class StringValueT : ValueT
	{
		public Ferda.Modules.StringTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class StringSeqValueT : ValueT
	{
		public Ferda.Modules.StringSeqTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class DateValueT : ValueT
	{
		public Ferda.Modules.DateTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class DateTimeValueT : ValueT
	{
		public Ferda.Modules.DateTimeTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class TimeValueT : ValueT
	{
		public Ferda.Modules.TimeTI Value;

		public override PropertyValue GetPropertyValue()
		{
			return Value;
		}
	}

	public class CategoriesValueT : ValueT
	{
		public Ferda.Modules.CategoriesTI.Categories Value;

		public override PropertyValue GetPropertyValue()
		{
			CategoriesTI result = new CategoriesTI();
			result.categoriesValue = Ferda.Modules.CategoriesTI.Categories.Struct2Dict(Value);
			return result;
		}
	}

	public class GenerationInfoValueT : ValueT
	{
		public Ferda.Modules.GeneratingStruct Value;

		public override PropertyValue GetPropertyValue()
		{
			return new GenerationInfoTI(Value);
		}
	}

	public class HypothesesValueT : ValueT
	{
		public Ferda.Modules.HypothesisStruct[] Value;

		public override PropertyValue GetPropertyValue()
		{
			return new HypothesesTI(Value);
		}
	}
}
