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
		}

		[Test]
		public void SirenField_DeserializesCorrectly() {
			ISirenField sirenField = new SirenField(
				name: "foo",
				@class: new[] { "bar" },
				type: "number",
				value: 1,
				title: "Some field" );

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
					@class: new[] { "bar" }
				);
			string serialized = JsonConvert.SerializeObject( field );
			Assert.GreaterOrEqual( serialized.IndexOf( "class", StringComparison.Ordinal ), 0 );

			field = new SirenField(
					name: "foo"
				);
			serialized = JsonConvert.SerializeObject( field );
			Assert.AreEqual( -1, serialized.IndexOf( "class", StringComparison.Ordinal ) );
		}

	}

}