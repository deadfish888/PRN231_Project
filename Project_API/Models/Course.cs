using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;

namespace Project_API.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? Creator { get; set; }

    public string? Teacher { get; set; }

    public int? CategoryId { get; set; }
    [AutoExpand]
    public virtual Category? Category { get; set; }
    [Expand]
    public virtual ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();
    [Expand]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
