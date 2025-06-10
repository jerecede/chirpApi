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
        Task<IEnumerable<ChirpViewModel>> GetChirpsByFilter(ChirpFilter filter);
    }
}
