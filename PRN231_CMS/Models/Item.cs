
using System;
using System.Collections.Generic;

namespace PRN231_CMS.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public int? SectionId { get; set; }

    public string? ItemName { get; set; }

    public string Type { get; set; } = null!;

    public virtual Assignment? Assignment { get; set; }

    public virtual Resource? Resource { get; set; } 

    public virtual Section? Section { get; set; }
}
