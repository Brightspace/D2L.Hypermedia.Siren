using System;
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
		}

	}

}