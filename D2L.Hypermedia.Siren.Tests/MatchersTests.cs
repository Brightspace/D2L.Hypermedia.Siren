using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	public class MatchersTests {

		[TestCase( null, null, ExpectedResult = true )]
		[TestCase( null, "foo", ExpectedResult = true )]
		[TestCase( "foo", null, ExpectedResult = false )]
		[TestCase( "foo", "bar", ExpectedResult = false)]
		[TestCase( "foo", "foo", ExpectedResult = true )]
		public bool MatchingHelpers_MatchesStringsCorrectly( string expected, string actual ) {
			string message;
			bool match = SirenMatchers.Matches( expected, actual, out message );
			if( !match ) {
				Assert.IsTrue( Regex.IsMatch( message, $"Expected {expected}, but was {actual}" ) );
			}
			return match;
		}

		[Test]
		public void MatchingHelpers_MatchesUrisCorrectly() {
			string message;

			Uri expected = null;
			Uri actual = null;
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			actual = new Uri( "http://example.com" );
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new Uri( "http://example.com" );
			actual = new Uri( "http://example.com" );
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new Uri( "http://foo.com" );
			actual = new Uri( "http://example.com" );
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, $"Expected {expected}, but was {actual}" ) );
		}

		[Test]
		public void MatchingHelpers_MatchesObjectsCorrectly() {
			string message;

			object expected = null;
			object actual = null;
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			actual = 1;
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			actual = "foo";
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = 1;
			actual = "foo";
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, $"Expected {expected}, but was {actual}" ) );

			expected = "foo";
			actual = "bar";
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, $"Expected {expected}, but was {actual}" ) );

			expected = "foo";
			actual = "foo";
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );
		}

		[Test]
		public void MatchingHelpers_MatchesStringArraysCorrectly() {
			string message;

			IEnumerable<object> expected = null;
			IEnumerable<object> actual = new string[] { };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { "foo" };
			actual = new[] { "foo" };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { "foo" };
			actual = new[] { "bar" };
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, $"Expected {expected}, but was {actual}" ) );
		}

		[Test]
		public void MatchingHelpers_MatchesSirenActionArraysCorrectly() {
			string message;
			ISirenAction action = TestHelpers.GetAction();

			IEnumerable<ISirenAction> expected = null;
			IEnumerable<ISirenAction> actual = null;
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			actual = new[] { action };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenAction(
				name: action.Name,
				href: action.Href
			) };
			actual = new[] { action };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenAction(
				name: action.Name,
				href: action.Href,
				@class: action.Class,
				method: action.Method,
				title: action.Title,
				type: action.Type,
				fields: new [] {
					action.Fields.ElementAt( 0 )
				}
			) };
			actual = new[] { action };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenAction(
				name: action.Name,
				href: action.Href,
				@class: action.Class,
				method: action.Method,
				title: action.Title,
				type: action.Type,
				fields: action.Fields
			) };
			actual = new[] { action };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenAction(
				name: action.Name + "-foobar",
				href: action.Href,
				@class: action.Class,
				method: action.Method,
				title: action.Title,
				type: action.Type,
				fields: action.Fields
			) };
			actual = new[] { action };
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, "Expected action-name-foobar, but was action-name" ), message );

			expected = new[] { new SirenAction(
				name: action.Name,
				href: action.Href,
				@class: action.Class,
				method: action.Method,
				title: action.Title,
				type: action.Type,
				fields: new [] {
					action.Fields.ElementAt( 0 ),
					action.Fields.ElementAt( 1 ),
					new SirenField( name: "field3", @class: new [] { "not-class" }, type: SirenFieldType.Range, value: 1 )
				}
			) };
			actual = new [] { action };
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, "Expected 1, but was " ), message );
		}

		[Test]
		public void MatchingHelpers_MatchesSirenEntityArraysCorrectly() {
			string message;
			ISirenEntity entity = TestHelpers.GetEntity();
			ISirenLink link = TestHelpers.GetLink();

			IEnumerable<ISirenEntity> expected = null;
			IEnumerable<ISirenEntity> actual = null;
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			actual = new[] { entity };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenEntity(
				rel: entity.Rel
			) };
			actual = new[] { entity };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenEntity(
				rel: entity.Rel,
				@class: entity.Class,
				properties: entity.Properties,
				entities: entity.Entities,
				links: entity.Links,
				actions: entity.Actions,
				title: entity.Title,
				href: entity.Href,
				type: entity.Type
			) };
			actual = new[] { entity };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenEntity(
				rel: entity.Rel,
				@class: entity.Class,
				properties: entity.Properties,
				entities: new [] {
					entity.Entities.ElementAt( 0 ),
					entity.Entities.ElementAt( 1 )
				},
				links: new [] {
					entity.Links.ElementAt( 0 ),
					entity.Links.ElementAt( 1 )
				},
				actions: new [] {
					entity.Actions.ElementAt( 0 ),
					entity.Actions.ElementAt( 1 )
				},
				title: entity.Title,
				href: entity.Href,
				type: entity.Type
			) };
			actual = new[] { entity };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenEntity(
				entities: new [] {
					entity.Entities.ElementAt( 0 ),
					entity.Entities.ElementAt( 1 ),
					new SirenEntity( rel: new [] { "not-child" }, @class: new [] { "class" }, type: "text/xml", title: "different-title" )
				}
			) };
			actual = new[] { entity };
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, "Expected different-title, but was entity3" ), message );

			expected = new[] { new SirenEntity(
				links: new [] {
					entity.Links.ElementAt( 0 ),
					entity.Links.ElementAt( 1 ),
					new SirenLink( rel: new[] { "next" }, href: new Uri( "http://example.com" ), @class: new [] { "not-class" }, type: "text/html", title: "different-title" )
				}
			) };
			actual = new[] { entity };
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, "Expected different-title, but was link3" ), message );

			expected = new[] { new SirenEntity(
				actions: new [] {
					entity.Actions.ElementAt( 0 ),
					entity.Actions.ElementAt( 1 ),
					new SirenAction( name: "different-name", href: new Uri( "http://example.com" ), @class: new[] { "not-class" } )
				}
			) };
			actual = new[] { entity };
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, "Expected different-name, but was action3" ), message );
		}

		[Test]
		public void MatchingHelpers_MatchesSirenFieldArraysCorrectly() {
			string message;
			ISirenField field = TestHelpers.GetField();

			IEnumerable<ISirenField> expected = null;
			IEnumerable<ISirenField> actual = null;
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			actual = new[] { field };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenField(
				name: null
			) };
			actual = new[] { field };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenField(
				name: field.Name
			) };
			actual = new[] { field };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenField(
				name: field.Name,
				@class: field.Class,
				type: field.Type,
				value: field.Value,
				title: field.Title
			) };
			actual = new[] { field };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenField(
				name: field.Name,
				@class: field.Class,
				type: field.Type,
				value: "foo",
				title: field.Title
			) };
			actual = new[] { field };
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, "Expected foo, but was 1" ) );
		}

		[Test]
		public void MatchingHelpers_MatchesSirenLinkArraysCorrectly() {
			string message;
			ISirenLink link = TestHelpers.GetLink();

			IEnumerable<ISirenLink> expected = null;
			IEnumerable<ISirenLink> actual = null;
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			actual = new[] { link };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenLink(
				rel: null,
				href: null
			) };
			actual = new[] { link };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenLink(
				rel: new string[] {},
				href: null
			) };
			actual = new[] { link };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenLink(
				rel: link.Rel,
				href: link.Href
			) };
			actual = new[] { link };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenLink(
				rel: link.Rel,
				href: link.Href,
				@class: link.Class,
				title: link.Title,
				type: link.Type
			) };
			actual = new[] { link };
			Assert.IsTrue( SirenMatchers.Matches( expected, actual, out message ) );

			expected = new[] { new SirenLink(
				rel: new [] { "different-rel" },
				href: link.Href,
				@class: link.Class,
				title: link.Title,
				type: link.Type
			) };
			actual = new[] { link };
			Assert.IsFalse( SirenMatchers.Matches( expected, actual, out message ) );
			Assert.IsTrue( Regex.IsMatch( message, "Expected .*, but was .*" ) );
		}

	}

}