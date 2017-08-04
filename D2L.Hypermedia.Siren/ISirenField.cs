using System;

namespace D2L.Hypermedia.Siren {

	public interface ISirenField : IEquatable<ISirenField>, IComparable<ISirenField> {

		string Name { get; }

		string[] Class { get; }

		string Type { get; }

		object Value { get; }

		string Title { get; }

	}

}