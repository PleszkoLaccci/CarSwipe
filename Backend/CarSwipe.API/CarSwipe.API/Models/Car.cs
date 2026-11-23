using System;
using System.Collections.Generic;

namespace CarSwipe.API.Models;

public partial class Car
{
    public int CarId { get; set; }

    public int UserId { get; set; }

    public string Marka { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int Evjarat { get; set; }

    public int Futasteljesitmeny { get; set; }

    public string Uzemanyag { get; set; } = null!;

    public string Valto { get; set; } = null!;

    public string Karosszeria { get; set; } = null!;

    public int Teljesitmeny { get; set; }

    public int Ar { get; set; }

    public string? Leiras { get; set; }

    public DateTime? Letrehozva { get; set; }

    public virtual ICollection<CarImage> CarImages { get; set; } = new List<CarImage>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Swipe> Swipes { get; set; } = new List<Swipe>();

    public virtual User User { get; set; } = null!;
}
