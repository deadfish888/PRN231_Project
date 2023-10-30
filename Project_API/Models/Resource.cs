using System;
using System.Collections.Generic;

namespace Project_API.Models;

public partial class Resource
{
    public int ResourceId { get; set; }

    public int? ItemId { get; set; }

    public byte[]? Data { get; set; }

    public DateTime? UploadedDate { get; set; }

    public virtual Item? Item { get; set; }
}
