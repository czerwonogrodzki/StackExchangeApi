using StackExchangeApi.Enums;

namespace StackExchangeApi.Models
{
	public class PaginationParams
	{
		public int Page { get; set; } = 1;

		public OrderByEnum? OrderBy { get; set; } = OrderByEnum.Id;

		public DirectionEnum? Direction { get; set; } = DirectionEnum.Asc;

	}
}
