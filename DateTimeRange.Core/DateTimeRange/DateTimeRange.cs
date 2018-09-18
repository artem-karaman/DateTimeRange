using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DateTimeRangeCore.DateTimeRange
{
	public class DateTimeRange : IEquatable<DateTimeRange>
	{
		private readonly DateTime end;
		private readonly TimeSpan duration;

		public DateTime End => end;
		public TimeSpan Begin => duration;

		public DateTimeRange(DateTime end, TimeSpan duration)
		{
			this.end = end;
			this.duration = duration;
		}

		public DateTimeRange(DateTime start, DateTime end)
		{
			this.end = end;
			this.duration = end - start;
			
		}

		public DateTimeRange(TimeSpan duration, DateTime end)
		{
			this.end = end;
			this.duration = duration;
		}

		public bool Equals(DateTimeRange other)
		{
			return Begin == other.Begin && End == other.End;
		}

		public static bool operator ==(DateTimeRange x, DateTimeRange y)
		{
			return (x.Equals(y));
		}

		public static bool operator !=(DateTimeRange first, DateTimeRange second)
		{
			return !(first == second);
		}

		public override int GetHashCode()
		{
			return End.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj != null && Equals((DateTimeRange)obj);
		}

		public static DateTimeRange operator +(DateTimeRange range, TimeSpan begin)
		{
			return new DateTimeRange(range.End, new TimeSpan().Add(range.Begin + begin));
		}

		public static IEnumerable<DateTimeRange> Create(IDictionary<DateTime, bool> pulse)
		{
			return GetListByPulse(pulse);
		}
		
		public static IEnumerable<DateTimeRange> Create<T>(IDictionary<DateTime, T> values, T min)
		{
			return GetListByMinValue(values, min);
		}

		private static List<DateTimeRange> GetListByPulse(IDictionary<DateTime, bool> pulse)
		{
			List<DateTimeRange> result = new List<DateTimeRange>();

			List<KeyValuePair<DateTime, bool>> items = new List<KeyValuePair<DateTime, bool>>();

			for (int i = 0; i < pulse.Count; i++)
			{
				items.Add(pulse.ElementAt(i));

				if (!pulse.ElementAt(i).Value || pulse.ElementAt(i).Equals(pulse.Last()))
				{
					result.Add(GetDateTimeRange(items));

					items.Clear();
				}
			}

			return result;
		}

		private static List<DateTimeRange> GetListByMinValue<T>(IDictionary<DateTime, T> values, T min)
		{
			List<DateTimeRange> result = new List<DateTimeRange>();

			List<KeyValuePair<DateTime, T>> items = new List<KeyValuePair<DateTime, T>>();

			for (int i = 0; i < values.Count; i++)
			{
				items.Add(values.ElementAt(i));

				if (Comparer<T>.Default.Compare(values.ElementAt(i).Value, min) < 0 || values.ElementAt(i).Equals(values.Last()))
				{
					result.Add(GetDateTimeRange(items));

					items.Clear();
				}
			}

			return result;
		}

		private static DateTimeRange GetDateTimeRange<T>(List<KeyValuePair<DateTime, T>> items)
		{
			return new DateTimeRange (items.First().Key, items.Last().Key);
		}

		public override string ToString()
		{
			return $"({Begin.ToString()}) - ({End.ToString()})";
		}
	}
}