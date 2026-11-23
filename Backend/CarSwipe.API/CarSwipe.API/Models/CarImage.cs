using System;
using System.Collections.Generic;

namespace CarSwipe.API.Models;

public partial class CarImage
{
    public int ImageId { get; set; }

    public int CarId { get; set; }

    public string FilePath { get; set; } = null!;

    public int SortOrder { get; set; }

    public virtual Car Car { get; set; } = null!;
}
