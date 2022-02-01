using Ferda.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferda.Modules
{
    internal static class PropertyValueToTie
    {
		public static Ice.Object GetTieForPropertyValue(PropertyValue value)
		{
			switch (value)
			{
				case StringTI stringValue:
					return new StringTInterfaceTie_(stringValue);
				case BoolTI boolValue:
					return new StringTInterfaceTie_(boolValue);
				case ShortTI shortValue:
					return new ShortTInterfaceTie_(shortValue);
				case IntTI v:
					return new IntTInterfaceTie_(v);
				case LongTI v:
					return new LongTInterfaceTie_(v);
				case DateTI v:
					return new DateTInterfaceTie_(v);
				case DateTimeTI v:
					return new DateTimeTInterfaceTie_(v);
				case DoubleTI v:
					return new DoubleTInterfaceTie_(v);
				case FloatTI v:
					return new FloatTInterfaceTie_(v);
				case StringSeqTI v:
					return new StringSeqTInterfaceTie_(v);
				case TimeTI v:
					return new TimeTInterfaceTie_(v);
				default:
					throw new Ferda.Modules.BadTypeError();
			}
		}
	}
}
