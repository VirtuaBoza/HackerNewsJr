using AutoMapper;
using HackerNewsJr.App.Models;
using HackerNewsJr.Services.Models;
using System;

namespace HackerNewsJr.Services.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, Story>()
                .ForMember(s => s.Time, ex => ex.MapFrom(i => i.Time.HasValue
                    ? (DateTime?)new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        .AddSeconds((double)i.Time)
                    : null));
        }
    }
}
