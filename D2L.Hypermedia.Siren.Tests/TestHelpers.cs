using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace D2L.Hypermedia.Siren.Tests {

	public static class TestHelpers {

		public static void BidirectionalEquality<T>(
			IComparable<T> first,
			IComparable<T> second,
			bool equal
		) {
			Assert.AreEqual( equal, first.Equals( second ) );
			Assert.AreEqual( equal, second.Equals( first ) );
		}

		public static void ArrayBidirectionalEquality<T>(
			IEnumerable<IComparable<T>> first,
			IEnumerable<IComparable<T>> second,
			bool equal
		) {
			Assert.AreEqual( equal, first.OrderBy( x => x ).SequenceEqual( second.OrderBy( x => x ) ) );
			Assert.AreEqual( equal, second.OrderBy( x => x ).SequenceEqual( first.OrderBy( x => x ) ) );
		}

		public static ISirenAction GetAction( string name = "action-name" ) {
			ISirenAction action = new SirenAction(
				name: name,
				href: new Uri( "http://example.com" ),
				@class: new[] { "foo" },
				method: "GET",
				title: "Action title",
				type: "some-type",
				fields: new[] {
					new SirenField( name: "field1", @class: new [] { "class" }, type: SirenFieldType.Text ),
					new SirenField( name: "field2", @class: new [] { "class" }, type: SirenFieldType.Text ),
					new SirenField( name: "field3", @class: new [] { "not-class" }, type: SirenFieldType.Range )
				}
			);

			return action;
		}

		public static ISirenEntity GetEntity( string title = "title" ) {
			ISirenEntity entity = new SirenEntity(
					title: title,
					rel: new[] { "rel" },
					@class: new[] { "class" },
					properties: new {
						foo = "bar"
					},
					href: new Uri( "http://example.com" ),
					type: "text/html",
					links: new[] {
						new SirenLink( rel: new[] { "self" }, href: new Uri( "http://example.com" ), @class: new [] { "class" }, type: "text/html", title: "link1" ),
						new SirenLink( rel: new[] { "next" }, href: new Uri( "http://example.com" ), @class: new [] { "class" }, type: "text/html", title: "link2" ),
						new SirenLink( rel: new[] { "next" }, href: new Uri( "http://example.com" ), @class: new [] { "not-class" }, type: "text/html", title: "link3" )
					},
					actions: new[] {
						new SirenAction( name: "action1", href: new Uri( "http://example.com" ), @class: new[] { "class" } ),
						new SirenAction( name: "action2", href: new Uri( "http://example.com" ), @class: new[] { "class" } ),
						new SirenAction( name: "action3", href: new Uri( "http://example.com" ), @class: new[] { "not-class" } )
					},
					entities: new[] {
						new SirenEntity( rel: new [] { "child" }, @class: new [] { "class" }, type: "text/html", title: "entity1" ),
						new SirenEntity( rel: new [] { "child" }, @class: new [] { "class" }, type: "text/html", title: "entity2" ),
						new SirenEntity( rel: new [] { "not-child" }, @class: new [] { "class" }, type: "text/xml", title: "entity3" )
					}
				);

			return entity;
		}

		public static ISirenField GetField( string name = "foo" ) {
			return new SirenField(
				name: name,
				@class: new[] { "bar" },
				type: "number",
				value: 1,
				title: "Some field",
				min: 0,
				max: 2
			);
		}

		public static SirenFieldValueObject GetFieldValueObject( string value = "foo" ) {
			return new SirenFieldValueObject(
				value: value,
				title: "Some field option"
			);
		}

		public static SirenFieldValueObject GetFieldValueObject( int value ) {
			return new SirenFieldValueObject(
				value: value,
				title: "Some field option"
			);
		}

		public static ISirenLink GetLink( string rel = "foo" ) {
			return new SirenLink(
				rel: new[] { rel },
				href: new Uri( "http://example.com" ),
				@class: new[] { "bar" },
				title: "Link title",
				type: "text/html"
			);
		}

	}

}
