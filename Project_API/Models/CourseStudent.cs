using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_API.Models;

public partial class CourseStudent
{
    [Key]
    public int UserId { get; set; }
    [Key]
    public int CourseId { get; set; }
    public DateTime? LastAccess { get; set; }
    [Expand]
    public virtual Course Course { get; set; } = null!;
    [Expand]
    public virtual Student User { get; set; } = null!;
}
