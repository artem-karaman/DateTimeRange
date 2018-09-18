using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DateTimeRangeCore.DateTimeRange
{
	public struct DateTimeRange : IEquatable<DateTimeRange>
	{
		private DateTime Start { get; }
		public DateTime End { get; }
		public TimeSpan Begin { get; }

		public DateTimeRange(DateTime rangeFrom, TimeSpan rangeTo)
		{
			Begin = rangeTo;
			End = rangeFrom;
			Start = new DateTime(rangeTo.Ticks);
		}

		public DateTimeRange(DateTime rangeFrom, DateTime start)
		{
			Start = start;
			End = rangeFrom;
			Begin = TimeSpan.FromTicks(start.Ticks);
		}

		public DateTimeRange(TimeSpan rangeTo, DateTime rangeFrom)
		{
			Begin = rangeTo;
			End = rangeFrom;
			Start = new DateTime(rangeTo.Ticks);
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
			return End.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj != null && Equals((DateTimeRange)obj);
		}

		public static DateTimeRange operator +(DateTimeRange range, TimeSpan begin)
		{
			return new DateTimeRange(range.End, new TimeSpan(range.Begin.Hours + begin.Hours, range.Begin.Minutes + begin.Minutes, range.Begin.Seconds + begin.Seconds));
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
			return $"Start - {Start.ToString(CultureInfo.InvariantCulture)}; End - {End.ToString(CultureInfo.InvariantCulture)}";
		}
	}
}