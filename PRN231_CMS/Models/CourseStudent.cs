using System;
using System.Collections.Generic;

namespace PRN231_CMS.Models;

public partial class CourseStudent
{
    public int UserId { get; set; }

    public int CourseId { get; set; }

    public DateTime? LastAccess { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student User { get; set; } = null!;
}
