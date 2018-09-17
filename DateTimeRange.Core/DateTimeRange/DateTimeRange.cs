using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DateTimeRangeCore.DateTimeRange
{
	public struct DateTimeRange : IEquatable<DateTimeRange>
	{
		public DateTime Start { get; private set;}
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

		public override bool Equals(object obj)
		{
			return obj != null && Equals((DateTimeRange)obj);
		}

		public static DateTimeRange operator +(DateTimeRange range, TimeSpan begin) 
			=> new DateTimeRange() { End = range.End, Begin = range.Begin + begin };

		public static IEnumerable<DateTimeRange> Create(IDictionary<DateTime, bool> pulse)
		{
			return GetListsByPulse(pulse);
		}

		private static List<DateTimeRange> GetListsByPulse(IDictionary<DateTime, bool> pulse)
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

		private static DateTimeRange GetDateTimeRange(List<KeyValuePair<DateTime, bool>> items)
		{
			return new DateTimeRange{End = items.First().Key, Start = items.Last().Key};
		}

		public override string ToString()
		{
			return $"Start - {Start.ToString()}; End - {End.ToString()}";
		}
	}
}