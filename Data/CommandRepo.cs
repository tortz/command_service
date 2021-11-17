using Microsoft.EntityFrameworkCore;
using CommandService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            _context.Add(platform);
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            return await _context.Platforms.ToListAsync();
        }

        public async Task<bool> PlaformExists(int platformId)
		{
            return await _context.Platforms.AnyAsync(p => p.Id == platformId);
		}

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _context.Platforms.Any(p => p.ExternalId == externalPlatformId);
        }
        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Add(command);
        }

        public async Task<IEnumerable<Command>> GetCommandsFormPlatformId(int platformId)
        { 
            return await _context.Commands.Where(c => c.PlatformId == platformId).ToListAsync();
        }

        public async Task<Command> GetCommand(int platformId, int commandId)
        {
            return await _context.Commands
                .FirstOrDefaultAsync(c => c.PlatformId == platformId && c.Id == commandId);
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
