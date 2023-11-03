using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_API.Models;

public partial class Resource
{
    [Key]
    public int ResourceId { get; set; }

    public int? ItemId { get; set; }

    public byte[]? Data { get; set; }

    public DateTime? UploadedDate { get; set; }
    [Expand]
    public virtual Item? Item { get; set; }
}
