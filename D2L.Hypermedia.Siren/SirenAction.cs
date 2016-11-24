using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public class SirenAction : ISirenAction {

		private readonly IEnumerable<ISirenField> m_fields;
		private readonly string m_type;
		private readonly string m_title;
		private readonly Uri m_href;
		private readonly string m_method;
		private readonly string[] m_class;
		private readonly string m_name;

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
			m_class = @class;
			m_method = method;
			m_href = href;
			m_title = title;
			m_type = type;
			m_fields = fields;
		}

		[JsonProperty( "name" )]
		string ISirenAction.Name {
			get { return m_name; }
		}

		[JsonProperty( "class", NullValueHandling = NullValueHandling.Ignore )]
		string[] ISirenAction.Class {
			get { return m_class; }
		}

		[JsonProperty( "method", NullValueHandling = NullValueHandling.Ignore )]
		string ISirenAction.Method {
			get { return m_method; }
		}

		[JsonProperty( "href" )]
		Uri ISirenAction.Href {
			get { return m_href; }
		}

		[JsonProperty( "title", NullValueHandling = NullValueHandling.Ignore )]
		string ISirenAction.Title {
			get { return m_title; }
		}

		[JsonProperty( "type", NullValueHandling = NullValueHandling.Ignore )]
		string ISirenAction.Type {
			get { return m_type; }
		}

		[JsonProperty( "fields", NullValueHandling = NullValueHandling.Ignore )]
		[JsonConverter( typeof( HypermediaFieldConverter ) )]
		IEnumerable<ISirenField> ISirenAction.Fields {
			get { return m_fields; }
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