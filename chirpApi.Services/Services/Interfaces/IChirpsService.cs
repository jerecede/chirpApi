using chirpApi.Models;
using chirpApi.Services.Models.DTOs;
using chirpApi.Services.Models.Filters;
using chirpApi.Services.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Services.Interfaces
{
    public interface IChirpsService
    {
        Task<IEnumerable<ChirpViewModel>> GetAllChirps();
        Task<IEnumerable<ChirpViewModel>> GetChirpsByFilter(ChirpFilter filter);
        Task<ChirpViewModel?> GetChirpById(int id);
        Task<bool> UpdateChirp(int id, ChirpUpdateDTO chirp); //per gestire piu errori invece di bool fare validate
        Task<int?> CreateChirp(ChirpCreateDTO chirp); //int cosi restituisce id del nuovo chirp creato, se non riesce a creare ritorna null
        Task<int?> DeleteChirp(int id);
    }
}
