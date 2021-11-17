using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommandService.Data;
using CommandService.Model;
using AutoMapper;
using CommandService.DTOs;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandRepo _repository;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        // GET: api/Commands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommandsForPlatform(int platformId)
        {            
            if (! await _repository.PlaformExists(platformId))
            {
                return NotFound();
            }
            var commands = await _repository.GetCommandsFormPlatformId(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        // GET: api/Commands/5
        [HttpGet("{commandId}")]
        public async Task<ActionResult<CommandReadDto>> GetCommand(int platformId, int commandId)
        {         
            if (! await _repository.PlaformExists(platformId))
            {
                return NotFound();
            }
            var command = await _repository.GetCommand(platformId, commandId);

            if (command == null)
            {
                return NotFound();
            }

            return _mapper.Map<CommandReadDto>(command);
        }

                // POST: api/Commands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommandReadDto>> CreateCommand(int platformId, CommandCreateDto commandDto)
        {
            if (!await _repository.PlaformExists(platformId))
            {
                return NotFound();
            }
            var command = _mapper.Map<Command>(commandDto);
            _repository.CreateCommand(platformId, command);
            await _repository.SaveChanges();

            return CreatedAtAction("GetCommand", 
                new { platformId = platformId, commandId = command.Id },
                _mapper.Map<CommandReadDto>(command));
        }     
    }
}
