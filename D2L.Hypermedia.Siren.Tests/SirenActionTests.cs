﻿using System;
using System.Collections.Generic;
using System.Linq;
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
		public void SirenAction_TryGetFieldByName_ReturnsCorrectField() {
			ISirenField field;
			Assert.IsFalse( TestHelpers.GetAction().TryGetFieldByName( "foo", out field ) );
			Assert.IsNull( field );

			Assert.IsTrue( TestHelpers.GetAction().TryGetFieldByName( "field1", out field ) );
			Assert.AreEqual( "field1", field.Name );
		}

		[Test]
		public void SirenAction_TryGetFieldByClass_ReturnsCorrectField() {
			ISirenField field;
			Assert.IsFalse( TestHelpers.GetAction().TryGetFieldByClass( "foo", out field ) );
			Assert.IsNull( field );

			Assert.IsTrue( TestHelpers.GetAction().TryGetFieldByClass( "class", out field ) );
			Assert.Contains( "class", field.Class );
			Assert.AreEqual( "field1", field.Name );
		}

		[Test]
		public void SirenAction_TryGetFieldByType_ReturnsCorrectField() {
			ISirenField field;
			Assert.IsFalse( TestHelpers.GetAction().TryGetFieldByType( "foo", out field ) );
			Assert.IsNull( field );

			Assert.IsTrue( TestHelpers.GetAction().TryGetFieldByType( "range", out field ) );
			Assert.AreEqual( "range", field.Type );
			Assert.AreEqual( "field3", field.Name );
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
				fields: new[] {
					action.Fields.ElementAt( 1 ),
					action.Fields.ElementAt( 2 ),
					action.Fields.ElementAt( 0 )
				}
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
				fields: new[] {
					new SirenField( "fieldName1" )
				}
			);
			TestHelpers.BidirectionalEquality( action, other, false );
		}

		[Test]
		public void SirenAction_ArrayEquality() {
			ISirenAction[] actions = { TestHelpers.GetAction( "foo" ), TestHelpers.GetAction( "bar" ) };
			ISirenAction[] others = { TestHelpers.GetAction( "foo" ), TestHelpers.GetAction( "bar" ) };
			TestHelpers.ArrayBidirectionalEquality( actions, others, true );

			others = new [] { TestHelpers.GetAction( "bar" ), TestHelpers.GetAction( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( actions, others, true );

			others = new [] { TestHelpers.GetAction( "foo" ), TestHelpers.GetAction( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( actions, others, false );

			others = new [] { TestHelpers.GetAction( "foo" ) };
			TestHelpers.ArrayBidirectionalEquality( actions, others, false );
		}

		private static ISirenAction[] HashCodeActions()
        {
			return new ISirenAction[]
            {
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" )
					),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					@class: new [] { "action" },
					method: "POST",
					title: "Create",
					type: "application/x-www-form-urlencoded",
					fields: new []{ TestHelpers.GetField() }
					),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					method: "POST",
					title: "Create",
					type: "application/x-www-form-urlencoded",
					fields: new []{ TestHelpers.GetField() }
					),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					@class: new [] { "action" },
					title: "Create",
					type: "application/x-www-form-urlencoded",
					fields: new []{ TestHelpers.GetField() }
					),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					@class: new [] { "action" },
					method: "POST",
					type: "application/x-www-form-urlencoded",
					fields: new []{ TestHelpers.GetField() }
					),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					@class: new [] { "action" },
					method: "POST",
					title: "Create",
					fields: new []{ TestHelpers.GetField() }
					),
				new SirenAction(
					name: "action",
					href: new Uri( "http://localhost" ),
					@class: new [] { "action" },
					method: "POST",
					title: "Create",
					type: "application/x-www-form-urlencoded"
					),
            };
        }

		private static TestCaseData[] HashCodeTests()
        {
           return HashCodeActions().Select( x => new TestCaseData( x ) ).ToArray() ;
        }

		private static IEnumerable<TestCaseData> HashCodeEqualityTests() {
            foreach( var action1 in HashCodeActions() )
            {
                var innerEntities = HashCodeActions().ToList();
				innerEntities.Remove( action1 );
                foreach (var action2 in innerEntities )
                {
                    yield return new TestCaseData( action1, action2 );
                }

            }
        }

        [TestCaseSource( nameof( HashCodeTests ) ) ]
		public void SirenAction_GetHashcodeNot0( ISirenAction action )
        {
			Assert.AreNotEqual( 0, action.GetHashCode() );
        }

		[TestCaseSource( nameof( HashCodeEqualityTests ) ) ]
		public void SirenAction_GetHashCode_NotEqual( ISirenAction action1, ISirenAction action2 )
        {
			Assert.AreNotEqual( action1.GetHashCode(), action2.GetHashCode() );
        }

	}

}