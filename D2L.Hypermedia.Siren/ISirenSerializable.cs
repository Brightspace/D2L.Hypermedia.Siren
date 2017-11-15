using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public interface ISirenSerializable {

		string ToJson();

		void ToJson( JsonWriter writer );

	}

}
