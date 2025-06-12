using chirpApi.Models;
using chirpApi.Services.Models.DTOs;
using chirpApi.Services.Models.Filters;
using chirpApi.Services.Models.ViewModels;
using chirpApi.Services.Services.Interfaces;
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
            if (!string.IsNullOrWhiteSpace(filter.ExtUrl))
            {
                query = query.Where(c => c.ExtUrl.Contains(filter.ExtUrl));
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
            if(chirp == null) return null;

            return new ChirpViewModel
            {
                Id = chirp.Id,
                Text = chirp.Text,
                ExtUrl = chirp.ExtUrl,
                CreationTime = chirp.CreationTime,
                Lat = chirp.Lat,
                Lng = chirp.Lng
            };
        }

        public async Task<bool> UpdateChirp(int id, ChirpUpdateDTO chirp)
        {
            var entity = await _context.Chirps.FindAsync(id);
            if (entity == null) return false;

            //se una proprietà è null non aggiorno, senno aggiorno
            if(!string.IsNullOrWhiteSpace(chirp.ExtUrl)) entity.ExtUrl = chirp.ExtUrl; // se il chirpcreatedto ha un valore per ExtUrl, lo aggiorno, se è null non modifico niente su entity che è l'istanza originale presa dal db
            if (!string.IsNullOrWhiteSpace(chirp.Text)) entity.Text = chirp.Text;
            if (chirp.Lng != null) entity.Lng = chirp.Lng;
            if (chirp.Lat != null) entity.Lat = chirp.Lat;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();// devo gestire se c'è errore nell'update

            return true;
        }

        public async Task<int?> CreateChirp(ChirpCreateDTO chirp)
        {
            if(!string.IsNullOrWhiteSpace(chirp.Text) && chirp.Text.Length > 140) return null;

            var entity = new Chirp
            {
                Text = chirp.Text,
                ExtUrl = chirp.ExtUrl,
                Lat = chirp.Lat,
                Lng = chirp.Lng
            };

            _context.Chirps.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;// ritorno l'id del chirp appena creato, automatico
        }

        public async Task<int?> DeleteChirp(int id)
        {
            //facciamo finta che non abbiamo il cascade, quindi se chirp eliminato che succede con i commenti?
            var entity = await _context.Chirps.Include(x => x.Comments) //join, prendiamo tutti i commenti
                                                .Where(x => x.Id == id) //filtriamo per id
                                                .SingleOrDefaultAsync();

            if (entity == null) return null; //non ha trovato chirp
            if(entity.Comments != null || entity.Comments.Count > 0) return -1; //ha trovato chirp ma ci sono commenti associati

            _context.Chirps.Remove(entity);
            await _context.SaveChangesAsync();

            return id;
        }
    }
}
