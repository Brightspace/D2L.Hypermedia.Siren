using System;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenActionTests {

		private ISirenAction GetAction( string name = "action-name" ) {
			ISirenAction action = new SirenAction(
				name: name,
				href: new Uri( "http://example.com" ),
				@class: new [] { "foo" },
				method: "GET",
				title: "Action title",
				type: "some-type",
				fields: new [] {
					new SirenField( name: "field1", @class: new [] { "class" }, type: "text/html" ),
					new SirenField( name: "field2", @class: new [] { "class" }, type: "text/html" ),
					new SirenField( name: "field3", @class: new [] { "not-class" }, type: "text/xml" )
				}
			);

			return action;
		}

		[Test]
		public void SirenAction_Serialized_DoesNotIncludeOptionalParametersIfNull() {
			ISirenAction sirenAction = new SirenAction(
				name: "foo",
				href: new Uri( "http://example.com" )
			);

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
				@class: new [] { "bar" },
				method: "GET",
				title: "Some action",
				type: "text/html",
				fields: new [] {
					new SirenField( name: "field" )
				}
			);

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
		public void SirenAction_TryGetField_ReturnsCorrectField() {
			ISirenField find = new SirenField( "field3" );
			ISirenField field;
			Assert.IsTrue( GetAction().TryGetField( find, out field ) );
			Assert.IsTrue( field.Contains( find ) );
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

		[Test]
		public void SirenAction_Equality_SameAction_ShouldBeEqual() {
			ISirenAction action = GetAction();
			ISirenAction other = GetAction();
			SirenTestHelpers.BidirectionalEquality( action, other, true );
		}

		[Test]
		public void SirenAction_Equality_DifferentFieldOrder_ShouldBeEqual() {
			ISirenAction action = GetAction();
			ISirenAction other = new SirenAction(
				name: action.Name,
				href: action.Href,
				@class: action.Class,
				method: action.Method,
				title: action.Title,
				type: action.Type,
				fields: new[] {
					action.Fields.ElementAt( 1 ),
					action.Fields.ElementAt( 2 ),
					action.Fields.ElementAt( 0 )
				}
			);
			SirenTestHelpers.BidirectionalEquality( action, other, true );
		}

		[Test]
		public void SirenAction_Equality_MissingAttributes_ShouldNotBeEqual() {
			ISirenAction action = GetAction();
			ISirenAction other = new SirenAction(
				name: action.Name,
				href: action.Href
			);
			SirenTestHelpers.BidirectionalEquality( action, other, false );
		}

		[Test]
		public void SirenAction_Equality_DifferentFields_ShouldNotBeEqual() {
			ISirenAction action = GetAction();
			ISirenAction other = new SirenAction(
				name: action.Name,
				href: action.Href,
				@class: action.Class,
				method: action.Method,
				title: action.Title,
				type: action.Type,
				fields: new[] {
					new SirenField( "fieldName1" )
				}
			);
			SirenTestHelpers.BidirectionalEquality( action, other, false );
		}

		[Test]
		public void SirenAction_ArrayEquality() {
			ISirenAction[] actions = { GetAction( "foo" ), GetAction( "bar" ) };
			ISirenAction[] others = { GetAction( "foo" ), GetAction( "bar" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( actions, others, true );

			others = new [] { GetAction( "bar" ), GetAction( "foo" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( actions, others, true );

			others = new [] { GetAction( "foo" ), GetAction( "foo" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( actions, others, false );

			others = new [] { GetAction( "foo" ) };
			SirenTestHelpers.ArrayBidirectionalEquality( actions, others, false );
		}

		[Test]
		public void SirenAction_Contains_SameAction_IsTrue() {
			ISirenAction action = GetAction();
			ISirenAction other = GetAction();
			Assert.IsTrue( action.Contains( action ) );
			Assert.IsTrue( action.Contains( other ) );
			Assert.IsTrue( other.Contains( action ) );
		}

		[Test]
		public void SirenAction_Contains_EmptyAction_IsTrue() {
			ISirenAction action = GetAction();
			ISirenAction other = new SirenAction( null, null );
			Assert.IsTrue( action.Contains( other ) );
		}

		[Test]
		public void SirenAction_Contains_SubsetAction_IsTrue() {
			ISirenAction action = GetAction();
			ISirenAction other = new SirenAction(
				name: action.Name,
				href: action.Href
			);
			Assert.IsTrue( action.Contains( other ) );
			Assert.IsFalse( other.Contains( action ) );
		}

		[Test]
		public void SirenAction_Contains_DifferentAction_IsFalse() {
			ISirenAction action = GetAction();
			ISirenAction other = new SirenAction(
				name: action.Name,
				href: action.Href,
				@class: action.Class,
				method: action.Method,
				title: action.Title,
				type: "different-type",
				fields: action.Fields
			);
			Assert.IsFalse( action.Contains( other ) );
			Assert.IsFalse( other.Contains( action ) );
		}

	}

}