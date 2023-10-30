using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Models;

public partial class Student
{
    [Key]
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Img { get; set; }

    public string? Description { get; set; }
    [Expand]
    public virtual ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();
    [Expand]
    public virtual ICollection<StudentSubmission> StudentSubmissions { get; set; } = new List<StudentSubmission>();
}
