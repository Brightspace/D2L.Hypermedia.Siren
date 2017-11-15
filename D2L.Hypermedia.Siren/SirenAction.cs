using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public class SirenAction : ISirenAction {

		private readonly string m_name;
		private readonly string[] m_class;
		private readonly IEnumerable<ISirenField> m_fields;
		private readonly string m_type;
		private readonly string m_title;
		private readonly Uri m_href;
		private readonly string m_method;

		public SirenAction(
			string name,
			Uri href,
			string[] @class = null,
			string method = null,
			string title = null,
			string type = null,
			IEnumerable<ISirenField> fields = null
		) {
			m_name = name;
			m_class = @class ?? new string[0];
			m_method = method;
			m_href = href;
			m_title = title;
			m_type = type;
			m_fields = fields ?? new List<ISirenField>();
		}

		[JsonProperty( "name" )]
		public string Name => m_name;

		[JsonProperty( "class", NullValueHandling = NullValueHandling.Ignore )]
		public string[] Class => m_class;

		[JsonProperty( "method", NullValueHandling = NullValueHandling.Ignore )]
		public string Method => m_method;

		[JsonProperty( "href" )]
		public Uri Href => m_href;

		[JsonProperty( "title", NullValueHandling = NullValueHandling.Ignore )]
		public string Title => m_title;

		[JsonProperty( "type", NullValueHandling = NullValueHandling.Ignore )]
		public string Type => m_type;

		[JsonProperty( "fields", NullValueHandling = NullValueHandling.Ignore )]
		[JsonConverter( typeof(HypermediaFieldEnumerableConverter) )]
		public IEnumerable<ISirenField> Fields => m_fields;

		public bool ShouldSerializeClass() {
			return Class.Length > 0;
		}

		public bool ShouldSerializeFields() {
			return Fields.Any();
		}

		bool IEquatable<ISirenAction>.Equals( ISirenAction other ) {
			if( other == null ) {
				return false;
			}

			bool name = m_name == other.Name;
			bool @class = m_class.OrderBy( x => x ).SequenceEqual( other.Class.OrderBy( x => x ) );
			bool method = m_method == other.Method;
			bool href = m_href == other.Href;
			bool title = m_title == other.Title;
			bool type = m_type == other.Type;
			bool fields = m_fields.OrderBy( x => x ).SequenceEqual( other.Fields.OrderBy( x => x ) );

			return name && @class && method && href && title && type && fields;
		}

		int IComparable<ISirenAction>.CompareTo( ISirenAction other ) {
			if( other == null ) {
				return 1;
			}

			return string.CompareOrdinal( m_name, other.Name );
		}

		int IComparable.CompareTo( object obj ) {
			ISirenAction @this = this;
			return @this.CompareTo( (ISirenAction)obj );
		}

		public override bool Equals( object obj ) {
			ISirenAction action = obj as ISirenAction;
			ISirenAction @this = this;
			return action != null && @this.Equals( action );
		}

		public override int GetHashCode() {
			return m_name.GetHashCode()
				^ string.Join( ",", m_class ).GetHashCode()
				^ m_method?.GetHashCode() ?? 0
				^ m_href?.GetHashCode() ?? 0
				^ m_title?.GetHashCode() ?? 0
				^ m_type?.GetHashCode() ?? 0
				^ m_fields.Select( x => x.GetHashCode() ).GetHashCode();
		}

		void ISirenSerializable.ToJson( JsonWriter writer ) {
			writer.WriteStartObject();

			JsonUtilities.WriteJsonArray( writer, "class", m_class );
			JsonUtilities.WriteJsonString( writer, "type", m_type );
			JsonUtilities.WriteJsonString( writer, "title", m_title );
			JsonUtilities.WriteJsonUri( writer, "href", m_href );
			JsonUtilities.WriteJsonString( writer, "name", m_name );
			JsonUtilities.WriteJsonString( writer, "method", m_method );
			JsonUtilities.WriteJsonSerializables( writer, "fields", m_fields );

			writer.WriteEndObject();
		}

	}

	public class HypermediaActionEnumerableConverter : JsonConverter {

		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			if( !( value is IEnumerable<ISirenAction> actions ) ) {
				return;
			}

			writer.WriteStartArray();
			foreach( ISirenAction action in actions ) {
				action.ToJson( writer );
			}
			writer.WriteEndArray();
		}

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			return serializer.Deserialize<SirenAction[]>( reader );
		}

		public override bool CanConvert( Type objectType ) {
			return typeof( IEnumerable<ISirenAction> ).IsAssignableFrom( objectType );
		}

	}

}