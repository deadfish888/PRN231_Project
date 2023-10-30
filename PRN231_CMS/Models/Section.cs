using System;
using System.Collections.Generic;

namespace PRN231_CMS.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public string? SectionName { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
