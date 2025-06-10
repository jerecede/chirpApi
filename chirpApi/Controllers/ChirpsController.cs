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
        private readonly ILogger<ChirpsController> _logger;

        public ChirpsController(IChirpsService chirpService, ILogger<ChirpsController> logger)
        {
            _chirpService = chirpService;
            _logger = logger;\
        }

        //// GET: api/Chirps/all
        //[HttpGet("all")]
        //public async Task<ActionResult<IEnumerable<Chirp>>> GetAllChirps()
        //{
        //    return await _context.Chirps.ToListAsync();
        //}

        // GET non può avere il body, quindi query
        // GET: api/Chirps?text=
        [HttpGet]
        public async Task<ActionResult> GetChirpsByFilter([FromQuery] ChirpFilter filter) //le query params, dove metterò tutte le proprietà della classe filter
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

        //// GET: api/Chirps/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Chirp>> GetChirp([FromRoute] int id)
        //{
        //    var chirp = await _context.Chirps.FindAsync(id);

        //    if (chirp == null)
        //    {
        //        return NotFound();
        //    }

        //    return chirp;
        //}

        //// PUT: api/Chirps/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutChirp([FromRoute] int id, [FromBody] Chirp chirp)
        //{
        //    //devo passare per forza ID
        //    if (id != chirp.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(chirp).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ChirpExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Chirps
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Chirp>> PostChirp([FromBody] Chirp chirp)
        //{
        //    _context.Chirps.Add(chirp);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetChirp", new { id = chirp.Id }, chirp);
        //}

        //// DELETE: api/Chirps/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteChirp([FromRoute] int id)
        //{
        //    var chirp = await _context.Chirps.FindAsync(id);
        //    if (chirp == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Chirps.Remove(chirp);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ChirpExists(int id)
        //{
        //    return _context.Chirps.Any(e => e.Id == id);
        //}
    }
}
