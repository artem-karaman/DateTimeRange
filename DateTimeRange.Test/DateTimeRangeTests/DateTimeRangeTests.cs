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

		[TestMethod]
		public void CreateFromValuesTest()
		{
			// arrange
			var begin = DateTime.Today;
			var values = new Dictionary<DateTime, int>
			{
				[begin.AddMinutes(1)] = 3,
				[begin.AddMinutes(2)] = 6,
				[begin.AddMinutes(3)] = 1,
				[begin.AddMinutes(4)] = 3,
				[begin.AddMinutes(5)] = 5,
			};

			// act
			var ranges = DateTimeRange.Create(values, 2);

			// assert
			Assert.AreEqual(2, ranges.Count());
			Assert.AreEqual(new DateTimeRange(begin.AddMinutes(1), begin.AddMinutes(3)), ranges.First());
			Assert.AreEqual(new DateTimeRange(begin.AddMinutes(4), begin.AddMinutes(5)), ranges.Last());
		}

		[TestMethod]
		public void MergeTest()
		{
			/*
             |---|
             |-----|
                      |---|
                             |---|
                                 |---|

             |-----|  |---|  |-------|
             */

			// arrange
			var now = DateTime.Now;
			var range1 = new DateTimeRange(now, TimeSpan.FromHours(1));                              // 00:00 - 01:00
			var range1plus = range1 + TimeSpan.FromMinutes(30);                                      // 00:00 - 01:30
			var range2 = new DateTimeRange(range1plus.End.AddMinutes(30), TimeSpan.FromMinutes(30)); // 02:00 - 02:30
			var range3 = new DateTimeRange(range2.End.AddMinutes(30), TimeSpan.FromMinutes(30));     // 03:00 - 03:30
			var range3split = new DateTimeRange(range3.End, TimeSpan.FromMinutes(30));               // 03:30 - 04:00

			var s = range3split.Begin;

			// act
			var merge = (new[] { range1, range2, range3, range1plus, range3split }).Merge().ToArray();

			//// assert
			//Assert.AreEqual(3, merge.Length);
			//Assert.AreEqual(new DateTimeRange(range1plus.Begin, range1plus.End), merge[0]); // 00:00 - 01:30
			//Assert.AreEqual(new DateTimeRange(range2.Begin, range2.End), merge[1]);         // 02:00 - 02:30
			//Assert.AreEqual(new DateTimeRange(range3.Begin, range3split.End), merge[2]);    // 03:00 - 04:00
		}
	}
}
