using System;
using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public class SirenField : ISirenField {

		private readonly string m_name;
		private readonly string m_title;
		private readonly object m_value;
		private readonly string m_type;
		private readonly string[] m_class;

		public SirenField(
			string name,
			string[] @class = null,
			string type = null,
			object value = null,
			string title = null
		) {
			m_name = name;
			m_class = @class;
			m_type = type;
			m_value = value;
			m_title = title;
		}

		[JsonProperty( "name" )]
		string ISirenField.Name {
			get { return m_name; }
		}

		[JsonProperty( "class", NullValueHandling = NullValueHandling.Ignore )]
		string[] ISirenField.Class {
			get { return m_class; }
		}

		[JsonProperty( "type", NullValueHandling = NullValueHandling.Ignore )]
		string ISirenField.Type {
			get { return m_type; }
		}

		[JsonProperty( "value", NullValueHandling = NullValueHandling.Ignore )]
		object ISirenField.Value {
			get { return m_value; }
		}

		[JsonProperty( "title", NullValueHandling = NullValueHandling.Ignore )]
		string ISirenField.Title {
			get { return m_title; }
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