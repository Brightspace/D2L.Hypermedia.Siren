﻿using System;
using System.Collections.Generic;
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
						foo = "bar",
						baz = new {
							baz1 = "cats",
							baz2 = 2,
							baz3 = true
						}
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
			Assert.AreEqual( "cats", (string)entity.Properties.baz.baz1 );
			Assert.AreEqual( 2, (int)entity.Properties.baz.baz2 );
			Assert.AreEqual( true, (bool)entity.Properties.baz.baz3 );
			Assert.AreEqual( 1, entity.Links.ToList().Count );
			Assert.Contains( "organization", entity.Rel );
			Assert.Contains( "some-class", entity.Class );
			Assert.AreEqual( 1, entity.Entities.ToList().Count );
			Assert.AreEqual( 1, entity.Actions.ToList().Count );
			Assert.AreEqual( "Entity title", entity.Title );
			Assert.AreEqual( "http://example.com/3", entity.Href.ToString() );
			Assert.AreEqual( "text/html", entity.Type );
			Assert.AreNotEqual( 0, entity.GetHashCode() );
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
			Assert.IsFalse( TestHelpers.GetEntity().TryGetActionByName( "foo", out action ) );
			Assert.IsNull( action );

			Assert.IsTrue( TestHelpers.GetEntity().TryGetActionByName( "action2", out action ) );
			Assert.AreEqual( "action2", action.Name );
		}

		[Test]
		public void SirenEntity_TryGetActionByClass_ReturnsCorrectAction() {
			ISirenAction action;
			Assert.IsFalse( TestHelpers.GetEntity().TryGetActionByClass( "foo", out action ) );
			Assert.IsNull( action );

			Assert.IsTrue( TestHelpers.GetEntity().TryGetActionByClass( "class", out action ) );
			Assert.Contains( "class", action.Class );
			Assert.AreEqual( "action1", action.Name );
		}

		[Test]
		public void SirenEntity_TryGetLinkByRel_ReturnsCorrectLink() {
			ISirenLink link;
			Assert.IsFalse( TestHelpers.GetEntity().TryGetLinkByRel( "foo", out link ) );
			Assert.IsNull( link );

			Assert.IsTrue( TestHelpers.GetEntity().TryGetLinkByRel( "next", out link ) );
			Assert.Contains( "next", link.Rel );
			Assert.AreEqual( "link2", link.Title );
		}

		[Test]
		public void SirenEntity_TryGetLinkByClass_ReturnsCorrectLink() {
			ISirenLink link;
			Assert.IsFalse( TestHelpers.GetEntity().TryGetLinkByClass( "foo", out link ) );
			Assert.IsNull( link );

			Assert.IsTrue( TestHelpers.GetEntity().TryGetLinkByClass( "class", out link ) );
			Assert.Contains( "class", link.Class );
			Assert.AreEqual( "link1", link.Title );
		}

		[Test]
		public void SirenEntity_TryGetSubEntityByRel_ReturnsCorrectEntity() {
			ISirenEntity entity;
			Assert.IsFalse( TestHelpers.GetEntity().TryGetSubEntityByClass( "foo", out entity ) );
			Assert.IsNull( entity );

			Assert.IsTrue( TestHelpers.GetEntity().TryGetSubEntityByClass( "class", out entity ) );
			Assert.Contains( "class", entity.Class );
			Assert.AreEqual( "entity1", entity.Title );
		}

		[Test]
		public void SirenEntity_TryGetSubEntityByClass_ReturnsCorrectEntity() {
			ISirenEntity entity;
			Assert.IsFalse( TestHelpers.GetEntity().TryGetSubEntityByRel( "foo", out entity ) );
			Assert.IsNull( entity );

			Assert.IsTrue( TestHelpers.GetEntity().TryGetSubEntityByRel( "child", out entity ) );
			Assert.Contains( "child", entity.Rel );
			Assert.AreEqual( "entity1", entity.Title );
		}

		[Test]
		public void SirenEntity_TryGetSubEntityByType_ReturnsCorrectEntity() {
			ISirenEntity entity;
			Assert.IsFalse( TestHelpers.GetEntity().TryGetSubEntityByType( "foo", out entity ) );
			Assert.IsNull( entity );

			Assert.IsTrue( TestHelpers.GetEntity().TryGetSubEntityByType( "text/xml", out entity ) );
			Assert.AreEqual( "text/xml", entity.Type );
			Assert.AreEqual( "entity3", entity.Title );
		}

		[Test]
		public void SirenEntity_Equality_SameEntity_ShouldBeEqual() {
			ISirenEntity entity = TestHelpers.GetEntity();
			ISirenEntity other = TestHelpers.GetEntity();
			TestHelpers.BidirectionalEquality( entity, other, true );
		}

		[Test]
		public void SirenEntity_Equality_MissingAttributes_ShouldNotBeEqual() {
			ISirenEntity entity = TestHelpers.GetEntity();
			ISirenEntity other = new SirenEntity();
			TestHelpers.BidirectionalEquality( entity, other, false );
		}

		[Test]
		public void SirenEntity_Equality_DifferentTitle_ShouldNotBeEqual() {
			ISirenEntity entity = TestHelpers.GetEntity();
			ISirenEntity other = TestHelpers.GetEntity( "different-title" );
			TestHelpers.BidirectionalEquality( entity, other, false );
		}

		[Test]
		public void SirenEntity_ArrayEquality() {
			ISirenEntity[] entities = { TestHelpers.GetEntity( "foo" ), TestHelpers.GetEntity( "bar" ) };
			ISirenEntity[] others = { TestHelpers.GetEntity( "foo" ), TestHelpers.GetEntity( "bar" ) };
			TestHelpers.ArrayBidirectionalEquality( entities, others, true );

			others = new [] { TestHelpers.GetEntity( "bar" ), TestHelpers.GetEntity( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( entities, others, true );

			others = new [] { TestHelpers.GetEntity( "foo" ), TestHelpers.GetEntity( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( entities, others, false );

			others = new [] { TestHelpers.GetEntity( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( entities, others, false );
		}

		public static SirenEntity[] HashCodeEntities() {
			return new[]
			{
				new SirenEntity(
							rel: new [] { "rootrel" },
							@class: new []{ "root" },
							properties: new { prop="value" },
							entities: new [] { TestHelpers.GetEntity() },
							links: new ISirenLink[] { TestHelpers.GetLink() },
							actions: new [] { TestHelpers.GetAction() },
							title: "fooTitle",
							href: new Uri("http://localhost"),
							type: "fooType"
					),
				new SirenEntity(
							@class: new []{ "root" },
							properties: new { prop="value" },
							entities: new [] { TestHelpers.GetEntity() },
							links: new ISirenLink[] { TestHelpers.GetLink() },
							actions: new [] { TestHelpers.GetAction() },
							title: "fooTitle",
							href: new Uri("http://localhost"),
							type: "fooType"
					),
				new SirenEntity(
							rel: new [] { "rootrel" },
							properties: new { prop="value" },
							entities: new [] { TestHelpers.GetEntity() },
							links: new ISirenLink[] { TestHelpers.GetLink() },
							actions: new [] { TestHelpers.GetAction() },
							title: "fooTitle",
							href: new Uri("http://localhost"),
							type: "fooType"
					),
				new SirenEntity(
							rel: new [] { "rootrel" },
							@class: new []{ "root" },
							entities: new [] { TestHelpers.GetEntity() },
							links: new ISirenLink[] { TestHelpers.GetLink() },
							actions: new [] { TestHelpers.GetAction() },
							title: "fooTitle",
							href: new Uri("http://localhost"),
							type: "fooType"
					),
				new SirenEntity(
							rel: new [] { "rootrel" },
							@class: new []{ "root" },
							properties: new { prop="value" },
							links: new ISirenLink[] { TestHelpers.GetLink() },
							actions: new [] { TestHelpers.GetAction() },
							title: "fooTitle",
							href: new Uri("http://localhost"),
							type: "fooType"
					),
				new SirenEntity(
							rel: new [] { "rootrel" },
							@class: new []{ "root" },
							properties: new { prop="value" },
							entities: new [] { TestHelpers.GetEntity() },
							actions: new [] { TestHelpers.GetAction() },
							title: "fooTitle",
							href: new Uri("http://localhost"),
							type: "fooType"
					),
				new SirenEntity(
							rel: new [] { "rootrel" },
							@class: new []{ "root" },
							properties: new { prop="value" },
							entities: new [] { TestHelpers.GetEntity() },
							links: new ISirenLink[] { TestHelpers.GetLink() },
							title: "fooTitle",
							href: new Uri("http://localhost"),
							type: "fooType"
					),
				new SirenEntity(
							rel: new [] { "rootrel" },
							@class: new []{ "root" },
							properties: new { prop="value" },
							entities: new [] { TestHelpers.GetEntity() },
							links: new ISirenLink[] { TestHelpers.GetLink() },
							actions: new [] { TestHelpers.GetAction() },
							href: new Uri("http://localhost"),
							type: "fooType"
					),
				new SirenEntity(
							rel: new [] { "rootrel" },
							@class: new []{ "root" },
							properties: new { prop="value" },
							entities: new [] { TestHelpers.GetEntity() },
							links: new ISirenLink[] { TestHelpers.GetLink() },
							actions: new [] { TestHelpers.GetAction() },
							title: "fooTitle",
							type: "fooType"
					),
				new SirenEntity(
							rel: new [] { "rootrel" },
							@class: new []{ "root" },
							properties: new { prop="value" },
							entities: new [] { TestHelpers.GetEntity() },
							links: new ISirenLink[] { TestHelpers.GetLink() },
							actions: new [] { TestHelpers.GetAction() },
							title: "fooTitle",
							href: new Uri("http://localhost")
					),
			};
		}

		private static TestCaseData[] HashCodeTests() {
			return HashCodeEntities().Select( x => new TestCaseData( x ) ).ToArray();
		}

		private static IEnumerable<TestCaseData> HashCodeEqualityTests() {
			foreach( var entity1 in HashCodeEntities() ) {
				var innerEntities = HashCodeEntities().ToList();
				innerEntities.Remove( entity1 );
				foreach( var entity2 in innerEntities ) {
					yield return new TestCaseData( entity1, entity2 );
				}

			}
		}

		[TestCaseSource( nameof( HashCodeTests ) )]
		public void SirenEntity_GetHashcodeNot0( ISirenEntity entity ) {
			Assert.AreNotEqual( 0, entity.GetHashCode() );
		}

		[TestCaseSource( nameof( HashCodeEqualityTests ) )]
		public void SirenEntity_GetHashCode_NotEqual( ISirenEntity entity1, ISirenEntity entity2 ) {
			Assert.AreNotEqual( entity1.GetHashCode(), entity2.GetHashCode() );
		}

	}

}
