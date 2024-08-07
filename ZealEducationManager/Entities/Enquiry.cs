using System;
using System.Collections.Generic;

namespace ZealEducationManager.Entities;

public partial class Enquiry
{
    public int EnquiryId { get; set; }

    public int CandidateId { get; set; }

    public int CourseId { get; set; }

    public DateOnly EnquiryDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}
