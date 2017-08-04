using System;
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
			ISirenLink sirenLink = new SirenLink(
				rel: new[] { "foo" },
				href: new Uri( "http://example.com" ),
				@class: new[] { "bar" },
				title: "Link title",
				type: "text/html"
			);

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
		public void SirenLink_Equality() {
			ISirenLink link = new SirenLink(
				rel: new [] { "foo" },
				href: new Uri( "http://example.com" ),
				@class: new [] { "bar" },
				title: "Link title",
				type: "text/html"
			);

			ISirenLink other = new SirenLink(
				rel: new [] { "foo" },
				href: new Uri( "http://example.com" ),
				@class: new [] { "bar" },
				title: "Link title",
				type: "text/html"
			);
			Assert.AreEqual( link, other );
			Assert.AreEqual( other, link );

			other = new SirenLink(
				rel: new [] { "foo" },
				href: new Uri( "http://example.com" )
			);
			Assert.AreNotEqual( link, other );
			Assert.AreNotEqual( other, link );

			other = new SirenLink(
				rel: new [] { "foobar" },
				href: new Uri( "http://example.com" ),
				@class: new [] { "bar" },
				title: "Link title",
				type: "text/html"
			);
			Assert.AreNotEqual( link, other );
			Assert.AreNotEqual( other, link );
		}

		[Test]
		public void SirenLink_ArrayEquality() {
			ISirenLink[] links = new ISirenLink[] {
				new SirenLink( new [] { "foo" }, new Uri( "http://example.com" ) ),
				new SirenLink( new [] { "bar" }, new Uri( "http://example.com" ) )
			};

			ISirenLink[] others = new ISirenLink[] {
				new SirenLink( new [] { "foo" }, new Uri( "http://example.com" ) ),
				new SirenLink( new [] { "bar" }, new Uri( "http://example.com" ) )
			};
			Assert.IsTrue( links.OrderBy( x => x ).SequenceEqual( others.OrderBy( x => x ) ) );
			Assert.IsTrue( others.OrderBy( x => x ).SequenceEqual( links.OrderBy( x => x ) ) );

			others = new ISirenLink[] {
				new SirenLink( new [] { "bar" }, new Uri( "http://example.com" ) ),
				new SirenLink( new [] { "foo" }, new Uri( "http://example.com" ) )
			};
			Assert.IsTrue( links.OrderBy( x => x ).SequenceEqual( others.OrderBy( x => x ) ) );
			Assert.IsTrue( others.OrderBy( x => x ).SequenceEqual( links.OrderBy( x => x ) ) );

			others = new ISirenLink[] {
				new SirenLink( new [] { "foo" }, new Uri( "http://example.com" ) ),
				new SirenLink( new [] { "foo" }, new Uri( "http://example.com" ) )
			};
			Assert.IsFalse( links.OrderBy( x => x ).SequenceEqual( others.OrderBy( x => x ) ) );
			Assert.IsFalse( others.OrderBy( x => x ).SequenceEqual( links.OrderBy( x => x ) ) );

			others = new ISirenLink[] {
				new SirenLink( new [] { "foo" }, new Uri( "http://example.com" ) )
			};
			Assert.IsFalse( links.OrderBy( x => x ).SequenceEqual( others.OrderBy( x => x ) ) );
			Assert.IsFalse( others.OrderBy( x => x ).SequenceEqual( links.OrderBy( x => x ) ) );
		}
	}

}