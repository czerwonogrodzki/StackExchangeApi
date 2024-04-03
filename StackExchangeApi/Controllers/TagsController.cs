using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackExchangeApi.Data;
using StackExchangeApi.Dtos;
using StackExchangeApi.Models;
using StackExchangeApi.Repositories;
using StackExchangeApi.Services;
using System.Net.Mime;

namespace StackExchangeApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TagsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogger<TagsController> _logger;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;
		private readonly ITagsRepository _tagsRepository;
		private readonly ITagsDataService _tagsDataService;

		private readonly int maxPageSize;

		public TagsController(ApplicationDbContext context, ILogger<TagsController> logger, IMapper mapper, IConfiguration configuration, ITagsRepository tagsRepository, ITagsDataService seedDataService)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
			_configuration = configuration;
			_tagsRepository = tagsRepository;
			_tagsDataService = seedDataService;

			maxPageSize = configuration.GetValue<int>("MaxPageSize");
		}

		[HttpGet(Name = nameof(GetAllTags))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult GetAllTags([FromQuery] PaginationParams paginationParams)
		{
			int pageCount = _context.Tags.Count() / maxPageSize + 1;

			if (paginationParams.Page > pageCount)
			{
				return BadRequest();
			}

			var tags = _tagsRepository.Get(paginationParams);

			return Ok(new { paginationParams.Page, PageCount = pageCount, Tags = tags });
		}

		[HttpGet]
		[Route("{id}", Name = nameof(GetTag))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType<TagModel>(StatusCodes.Status200OK)]
		public ActionResult GetTag(int id)
		{
			TagModel tag = _tagsRepository.GetSingle(id);

			if (tag == null)
			{
				return NotFound();
			}

			return Ok(tag);
		}

		[HttpPost]
		[Route("add", Name = nameof(AddTag))]
		[ProducesResponseType<TagModel>(StatusCodes.Status201Created)]
		[Consumes(MediaTypeNames.Application.Json)]
		public ActionResult<TagModel> AddTag([FromBody] TagCreateDto tagDto)
		{
			TagModel tag = _mapper.Map<TagModel>(tagDto);

			_tagsRepository.Add(tag);

			return CreatedAtAction(nameof(AddTag), tag);
		}

		[HttpPost]
		[Route("update-tags", Name = nameof(UpdateTags))]
		public async Task<ActionResult> UpdateTags()
		{
			_tagsRepository.RemoveAll();

			await _tagsDataService.PopulateDatabaseAsync();

			return Ok();
		}
	}
}
