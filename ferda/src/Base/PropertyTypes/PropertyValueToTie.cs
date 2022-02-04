using Ferda.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferda.Modules
{
    public static class PropertyValueToTie
    {
		public static Ice.Object GetTieForPropertyValue(PropertyValue value)
		{
			switch (value)
			{
				
				case BoolTI boolValue:
					return new BoolTInterfaceTie_(boolValue);
				case ShortTI shortValue:
					return new ShortTInterfaceTie_(shortValue);
				case IntTI v:
					return new IntTInterfaceTie_(v);
				case LongTI v:
					return new LongTInterfaceTie_(v);
				case DateTimeTI v:
					return new DateTimeTInterfaceTie_(v);
				case DateTI v:
					return new DateTInterfaceTie_(v);
				case TimeTI v:
					return new TimeTInterfaceTie_(v);
				case StringSeqTI v:
					return new StringSeqTInterfaceTie_(v);
				case FloatTI v:
					return new FloatTInterfaceTie_(v);
				case DoubleTI v:
					return new DoubleTInterfaceTie_(v);
				case StringTI stringValue:
					return new StringTInterfaceTie_(stringValue);
				default:
					throw new Ferda.Modules.BadTypeError();
			}
		}
	}
}
