using System;
using System.Collections.Generic;

namespace PRN231_CMS.Models;

public partial class StudentSubmission
{
    public int UserId { get; set; }

    public int AssignmentId { get; set; }

    public bool? Status { get; set; }

    public DateTime? LastModified { get; set; }

    public string? FileName { get; set; }

    public byte[]? FileData { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual Student User { get; set; } = null!;
}
