using System;
using System.Collections.Generic;
using System.Linq;

namespace DateTimeRangeCore.DateTimeRange
{
	public struct DateTimeRange : IEquatable<DateTimeRange>
	{
		public DateTime Start { get; private set; }
		public DateTime End { get; private set; }
		public TimeSpan Begin { get; private set; }
		public DateTimeRange(DateTime end, TimeSpan begin)
		{
			Begin = begin;
			End = end;
			Start = new DateTime(begin.Ticks);
		}

		public DateTimeRange(DateTime end, DateTime start)
		{
			Start = start;
			End = end;
			Begin = new TimeSpan(end.Ticks);
		}

		public bool Equals(DateTimeRange other)
		{
			return End == other.End && Start == other.Start;
		}

		public static bool operator ==(DateTimeRange dateTimeRange1, DateTimeRange dateTimeRange2)
		{
			return Equals(dateTimeRange1, dateTimeRange2) || (dateTimeRange1.Equals(dateTimeRange2));
		}

		public static bool operator !=(DateTimeRange first, DateTimeRange second)
		{
			return !(first == second);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public int CompareTo(object obj)
		{
			throw new NotImplementedException();
		}

		public override bool Equals(object obj)
		{
			return obj != null && Equals((DateTimeRange)obj);
		}

		public static DateTimeRange operator +(DateTimeRange range, TimeSpan begin)
		{
			return new DateTimeRange() { End = range.End, Begin = range.Begin + begin };
		}

		public static IEnumerable<DateTimeRange> Create(IDictionary<DateTime, bool> pulse)
		{
			return GetListByPulse(pulse);
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

		//public override string ToString()
		//{
		//	return $"Start - {Start.ToString()}; End - {End.ToString()}";
		//}

		public static IEnumerable<DateTimeRange> Create<T>(IDictionary<DateTime, T> values, T min)
		{
			return GetListByMinValue(values, min);
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
			return new DateTimeRange { End = items.First().Key, Start = items.Last().Key };
		}
	}
}