using System;
using System.Linq;
using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public class SirenLink : ISirenLink {

		private readonly string m_type;
		private readonly string m_title;
		private readonly Uri m_href;
		private readonly string[] m_class;
		private readonly string[] m_rel;

		public SirenLink(
			string[] rel,
			Uri href,
			string[] @class = null,
			string title = null,
			string type = null
		) {
			m_rel = rel ?? new string[0];
			m_href = href;
			m_class = @class ?? new string[0];
			m_title = title;
			m_type = type;
		}

		[JsonProperty( "rel" )]
		public string[] Rel {
			get { return m_rel; }
		}

		[JsonProperty( "class", NullValueHandling = NullValueHandling.Ignore )]
		public string[] Class {
			get { return m_class; }
		}

		[JsonProperty( "href" )]
		public Uri Href {
			get { return m_href; }
		}

		[JsonProperty( "title", NullValueHandling = NullValueHandling.Ignore )]
		public string Title {
			get { return m_title; }
		}

		[JsonProperty( "type", NullValueHandling = NullValueHandling.Ignore )]
		public string Type {
			get { return m_type; }
		}

		public bool ShouldSerializeClass() {
			return Class.Length > 0;
		}

		bool IEquatable<ISirenLink>.Equals( ISirenLink other ) {
			if( other == null ) {
				return false;
			}

			bool rel = m_rel.OrderBy( x => x ).SequenceEqual( other.Rel.OrderBy( x => x ) );
			bool href = m_href == other.Href;
			bool @class = m_class.OrderBy( x => x ).SequenceEqual( other.Class.OrderBy( x => x ) );
			bool title = m_title == other.Title;
			bool type = m_type == other.Type;

			return rel && href && @class && title && type;
		}

		int IComparable<ISirenLink>.CompareTo( ISirenLink other ) {
			if( other == null ) {
				return 1;
			}

			string first = string.Join( ",", m_rel ) + m_href;
			string second = string.Join( ",", other.Rel ) + other.Href;
			return string.CompareOrdinal( first, second );
		}

		int IComparable.CompareTo( object obj ) {
			ISirenLink @this = this;
			return @this.CompareTo( (ISirenLink)obj );
		}

		public override bool Equals( object obj ) {
			ISirenLink link = obj as ISirenLink;
			ISirenLink @this = this;
			return link != null && @this.Equals( link );
		}

		public override int GetHashCode() {
			return string.Join( ",", m_rel ).GetHashCode()
				^ m_href.GetHashCode()
				^ string.Join( ",", m_class ).GetHashCode()
				^ m_title?.GetHashCode() ?? 0
				^ m_type?.GetHashCode() ?? 0;
		}

		string ISirenSerializable.ToJson() {
			throw new NotImplementedException();
		}

	}

	public class HypermediaLinkConverter : JsonConverter {

		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			serializer.Serialize( writer, value );
		}

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			return serializer.Deserialize<SirenLink[]>( reader );
		}

		public override bool CanConvert( Type objectType ) {
			return objectType == typeof( SirenLink );
		}

	}

}