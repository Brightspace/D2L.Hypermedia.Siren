using System;
using System.Collections.Generic;

namespace D2L.Hypermedia.Siren {

	public interface ISirenAction : IEquatable<ISirenAction>, IComparable<ISirenAction>, IComparable {

		string Name { get; }

		string[] Class { get; }

		string Method { get; }

		Uri Href { get; }

		string Title { get; }

		string Type { get; }

		IEnumerable<ISirenField> Fields { get; }

		bool Matches(
			out string message,
			string name = null,
			string[] @class = null,
			string method = null,
			Uri href = null,
			string title = null,
			string type = null,
			IEnumerable<ISirenField> fields = null
		);

	}

}