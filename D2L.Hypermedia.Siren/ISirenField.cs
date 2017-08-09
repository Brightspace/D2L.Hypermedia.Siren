using System;

namespace D2L.Hypermedia.Siren {

	public interface ISirenField : IEquatable<ISirenField>, IComparable<ISirenField>, IComparable {

		string Name { get; }

		string[] Class { get; }

		string Type { get; }

		object Value { get; }

		string Title { get; }

		bool Matches(
			out string message,
			string name = null,
			string[] @class = null,
			string type = null,
			object value = null,
			string title = null
		);

	}

}