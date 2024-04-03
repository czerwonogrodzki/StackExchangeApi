using StackExchangeApi.Models;

namespace StackExchangeApi.Repositories
{
	public interface ITagsRepository
	{
		TagModel GetSingle(int id);
		IEnumerable<TagModel> Get(PaginationParams paginationParams);
		void Add(TagModel tag);
		void RemoveAll();
		void AddRange(List<TagModel> tags);
	}
}
