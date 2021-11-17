using AutoMapper;
using CommandService.DTOs;
using CommandService.Model;

namespace CommandService.Profiles
{
    public class CommandProfile: Profile
    {
        public CommandProfile()
        {
            //Source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

        }
    }
}
