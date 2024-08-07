using System;
using System.Collections.Generic;

namespace ZealEducationManager.Entities;

public partial class FacultyBatch
{
    public int FacultyBatchId { get; set; }

    public int FacultyId { get; set; }

    public int BatchId { get; set; }

    public virtual Batch Batch { get; set; } = null!;

    public virtual Faculty Faculty { get; set; } = null!;
}
