
using CommandService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        Task<bool> SaveChanges();

        Task<IEnumerable<Platform>> GetAllPlatforms();
        Task<bool> PlaformExists(int platformId);

        bool ExternalPlatformExists(int externalPlatformId);
        void CreatePlatform(Platform platform);


        Task<IEnumerable<Command>> GetCommandsFormPlatformId(int platformId);
        Task<Command> GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command Command);
    }
}
