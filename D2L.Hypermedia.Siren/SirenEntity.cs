using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public class SirenEntity : ISirenEntity {

		private readonly string[] m_class;
		private readonly dynamic m_properties;
		private readonly string m_type;
		private readonly Uri m_href;
		private readonly string[] m_rel;
		private readonly string m_title;
		private readonly IEnumerable<ISirenAction> m_actions;
		private readonly IEnumerable<ISirenLink> m_links;
		private readonly IEnumerable<ISirenEntity> m_entities;

		public SirenEntity(
			string[] rel = null,
			string[] @class = null,
			dynamic properties = null,
			IEnumerable<ISirenEntity> entities = null,
			IEnumerable<ISirenLink> links = null,
			IEnumerable<ISirenAction> actions = null,
			string title = null,
			Uri href = null,
			string type = null
		) {
			m_rel = rel;
			m_class = @class;
			m_properties = properties;
			m_entities = entities;
			m_links = links;
			m_actions = actions;
			m_title = title;
			m_href = href;
			m_type = type;
		}

		[JsonProperty( "class", NullValueHandling = NullValueHandling.Ignore )]
		string[] ISirenEntity.Class {
			get { return m_class; }
		}

		[JsonProperty( "properties", NullValueHandling = NullValueHandling.Ignore )]
		dynamic ISirenEntity.Properties {
			get { return m_properties; }
		}

		[JsonProperty( "entities", NullValueHandling = NullValueHandling.Ignore )]
		[JsonConverter( typeof( HypermediaEntityConverter ) )]
		IEnumerable<ISirenEntity> ISirenEntity.Entities {
			get { return m_entities; }
		}

		[JsonProperty( "links", NullValueHandling = NullValueHandling.Ignore )]
		[JsonConverter( typeof( HypermediaLinkConverter ) )]
		IEnumerable<ISirenLink> ISirenEntity.Links {
			get { return m_links; }
		}

		[JsonProperty( "actions", NullValueHandling = NullValueHandling.Ignore )]
		[JsonConverter( typeof( HypermediaActionConverter ) )]
		IEnumerable<ISirenAction> ISirenEntity.Actions {
			get { return m_actions; }
		}

		[JsonProperty( "title", NullValueHandling = NullValueHandling.Ignore )]
		string ISirenEntity.Title {
			get { return m_title; }
		}

		[JsonProperty( "rel", NullValueHandling = NullValueHandling.Ignore )]
		string[] ISirenEntity.Rel {
			get { return m_rel; }
		}

		[JsonProperty( "href", NullValueHandling = NullValueHandling.Ignore )]
		Uri ISirenEntity.Href {
			get { return m_href; }
		}

		[JsonProperty( "type", NullValueHandling = NullValueHandling.Ignore )]
		string ISirenEntity.Type {
			get { return m_type; }
		}

	}

	public class HypermediaEntityConverter : JsonConverter {


		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			serializer.Serialize( writer, value );
		}

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			return serializer.Deserialize<SirenEntity[]>( reader );
		}

		public override bool CanConvert( Type objectType ) {
			return objectType == typeof( SirenEntity );
		}

	}

}