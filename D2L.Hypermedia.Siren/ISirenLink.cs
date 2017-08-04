using System;

namespace D2L.Hypermedia.Siren {

	public interface ISirenLink : IEquatable<ISirenLink>, IComparable<ISirenLink> {

		string[] Rel { get; }

		string[] Class { get; }

		Uri Href { get; }

		string Title { get; }

		string Type { get; }

	}

}