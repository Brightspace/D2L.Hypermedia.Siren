using System;

namespace D2L.Hypermedia.Siren {

	public interface ISirenLink : IEquatable<ISirenLink>, IComparable<ISirenLink>, IComparable {

		string[] Rel { get; }

		string[] Class { get; }

		Uri Href { get; }

		string Title { get; }

		string Type { get; }

		bool Matches(
			out string message,
			string[] rel = null,
			string[] @class = null,
			Uri href = null,
			string title = null,
			string type = null
		);

	}

}