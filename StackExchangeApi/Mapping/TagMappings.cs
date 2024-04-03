using AutoMapper;
using StackExchangeApi.Dtos;
using StackExchangeApi.Models;

namespace StackExchangeApi.Mapping
{
	public class TagMappings : Profile
	{
		public TagMappings()
		{
			CreateMap<TagModel, TagCreateDto>().ReverseMap();
			CreateMap<TagModel, Tag>().ReverseMap();
		}
	}
}
