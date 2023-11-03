using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Models;

public partial class Assignment
{
    [Key]
    public int AssignmentId { get; set; }

    public int? ItemId { get; set; }

    public DateTime? DueDate { get; set; }
    [Expand]
    public virtual Item? Item { get; set; }
    [Expand]
    public virtual ICollection<StudentSubmission> StudentSubmissions { get; set; } = new List<StudentSubmission>();
}
