using System.ComponentModel.DataAnnotations;
using ZealEducationManager.Entities;

namespace ZealEducationManager.Models.PaymentViewModels
{
    public class AddPaymentModel
    {
        public int PaymentId { get; set; }

        public int CandidateId { get; set; }
        [Range(0, 1000000, ErrorMessage ="Amount must be less than 1000000")]
        public decimal Amount { get; set; }

        public DateOnly PaymentDate { get; set; }

        public virtual Candidate? Candidate { get; set; } = null!;
    }
}
