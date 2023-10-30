using System;
using System.Collections.Generic;

namespace PRN231_CMS.Models;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public int? ItemId { get; set; }

    public DateTime? DueDate { get; set; }

    public virtual Item? Item { get; set; }

    public virtual ICollection<StudentSubmission> StudentSubmissions { get; set; } = new List<StudentSubmission>();
}
