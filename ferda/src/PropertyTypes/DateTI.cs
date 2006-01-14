
using Ice;namespace Ferda.Modules
{
	public class DateTI : DateT, IValue
	{
		public ValueT getValueT()
		{
			DateValueT result = new DateValueT();
			result.Value = this;
			return result;
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

	}
}
