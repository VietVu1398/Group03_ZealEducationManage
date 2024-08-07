using System;
using System.Collections.Generic;

namespace ZealEducationManager.Entities;

public partial class ExamResult
{
    public int ResultId { get; set; }

    public int ExamId { get; set; }

    public int CandidateId { get; set; }

    public decimal? MarksObtained { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual Exam Exam { get; set; } = null!;
}
