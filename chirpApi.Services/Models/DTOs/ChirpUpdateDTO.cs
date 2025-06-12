using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Models.DTOs
{
    public class ChirpUpdateDTO
    {
        //tutto nullable per l'update (se non viene passato un valore non lo aggiorno)
        public string? Text { get; set; }

        public string? ExtUrl { get; set; }

        public double? Lat { get; set; }

        public double? Lng { get; set; }
    }
}
