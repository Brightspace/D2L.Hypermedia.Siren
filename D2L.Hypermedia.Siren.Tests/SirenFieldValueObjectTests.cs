using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenFieldValueObjectTests {

		[Test]
		public void SirenFieldValueObject_Serialized_DoesNotIncludeOptionalParametersIfNull() {
			SirenFieldValueObject sirenFieldValueObject = TestHelpers.GetFieldValueObject();

			string serialized = JsonConvert.SerializeObject( sirenFieldValueObject );
			SirenFieldValueObject field = JsonConvert.DeserializeObject<SirenFieldValueObject>( serialized );

			Assert.AreEqual( "foo", field.Value );
			Assert.IsNull( field.Title );
			Assert.AreEqual( false, field.Selected );
		}

		[Test]
		public void SirenFieldValueObject_DeserializesCorrectly() {
			SirenFieldValueObject sirenFieldValueObject = TestHelpers.GetFieldValueObject( 1 );

			string serialized = JsonConvert.SerializeObject( sirenFieldValueObject );
			SirenFieldValueObject field = JsonConvert.DeserializeObject<SirenFieldValueObject>( serialized );

			Assert.AreEqual( 1, field.Value );
			Assert.AreEqual( "Some field", field.Title );
			Assert.AreEqual( false, field.Selected );
		}

		[Test]
		public void SirenFieldValueObject_Equality_SameField_ShouldBeEqual() {
			SirenFieldValueObject field = TestHelpers.GetFieldValueObject();
			SirenFieldValueObject other = TestHelpers.GetFieldValueObject();
			Assert.AreEqual(field, other);
		}

		[Test]
		public void SirenFieldValueObject_Equality_MissingAttributes_ShouldNotBeEqual() {
			SirenFieldValueObject field = TestHelpers.GetFieldValueObject();
			SirenFieldValueObject other = new SirenFieldValueObject( "foo" );
			Assert.AreNotEqual( field, other );
		}

		[Test]
		public void SirenFieldValueObject_Equality_DifferentValue_ShouldNotBeEqual() {
			SirenFieldValueObject field = TestHelpers.GetFieldValueObject();
			SirenFieldValueObject other = TestHelpers.GetFieldValueObject( "bar" );
			Assert.AreNotEqual( field, other );
		}

		[Test]
		public void SirenFieldValueObject_ValidatesType() {
			Assert.Throws<ArgumentNullException>( () => new SirenFieldValueObject( null ) );
		}

	}

}
