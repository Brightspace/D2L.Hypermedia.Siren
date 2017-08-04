using System.Collections.Generic;
using System.Linq;

namespace D2L.Hypermedia.Siren {

	public static class SirenActionExtensions {

		public static bool TryGetField(
				this ISirenAction @this,
				ISirenField find,
				out ISirenField field
		) {
			field = @this.Fields.FirstOrDefault( f => f.Equals( find ) );
			return field != default( ISirenField );
		}

		public static bool TryGetFieldByName(
				this ISirenAction @this,
				string name,
				out ISirenField field
		) {
			field = @this.Fields.FirstOrDefault( f => f.Name != null && f.Name.Equals( name ) );
			return field != default( ISirenField );
		}

		public static bool TryGetFieldByClass(
				this ISirenAction @this,
				string @class,
				out ISirenField field
		) {
			field = @this.FieldsByClass( @class ).FirstOrDefault();
			return field != default( ISirenField );
		}

		public static bool TryGetFieldByType(
				this ISirenAction @this,
				string type,
				out ISirenField field
		) {
			field = @this.FieldsByType( type ).FirstOrDefault();
			return field != default( ISirenField );
		}

		public static IEnumerable<ISirenField> FieldsByClass(
				this ISirenAction @this,
				string @class
		) {
			return @this.Fields.Where( field => field.Class != null && field.Class.Contains( @class ) );
		}

		public static IEnumerable<ISirenField> FieldsByType(
				this ISirenAction @this,
				string type
		) {
			return @this.Fields.Where( field => field.Type != null && field.Type.Equals( type ) );
		}

	}

}