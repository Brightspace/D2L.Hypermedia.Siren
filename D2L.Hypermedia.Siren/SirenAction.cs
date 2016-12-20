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
		public string Name {
			get { return m_name; }
		}

		[JsonProperty( "class", NullValueHandling = NullValueHandling.Ignore )]
		public string[] Class {
			get { return m_class; }
		}

		[JsonProperty( "method", NullValueHandling = NullValueHandling.Ignore )]
		public string Method {
			get { return m_method; }
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

		[JsonProperty( "fields", NullValueHandling = NullValueHandling.Ignore )]
		[JsonConverter( typeof(HypermediaFieldConverter) )]
		public IEnumerable<ISirenField> Fields {
			get { return m_fields; }
		}

		public bool ShouldSerializeClass() {
			return Class.Length > 0;
		}

		public bool ShouldSerializeFields() {
			return Fields.Any();
		}

	}

	public class HypermediaActionConverter : JsonConverter {

		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			serializer.Serialize( writer, value );
		}

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			return serializer.Deserialize<SirenAction[]>( reader );
		}

		public override bool CanConvert( Type objectType ) {
			return objectType == typeof( SirenAction );
		}

	}

}