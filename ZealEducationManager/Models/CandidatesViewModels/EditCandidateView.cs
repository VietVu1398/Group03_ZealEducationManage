using Microsoft.AspNetCore.Mvc.Rendering;
using ZealEducationManager.Entities;
namespace ZealEducationManager.Models.CandidatesViewModels
{
    public class EditCandidateView
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
		public IEnumerable<SelectListItem>? BatchCode { get; set; }
	}
}

