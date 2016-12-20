using System;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenEntityTests {

		private ISirenEntity GetEntity() {
			ISirenEntity entity = new SirenEntity(
					rel: new [] { "rel" },
					@class: new [] { "class" },
					links: new [] {
						new SirenLink( rel: new[] { "self" }, href: new Uri( "http://example.com" ), @class: new [] { "class" }, type: "text/html", title: "link1" ),
						new SirenLink( rel: new[] { "next" }, href: new Uri( "http://example.com" ), @class: new [] { "class" }, type: "text/html", title: "link2" ),
						new SirenLink( rel: new[] { "next" }, href: new Uri( "http://example.com" ), @class: new [] { "not-class" }, type: "text/html", title: "link3" )
					},
					actions: new [] {
						new SirenAction( name: "action1", href: new Uri( "http://example.com" ), @class: new[] { "class" } ),
						new SirenAction( name: "action2", href: new Uri( "http://example.com" ), @class: new[] { "class" } ),
						new SirenAction( name: "action3", href: new Uri( "http://example.com" ), @class: new[] { "not-class" } )
					},
					entities: new [] {
						new SirenEntity( rel: new [] { "child" }, @class: new [] { "class" }, type: "text/html", title: "entity1" ),
						new SirenEntity( rel: new [] { "child" }, @class: new [] { "class" }, type: "text/html", title: "entity2" ),
						new SirenEntity( rel: new [] { "not-child" }, @class: new [] { "class" }, type: "text/xml", title: "entity3" )
					}
				);

			return entity;
		}

		[Test]
		public void SirenEntity_Serialized_DoesNotIncludeOptionalParametersIfNull() {
			ISirenEntity sirenEntity = new SirenEntity();

			string serialized = JsonConvert.SerializeObject( sirenEntity );

			ISirenEntity entity = JsonConvert.DeserializeObject<SirenEntity>( serialized );

			Assert.IsEmpty( entity.Class );
			Assert.IsNull( entity.Properties );
			Assert.IsEmpty( entity.Entities );
			Assert.IsEmpty( entity.Links );
			Assert.IsEmpty( entity.Actions );
			Assert.IsNull( entity.Title );
			Assert.IsEmpty( entity.Rel );
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
						new SirenLink( rel: new[] { "self" }, href: new Uri( "http://example.com" ), @class: new[] { "class" } )
					},
					rel: new[] { "organization" },
					@class: new[] { "some-class" },
					entities: new[] {
						new SirenEntity()
					},
					actions: new[] {
						new SirenAction( name: "action-name", href: new Uri( "http://example.com" ), @class: new[] { "class" } )
					},
					title: "Entity title",
					href: new Uri( "http://example.com/3" ),
					type: "text/html"
				);

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

		[Test]
		public void SirenEntity_Serialize_ExcludesRelClassEntitiesLinksAndActionsIfEmpty() {
			ISirenEntity entity = new SirenEntity(
					@class: new[] { "foo" },
					rel: new[] { "bar" },
					entities: new[] {
						new SirenEntity()
					},
					links: new[] {
						new SirenLink( rel: new[] { "self" }, href: new Uri( "http://example.com" ), @class: new[] { "class" } )
					},
					actions: new[] {
						new SirenAction( name: "action-name", href: new Uri( "http://example.com" ), @class: new[] { "class" } )
					}
				);
			string serialized = JsonConvert.SerializeObject( entity );
			Assert.GreaterOrEqual( serialized.IndexOf( "rel", StringComparison.Ordinal ), -1 );
			Assert.GreaterOrEqual( serialized.IndexOf( "class", StringComparison.Ordinal ), -1 );
			Assert.GreaterOrEqual( serialized.IndexOf( "entities", StringComparison.Ordinal ), -1 );
			Assert.GreaterOrEqual( serialized.IndexOf( "links", StringComparison.Ordinal ), -1 );
			Assert.GreaterOrEqual( serialized.IndexOf( "actions", StringComparison.Ordinal ), -1 );

			entity = new SirenEntity();
			serialized = JsonConvert.SerializeObject( entity );
			Assert.AreEqual( -1, serialized.IndexOf( "rel", StringComparison.Ordinal ) );
			Assert.AreEqual( -1, serialized.IndexOf( "class", StringComparison.Ordinal ) );
			Assert.AreEqual( -1, serialized.IndexOf( "entities", StringComparison.Ordinal ) );
			Assert.AreEqual( -1, serialized.IndexOf( "links", StringComparison.Ordinal ) );
			Assert.AreEqual( -1, serialized.IndexOf( "actions", StringComparison.Ordinal ) );
		}

		[Test]
		public void SirenEntity_TryGetActionByName_ReturnsCorrectAction() {
			ISirenAction action;
			Assert.IsFalse( GetEntity().TryGetActionByName( "foo", out action ) );
			Assert.IsNull( action );

			Assert.IsTrue( GetEntity().TryGetActionByName( "action2", out action ) );
			Assert.AreEqual( "action2", action.Name );
		}

		[Test]
		public void SirenEntity_TryGetActionByClass_ReturnsCorrectAction() {
			ISirenAction action;
			Assert.IsFalse( GetEntity().TryGetActionByClass( "foo", out action ) );
			Assert.IsNull( action );

			Assert.IsTrue( GetEntity().TryGetActionByClass( "class", out action ) );
			Assert.Contains( "class", action.Class );
			Assert.AreEqual( "action1", action.Name );
		}

		[Test]
		public void SirenEntity_TryGetLinkByRel_ReturnsCorrectLink() {
			ISirenLink link;
			Assert.IsFalse( GetEntity().TryGetLinkByRel( "foo", out link ) );
			Assert.IsNull( link );

			Assert.IsTrue( GetEntity().TryGetLinkByRel( "next", out link ) );
			Assert.Contains( "next", link.Rel );
			Assert.AreEqual( "link2", link.Title );
		}

		[Test]
		public void SirenEntity_TryGetLinkByClass_ReturnsCorrectLink() {
			ISirenLink link;
			Assert.IsFalse( GetEntity().TryGetLinkByClass( "foo", out link ) );
			Assert.IsNull( link );

			Assert.IsTrue( GetEntity().TryGetLinkByClass( "class", out link ) );
			Assert.Contains( "class", link.Class );
			Assert.AreEqual( "link1", link.Title );
		}

		[Test]
		public void SirenEntity_TryGetSubEntityByRel_ReturnsCorrectEntity() {
			ISirenEntity entity;
			Assert.IsFalse( GetEntity().TryGetSubEntityByClass( "foo", out entity ) );
			Assert.IsNull( entity );

			Assert.IsTrue( GetEntity().TryGetSubEntityByClass( "class", out entity ) );
			Assert.Contains( "class", entity.Class );
			Assert.AreEqual( "entity1", entity.Title );
		}

		[Test]
		public void SirenEntity_TryGetSubEntityByClass_ReturnsCorrectEntity() {
			ISirenEntity entity;
			Assert.IsFalse( GetEntity().TryGetSubEntityByRel( "foo", out entity ) );
			Assert.IsNull( entity );

			Assert.IsTrue( GetEntity().TryGetSubEntityByRel( "child", out entity ) );
			Assert.Contains( "child", entity.Rel );
			Assert.AreEqual( "entity1", entity.Title );
		}

		[Test]
		public void SirenEntity_TryGetSubEntityByType_ReturnsCorrectEntity() {
			ISirenEntity entity;
			Assert.IsFalse( GetEntity().TryGetSubEntityByType( "foo", out entity ) );
			Assert.IsNull( entity );

			Assert.IsTrue( GetEntity().TryGetSubEntityByType( "text/xml", out entity ) );
			Assert.AreEqual( "text/xml", entity.Type );
			Assert.AreEqual( "entity3", entity.Title );
		}

	}

}