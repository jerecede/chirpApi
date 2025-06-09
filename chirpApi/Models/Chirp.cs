using System;
using System.Collections.Generic;

namespace chirpApi.Models;

public partial class Chirp
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public string? ExtUrl { get; set; }

    public DateTime CreationTime { get; set; }

    public double? Lat { get; set; }

    public double? Lgn { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
