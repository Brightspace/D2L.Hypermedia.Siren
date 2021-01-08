using System;
using System.Collections.Generic;
using System.Linq;

namespace D2L.Hypermedia.Siren {

	public static class SirenMatchers {

		private const string m_messageTemplate = "Expected {0}, but was {1}";
		private const string m_arrayMessageTemplate = "Expected [{0}], but was [{1}]";

		public static bool Matches( string expected, string actual, out string message ) {
			if( expected == null || expected == actual || expected.Equals( actual ) ) {
				message = null;
				return true;
			}

			message = string.Format( m_messageTemplate, expected, actual );
			return false;
		}

		public static bool Matches( object expected, object actual, out string message ) {
			if( expected == null || expected == actual || expected.Equals( actual ) ) {
				message = null;
				return true;
			}

			message = string.Format( m_messageTemplate, expected, actual );
			return false;
		}

		public static bool Matches( ISirenAction expected, ISirenAction actual, out string message ) {
			return Matches( expected.Name, actual.Name, out message )
				&& Matches( expected.Method, actual.Method, out message )
				&& Matches( expected.Title, actual.Title, out message )
				&& Matches( expected.Type, actual.Type, out message )
				&& Matches( expected.Href, actual.Href, out message )
				&& Matches( expected.Class, actual.Class, out message )
				&& Matches( expected.Fields, actual.Fields, out message );
		}

		public static bool Matches( ISirenEntity expected, ISirenEntity actual, out string message ) {
			if( expected.Properties != null ) {
				throw new ArgumentException( "SirenMatchers cannot compare properties - remove them from the expected entity" );
			}

			return Matches( expected.Rel, actual.Rel, out message )
				&& Matches( expected.Class, actual.Class, out message )
				// Need to figure out a good way to match dynamics - considering moving away from dynamic for Properties
				//&& Matches( expected.Properties, actual.Properties, out message )
				&& Matches( expected.Entities, actual.Entities, out message )
				&& Matches( expected.Links, actual.Links, out message )
				&& Matches( expected.Actions, actual.Actions, out message )
				&& Matches( expected.Href, actual.Href, out message )
				&& Matches( expected.Title, actual.Title, out message )
				&& Matches( expected.Type, actual.Type, out message );
		}

		public static bool Matches( ISirenField expected, ISirenField actual, out string message ) {
			return Matches( expected.Name, actual.Name, out message )
				&& Matches( expected.Type, actual.Type, out message )
				&& Matches( expected.Title, actual.Title, out message )
				&& Matches( expected.Class, actual.Class, out message )
				&& Matches( expected.Value, actual.Value, out message )
				&& Matches( expected.Min, actual.Min, out message )
				&& Matches( expected.Max, actual.Max, out message );
		}

		public static bool Matches( ISirenLink expected, ISirenLink actual, out string message ) {
			return Matches( expected.Title, actual.Title, out message )
				&& Matches( expected.Type, actual.Type, out message )
				&& Matches( expected.Href, actual.Href, out message )
				&& Matches( expected.Rel, actual.Rel, out message )
				&& Matches( expected.Class, actual.Class, out message );
		}

		public static bool Matches<T>( IEnumerable<T> expectedSet, IEnumerable<T> actualSet, out string message ) {
			if( expectedSet == null || expectedSet.All( actualSet.Contains ) ) {
				message = null;
				return true;
			}

			string tempMessage = null;
			bool matches = false;

			if( typeof( T ) == typeof( ISirenAction ) ) {
				matches = ( (IEnumerable<ISirenAction>)expectedSet ).All(
					expectedAction => actualSet.Any(
						actualAction => Matches( expectedAction, (ISirenAction)actualAction, out tempMessage )
					) );
			} else if( typeof( T ) == typeof( ISirenEntity ) ) {
				matches = ( (IEnumerable<ISirenEntity>)expectedSet ).All(
					expectedEntity => actualSet.Any(
						actualEntity => Matches( expectedEntity, (ISirenEntity)actualEntity, out tempMessage )
					) );
			} else if( typeof( T ) == typeof( ISirenField ) ) {
				matches = ( (IEnumerable<ISirenField>)expectedSet ).All(
					expectedField => actualSet.Any(
						actualField => Matches( expectedField, (ISirenField)actualField, out tempMessage )
					) );
			} else if( typeof( T ) == typeof( ISirenLink ) ) {
				matches = ( (IEnumerable<ISirenLink>)expectedSet ).All(
					expectedLink => actualSet.Any(
						actualLink => Matches( expectedLink, (ISirenLink)actualLink, out tempMessage )
					) );
			} else {
				tempMessage = string.Format( m_arrayMessageTemplate, string.Join( ",", expectedSet ), string.Join( ",", actualSet ) );
			}

			if( matches ) {
				message = null;
				return true;
			}

			message = tempMessage;
			return false;
		}

	}

}