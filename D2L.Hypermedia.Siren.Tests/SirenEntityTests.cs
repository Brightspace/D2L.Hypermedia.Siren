using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
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

            entity.Class.Should().BeEmpty();
            entity.Properties?.Should().BeNull();
            entity.Entities.Should().BeEmpty();
            entity.Links.Should().BeEmpty();
            entity.Actions.Should().BeEmpty();
            entity.Title.Should().BeNull();
            entity.Rel.Should().BeEmpty();
            entity.Href.Should().BeNull();
            entity.Type.Should().BeNull();
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

            ((string)entity.Properties.foo).Should().Be( "bar" );
			((string)entity.Properties.baz.baz1).Should().Be( "cats" );
			((int)entity.Properties.baz.baz2).Should().Be( 2 );
			((bool)entity.Properties.baz.baz3).Should().BeTrue();
            entity.Links.ToList().Count.Should().Be( 1 );
            entity.Rel.Should().Contain( "organization" );
            entity.Class.Should().Contain( "some-class" );
            entity.Entities.Should().HaveCount( 1 );
            entity.Actions.Should().HaveCount( 1 );
            entity.Title.Should().Be( "Entity title" );
            entity.Href.Should().Be( "http://example.com/3" );
            entity.Type.Should().Be( "text/html" );
            entity.GetHashCode().Should().NotBe( 0 );
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
            serialized.IndexOf( "rel", StringComparison.Ordinal ).Should().BeGreaterThanOrEqualTo(-1);
            serialized.IndexOf( "class", StringComparison.Ordinal ).Should().BeGreaterThanOrEqualTo( -1 );
            serialized.IndexOf( "entities", StringComparison.Ordinal ).Should().BeGreaterThanOrEqualTo( -1 );
            serialized.IndexOf( "links", StringComparison.Ordinal ).Should().BeGreaterThanOrEqualTo( -1 );
            serialized.IndexOf( "actions", StringComparison.Ordinal ).Should().BeGreaterThanOrEqualTo( -1 );

            entity = new SirenEntity();
            serialized = JsonConvert.SerializeObject( entity );
            serialized.IndexOf( "rel", StringComparison.Ordinal ).Should().Be( -1 );
            serialized.IndexOf( "class", StringComparison.Ordinal ).Should().Be( -1 );
            serialized.IndexOf( "entities", StringComparison.Ordinal ).Should().Be( -1 );
            serialized.IndexOf( "links", StringComparison.Ordinal ).Should().Be( -1 );
            serialized.IndexOf( "actions", StringComparison.Ordinal ).Should().Be( -1 );
        }

        [Test]
        public void SirenEntity_TryGetActionByName_ReturnsCorrectAction() {
            ISirenAction action;
            TestHelpers.GetEntity().TryGetActionByName( "foo", out action ).Should().BeFalse();
            action.Should().BeNull();

            TestHelpers.GetEntity().TryGetActionByName( "action2", out action ).Should().BeTrue();
            action.Name.Should().Be( "action2" );
        }

        [Test]
        public void SirenEntity_TryGetActionByClass_ReturnsCorrectAction() {
            ISirenAction action;
            TestHelpers.GetEntity().TryGetActionByClass("foo", out action).Should().BeFalse();
            action.Should().BeNull();

            TestHelpers.GetEntity().TryGetActionByClass( "class", out action ).Should().BeTrue();
            action.Class.Contains( " class");
            action.Name.Should().Be( "action1" );
        }

        [Test]
        public void SirenEntity_TryGetLinkByRel_ReturnsCorrectLink() {
            ISirenLink link;
            TestHelpers.GetEntity().TryGetLinkByRel("foo", out link).Should().BeFalse();
            link.Should().BeNull();

            TestHelpers.GetEntity().TryGetLinkByRel( "next", out link ).Should().BeTrue();
            link.Rel.Should().Contain( "next" );
            link.Title.Should().Be( "link2" );
        }

        [Test]
        public void SirenEntity_TryGetLinkByClass_ReturnsCorrectLink() {
            ISirenLink link;
            TestHelpers.GetEntity().TryGetLinkByClass( "foo", out link ).Should().BeFalse();
            link.Should().BeNull();

            TestHelpers.GetEntity().TryGetLinkByClass( "class", out link ).Should().BeTrue();
            link.Class.Should().Contain( "class" );
            link.Title.Should().Be( "link1" );
        }

        [Test]
        public void SirenEntity_TryGetSubEntityByRel_ReturnsCorrectEntity() {
            ISirenEntity entity;
            TestHelpers.GetEntity().TryGetSubEntityByClass( "foo", out entity ).Should().BeFalse();
            entity.Should().BeNull();

            TestHelpers.GetEntity().TryGetSubEntityByClass( "class", out entity ).Should().BeTrue();
            entity.Class.Should().Contain( "class" );
            entity.Title.Should().Be( "entity1" );
        }

        [Test]
        public void SirenEntity_TryGetSubEntityByClass_ReturnsCorrectEntity() {
            ISirenEntity entity;
            TestHelpers.GetEntity().TryGetSubEntityByRel("foo", out entity).Should().BeFalse();
            entity.Should().BeNull();

            TestHelpers.GetEntity().TryGetSubEntityByRel( "child", out entity ).Should().BeTrue();
            entity.Rel.Should().Contain( "child" );
            entity.Title.Should().Be( "entity1" );
        }

        [Test]
        public void SirenEntity_TryGetSubEntityByType_ReturnsCorrectEntity() {
            ISirenEntity entity;
            TestHelpers.GetEntity().TryGetSubEntityByType( "foo", out entity ).Should().BeFalse();
            entity.Should().BeNull();

            TestHelpers.GetEntity().TryGetSubEntityByType( "text/xml", out entity ).Should().BeTrue();
            entity.Type.Should().Be( "text/xml" );
            entity.Title.Should().Be("entity3");
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
            entity.GetHashCode().Should().NotBe( 0 );
        }

        [TestCaseSource( nameof( HashCodeEqualityTests ) )]
        public void SirenEntity_GetHashCode_NotEqual( ISirenEntity entity1, ISirenEntity entity2 ) {
            entity1.GetHashCode().Should().NotBe( entity2.GetHashCode() );
        }

    }

}
