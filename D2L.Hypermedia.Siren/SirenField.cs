using System;
using System.Linq;
using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public class SirenField : ISirenField {

		private readonly string m_title;
		private readonly object m_value;
		private readonly string m_type;
		private readonly string[] m_class;
		private readonly string m_name;

		public SirenField(
			string name,
			string[] @class = null,
			string type = null,
			object value = null,
			string title = null
		) {
			m_name = name;
			m_class = @class ?? new string[0];
			m_type = type;
			m_value = value;
			m_title = title;
		}

		[JsonProperty( "name" )]
		public string Name {
			get { return m_name; }
		}

		[JsonProperty( "class", NullValueHandling = NullValueHandling.Ignore )]
		public string[] Class {
			get { return m_class; }
		}

		[JsonProperty( "type", NullValueHandling = NullValueHandling.Ignore )]
		public string Type {
			get { return m_type; }
		}

		[JsonProperty( "value", NullValueHandling = NullValueHandling.Ignore )]
		public object Value {
			get { return m_value; }
		}

		[JsonProperty( "title", NullValueHandling = NullValueHandling.Ignore )]
		public string Title {
			get { return m_title; }
		}

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

			return name && @class && type && value && title;
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
				^ m_type?.GetHashCode() ?? 0
				^ m_value?.ToString().GetHashCode() ?? 0
				^ m_title?.GetHashCode() ?? 0;
		}

	}

	public class HypermediaFieldConverter : JsonConverter {

		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			serializer.Serialize( writer, value );
		}

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			return serializer.Deserialize<SirenField[]>( reader );
		}

		public override bool CanConvert( Type objectType ) {
			return objectType == typeof( SirenField );
		}

	}

}