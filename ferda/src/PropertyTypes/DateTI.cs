using Ice;
using System;

namespace Ferda.Modules
{
	public class DateTI : DateT, IValue
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
			iface.getDateValue(out year, out month, out day);
		}

		/// <summary>
		/// Method getdateValue
		/// </summary>
		/// <param name="year">A  short</param>
		/// <param name="month">A  short</param>
		/// <param name="day">A  short</param>
		/// <param name="__current">An Ice.Current</param>
		public override void getDateValue(out int year, out short month, out short day, Current __current)
		{
			year = this.year;
			month = this.month;
			day = this.day;
		}

        public override void getDateTimeValue(out int year, out short month, out short day, out short hour, out short minute, out short second, Current __current)
        {
            year = this.year;
            month = this.month;
            day = this.day;
            hour = 0;
            minute = 0;
            second = 0;
        }

        public override String getStringValue(Current __current)
        {
            DateTime dateTime = new DateTime();
            this.TryGetDateTime(out dateTime);
            return dateTime.ToString();
        }

	}
}
