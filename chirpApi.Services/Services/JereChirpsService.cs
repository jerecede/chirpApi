using chirpApi.Models;
using chirpApi.Services.Services.Interfaces;
using chirpApi.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Services
{
    public class JereChirpsService : IChirpsService
    {
        private readonly CinguettioContext _context;

        public JereChirpsService(CinguettioContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChirpViewModel>> GetAllChirps()
        {
            var result = await _context.Chirps.ToListAsync();

            return result.Select(c => new ChirpViewModel
            {
                Id = c.Id,
                Text = c.Text,
                ExtUrl = c.ExtUrl,
                CreationTime = c.CreationTime,
                Lat = c.Lat,
                Lng = c.Lng
            });
        }

        public async Task<IEnumerable<ChirpViewModel>> GetChirpsByFilter(ChirpFilter filter)
        {
            IQueryable<Chirp> query = _context.Chirps.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Text))
            {
                query = query.Where(c => c.Text == filter.Text);
            }

            var result = await query.Select(c => new ChirpViewModel
            {
                Id = c.Id,
                Text = c.Text,
                ExtUrl = c.ExtUrl,
                CreationTime = c.CreationTime,
                Lat = c.Lat,
                Lng = c.Lng
            }).ToListAsync();

            return result;
        }

        public async Task<ChirpViewModel> GetChirpById(int id)
        {
            var chirp = await _context.Chirps.FindAsync(id);

            var result = chirp != null ? new ChirpViewModel
            {
                Id = chirp.Id,
                Text = chirp.Text,
                ExtUrl = chirp.ExtUrl,
                CreationTime = chirp.CreationTime,
                Lat = chirp.Lat,
                Lng = chirp.Lng
            } : null;

            return result;
        }

        public async Task PutChirp(Chirp chirp)
        {
            _context.Entry(chirp).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task PostChirp(Chirp chirp)
        {
            _context.Chirps.Add(chirp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChirp(int id)
        {
            var chirp = await _context.Chirps.FindAsync(id);
            if (chirp != null)
            {
                _context.Chirps.Remove(chirp);
                await _context.SaveChangesAsync();
            }
        }

        public bool ChirpExists(int id) => _context.Chirps.Any(e => e.Id == id);
    }
}
