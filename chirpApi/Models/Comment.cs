using System;
using System.Collections.Generic;

namespace chirpApi.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int ChirpId { get; set; }

    public int? ParentId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public virtual Chirp Chirp { get; set; } = null!;

    public virtual ICollection<Comment> InverseParent { get; set; } = new List<Comment>();

    public virtual Comment? Parent { get; set; }
}
