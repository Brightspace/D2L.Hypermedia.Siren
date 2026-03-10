using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	[TestFixture]
	public class SirenActionTests {

		[Test]
		public void SirenAction_Serialized_DoesNotIncludeOptionalParametersIfNull() {
			ISirenAction sirenAction = new SirenAction(
				name: "foo",
				href: new Uri( "http://example.com" )
			);

			string serialized = JsonConvert.SerializeObject( sirenAction );
			ISirenAction action = JsonConvert.DeserializeObject<SirenAction>( serialized );

			action.Name.Should().Be( "foo" );
			action.Href.ToString().Should().Be( "http://example.com/" );
			action.Class.Should().BeEmpty();
			action.Method.Should().BeNull();
			action.Title.Should().BeNull();
			action.Type.Should().BeNull();
			action.Fields.Should().BeEmpty();
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
				fields: new[] { new SirenField( name: "field" ) }
			);

			string serialized = JsonConvert.SerializeObject( sirenAction );
			ISirenAction action = JsonConvert.DeserializeObject<SirenAction>( serialized );

			action.Name.Should().Be( "foo" );
			action.Href.ToString().Should().Be( "http://example.com/" );
			action.Class.Should().Contain( "bar" );
			action.Method.Should().Be( "GET" );
			action.Title.Should().Be( "Some action" );
			action.Type.Should().Be( "text/html" );
			action.Fields.ToList().Count.Should().Be( 1 );
		}

		[Test]
		public void SirenAction_Serialize_ExcludesClassAndFieldsIfEmpty() {
			ISirenAction action = new SirenAction(
				name: "foo",
				href: new Uri( "http://example.com" ),
				@class: new[] { "bar" },
				fields: new[] { new SirenField( "baz" ) }
			);
			string serialized = JsonConvert.SerializeObject( action );
			serialized.IndexOf( "class", StringComparison.Ordinal ).Should().BeGreaterThanOrEqualTo( 0 );
			serialized.IndexOf( "fields", StringComparison.Ordinal ).Should().BeGreaterThanOrEqualTo( 0 );

			action = new SirenAction(
				name: "foo",
				href: new Uri( "http://example.com" )
			);
			serialized = JsonConvert.SerializeObject( action );
			serialized.IndexOf( "class", StringComparison.Ordinal ).Should().Be( -1 );
			serialized.IndexOf( "fields", StringComparison.Ordinal ).Should().Be( -1 );
		}

		[Test]
		public void SirenAction_TryGetFieldByName_ReturnsCorrectField() {
			ISirenField field;
			TestHelpers.GetAction().TryGetFieldByName( "foo", out field ).Should().BeFalse();
			field.Should().BeNull();

			TestHelpers.GetAction().TryGetFieldByName( "field1", out field ).Should().BeTrue();
			field.Name.Should().Be( "field1" );
		}

		[Test]
		public void SirenAction_TryGetFieldByClass_ReturnsCorrectField() {
			ISirenField field;
			TestHelpers.GetAction().TryGetFieldByClass( "foo", out field ).Should().BeFalse();
			field.Should().BeNull();

			TestHelpers.GetAction().TryGetFieldByClass( "class", out field ).Should().BeTrue();
			field.Class.Should().Contain( "class" );
			field.Name.Should().Be( "field1" );
		}

		[Test]
		public void SirenAction_TryGetFieldByType_ReturnsCorrectField() {
			ISirenField field;
			TestHelpers.GetAction().TryGetFieldByType( "foo", out field ).Should().BeFalse();
			field.Should().BeNull();

			TestHelpers.GetAction().TryGetFieldByType( "range", out field ).Should().BeTrue();
			field.Type.Should().Be( "range" );
			field.Name.Should().Be( "field3" );
		}

		[Test]
		public void SirenAction_Equality_SameAction_ShouldBeEqual() {
			ISirenAction action = TestHelpers.GetAction();
			ISirenAction other = TestHelpers.GetAction();
			TestHelpers.BidirectionalEquality( action, other, true );
		}

		[Test]
		public void SirenAction_Equality_DifferentFieldOrder_ShouldBeEqual() {
			ISirenAction action = TestHelpers.GetAction();
			ISirenAction other = new SirenAction(
				name: action.Name,
				href: action.Href,
				@class: action.Class,
				method: action.Method,
				title: action.Title,
				type: action.Type,
				fields: new[] { action.Fields.ElementAt( 1 ), action.Fields.ElementAt( 2 ), action.Fields.ElementAt( 0 ) }
			);
			TestHelpers.BidirectionalEquality( action, other, true );
		}

		[Test]
		public void SirenAction_Equality_MissingAttributes_ShouldNotBeEqual() {
			ISirenAction action = TestHelpers.GetAction();
			ISirenAction other = new SirenAction(
				name: action.Name,
				href: action.Href
			);
			TestHelpers.BidirectionalEquality( action, other, false );
		}

		[Test]
		public void SirenAction_Equality_DifferentFields_ShouldNotBeEqual() {
			ISirenAction action = TestHelpers.GetAction();
			ISirenAction other = new SirenAction(
				name: action.Name,
				href: action.Href,
				@class: action.Class,
				method: action.Method,
				title: action.Title,
				type: action.Type,
				fields: new[] { new SirenField( "fieldName1" ) }
			);
			TestHelpers.BidirectionalEquality( action, other, false );
		}

		[Test]
		public void SirenAction_ArrayEquality() {
			ISirenAction[] actions = { TestHelpers.GetAction( "foo" ), TestHelpers.GetAction( "bar" ) };
			ISirenAction[] others = { TestHelpers.GetAction( "foo" ), TestHelpers.GetAction( "bar" ) };
			TestHelpers.ArrayBidirectionalEquality( actions, others, true );

			others = new[] { TestHelpers.GetAction( "bar" ), TestHelpers.GetAction( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( actions, others, true );

			others = new[] { TestHelpers.GetAction( "foo" ), TestHelpers.GetAction( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( actions, others, false );

			others = new[] { TestHelpers.GetAction( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( actions, others, false );
		}

		private static ISirenAction[] HashCodeActions() {
			return new ISirenAction[] {
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" )
				),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					@class: new[] { "action" },
					method: "POST",
					title: "Create",
					type: "application/x-www-form-urlencoded",
					fields: new[] { TestHelpers.GetField() }
				),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					method: "POST",
					title: "Create",
					type: "application/x-www-form-urlencoded",
					fields: new[] { TestHelpers.GetField() }
				),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					@class: new[] { "action" },
					title: "Create",
					type: "application/x-www-form-urlencoded",
					fields: new[] { TestHelpers.GetField() }
				),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					@class: new[] { "action" },
					method: "POST",
					type: "application/x-www-form-urlencoded",
					fields: new[] { TestHelpers.GetField() }
				),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					@class: new[] { "action" },
					method: "POST",
					title: "Create",
					fields: new[] { TestHelpers.GetField() }
				),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					@class: new[] { "action" },
					method: "POST",
					title: "Create",
					type: "application/x-www-form-urlencoded"
				),
			};
		}

		private static TestCaseData[] HashCodeTests() {
			return HashCodeActions().Select( x => new TestCaseData( x ) ).ToArray();
		}

		private static IEnumerable<TestCaseData> HashCodeEqualityTests() {
			foreach( var action1 in HashCodeActions() ) {
				var innerEntities = HashCodeActions().ToList();
				innerEntities.Remove( action1 );
				foreach( var action2 in innerEntities ) {
					yield return new TestCaseData( action1, action2 );
				}
			}
		}

		[TestCaseSource( nameof( HashCodeTests ) )]
		public void SirenAction_GetHashcodeNot0( ISirenAction action ) {
			action.GetHashCode().Should().NotBe( 0 );
		}

		[TestCaseSource( nameof( HashCodeEqualityTests ) )]
		public void SirenAction_GetHashCode_NotEqual( ISirenAction action1, ISirenAction action2 ) {
			action1.GetHashCode().Should().NotBe( action2.GetHashCode() );
		}
	}
}
