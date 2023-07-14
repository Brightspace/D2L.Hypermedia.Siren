using System;
using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public record struct SirenFieldValueObject {

		public SirenFieldValueObject( string value, string title = null, bool selected = false ) {
			Value = value ?? throw new ArgumentNullException( nameof( value ) );
			Title = title;
			Selected = selected;
		}

		public SirenFieldValueObject( int value, string title = null, bool selected = false ) {
			Value = value;
			Title = title;
			Selected = selected;
		}

		public SirenFieldValueObject( double value, string title = null, bool selected = false ) {
			Value = value;
			Title = title;
			Selected = selected;
		}

		[JsonProperty( "title", NullValueHandling = NullValueHandling.Ignore )]
		public string Title;

		[JsonProperty( "value" )]
		public object Value; // Can only be string or number, according to the spec

		[JsonProperty( "selected" )]
		public bool Selected = false;
	}

}
