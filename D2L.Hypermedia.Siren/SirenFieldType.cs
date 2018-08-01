namespace D2L.Hypermedia.Siren {

	public class SirenFieldType {

		internal static string[] ValidTypes = {
			"hidden",
			"text",
			"search",
			"tel",
			"url",
			"email",
			"password",
			"datetime",
			"date",
			"month",
			"week",
			"time",
			"datetime-local",
			"number",
			"range",
			"color",
			"checkbox",
			"radio",
			"file"
		};

		public static string Hidden => "hidden";
		public static string Text => "text";
		public static string Search => "search";
		public static string Tel => "tel";
		public static string Url => "url";
		public static string Email => "email";
		public static string Password => "password";
		public static string Datetime => "datetime";
		public static string Date => "date";
		public static string Month => "month";
		public static string Week => "week";
		public static string Time => "time";
		public static string DatetimeLocal => "datetime-local";
		public static string Number => "number";
		public static string Range => "range";
		public static string Color => "color";
		public static string Checkbox => "checkbox";
		public static string Radio => "radio";
		public static string File => "file";

	}

}
