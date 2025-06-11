using chirpApi.Models;
using chirpApi.Services.ViewModels;
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
        Task<ChirpViewModel> GetChirpById(int id);
        Task PutChirp(Chirp chirp);
        Task PostChirp(Chirp chirp);
        Task DeleteChirp(int id);
        bool ChirpExists(int id);
    }
}
