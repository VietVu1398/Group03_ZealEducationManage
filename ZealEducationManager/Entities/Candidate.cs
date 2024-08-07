using System;
using System.Collections.Generic;

namespace ZealEducationManager.Entities;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public DateOnly DateOfJoining { get; set; }

    public string? ContactInfo { get; set; }

    public double? OutstandingFee { get; set; }

    public int Status { get; set; }

    public int? BatchId { get; set; }

    public virtual Batch? Batch { get; set; }


    public virtual ICollection<Enquiry> Enquiries { get; set; } = new List<Enquiry>();

    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
