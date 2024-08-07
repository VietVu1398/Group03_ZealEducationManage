using System;
using System.Collections.Generic;

namespace ZealEducationManager.Entities;

public partial class Exam
{
    public int ExamId { get; set; }

    public DateOnly ExamDate { get; set; }

    public int CourseId { get; set; }

    public int BatchId { get; set; }

    public virtual Batch Batch { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
}
