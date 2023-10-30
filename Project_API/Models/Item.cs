using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;

namespace Project_API.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public int? SectionId { get; set; }

    public string? ItemName { get; set; }

    public string Type { get; set; } = null!;

    [Expand]
    public virtual Assignment? Assignment { get; set; }

    [Expand]
    public virtual Resource? Resource { get; set; }
    [Expand]
    public virtual Section? Section { get; set; }
}
