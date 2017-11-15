using Newtonsoft.Json;

namespace D2L.Hypermedia.Siren {

	public interface ISirenSerializable {

		void ToJson( JsonWriter writer );

	}

}
