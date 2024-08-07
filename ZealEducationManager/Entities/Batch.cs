using System;
using System.Collections.Generic;

namespace ZealEducationManager.Entities;

public partial class Batch
{
    public int BatchId { get; set; }

    public string BatchCode { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<FacultyBatch> FacultyBatches { get; set; } = new List<FacultyBatch>();
}
