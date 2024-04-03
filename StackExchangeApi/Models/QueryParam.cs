namespace StackExchangeApi.Models
{
	public class QueryParam
	{
		public QueryParam(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; set; }

		public string Value { get; set; }
	}
}
