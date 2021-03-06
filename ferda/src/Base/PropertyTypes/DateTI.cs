using Ice;
using System;

namespace Ferda.Modules
{
	public class DateTI : DateT, DateTInterfaceOperations_, IValue
	{
		public ValueT getValueT()
		{
			DateValueT result = new DateValueT();
			result.Value = this;
			return result;
		}

        public bool TryGetDateTime(out DateTime dateTime)
        {
            try
            {
                dateTime = new DateTime(this.year, this.month, this.day);
                return true;
            }
            catch (ArgumentOutOfRangeException) { }
            catch (ArgumentException) { }
            dateTime = new DateTime();
            return false;
        }

		public DateTI()
		{}

		public DateTI(int year, short month, short day)
		{
			this.year = year;
			this.month = month;
			this.day = day;
		}

		public DateTI(System.DateTime date)
		{
			this.year = date.Year;
			this.month = (short)date.Month;
			this.day = (short)date.Day;
		}

		public DateTI(DateTInterfacePrx iface)
		{
			if (iface != null)
				iface.getDateValue(out year, out month, out day);
		}

        public static implicit operator DateTime(DateTI v)
        {
            DateTime result;
            if (v.TryGetDateTime(out result))
                return result;
            else
                return DateTime.MinValue;
                
        }

        public static implicit operator DateTI(DateTime v)
        {
            return new DateTI(v);
        }

		/// <summary>
		/// Method getdateValue
		/// </summary>
		/// <param name="year">A  short</param>
		/// <param name="month">A  short</param>
		/// <param name="day">A  short</param>
		/// <param name="__current">An Ice.Current</param>
		public void getDateValue(out int year, out short month, out short day, Current __current)
		{
			year = this.year;
			month = this.month;
			day = this.day;
		}

        public void getDateTimeValue(out int year, out short month, out short day, out short hour, out short minute, out short second, Current __current)
        {
            year = this.year;
            month = this.month;
            day = this.day;
            hour = 0;
            minute = 0;
            second = 0;
        }

        public String getStringValue(Current __current)
        {
            DateTime dateTime = new DateTime();
            this.TryGetDateTime(out dateTime);
            return dateTime.ToString();
        }

	}
}
