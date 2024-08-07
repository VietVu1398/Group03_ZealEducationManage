using ZealEducationManager.Entities;

namespace ZealEducationManager.Models.BatchViewModels
{
    public class BatchCandidatesView
    {
        public int CandidateId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ContactInfo { get; set; }

        public int Status { get; set; }

        public int? BatchId { get; set; }
        public string? BatchCode { get; set; }
    }
}
