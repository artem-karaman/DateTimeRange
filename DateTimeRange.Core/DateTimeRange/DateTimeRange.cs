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
			=> !ReferenceEquals(other, null)
				&& End == other.End
				&& Begin == other.Begin;

		public static bool operator ==(DateTimeRange dateTimeRange1, DateTimeRange dateTimeRange2)
			=> ReferenceEquals(dateTimeRange1, dateTimeRange2) 
			||(!ReferenceEquals(dateTimeRange1, null) 
			&& dateTimeRange1.Equals(dateTimeRange2));

		public static bool operator !=(DateTimeRange first, DateTimeRange second)
			=> !(first == second);

		public override int GetHashCode()
			=> End.GetHashCode();

		public override bool Equals(object obj)
			=> Equals((DateTimeRange)obj);

		public static DateTimeRange operator +(DateTimeRange range, TimeSpan begin) 
			=> new DateTimeRange() { End = range.End, Begin = range.Begin + begin };

		public static IEnumerable<DateTimeRange> Create(IDictionary<DateTime, bool> pulse)
		{
			List<DateTimeRange> result = new List<DateTimeRange>();
			
			foreach (var item in pulse)
			{
				
				if (item.Value == false)
				{

				}
			}

			return result;
		}

		private static DateTimeRange GetDateTimeRange(IDictionary<DateTime, bool> dicts)
		{
			DateTime start = DateTime.MinValue;
			DateTime end = DateTime.MinValue;

			start = dicts.First().Key;

			foreach (var item in dicts)
			{
				if (item.Value == false || item.Equals(dicts.Last()))
				{
					end = item.Key;
				}
			}

			return new DateTimeRange{End = end, Start = start };
		}
	}
}