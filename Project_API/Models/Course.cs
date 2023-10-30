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

    public virtual Category? Category { get; set; }

    public virtual ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
