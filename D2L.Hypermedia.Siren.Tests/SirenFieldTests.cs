using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenFieldTests {

		[Test]
		public void SirenField_Serialized_DoesNotIncludeOptionalParametersIfNull() {
			ISirenField sirenField = new SirenField( name: "foo" );

			string serialized = JsonConvert.SerializeObject( sirenField );
			ISirenField field = JsonConvert.DeserializeObject<SirenField>( serialized );

			field.Name.Should().Be( "foo" );
			field.Class.Should().BeEmpty();
			field.Type.Should().BeNull();
			field.Value.Should().BeNull();
			field.Title.Should().BeNull();
			field.Min.Should().BeNull();
			field.Max.Should().BeNull();
		}

		[Test]
		public void SirenField_DeserializesCorrectly() {
			ISirenField sirenField = TestHelpers.GetField();

			string serialized = JsonConvert.SerializeObject( sirenField );
			ISirenField field = JsonConvert.DeserializeObject<SirenField>( serialized );

			field.Name.Should().Be( "foo" );
			field.Class.Should().Contain( "bar" );
			field.Type.Should().Be( "number" );
			field.Value.Should().Be( 1 );
			field.Title.Should().Be( "Some field" );
			field.Min.Should().Be( 0 );
			field.Max.Should().Be( 2 );
		}

		[Test]
		public void SirenField_Serialize_ExcludesClassIfEmpty() {
			ISirenField field = new SirenField(
				name: "foo",
				@class: new [] { "bar" }
			);
			string serialized = JsonConvert.SerializeObject( field );
			serialized.IndexOf( "class", StringComparison.Ordinal ).Should().BeGreaterThanOrEqualTo(0);

			field = new SirenField( name: "foo" );
			serialized = JsonConvert.SerializeObject( field );
			serialized.IndexOf( "class", StringComparison.Ordinal ).Should().Be( -1 );
		}

		[Test]
		public void SirenField_Equality_SameField_ShouldBeEqual() {
			ISirenField field = TestHelpers.GetField();
			ISirenField other = TestHelpers.GetField();
			TestHelpers.BidirectionalEquality( field, other, true );
		}

		[Test]
		public void SirenField_Equality_MissingAttributes_ShouldNotBeEqual() {
			ISirenField field = TestHelpers.GetField();
			ISirenField other = new SirenField(
				name: "foo"
			);
			TestHelpers.BidirectionalEquality( field, other, false );
		}

		[Test]
		public void SirenField_Equality_DifferentName_ShouldNotBeEqual() {
			ISirenField field = TestHelpers.GetField();
			ISirenField other = TestHelpers.GetField( "other-name" );
			TestHelpers.BidirectionalEquality( field, other, false );
		}

		[Test]
		public void SirenField_ArrayEquality() {
			ISirenField[] fields = { TestHelpers.GetField( "foo" ), TestHelpers.GetField( "bar" ) };
			ISirenField[] others = { TestHelpers.GetField( "foo" ), TestHelpers.GetField( "bar" ) };
			TestHelpers.ArrayBidirectionalEquality( fields, others, true );

			others = new [] { TestHelpers.GetField( "bar" ), TestHelpers.GetField( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( fields, others, true );

			others = new [] { TestHelpers.GetField( "foo" ), TestHelpers.GetField( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( fields, others, false );

			others = new [] { TestHelpers.GetField( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( fields, others, false );
		}

		[Test]
		public void SirenField_ValidatesType() {
			Assert.Throws<ArgumentException>( () => new SirenField( "foo", type: "invalid-type" ) );
			Assert.DoesNotThrow( () => new SirenField( "foo", type: "search" ) );
			Assert.DoesNotThrow( () => new SirenField( "foo", type: SirenFieldType.Search ) );
		}

		private static ISirenField[] HashCodeFields() {
			return new ISirenField[]
			{
				new SirenField(
					name: "field",
					@class: new[] { "fieldclass" },
					type: SirenFieldType.Color,
					value: "value",
					title: "title",
					min: decimal.MinValue,
					max: decimal.MaxValue
					),
				new SirenField(
					name: "field",
					type: SirenFieldType.Color,
					value: "value",
					title: "title",
					min: decimal.MinValue,
					max: decimal.MaxValue
					),
				new SirenField(
					name: "field",
					@class: new[] { "fieldclass" },
					value: "value",
					title: "title",
					min: decimal.MinValue,
					max: decimal.MaxValue
					),
				new SirenField(
					name: "field",
					@class: new[] { "fieldclass" },
					type: SirenFieldType.Color,
					title: "title",
					min: decimal.MinValue,
					max: decimal.MaxValue
					),
				new SirenField(
					name: "field",
					@class: new[] { "fieldclass" },
					type: SirenFieldType.Color,
					value: "value",
					min: decimal.MinValue,
					max: decimal.MaxValue
					),
				new SirenField(
					name: "field",
					@class: new[] { "fieldclass" },
					type: SirenFieldType.Color,
					value: "value",
					title: "title",
					max: decimal.MaxValue
					),
				new SirenField(
					name: "field",
					@class: new[] { "fieldclass" },
					type: SirenFieldType.Color,
					value: "value",
					title: "title",
					min: decimal.MinValue
					),
			};
		}

		private static TestCaseData[] HashCodeTests() {
			return HashCodeFields().Select( x => new TestCaseData( x ) ).ToArray();
		}

		private static IEnumerable<TestCaseData> HashCodeEqualityTests() {
			foreach( var field1 in HashCodeFields() ) {
				var innerEntities = HashCodeFields().ToList();
				innerEntities.Remove( field1 );
				foreach( var field2 in innerEntities ) {
					yield return new TestCaseData( field1, field2 );
				}

			}
		}

		[TestCaseSource( nameof( HashCodeTests ) )]
		public void SirenField_GetHashcodeNot0( ISirenField field ) {
			field.GetHashCode().Should().NotBe( 0 );
		}

		[TestCaseSource( nameof( HashCodeEqualityTests ) )]
		public void SirenField_GetHashCode_NotEqual( ISirenField field1, ISirenField field2 ) {
			field1.GetHashCode().Should().NotBe( field2.GetHashCode() );
		}

	}

}
