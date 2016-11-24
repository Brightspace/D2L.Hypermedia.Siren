using System;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenLinkTests {

		[Test]
		public void SirenLink_Serialized_DoesNotIncludeOptionalParametersIfNull() {

			ISirenLink sirenLink = new SirenLink(
					rel: new[] { "foo" },
					href: new Uri( "http://example.com" ) );

			string serialized = JsonConvert.SerializeObject( sirenLink );

			ISirenLink link = JsonConvert.DeserializeObject<SirenLink>( serialized );

			Assert.AreEqual( "foo", link.Rel[0] );
			Assert.AreEqual( "http://example.com/", link.Href.ToString() );
			Assert.IsNull( link.Class );
			Assert.IsNull( link.Type );
			Assert.IsNull( link.Title );

		}

		[Test]
		public void SirenLink_DeserializesCorrectly() {

			ISirenLink sirenLink = new SirenLink(
					rel: new[] { "foo" },
					href: new Uri( "http://example.com" ),
					@class: new[] { "foo" },
					title: "Link title",
					type: "text/html" );

			string serialized = JsonConvert.SerializeObject( sirenLink );

			ISirenLink link = JsonConvert.DeserializeObject<SirenLink>( serialized );

			Assert.Contains( "foo", link.Rel );
			Assert.AreEqual( "http://example.com/", link.Href.ToString() );
			Assert.Contains( "foo", link.Class );
			Assert.AreEqual( "Link title", link.Title );
			Assert.AreEqual( "text/html", link.Type );

		}
	}

}