using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public class SirenEntity : ISirenEntity {

		private readonly string m_type;
		private readonly Uri m_href;
		private readonly string[] m_rel;
		private readonly string m_title;
		private readonly IEnumerable<ISirenAction> m_actions;
		private readonly IEnumerable<ISirenLink> m_links;
		private readonly IEnumerable<ISirenEntity> m_entities;
		private readonly dynamic m_properties;
		private readonly string[] m_class;

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
			m_rel = rel ?? new string[0];
			m_class = @class ?? new string[0];
			m_properties = properties;
			m_entities = entities ?? new List<ISirenEntity>();
			m_links = links ?? new List<ISirenLink>();
			m_actions = actions ?? new List<ISirenAction>();
			m_title = title;
			m_href = href;
			m_type = type;
		}

		[JsonProperty( "class", NullValueHandling = NullValueHandling.Ignore )]
		public string[] Class {
			get { return m_class; }
		}

		[JsonProperty( "properties", NullValueHandling = NullValueHandling.Ignore )]
		public dynamic Properties {
			get { return m_properties; }
		}

		[JsonProperty( "entities", NullValueHandling = NullValueHandling.Ignore )]
		[JsonConverter( typeof(HypermediaEntityConverter) )]
		public IEnumerable<ISirenEntity> Entities {
			get { return m_entities; }
		}

		[JsonProperty( "links", NullValueHandling = NullValueHandling.Ignore )]
		[JsonConverter( typeof(HypermediaLinkConverter) )]
		public IEnumerable<ISirenLink> Links {
			get { return m_links; }
		}

		[JsonProperty( "actions", NullValueHandling = NullValueHandling.Ignore )]
		[JsonConverter( typeof(HypermediaActionConverter) )]
		public IEnumerable<ISirenAction> Actions {
			get { return m_actions; }
		}

		[JsonProperty( "title", NullValueHandling = NullValueHandling.Ignore )]
		public string Title {
			get { return m_title; }
		}

		[JsonProperty( "rel", NullValueHandling = NullValueHandling.Ignore )]
		public string[] Rel {
			get { return m_rel; }
		}

		[JsonProperty( "href", NullValueHandling = NullValueHandling.Ignore )]
		public Uri Href {
			get { return m_href; }
		}

		[JsonProperty( "type", NullValueHandling = NullValueHandling.Ignore )]
		public string Type {
			get { return m_type; }
		}

		public bool ShouldSerializeRel() {
			return Rel.Length > 0;
		}

		public bool ShouldSerializeClass() {
			return Class.Length > 0;
		}

		public bool ShouldSerializeEntities() {
			return Entities.Any();
		}

		public bool ShouldSerializeLinks() {
			return Links.Any();
		}

		public bool ShouldSerializeActions() {
			return Actions.Any();
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