using System;
using System.Collections.Generic;
using AwesomeAssertions;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

    [TestFixture]
    public class SirenEntityBuilderTests {

        private static readonly Uri TestHref = new Uri( "http://example.com" );

        [Test]
        public void SirenEntityBuilder_Build_NothingAdded_ExpectEmptyEntity() {
            SirenEntityBuilder builder = new SirenEntityBuilder();

            ISirenEntity entity = builder.Build();

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
        public void SirenEntityBuilder_Build_WithParams_ExpectEntityWithParams() {
            string expectedTitle = "test-title";
            Uri expectedHref = TestHref;
            string expectedType = "test-type";

            SirenEntityBuilder builder = new SirenEntityBuilder();

            ISirenEntity entity = builder.Build( expectedTitle, expectedHref, expectedType );

            entity.Title.Should().Be( expectedTitle );
            entity.Href.Should().Be( expectedHref );
            entity.Type.Should().Be( expectedType );
        }

        [Test]
        public void SirenEntityBuilder_AddRel_Single_ExpectEntityWithRels() {
            string expectedRel1 = "first-rel";
            string expectedRel2 = "second-rel";

            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddRel( expectedRel1 );
            builder.AddRel( expectedRel2 );
            ISirenEntity entity = builder.Build();

            entity.Rel.Should().HaveCount( 2 );
			entity.Rel.Should().BeEquivalentTo( new[] { expectedRel1, expectedRel2 } );
        }

        [Test]
        public void SirenEntityBuilder_AddRel_Bulk_ExpectEntityWithRels() {
            string expectedRel1 = "first-bulk-rel";
            string expectedRel2 = "second-bulk-rel";
            string expectedRel3 = "third-bulk-rel";
            string[] rels = new[] {
                expectedRel1, expectedRel3
            };

            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddRel( rels );
            builder.AddRel( expectedRel2 );
            ISirenEntity entity = builder.Build();

            entity.Rel.Should().HaveCount( 3 );
			entity.Rel.Should().BeEquivalentTo( new[] { expectedRel1, expectedRel2, expectedRel3 } );
		}

        [Test]
        public void SirenEntityBuilder_AddAction_ExpectEntityWithActions() {
            ISirenAction expectedAction1 = TestHelpers.GetAction( "action-1" );
            ISirenAction expectedAction2 = TestHelpers.GetAction( "action-2" );

            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddAction( expectedAction1 );
            builder.AddAction( expectedAction2 );
            ISirenEntity entity = builder.Build();

            entity.Actions.Should().HaveCount( 2 );
            entity.Actions.Should().BeEquivalentTo( new[] { expectedAction1, expectedAction2 } );
        }

        [Test]
        public void SirenEntityBuilder_AddActions_ExpectEntityWithActions() {
            ISirenAction expectedAction1 = TestHelpers.GetAction( "action-bulk-1" );
            ISirenAction expectedAction2 = TestHelpers.GetAction( "action-bulk-2" );
            ISirenAction expectedAction3 = TestHelpers.GetAction( "action-bulk-3" );
            IEnumerable<ISirenAction> actions = new[] {
                expectedAction2, expectedAction3
            };

            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddAction( expectedAction1 );
            builder.AddActions( actions );
            ISirenEntity entity = builder.Build();

            entity.Actions.Should().HaveCount( 3 );
            entity.Actions.Should().BeEquivalentTo( new[] { expectedAction1, expectedAction2, expectedAction3 } );
        }

        [Test]
        public void SirenEntityBuilder_AddLink_ExpectEntityWithLinks() {
            ISirenLink expectedLink1 = TestHelpers.GetLink( "link-1" );
            ISirenLink expectedLink2 = TestHelpers.GetLink( "link-2" );

            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddLink( expectedLink1 );
            builder.AddLink( expectedLink2 );
            ISirenEntity entity = builder.Build();

            entity.Links.Should().HaveCount( 2 );
            entity.Links.Should().BeEquivalentTo( new[] { expectedLink1, expectedLink2 } );
        }

        [Test]
        public void SirenEntityBuilder_AddLinks_ExpectEntityWithLinks() {
            ISirenLink expectedLink1 = TestHelpers.GetLink( "link-bulk-1" );
            ISirenLink expectedLink2 = TestHelpers.GetLink( "link-bulk-2" );
            ISirenLink expectedLink3 = TestHelpers.GetLink( "link-bulk-3" );
            IEnumerable<ISirenLink> links = new[] {
                expectedLink1, expectedLink2
            };

            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddLinks( links );
            builder.AddLink( expectedLink3 );
            ISirenEntity entity = builder.Build();

            entity.Links.Should().HaveCount( 3 );
            entity.Links.Should().BeEquivalentTo( new[] { expectedLink1, expectedLink2, expectedLink3 } );
        }

        [Test]
        public void SirenEntityBuilder_AddEntity_ExpectEntityWithEntities() {
            ISirenEntity expectedEntity1 = TestHelpers.GetEntity( "entity-1" );
            ISirenEntity expectedEntity2 = TestHelpers.GetEntity( "entity-2" );

            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddEntity( expectedEntity1 );
            builder.AddEntity( expectedEntity2 );
            ISirenEntity entity = builder.Build();

			entity.Entities.Should().HaveCount( 2 );
			entity.Entities.Should().BeEquivalentTo( new[] { expectedEntity1, expectedEntity2 } );
        }

        [Test]
        public void SirenEntityBuilder_AddEntities_ExpectEntityWithEntities() {
            ISirenEntity expectedEntity1 = TestHelpers.GetEntity( "entity-bulk-1" );
            ISirenEntity expectedEntity2 = TestHelpers.GetEntity( "entity-bulk-2" );
            ISirenEntity expectedEntity3 = TestHelpers.GetEntity( "entity-bulk-3" );
            IEnumerable<ISirenEntity> entities = new[] {
                expectedEntity1, expectedEntity2
            };
            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddEntities( entities );
            builder.AddEntity( expectedEntity3 );
            ISirenEntity entity = builder.Build();
			
            entity.Entities.Should().HaveCount( 3 );
            entity.Entities.Should().BeEquivalentTo( new[] { expectedEntity1, expectedEntity2, expectedEntity3 } );
        }

        [Test]
        public void SirenEntityBuilder_AddProperty_NullProperty_ExpectEntityWithNullProperty() {

            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddProperty( "testNullProperty", null );
            ISirenEntity entity = builder.Build();

			Assert.That( entity.Properties, Is.Not.Null);
			Assert.That( entity.Properties.testNullProperty, Is.Null );
        }

        [Test]
        public void SirenEntityBuilder_AddProperty_NonNullProperties_ExpectEntityWithProperties() {

            IList<string> value1 = new List<string>();
            string value2 = "my-value-2";

            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddProperty( "myProp1", value1 );
            builder.AddProperty( "myProp2", value2 );
            ISirenEntity entity = builder.Build();

            Assert.That( entity.Properties, Is.Not.Null );
            Assert.That( entity.Properties.myProp1, Is.EquivalentTo(value1));
            Assert.That( entity.Properties.myProp2, Is.EqualTo( value2 ) );
        }

        [Test]
        public void SirenEntityBuilder_AddClass_Single_ExpectEntityWithClasses() {
            string expectedClass1 = "first-class";
            string expectedClass2 = "second-class";

            SirenEntityBuilder builder = new SirenEntityBuilder();

            builder.AddClass( expectedClass1 );
            builder.AddClass( expectedClass2 );
            ISirenEntity entity = builder.Build();

            entity.Class.Should().HaveCount( 2 );
            entity.Class.Should().BeEquivalentTo( new[] { expectedClass1, expectedClass2 } );
        }

        [Test]
        public void SirenEntityBuilder_AddClass_Bulk_ExpectEntityWithClasses() {
            string expectedClass1 = "first-bulk-class";
            string expectedClass2 = "second-bulk-class";
            string expectedClass3 = "third-bulk-class";
            string[] classes = new[] {
                expectedClass1, expectedClass2
            };

            SirenEntityBuilder builder = new SirenEntityBuilder();
            builder.AddClass( classes );
            builder.AddClass( expectedClass3 );
            ISirenEntity entity = builder.Build();

            entity.Class.Should().HaveCount( 3 );
            entity.Class.Should().BeEquivalentTo( new[] { expectedClass1, expectedClass2, expectedClass3 } );
        }

    }
}
