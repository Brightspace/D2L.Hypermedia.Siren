using System;

namespace D2L.Hypermedia.Siren {

	public interface ISirenField : ISirenSerializable, IEquatable<ISirenField>, IComparable<ISirenField>, IComparable {

		string Name { get; }

		string[] Class { get; }

		string Type { get; }

		object Value { get; }

		string Title { get; }

		decimal? Min { get; }

		decimal? Max { get; }

	}

}