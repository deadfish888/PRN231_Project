using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Models;

public partial class StudentSubmission
{
    [Key]
    public int UserId { get; set; }
    [Key]
    public int AssignmentId { get; set; }

    public bool? Status { get; set; }

    public DateTime? LastModified { get; set; }

    public string? FileName { get; set; }

    public byte[]? FileData { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual Student User { get; set; } = null!;
}
