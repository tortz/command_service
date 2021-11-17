using System;
using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Model;
using Microsoft.Extensions.DependencyInjection;

namespace CommandService.EventProcessing
{
    public class EventProcessor: IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            EventType type = GetEventType(message);
            switch (type) {
                case EventType.PlatformPublished:
                    var platform = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
                    addPlatform(platform);
                    break;
                default:
                    Console.WriteLine("--> The Event type is invalid or not supported");
                    break;

            }
        }

        private EventType GetEventType(string message)
        {
            var eventObj = JsonSerializer.Deserialize<GenericEventDto>(message);
            switch (eventObj.Event)
            {
                case "Platform_Published":
                    return EventType.PlatformPublished;
                default:
                    return EventType.Undetermined;
            }
        }

        private void addPlatform(PlatformPublishedDto platformDto)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<ICommandRepo>();
                try {
                    var platform = _mapper.Map<Platform>(platformDto);
                    if (!repository.ExternalPlatformExists(platform.ExternalId))
                    {
                        repository.CreatePlatform(platform);
                        repository.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}