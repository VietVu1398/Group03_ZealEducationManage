using System;
using System.Collections.Generic;

namespace ZealEducationManager.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int CandidateId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly PaymentDate { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;
}
