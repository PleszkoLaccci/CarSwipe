using System;
using System.Collections.Generic;

namespace CarSwipe.API.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int CarId { get; set; }

    public int BuyerId { get; set; }

    public bool FromBuyer { get; set; }

    public string Body { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime Letrehozva { get; set; }

    public virtual User Buyer { get; set; } = null!;

    public virtual Car Car { get; set; } = null!;
}
