using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chirpApi.Models;
using chirpApi.Services.Services.Interfaces;

namespace chirpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChirpsController : ControllerBase
    {
        private readonly IChirpsService _chirpService;
        //private readonly ILogger<ChirpsController> _logger;

        public ChirpsController(IChirpsService chirpService)// ILogger<ChirpsController> logger
        {
            _chirpService = chirpService;
            //_logger = logger;
        }

        // GET: api/Chirps/all
        [HttpGet("all")]
        public async Task<ActionResult> GetAllChirps()
        {
            var result = await _chirpService.GetAllChirps();
            return Ok(result);
        }

        // GET non può avere il body, quindi query
        // GET: api/Chirps?text=
        [HttpGet]
        public async Task<ActionResult> GetChirpsByFilter([FromQuery] ChirpFilter filter) //le query params, dove metterò tutte le proprietà della classe filter
        {
            //_logger.LogInformation("Received request to get chirps with filter: {@Filter}", filter);

            var result = await _chirpService.GetChirpsByFilter(filter);

            if (result == null || !result.Any())
            {
                //_logger.LogInformation("No chirps found for the given filter: {@Filter}", filter);
                return NoContent();
            }
            else
            {
                //_logger.LogInformation("Found {Count} chirps for the given filter: {@Filter}", result.Count(), filter);
                return Ok(result);
            } 
        }

        // GET: api/Chirps/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetChirpById([FromRoute] int id)
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
        public async Task<ActionResult> PutChirp([FromRoute] int id, [FromBody] Chirp chirp)
        {
            //devo passare per forza ID
            if (id == chirp.Id)
            {
                try
                {
                    await _chirpService.PutChirp(chirp);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_chirpService.ChirpExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent(); //è andata a buon fine, non restituisce niente
            }

            return BadRequest();
        }

        // POST: api/Chirps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostChirp([FromBody] Chirp chirp)
        {
            try
            {
                await _chirpService.PostChirp(chirp);
                return CreatedAtAction("GetChirp", new { id = chirp.Id }, chirp);
            }
            catch (DbUpdateException)
            {
                return Conflict("Chirp already exists or there was an error saving the chirp.");
            }
        }

        // DELETE: api/Chirps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChirp([FromRoute] int id)
        {
            try
            {
                await _chirpService.DeleteChirp(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            //var chirp = await _context.Chirps.FindAsync(id);
            //if (chirp == null)
            //{
            //    return NotFound();
            //}

            //_context.Chirps.Remove(chirp);
            //await _context.SaveChangesAsync();

            //return NoContent();
        }
    }
}
