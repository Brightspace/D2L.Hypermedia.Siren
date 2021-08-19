using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenLinkTests {

		[Test]
		public void SirenLink_Serialized_DoesNotIncludeOptionalParametersIfNull() {
			ISirenLink sirenLink = new SirenLink(
				rel: new[] { "foo" },
				href: new Uri( "http://example.com" )
			);

			string serialized = JsonConvert.SerializeObject( sirenLink );

			ISirenLink link = JsonConvert.DeserializeObject<SirenLink>( serialized );

			Assert.AreEqual( "foo", link.Rel[0] );
			Assert.AreEqual( "http://example.com/", link.Href.ToString() );
			Assert.IsEmpty( link.Class );
			Assert.IsNull( link.Type );
			Assert.IsNull( link.Title );
		}

		[Test]
		public void SirenLink_DeserializesCorrectly() {
			ISirenLink sirenLink = TestHelpers.GetLink();

			string serialized = JsonConvert.SerializeObject( sirenLink );

			ISirenLink link = JsonConvert.DeserializeObject<SirenLink>( serialized );

			Assert.Contains( "foo", link.Rel );
			Assert.AreEqual( "http://example.com/", link.Href.ToString() );
			Assert.Contains( "bar", link.Class );
			Assert.AreEqual( "Link title", link.Title );
			Assert.AreEqual( "text/html", link.Type );
		}

		[Test]
		public void SirenLink_Serialize_ExcludesClassIfEmptyArray() {
			ISirenLink link = new SirenLink(
				rel: new [] { "foo" },
				href: new Uri( "http://example.com" ),
				@class: new [] { "bar" }
			);
			string serialized = JsonConvert.SerializeObject( link );
			Assert.GreaterOrEqual( serialized.IndexOf( "class", StringComparison.Ordinal ), 0 );

			link = new SirenLink(
				rel: new [] { "foo" },
				href: new Uri( "http://example.com" )
			);
			serialized = JsonConvert.SerializeObject( link );
			Assert.AreEqual( -1, serialized.IndexOf( "class", StringComparison.Ordinal ) );
		}

		[Test]
		public void SirenLink_Equality_SameLink_ShouldBeEqual() {
			ISirenLink link = TestHelpers.GetLink();
			ISirenLink other = TestHelpers.GetLink();
			TestHelpers.BidirectionalEquality( link, other, true );
		}

		[Test]
		public void SirenLink_Equality_MissingAttributes_ShouldNotBeEqual() {
			ISirenLink link = TestHelpers.GetLink();
			ISirenLink other = new SirenLink(
				rel: new [] { "foo" },
				href: new Uri( "http://example.com" )
			);
			TestHelpers.BidirectionalEquality( link, other, false );
		}

		[Test]
		public void SirenLink_Equality_DifferentRel_ShouldNotBeEqual() {
			ISirenLink link = TestHelpers.GetLink();
			ISirenLink other = TestHelpers.GetLink( "different-rel" );
			TestHelpers.BidirectionalEquality( link, other, false );
		}

		[Test]
		public void SirenLink_ArrayEquality() {
			ISirenLink[] links = { TestHelpers.GetLink( "foo" ), TestHelpers.GetLink( "bar" ) };
			ISirenLink[] others = { TestHelpers.GetLink( "foo" ), TestHelpers.GetLink( "bar" ) };
			TestHelpers.ArrayBidirectionalEquality( links, others, true );

			others = new [] { TestHelpers.GetLink( "bar" ), TestHelpers.GetLink( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( links, others, true );

			others = new [] { TestHelpers.GetLink( "foo" ), TestHelpers.GetLink( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( links, others, false );

			others = new [] { TestHelpers.GetLink( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( links, others, false );
		}

		private static ISirenLink[] HashCodeLinks() {
			return new[]
			{
				new SirenLink(
					rel: new []{ "linkrel" },
					href: new Uri( "http://localhost" ),
					@class: new [] { "linkclass" },
					title: "title",
					type: "type"
					),
				new SirenLink(
					rel: new []{ "linkrel" },
					href: new Uri( "http://localhost" ),
					title: "title",
					type: "type"
					),
				new SirenLink(
					rel: new []{ "linkrel" },
					href: new Uri( "http://localhost" ),
					@class: new [] { "linkclass" },
					type: "type"
					),
				new SirenLink(
					rel: new []{ "linkrel" },
					href: new Uri( "http://localhost" ),
					@class: new [] { "linkclass" },
					title: "title"
					)
			};
		}

		private static TestCaseData[] HashCodeTests() {
			return HashCodeLinks().Select( x => new TestCaseData( x ) ).ToArray();
		}

		private static IEnumerable<TestCaseData> HashCodeEqualityTests() {
			foreach( var link1 in HashCodeLinks() ) {
				var innerEntities = HashCodeLinks().ToList();
				innerEntities.Remove( link1 );
				foreach( var link2 in innerEntities ) {
					yield return new TestCaseData( link1, link2 );
				}

			}
		}

		[TestCaseSource( nameof( HashCodeTests ) )]
		public void SirenLink_GetHashcodeNot0( ISirenLink link ) {
			Assert.AreNotEqual( 0, link.GetHashCode() );
		}

		[TestCaseSource( nameof( HashCodeEqualityTests ) )]
		public void SirenLink_GetHashCode_NotEqual( ISirenLink link1, ISirenLink link2 ) {
			Assert.AreNotEqual( link1.GetHashCode(), link2.GetHashCode() );
		}

	}

}
