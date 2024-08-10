using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ZealEducationManager.Entities;
namespace ZealEducationManager.Models.CandidatesViewModels
{
    public class EditCandidateView
    {
		public int CandidateId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "The First Name value cannot exceed 50 characters. ")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "The Last Name value cannot exceed 50 characters. ")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "DateOfJoining is required")]
        public DateOnly DateOfJoining { get; set; }

        [Required(ErrorMessage = "ContactInfo is required")]
        [StringLength(100, ErrorMessage = "The  Contact Info value cannot exceed 100 characters. ")]
        public string? ContactInfo { get; set; }

        public double? OutstandingFee { get; set; }

		public int Status { get; set; }

		public int? BatchId { get; set; }

		public virtual Batch? Batch { get; set; }
		public IEnumerable<SelectListItem>? BatchCode { get; set; }
	}
}

