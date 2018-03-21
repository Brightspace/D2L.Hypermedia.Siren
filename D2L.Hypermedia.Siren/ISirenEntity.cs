using System;
using System.Collections.Generic;

namespace D2L.Hypermedia.Siren {

	public interface ISirenEntity : ISirenSerializable, IEquatable<ISirenEntity>, IComparable<ISirenEntity>, IComparable {

		string[] Class { get; }

		dynamic Properties { get; }

		IEnumerable<ISirenEntity> Entities { get; }

		IEnumerable<ISirenLink> Links { get; }

		IEnumerable<ISirenAction> Actions { get; }

		string Title { get; }

		string[] Rel { get; }

		Uri Href { get; }

		string Type { get; }

	}

}
