using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateTimeRangeCore.DateTimeRange;
using System.Collections.Generic;
using System.Linq;

namespace DateTimeRangeTest.Test
{
	[TestClass]
	public class DateTimeRangeTests
	{
		[TestMethod]
		public void EqualityTest()
		{
			// arrange
			var now = DateTime.Now;
			var range = new DateTimeRange(now, TimeSpan.FromHours(1));
			var equal = new DateTimeRange(range.End, range.Begin);
			var other = range + TimeSpan.FromMinutes(1);

			// assert: Operators
			Assert.IsTrue(range == equal);
			Assert.IsTrue(range != other);

			// assert: Equals(object) method
			Assert.AreEqual(range, equal);
			Assert.AreNotEqual(range, other);

			// assert: GetHashCode() method
			var set = new HashSet<DateTimeRange>();
			Assert.IsTrue(set.Add(range));
			Assert.IsFalse(set.Add(equal));
			Assert.IsTrue(set.Add(other));
		}

		[TestMethod]
		public void CreateFromPulseTest()
		{
			// arrange
			var begin = DateTime.Today;
			var pulse = new Dictionary<DateTime, bool>
			{
				[begin.AddMinutes(1)] = true,
				[begin.AddMinutes(2)] = true,
				[begin.AddMinutes(3)] = false,
				[begin.AddMinutes(4)] = true,
				[begin.AddMinutes(5)] = true,
			};

			// act
			var ranges = DateTimeRange.Create(pulse);

			// assert
			Assert.AreEqual(2, ranges.Count());
			Assert.AreEqual(new DateTimeRange(begin.AddMinutes(1), begin.AddMinutes(3)), ranges.First());
			Assert.AreEqual(new DateTimeRange(begin.AddMinutes(4), begin.AddMinutes(5)), ranges.Last());
		}
	}
}
