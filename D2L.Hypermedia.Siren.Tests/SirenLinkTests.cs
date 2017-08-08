using System;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenLinkTests {

		private ISirenLink GetLink( string rel = "foo" ) {
			return new SirenLink(
				rel: new [] { rel },
				href: new Uri( "http://example.com" ),
				@class: new [] { "bar" },
				title: "Link title",
				type: "text/html"
			);
		}

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
			ISirenLink sirenLink = GetLink();

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
			ISirenLink link = GetLink();
			ISirenLink other = GetLink();
			SirenTestHelpers.BidirectionalEquality( link, other, true );
		}

		[Test]
		public void SirenLink_Equality_MissingAttributes_ShouldNotBeEqual() {
			ISirenLink link = GetLink();
			ISirenLink other = new SirenLink(
				rel: new [] { "foo" },
				href: new Uri( "http://example.com" )
			);
			SirenTestHelpers.BidirectionalEquality( link, other, false );
		}

		[Test]
		public void SirenLink_Equality_DifferentRel_ShouldNotBeEqual() {
			ISirenLink link = GetLink();
			ISirenLink other = GetLink( "different-rel" );
			SirenTestHelpers.BidirectionalEquality( link, other, false );
		}

		[Test]
		public void SirenLink_ArrayEquality() {
			ISirenLink[] links = { GetLink( "foo" ), GetLink( "bar" ) };
			ISirenLink[] others = { GetLink( "foo" ), GetLink( "bar" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( links, others, true );

			others = new [] { GetLink( "bar" ), GetLink( "foo" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( links, others, true );

			others = new [] { GetLink( "foo" ), GetLink( "foo" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( links, others, false );

			others = new [] { GetLink( "foo" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( links, others, false );
		}

	}

}