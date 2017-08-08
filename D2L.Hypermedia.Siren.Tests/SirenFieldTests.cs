using System;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenFieldTests {

		private ISirenField GetField( string name = "foo" ) {
			return new SirenField(
				name: name,
				@class: new [] { "bar" },
				type: "number",
				value: 1,
				title: "Some field"
			);
		}

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
		}

		[Test]
		public void SirenField_DeserializesCorrectly() {
			ISirenField sirenField = GetField();

			string serialized = JsonConvert.SerializeObject( sirenField );
			ISirenField field = JsonConvert.DeserializeObject<SirenField>( serialized );

			Assert.AreEqual( "foo", field.Name );
			Assert.Contains( "bar", field.Class );
			Assert.AreEqual( "number", field.Type );
			Assert.AreEqual( 1, int.Parse( field.Value.ToString() ) );
			Assert.AreEqual( "Some field", field.Title );
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
			ISirenField field = GetField();
			ISirenField other = GetField();
			SirenTestHelpers.BidirectionalEquality( field, other, true );
		}

		[Test]
		public void SirenField_Equality_MissingAttributes_ShouldNotBeEqual() {
			ISirenField field = GetField();
			ISirenField other = new SirenField(
				name: "foo"
			);
			SirenTestHelpers.BidirectionalEquality( field, other, false );
		}

		[Test]
		public void SirenField_Equality_DifferentName_ShouldNotBeEqual() {
			ISirenField field = GetField();
			ISirenField other = GetField( "other-name" );
			SirenTestHelpers.BidirectionalEquality( field, other, false );
		}

		[Test]
		public void SirenField_ArrayEquality() {
			ISirenField[] fields = { GetField( "foo" ), GetField( "bar" ) };
			ISirenField[] others = { GetField( "foo" ), GetField( "bar" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( fields, others, true );

			others = new [] { GetField( "bar" ), GetField( "foo" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( fields, others, true );

			others = new [] { GetField( "foo" ), GetField( "foo" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( fields, others, false );

			others = new [] { GetField( "foo" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( fields, others, false );
		}

	}

}