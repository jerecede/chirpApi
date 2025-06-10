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
    }
}
