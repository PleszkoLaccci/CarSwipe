using System;
using System.Collections.Generic;

namespace CarSwipe.API.Models;

public partial class Swipe
{
    public int UserId { get; set; }

    public int CarId { get; set; }

    public bool Liked { get; set; }

    public DateTime? Letrehozva { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
