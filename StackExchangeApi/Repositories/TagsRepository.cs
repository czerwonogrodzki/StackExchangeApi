using StackExchangeApi.Data;
using StackExchangeApi.Enums;
using StackExchangeApi.Models;
using System.Linq.Expressions;

namespace StackExchangeApi.Repositories
{
	public class TagsRepository : ITagsRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;

		public TagsRepository(ApplicationDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		public void Add(TagModel tag)
		{
			_context.Tags.Add(tag);

			_context.SaveChanges();

		}

		public void AddRange(List<TagModel> tags)
		{
			_context.Tags.AddRange(tags);

			_context.SaveChanges();
		}

		public IEnumerable<TagModel> Get(PaginationParams paginationParams)
		{
			int pageSize = _configuration.GetValue<int>("MaxPageSize");

			IEnumerable<TagModel> tags;

			Expression<Func<TagModel, object>> orderBy = paginationParams.OrderBy switch
			{
				OrderByEnum.Name => TagModel => TagModel.Name,
				OrderByEnum.Id => TagModel => TagModel.Id,
				OrderByEnum.Count => TagModel => TagModel.Count,
				OrderByEnum.Percentage => TagModel => TagModel.Percentage,
				_ => TagModel => TagModel.Id
			};

			if (paginationParams.Direction == DirectionEnum.Asc)
			{
				tags = _context.Tags
					.OrderBy(orderBy)
					.Skip((paginationParams.Page - 1) * pageSize)
					.Take(pageSize);
			}
			else
			{
				tags = _context.Tags
					.OrderByDescending(orderBy)
					.Skip((paginationParams.Page - 1) * pageSize)
					.Take(pageSize);
			}

			return tags;
		}

		public TagModel GetSingle(int id)
		{
			return _context.Tags.FirstOrDefault(t => t.Id == id);
		}

		public void RemoveAll()
		{
			_context.Tags.RemoveRange(_context.Tags);

			_context.SaveChanges();
		}
	}
}
