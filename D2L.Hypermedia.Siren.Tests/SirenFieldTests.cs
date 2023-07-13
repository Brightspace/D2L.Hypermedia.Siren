using System;
using System.Collections.Generic;
using System.Linq;
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

			Assert.AreEqual( "foo", field.Name );
			Assert.IsEmpty( field.Class );
			Assert.IsNull( field.Type );
			Assert.IsNull( field.Value );
			Assert.IsNull( field.Title );
			Assert.IsNull( field.Min );
			Assert.IsNull( field.Max );
		}

		[Test]
		public void SirenField_DeserializesCorrectly() {
			ISirenField sirenField = TestHelpers.GetField();

			string serialized = JsonConvert.SerializeObject( sirenField );
			ISirenField field = JsonConvert.DeserializeObject<SirenField>( serialized );

			Assert.AreEqual( "foo", field.Name );
			Assert.Contains( "bar", field.Class );
			Assert.AreEqual( "number", field.Type );
			Assert.AreEqual( 1, int.Parse( field.Value.ToString() ) );
			Assert.AreEqual( "Some field", field.Title );
			Assert.AreEqual( 0, field.Min );
			Assert.AreEqual( 2, field.Max );
		}

		[Test]
		public void SirenField_Serialize_ExcludesClassIfEmpty() {
			ISirenField field = new SirenField(
				name: "foo",
				@class: new [] { "bar" }
			);
			string serialized = JsonConvert.SerializeObject( field );
			Assert.GreaterOrEqual( serialized.IndexOf( "class", StringComparison.Ordinal ), 0 );

			field = new SirenField( name: "foo" );
			serialized = JsonConvert.SerializeObject( field );
			Assert.AreEqual( -1, serialized.IndexOf( "class", StringComparison.Ordinal ) );
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

			Assert.Throws<ArgumentException>( () => new SirenField( "foo", new[] { TestHelpers.GetFieldValueObject() }, type: "invalid-type" ) );
			Assert.Throws<ArgumentException>( () => new SirenField( "foo", new[] { TestHelpers.GetFieldValueObject() }, type: SirenFieldType.Search ) );
			Assert.DoesNotThrow( () => new SirenField( "foo", new[] { TestHelpers.GetFieldValueObject() }, type: "radio" ) );
			Assert.DoesNotThrow( () => new SirenField( "foo", new[] { TestHelpers.GetFieldValueObject() }, type: SirenFieldType.Radio ) );
			Assert.DoesNotThrow( () => new SirenField( "foo", new[] { TestHelpers.GetFieldValueObject() }, type: "checkbox" ) );
			Assert.DoesNotThrow( () => new SirenField( "foo", new[] { TestHelpers.GetFieldValueObject() }, type: SirenFieldType.Checkbox ) );
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
			Assert.AreNotEqual( 0, field.GetHashCode() );
		}

		[TestCaseSource( nameof( HashCodeEqualityTests ) )]
		public void SirenField_GetHashCode_NotEqual( ISirenField field1, ISirenField field2 ) {
			Assert.AreNotEqual( field1.GetHashCode(), field2.GetHashCode() );
		}

		[Test]
		public void SirenField_BuildWithSirenFieldValueObject() {
			SirenFieldValueObject[] sirenFieldValueObjects = new[] {
				new SirenFieldValueObject( 1, "This is the first radio button", true ),
				new SirenFieldValueObject( 2, "This is the second radio button", false )
			};

			SirenField sirenField = new SirenField( "radio buttons", sirenFieldValueObjects, SirenFieldType.Radio );

			Assert.AreEqual( sirenFieldValueObjects, sirenField.Value );
		}

	}

}
