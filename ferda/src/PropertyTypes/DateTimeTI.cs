using Ice;
using System;
namespace Ferda.Modules
{
	public class DateTimeTI : DateTimeT, IValue, IComparable
	{
		public ValueT getValueT()
		{
			DateTimeValueT result = new DateTimeValueT();
			result.Value = this;
			return result;
		}

		public bool TryGetDateTime(out DateTime dateTime)
		{
			try
			{
				dateTime = new DateTime(this.year, this.month, this.day, this.hour, this.minute, this.second);
				return true;
			}
			catch (ArgumentOutOfRangeException) { }
			catch (ArgumentException) { }
			dateTime = new DateTime();
			return false;
		}

		public DateTimeTI()
		{}

		public DateTimeTI(int year, short month, short day, short hour, short minute, short second)
		{
			this.year = year;
			this.month = month;
			this.day = day;
			this.hour = hour;
			this.minute = minute;
			this.second = second;
		}

		public DateTimeTI(System.DateTime datetime)
		{
			this.year = datetime.Year;
			this.month = (short)datetime.Month;
			this.day = (short)datetime.Day;
			this.hour = (short)datetime.Hour;
			this.minute = (short)datetime.Minute;
			this.second = (short)datetime.Second;
		}

		public DateTimeTI(DateTimeTInterfacePrx iface)
		{
			iface.getDateTimeValue(out year, out month, out day,
								   out hour, out minute, out second);
		}

		/// <summary>
		/// Method getdateValue
		/// </summary>
		/// <param name="year">A  short</param>
		/// <param name="month">A  short</param>
		/// <param name="day">A  short</param>
		/// <param name="__current">An Ice.Current</param>
		public override void getDateTimeValue(out int year, out short month, out short day, out short hour, out short minute, out short second, Current __current)
		{
			year = this.year;
			month = this.month;
			day = this.day;
			hour = this.hour;
			minute = this.minute;
			second = this.second;
		}

		#region IComparable Members

		public bool IsInicialised()
		{
			if (this.year != 0 || this.month != 0 || this.day != 0
				|| this.hour != 0 || this.minute != 0 || this.second != 0)
				return true;
			return false;
		}
		public int CompareTo(object obj)
		{
			if (obj is DateTimeTI)
			{
				DateTimeTI other = (DateTimeTI)obj;
				if (!this.IsInicialised() && !other.IsInicialised())
					return 0;
				else if (this.IsInicialised() && !other.IsInicialised())
					return 1;
				else if (!this.IsInicialised() && other.IsInicialised())
					return -1;
				else if (this.year == other.year &&
					this.month == other.month &&
					this.day == other.day &&
					this.hour == other.hour &&
					this.minute == other.minute &&
					this.second == other.second)
					return 0;
				else if (this.year >= other.year &&
					this.month >= other.month &&
					this.day >= other.day &&
					this.hour >= other.hour &&
					this.minute >= other.minute &&
					this.second >= other.second)
					return 1;
				else
					return -1;
			}
			throw new ArgumentException("object is not a DateTimeT");
		}

		#endregion
	}
}
