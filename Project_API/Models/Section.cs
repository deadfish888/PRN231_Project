using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;

namespace Project_API.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public string? SectionName { get; set; }

    public int? CourseId { get; set; }
    [Expand]
    public virtual Course? Course { get; set; }
    [Expand]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
