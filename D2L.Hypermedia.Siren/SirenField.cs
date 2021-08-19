using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public class SirenField : ISirenField {

		private readonly string m_title;
		private readonly object m_value;
		private readonly string m_type;
		private readonly string[] m_class;
		private readonly string m_name;
		private readonly decimal? m_min;
		private readonly decimal? m_max;

		public SirenField(
			string name,
			string[] @class = null,
			string type = null,
			object value = null,
			string title = null,
			decimal? min = null,
			decimal? max = null
		) {
			m_name = name;
			m_class = @class ?? new string[0];
			m_type = ValidateType( type );
			m_value = value;
			m_title = title;
			m_min = min;
			m_max = max;
		}

		private string ValidateType( string type ) {
			if( type == null ) {
				return null;
			}

			type = type.ToLowerInvariant();
			if (SirenFieldType.ValidTypes.Contains( type ) ) {
				return type;
			}

			throw new ArgumentException( $"\"{type}\" is not a valid type for a Siren Field. See the Siren documentation, or use SirenFieldTypes.");
		}

		[JsonProperty( "name" )]
		public string Name => m_name;

		[JsonProperty( "class", NullValueHandling = NullValueHandling.Ignore )]
		public string[] Class => m_class;

		[JsonProperty( "type", NullValueHandling = NullValueHandling.Ignore )]
		public string Type => m_type;

		[JsonProperty( "value", NullValueHandling = NullValueHandling.Ignore )]
		public object Value => m_value;

		[JsonProperty( "title", NullValueHandling = NullValueHandling.Ignore )]
		public string Title => m_title;

		[JsonProperty( "min", NullValueHandling = NullValueHandling.Ignore )]
		public decimal? Min => m_min;

		[JsonProperty( "max", NullValueHandling = NullValueHandling.Ignore )]
		public decimal? Max => m_max;

		public bool ShouldSerializeClass() {
			return Class.Length > 0;
		}

		bool IEquatable<ISirenField>.Equals( ISirenField other ) {
			if( other == null ) {
				return false;
			}

			bool name = m_name == other.Name;
			bool @class = m_class.OrderBy( x => x ).SequenceEqual( other.Class.OrderBy( x => x ) );
			bool type = m_type == other.Type;
			bool value = m_value == other.Value || m_value != null && m_value.Equals( other.Value );
			bool title = m_title == other.Title;
			bool min = m_min == other.Min;
			bool max = m_max == other.Max;

			return name && @class && type && value && title && min && max;
		}

		int IComparable<ISirenField>.CompareTo( ISirenField other ) {
			if( other == null ) {
				return 1;
			}

			return string.CompareOrdinal( m_name, other.Name );
		}

		int IComparable.CompareTo( object obj ) {
			ISirenField @this = this;
			return @this.CompareTo( (ISirenField)obj );
		}

		public override bool Equals( object obj ) {
			ISirenField field = obj as ISirenField;
			ISirenField @this = this;
			return field != null && @this.Equals( field );
		}

		public override int GetHashCode() {
			return m_name.GetHashCode()
				^ string.Join( ",", m_class ).GetHashCode()
				^ ( m_type?.GetHashCode() ?? 0 )
				^ ( m_value?.ToString().GetHashCode() ?? 0 )
				^ ( m_title?.GetHashCode() ?? 0 )
				^ ( m_min?.GetHashCode() ?? 0 )
				^ ( m_max?.GetHashCode() ?? 0 );
		}

		void ISirenSerializable.ToJson( JsonWriter writer ) {
			writer.WriteStartObject();

			JsonUtilities.WriteJsonArray( writer, "class", m_class );
			JsonUtilities.WriteJsonString( writer, "type", m_type );
			JsonUtilities.WriteJsonString( writer, "title", m_title );
			JsonUtilities.WriteJsonString( writer, "name", m_name );
			JsonUtilities.WriteJsonObject( writer, "value", m_value );
			JsonUtilities.WriteJsonNumber( writer, "min", m_min );
			JsonUtilities.WriteJsonNumber( writer, "max", m_max );

			writer.WriteEndObject();
		}

	}

	public class HypermediaFieldEnumerableConverter : JsonConverter {

		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			if( !( value is IEnumerable<ISirenField> ) ) {
				return;
			}

			IEnumerable<ISirenField> fields = (IEnumerable<ISirenField>)value;
			writer.WriteStartArray();
			foreach( ISirenField field in fields ) {
				field.ToJson( writer );
			}
			writer.WriteEndArray();
		}

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			return serializer.Deserialize<SirenField[]>( reader );
		}

		public override bool CanConvert( Type objectType ) {
			return typeof( IEnumerable<ISirenField> ).IsAssignableFrom( objectType );
		}

	}

}