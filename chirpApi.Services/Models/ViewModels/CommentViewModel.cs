using chirpApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public int ChirpId { get; set; }

        public int? ParentId { get; set; }

        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

        public Chirp Chirp { get; set; }
    }
}
