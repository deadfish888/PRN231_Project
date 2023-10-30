using System;
using System.Collections.Generic;

namespace PRN231_CMS.Models;

public partial class Student
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Img { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();

    public virtual ICollection<StudentSubmission> StudentSubmissions { get; set; } = new List<StudentSubmission>();
}
