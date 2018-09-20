using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTimeRangeCore.DateTimeRange
{
	public static class DateTimeRangeExtensions
	{
		public static IEnumerable<DateTimeRange> Merge(this IEnumerable<DateTimeRange> range)
		{
			var tmp = new DateTimeRange();

			List<DateTimeRange> ranges = range.ToList();
			List<DateTimeRange> result = new List<DateTimeRange>();
			int next = 0;

			for (int i = 0; i < ranges.Count(); i++)
			{
				if (i < ranges.Count() - 1)
				{
					next ++;

					if (ranges[i].Begin == ranges[next].Begin)
					{
						result.Add(new DateTimeRange(ranges[i].Begin, ranges[i].Duration + ranges[next].Duration));
					}
					else if (ranges[i].End == ranges[next].Begin)
					{
						result.Add(new DateTimeRange(ranges[i].Begin, ranges[i].Duration + ranges[next].Duration));
					}
					else
					{
						result.Add(new DateTimeRange(ranges[i].Begin, ranges[i].Duration));
					}
				}
			}

			return result;
		}
	}
}
