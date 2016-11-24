namespace D2L.Hypermedia.Siren {

	public interface ISirenField {

		string Name { get; }

		string[] Class { get; }

		string Type { get; }

		object Value { get; }

		string Title { get; }

	}

}