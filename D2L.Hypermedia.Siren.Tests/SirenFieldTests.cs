using System;
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
		}

		[Test]
		public void SirenField_DeserializesCorrectly() {
			ISirenField sirenField = new SirenField(
				name: "foo",
				@class: new [] { "bar" },
				type: "number",
				value: 1,
				title: "Some field"
			);

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
		public void SirenField_Equality() {
			ISirenField field = new SirenField(
				name: "foo",
				@class: new [] { "bar" },
				type: "number",
				value: 1,
				title: "Some field"
			);

			ISirenField other = new SirenField(
				name: "foo",
				@class: new [] { "bar" },
				type: "number",
				value: 1,
				title: "Some field"
			);
			Assert.AreEqual( field, other );
			Assert.AreEqual( other, field );

			other = new SirenField(
				name: "foo"
			);
			Assert.AreNotEqual( field, other );
			Assert.AreNotEqual( other, field );

			other = new SirenField(
				name: "foobar",
				@class: new [] { "bar" },
				type: "number",
				value: 1,
				title: "Some field"
			);
			Assert.AreNotEqual( field, other );
			Assert.AreNotEqual( other, field );
		}

		[Test]
		public void SirenField_ArrayEquality() {
			ISirenField[] fields = new ISirenField[] {
				new SirenField( "foo" ),
				new SirenField( "bar" )
			};

			ISirenField[] others = new ISirenField[] {
				new SirenField( "foo" ),
				new SirenField( "bar" )
			};
			Assert.IsTrue( fields.OrderBy( x => x ).SequenceEqual( others.OrderBy( x => x ) ) );
			Assert.IsTrue( others.OrderBy( x => x ).SequenceEqual( fields.OrderBy( x => x ) ) );

			others = new ISirenField[] {
				new SirenField( "bar" ),
				new SirenField( "foo" )
			};
			Assert.IsTrue( fields.OrderBy( x => x ).SequenceEqual( others.OrderBy( x => x ) ) );
			Assert.IsTrue( others.OrderBy( x => x ).SequenceEqual( fields.OrderBy( x => x ) ) );

			others = new ISirenField[] {
				new SirenField( "foo" ),
				new SirenField( "foo" )
			};
			Assert.IsFalse( fields.OrderBy( x => x ).SequenceEqual( others.OrderBy( x => x ) ) );
			Assert.IsFalse( others.OrderBy( x => x ).SequenceEqual( fields.OrderBy( x => x ) ) );

			others = new ISirenField[] {
				new SirenField( "foo" )
			};
			Assert.IsFalse( fields.OrderBy( x => x ).SequenceEqual( others.OrderBy( x => x ) ) );
			Assert.IsFalse( others.OrderBy( x => x ).SequenceEqual( fields.OrderBy( x => x ) ) );
		}

	}

}