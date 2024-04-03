using System.Text.Json.Serialization;

namespace StackExchangeApi.Models
{
	public class StackExchangeResponseModel
	{
		[JsonPropertyName("items")]
		public IEnumerable<Tag> Tags { get; set; }
	}

	public class Tag
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("count")]
		public int Count { get; set; }

	}
}
