using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public static class JsonUtilities {

		public static void WriteJsonArray( JsonWriter writer, string propertyName, string[] values ) {
			if( values == null || values.Length == 0 ) {
				return;
			}

			writer.WritePropertyName( propertyName );
			writer.WriteStartArray();

			foreach( string value in values ) {
				writer.WriteValue( value );
			}

			writer.WriteEndArray();
		}

		public static void WriteJsonSerializable( JsonWriter writer, string propertyName, IEnumerable<ISirenSerializable> values ) {
			if( values == null || !values.Any() ) {
				return;
			}

			writer.WritePropertyName( propertyName );
			writer.WriteStartArray();

			foreach( ISirenSerializable serializable in values ) {
				writer.WriteRawValue( serializable.ToJson() );
			}

			writer.WriteEndArray();
		}

		public static void WriteJsonString( JsonWriter writer, string propertyName, string value ) {
			if( value == null ) {
				return;
			}

			writer.WritePropertyName( propertyName );
			writer.WriteValue( value );
		}

		public static void WriteJsonUri( JsonWriter writer, string propertyName, Uri value ) {
			if( value == null ) {
				return;
			}

			writer.WritePropertyName( propertyName );
			writer.WriteValue( value.AbsoluteUri );
		}

		public static void WriteJsonObject( JsonWriter writer, string propertyName, object value ) {
			if( value == null ) {
				return;
			}

			writer.WritePropertyName( propertyName );
			writer.WriteRawValue( JsonConvert.SerializeObject( value ) );
		}

	}

}
