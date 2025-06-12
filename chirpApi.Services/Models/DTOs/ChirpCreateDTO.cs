using chirpApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Models.DTOs
{
    public class ChirpCreateDTO
    {
        public string Text { get; set; } = null!;

        public string? ExtUrl { get; set; }

        public double? Lat { get; set; }

        public double? Lng { get; set; }
    }
}
