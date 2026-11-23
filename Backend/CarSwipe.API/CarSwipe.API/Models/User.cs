using System;
using System.Collections.Generic;

namespace CarSwipe.API.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? Letrehozva { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Swipe> Swipes { get; set; } = new List<Swipe>();
}
