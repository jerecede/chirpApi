using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chirpApi.Models;
using chirpApi.Services.Services.Interfaces;
using chirpApi.Services.Models.Filters;
using chirpApi.Services.Models.DTOs;

namespace chirpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChirpsController : ControllerBase
    {
        private readonly IChirpsService _chirpService;
        private readonly ILogger<ChirpsController> _logger;

        public ChirpsController(IChirpsService chirpService, ILogger<ChirpsController> logger)
        {
            _chirpService = chirpService;
            _logger = logger;
        }

        // GET: api/Chirps/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllChirps()
        {
            var result = await _chirpService.GetAllChirps();
            return Ok(result);
        }

        // GET non può avere il body, quindi query
        // GET: api/Chirps?text=
        [HttpGet]
        public async Task<IActionResult> GetChirpsByFilter([FromQuery] ChirpFilter filter) //le query params, dove metterò tutte le proprietà della classe filter
        {
            _logger.LogInformation("Received request to get chirps with filter: {@Filter}", filter);

            var result = await _chirpService.GetChirpsByFilter(filter);

            if (result == null || !result.Any())
            {
                _logger.LogInformation("No chirps found for the given filter: {@Filter}", filter);
                return NoContent();
            }
            else
            {
                _logger.LogInformation("Found {Count} chirps for the given filter: {@Filter}", result.Count(), filter);
                return Ok(result);
            }
        }

        // GET: api/Chirps/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChirpById([FromRoute] int id)
        {
            var chirp = await _chirpService.GetChirpById(id);

            if (chirp == null)
            {
                return NotFound();
            }

            return Ok(chirp);
        }

        // PUT: api/Chirps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChirp([FromRoute] int id, [FromBody] ChirpUpdateDTO chirp)
        {
            var result = await _chirpService.UpdateChirp(id, chirp);
            return result ? NoContent() : NotFound(); //oppure badrequest("non esiste")
        }

        // POST: api/Chirps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostChirp([FromBody] ChirpCreateDTO chirp)
        {
            var result = await _chirpService.CreateChirp(chirp);
            if (result == null) return BadRequest("Chirp could not be created.");
            return CreatedAtAction("GetChirp", new { id = result }, chirp);
        }

        // DELETE: api/Chirps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChirp([FromRoute] int id)
        {
            var result = await _chirpService.DeleteChirp(id);
            if(result == null) return NotFound(); //oppure badrequest("non esiste")
            if(result == -1) return BadRequest("Warning, delete all the comments related to chirp");
            return Ok();
        }
    }
}
