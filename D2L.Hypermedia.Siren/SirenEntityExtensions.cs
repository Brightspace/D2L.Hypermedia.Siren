using System.Collections.Generic;
using System.Linq;

namespace D2L.Hypermedia.Siren {

	public static class SirenEntityExtensions {

		public static bool TryGetActionByName(
				this ISirenEntity @this,
				string name,
				out ISirenAction action
		) {
			action = @this.Actions.FirstOrDefault( a => a.Name != null && a.Name.Equals( name ) );
			return action != default( ISirenAction );
		}

		public static bool TryGetActionByClass(
				this ISirenEntity @this,
				string @class,
				out ISirenAction action
		) {
			action = @this.ActionsByClass( @class ).FirstOrDefault();
			return action != default( ISirenAction );
		}

		public static bool TryGetLinkByRel(
				this ISirenEntity @this,
				string rel,
				out ISirenLink link
		) {
			link = @this.LinksByRel( rel ).FirstOrDefault();
			return link != default( ISirenLink );
		}

		public static bool TryGetLinkByClass(
				this ISirenEntity @this,
				string @class,
				out ISirenLink link
		) {
			link = @this.LinksByClass( @class ).FirstOrDefault();
			return link != default( ISirenLink );
		}

		public static bool TryGetLinkByType(
				this ISirenEntity @this,
				string type,
				out ISirenLink link
		) {
			link = @this.LinksByType( type ).FirstOrDefault();
			return link != default( ISirenLink );
		}

		public static bool TryGetSubEntityByRel(
				this ISirenEntity @this,
				string rel,
				out ISirenEntity entity
		) {
			entity = @this.SubEntitiesByRel( rel ).FirstOrDefault();
			return entity != default( ISirenEntity );
		}

		public static bool TryGetSubEntityByClass(
				this ISirenEntity @this,
				string @class,
				out ISirenEntity entity
		) {
			entity = @this.SubEntitiesByClass( @class ).FirstOrDefault();
			return entity != default( ISirenEntity );
		}

		public static bool TryGetSubEntityByType(
				this ISirenEntity @this,
				string type,
				out ISirenEntity entity
		) {
			entity = @this.SubEntitiesByType( type ).FirstOrDefault();
			return entity != default( ISirenEntity );
		}

		public static IEnumerable<ISirenAction> ActionsByClass(
				this ISirenEntity @this,
				string @class
		) {
			return @this.Actions.Where( action => action.Class != null && action.Class.Contains( @class ) );
		}

		public static IEnumerable<ISirenLink> LinksByRel(
				this ISirenEntity @this,
				string rel
		) {
			return @this.Links.Where( link => link.Rel != null && link.Rel.Contains( rel ) );
		}

		public static IEnumerable<ISirenLink> LinksByClass(
				this ISirenEntity @this,
				string @class
		) {
			return @this.Links.Where( link => link.Class != null && link.Class.Contains( @class ) );
		}

		public static IEnumerable<ISirenLink> LinksByType(
				this ISirenEntity @this,
				string type
		) {
			return @this.Links.Where( link => link.Type != null && link.Type.Equals( type ) );
		}

		public static IEnumerable<ISirenEntity> SubEntitiesByRel(
				this ISirenEntity @this,
				string rel
		) {
			return @this.Entities.Where( entity => entity.Rel != null && entity.Rel.Contains( rel ) );
		}

		public static IEnumerable<ISirenEntity> SubEntitiesByClass(
				this ISirenEntity @this,
				string @class
		) {
			return @this.Entities.Where( entity => entity.Class != null && entity.Class.Contains( @class ) );
		}

		public static IEnumerable<ISirenEntity> SubEntitiesByType(
				this ISirenEntity @this,
				string type
		) {
			return @this.Entities.Where( entity => entity.Type != null && entity.Type.Equals( type ) );
		}

	}

}