using System;
using System.Collections.Generic;

namespace PRN231_CMS.Models;

public partial class Resource
{
    public int ResourceId { get; set; }

    public int? ItemId { get; set; }

    public byte[]? Data { get; set; }

    public DateTime? UploadedDate { get; set; }

    public virtual Item? Item { get; set; }
}
