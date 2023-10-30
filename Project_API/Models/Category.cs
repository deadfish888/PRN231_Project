using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;

namespace Project_API.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }
    [Expand]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
