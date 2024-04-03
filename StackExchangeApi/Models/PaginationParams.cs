using StackExchangeApi.Enums;

namespace StackExchangeApi.Models
{
	public class PaginationParams
	{
		//private int _pageCount = 1500 / 50;

		//[BindNever]
		//public int PageCount
		//{
		//	get { return _pageCount; }
		//	set { _pageCount = (value > 50) ? 50 : value; }
		//}

		public int Page { get; set; } = 1;

		public OrderByEnum? OrderBy { get; set; } = OrderByEnum.Id;

		public DirectionEnum? Direction { get; set; } = DirectionEnum.Asc;

	}
}
