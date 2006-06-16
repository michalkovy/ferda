using Ice;
using System;

namespace Ferda.Modules
{
	public class TimeTI : TimeT, IValue
	{
		public ValueT getValueT()
		{
			TimeValueT result = new TimeValueT();
			result.Value = this;
			return result;
		}

        public bool TryGetTimeSpan(out TimeSpan timeSpan)
        {
            try
            {
                timeSpan = new TimeSpan(this.hour, this.minute, this.second);
                return true;
            }
            catch (ArgumentOutOfRangeException) { }
            catch (ArgumentException) { }
            timeSpan = new TimeSpan();
            return false;
        }

		public TimeTI()
		{}

		public TimeTI(short hour, short minute, short second)
		{
			this.hour = hour;
			this.minute = minute;
			this.second = second;
		}

		public TimeTI(System.TimeSpan timeSpan)
		{
			this.hour = (short)timeSpan.Hours;
			this.minute = (short)timeSpan.Minutes;
			this.second = (short)timeSpan.Seconds;
		}

		public TimeTI(System.DateTime dateTime)
		{
			this.hour = (short)dateTime.Hour;
			this.minute = (short)dateTime.Minute;
			this.second = (short)dateTime.Second;
		}

		public TimeTI(TimeTInterfacePrx iface)
		{
			iface.getTimeValue(out hour, out minute, out second);
		}

        public static implicit operator TimeSpan(TimeTI v)
        {
            TimeSpan result;
            if (v.TryGetTimeSpan(out result))
                return result;
            else
                return TimeSpan.MinValue;

        }

        public static implicit operator TimeTI(TimeSpan v)
        {
            return new TimeTI(v);
        }

		/// <summary>
		/// Method getdateValue
		/// </summary>
		/// <param name="year">A  short</param>
		/// <param name="month">A  short</param>
		/// <param name="day">A  short</param>
		/// <param name="__current">An Ice.Current</param>
		public override void getTimeValue(out short hour, out short minute, out short second, Current __current)
		{
			hour = this.hour;
			minute = this.minute;
			second = this.second;
		}

        public override String getStringValue(Current __current)
        {
            TimeSpan timeSpan = new TimeSpan();
            this.TryGetTimeSpan(out timeSpan);
            return timeSpan.ToString();
        }
	}
}
