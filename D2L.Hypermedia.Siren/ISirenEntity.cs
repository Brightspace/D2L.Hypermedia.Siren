using System;
using System.Collections.Generic;

namespace D2L.Hypermedia.Siren {

	public interface ISirenEntity : IEquatable<ISirenEntity>, IComparable<ISirenEntity>, IComparable {

		string[] Class { get; }

		dynamic Properties { get; }

		IEnumerable<ISirenEntity> Entities { get; }

		IEnumerable<ISirenLink> Links { get; }

		IEnumerable<ISirenAction> Actions { get; }

		string Title { get; }

		string[] Rel { get; }

		Uri Href { get; }

		string Type { get; }

		bool Matches(
			out string message,
			string[] @class = null,
			dynamic properties = null,
			IEnumerable<ISirenEntity> entities = null,
			IEnumerable<ISirenLink> links = null,
			IEnumerable<ISirenAction> actions = null,
			string title = null,
			string[] rel = null,
			Uri href = null,
			string type = null
		);

	}

}
