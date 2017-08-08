using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	public static class SirenTestHelpers {

		public static void BidirectionalEquality<T>(
			IComparable<T> first,
			IComparable<T> second,
			bool equal
		) {
			Assert.AreEqual( equal, first.Equals( second ) );
			Assert.AreEqual( equal, second.Equals( first ) );
		}

		public static void ArrayBidirectionalEquality<T>(
			IEnumerable<IComparable<T>> first,
			IEnumerable<IComparable<T>> second,
			bool equal
		) {
			Assert.AreEqual( equal, first.OrderBy( x => x ).SequenceEqual( second.OrderBy( x => x ) ) );
			Assert.AreEqual( equal, second.OrderBy( x => x ).SequenceEqual( first.OrderBy( x => x ) ) );
		}

	}

}