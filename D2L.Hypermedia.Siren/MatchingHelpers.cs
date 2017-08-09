using System.Collections.Generic;
using System.Linq;

namespace D2L.Hypermedia.Siren {

	public static class MatchingHelpers {

		private const string m_messageTemplate = "Expected {0}, but was {1}";

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

		public static bool Matches<T>( IEnumerable<T> expectedSet, IEnumerable<T> actualSet, out string message ) {
			if( expectedSet == null || expectedSet.All( actualSet.Contains ) ) {
				message = null;
				return true;
			}

			string tempMessage = null;
			bool matches = false;

			if( typeof(T) == typeof(ISirenAction) ) {
				matches = ((IEnumerable<ISirenAction>)expectedSet).All( delegate ( ISirenAction expectedAction ) {
					return actualSet.Any( actualAction => ((ISirenAction)actualAction).Matches(
						out tempMessage,
						name: expectedAction.Name,
						@class: expectedAction.Class,
						method: expectedAction.Method,
						href: expectedAction.Href,
						title: expectedAction.Title,
						type: expectedAction.Type,
						fields: expectedAction.Fields
					) );
				} );
			} else if( typeof(T) == typeof(ISirenEntity) ) {
				matches = ((IEnumerable<ISirenEntity>)expectedSet).All( delegate ( ISirenEntity expectedEntity ) {
					return actualSet.Any( actualEntity => ((ISirenEntity)actualEntity).Matches(
						out tempMessage,
						@class: expectedEntity.Class,
						properties: expectedEntity.Properties,
						entities: expectedEntity.Entities,
						links: expectedEntity.Links,
						actions: expectedEntity.Actions,
						title: expectedEntity.Title,
						rel: expectedEntity.Rel,
						href: expectedEntity.Href,
						type: expectedEntity.Type
					) );
				} );
			} else if( typeof(T) == typeof(ISirenField) ) {
				matches = ((IEnumerable<ISirenField>)expectedSet).All( delegate ( ISirenField expectedField ) {
					return actualSet.Any( actualField => ((ISirenField)actualField).Matches(
						out tempMessage,
						name: expectedField.Name,
						@class: expectedField.Class,
						type: expectedField.Type,
						value: expectedField.Value,
						title: expectedField.Title
					) );
				} );
			} else if ( typeof(T) == typeof(ISirenLink) ) {
				matches = ((IEnumerable<ISirenLink>)expectedSet).All( delegate ( ISirenLink expectedLink ) {
					return actualSet.Any( actualLink => ((ISirenLink)actualLink).Matches(
						out tempMessage,
						rel: expectedLink.Rel,
						@class: expectedLink.Class,
						href: expectedLink.Href,
						title: expectedLink.Title,
						type: expectedLink.Type
					) );
				} );
			} else {
				tempMessage = string.Format( m_messageTemplate, expectedSet, actualSet );
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