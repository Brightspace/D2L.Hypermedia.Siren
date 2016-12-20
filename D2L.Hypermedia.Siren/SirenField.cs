using System;
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