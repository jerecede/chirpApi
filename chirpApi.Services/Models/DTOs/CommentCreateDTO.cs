using chirpApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Models.DTOs
{
    public class CommentCreateDTO
    {
        public int ChirpId { get; set; }

        public int? ParentId { get; set; }

        public string Text { get; set; } = null!;
    }
}
