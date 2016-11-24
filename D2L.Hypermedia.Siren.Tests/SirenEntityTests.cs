using System;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenEntityTests {

		[Test]
		public void SirenEntity_Serialized_DoesNotIncludeOptionalParametersIfNull() {

			ISirenEntity sirenEntity = new SirenEntity();

			string serialized = JsonConvert.SerializeObject( sirenEntity );

			ISirenEntity entity = JsonConvert.DeserializeObject<SirenEntity>( serialized );

			Assert.IsNull( entity.Class );
			Assert.IsNull( entity.Properties );
			Assert.IsNull( entity.Entities );
			Assert.IsNull( entity.Links );
			Assert.IsNull( entity.Actions );
			Assert.IsNull( entity.Title );
			Assert.IsNull( entity.Rel );
			Assert.IsNull( entity.Href );
			Assert.IsNull( entity.Type );

		}

		[Test]
		public void SirenEntity_DeserializesCorrectly() {

			ISirenEntity sirenEntity = new SirenEntity(
				properties: new {
					foo = "bar"
				},
				links: new[] {
					new SirenLink( rel: new [] { "foo" }, href: new Uri( "http://example.com" ) )
				},
				rel: new[] { "organization" },
				@class: new[] { "some-class" },
				entities: new[] {
					new SirenEntity()
				},
				actions: new[] {
					new SirenAction( name: "action", href: new Uri( "http://example.com/2" ) )
				},
				title: "Entity title",
				href: new Uri( "http://example.com/3" ),
				type: "text/html" );

			string serialized = JsonConvert.SerializeObject( sirenEntity );
			ISirenEntity entity = JsonConvert.DeserializeObject<SirenEntity>( serialized );

			Assert.AreEqual( "bar", (string)entity.Properties.foo );
			Assert.AreEqual( 1, entity.Links.ToList().Count );
			Assert.Contains( "organization", entity.Rel );
			Assert.Contains( "some-class", entity.Class );
			Assert.AreEqual( 1, entity.Entities.ToList().Count );
			Assert.AreEqual( 1, entity.Actions.ToList().Count );
			Assert.AreEqual( "Entity title", entity.Title );
			Assert.AreEqual( "http://example.com/3", entity.Href.ToString() );
			Assert.AreEqual( "text/html", entity.Type );

		}

	}

}