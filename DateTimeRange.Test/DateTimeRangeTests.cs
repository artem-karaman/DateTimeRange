using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateTimeRangeCore.DateTimeRange;
using System.Collections.Generic;

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
	}
}
