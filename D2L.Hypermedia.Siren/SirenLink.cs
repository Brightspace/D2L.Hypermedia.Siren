using System;
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
			m_rel = rel;
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