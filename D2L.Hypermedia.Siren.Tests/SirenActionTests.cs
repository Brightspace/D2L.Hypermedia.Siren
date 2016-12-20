using System;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenActionTests {

		private ISirenAction GetAction() {
			ISirenAction action = new SirenAction(
					name: "action-name",
					href: new Uri( "http://example.com" ),
					fields: new[] {
						new SirenField( name: "field1", @class: new [] { "class" }, type: "text/html" ),
						new SirenField( name: "field2", @class: new [] { "class" }, type: "text/html" ),
						new SirenField( name: "field3", @class: new [] { "not-class" }, type: "text/xml" ),
					}
				);

			return action;
		}

		[Test]
		public void SirenAction_Serialized_DoesNotIncludeOptionalParametersIfNull() {
			ISirenAction sirenAction = new SirenAction(
					name: "foo",
					href: new Uri( "http://example.com" ) );

			string serialized = JsonConvert.SerializeObject( sirenAction );
			ISirenAction action = JsonConvert.DeserializeObject<SirenAction>( serialized );

			Assert.AreEqual( "foo", action.Name );
			Assert.AreEqual( "http://example.com/", action.Href.ToString() );
			Assert.IsEmpty( action.Class );
			Assert.IsNull( action.Method );
			Assert.IsNull( action.Title );
			Assert.IsNull( action.Type );
			Assert.IsEmpty( action.Fields );
		}

		[Test]
		public void SirenAction_DeserializesCorrectly() {
			ISirenAction sirenAction = new SirenAction(
				name: "foo",
				href: new Uri( "http://example.com" ),
				@class: new[] { "bar" },
				method: "GET",
				title: "Some action",
				type: "text/html",
				fields: new[] {
					new SirenField( name: "field" )
				} );

			string serialized = JsonConvert.SerializeObject( sirenAction );
			ISirenAction action = JsonConvert.DeserializeObject<SirenAction>( serialized );

			Assert.AreEqual( "foo", action.Name );
			Assert.AreEqual( "http://example.com/", action.Href.ToString() );
			Assert.Contains( "bar", action.Class );
			Assert.AreEqual( "GET", action.Method );
			Assert.AreEqual( "Some action", action.Title );
			Assert.AreEqual( "text/html", action.Type );
			Assert.AreEqual( 1, action.Fields.ToList().Count );
		}

		[Test]
		public void SirenAction_Serialize_ExcludesClassAndFieldsIfEmpty() {
			ISirenAction action = new SirenAction(
					name: "foo",
					href: new Uri( "http://example.com" ),
					@class: new [] { "bar" },
					fields: new [] { new SirenField( "baz" ) }
				);
			string serialized = JsonConvert.SerializeObject( action );
			Assert.GreaterOrEqual( serialized.IndexOf( "class", StringComparison.Ordinal ), 0 );
			Assert.GreaterOrEqual( serialized.IndexOf( "fields", StringComparison.Ordinal ), 0 );

			action = new SirenAction(
					name: "foo",
					href: new Uri( "http://example.com" )
				);
			serialized = JsonConvert.SerializeObject( action );
			Assert.AreEqual( -1, serialized.IndexOf( "class", StringComparison.Ordinal ) );
			Assert.AreEqual( -1, serialized.IndexOf( "fields", StringComparison.Ordinal ) );
		}

		[Test]
		public void SirenAction_TryGetFieldByName_ReturnsCorrectField() {
			ISirenField field;
			Assert.IsFalse( GetAction().TryGetFieldByName( "foo", out field ) );
			Assert.IsNull( field );

			Assert.IsTrue( GetAction().TryGetFieldByName( "field1", out field ) );
			Assert.AreEqual( "field1", field.Name );
		}

		[Test]
		public void SirenAction_TryGetFieldByClass_ReturnsCorrectField() {
			ISirenField field;
			Assert.IsFalse( GetAction().TryGetFieldByClass( "foo", out field ) );
			Assert.IsNull( field );

			Assert.IsTrue( GetAction().TryGetFieldByClass( "class", out field ) );
			Assert.Contains( "class", field.Class );
			Assert.AreEqual( "field1", field.Name );
		}

		[Test]
		public void SirenAction_TryGetFieldByType_ReturnsCorrectField() {
			ISirenField field;
			Assert.IsFalse( GetAction().TryGetFieldByType( "foo", out field ) );
			Assert.IsNull( field );

			Assert.IsTrue( GetAction().TryGetFieldByType( "text/xml", out field ) );
			Assert.AreEqual( "text/xml", field.Type );
			Assert.AreEqual( "field3", field.Name );
		}
	}

}