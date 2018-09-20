using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DateTimeRangeCore.DateTimeRange
{
	public struct DateTimeRange : IEquatable<DateTimeRange>
	{
		public DateTime End { get; }
		public DateTime Begin { get; }
		public TimeSpan Duration { get; }
		public DateTimeRange(DateTime start, TimeSpan duration)
		{
			this = new DateTimeRange(start, start + duration);
		}

		public DateTimeRange(DateTime end, DateTime start)
		{
			Begin = start;
			End = end;
			Duration = end - start;
		}
		public bool Equals(DateTimeRange other)
		{
			return Begin == other.Begin && End == other.End;
		}

		public static bool operator ==(DateTimeRange x, DateTimeRange y)
		{
			return (x.Equals(y));
		}

		public static bool operator !=(DateTimeRange x, DateTimeRange y)
		{
			return !(x == y);
		}

		public override int GetHashCode()
		{
			return Begin.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj != null && Equals((DateTimeRange)obj);
		}

		public static DateTimeRange operator +(DateTimeRange range, TimeSpan duration)
		{
			return new DateTimeRange(range.Begin, range.End + duration);
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

		private static DateTimeRange GetDateTimeRange<T>(IReadOnlyCollection<KeyValuePair<DateTime, T>> items)
		{
			return new DateTimeRange (items.First().Key, items.Last().Key);
		}

		public override string ToString()
		{
			return $"({Begin.ToString()}) - ({End.ToString()})";
		}
	}
}