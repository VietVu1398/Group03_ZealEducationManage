using System.ComponentModel.DataAnnotations;
using ZealEducationManager.Entities;

namespace ZealEducationManager.Models.ExamsViewModel
{
    public class EnterMarksModel
    {
        public int ResultId { get; set; }

        public int ExamId { get; set; }

        public int CandidateId { get; set; }
        [Range(0, 100, ErrorMessage = "Marks must be between 0 and 100.")]
        public decimal? MarksObtained { get; set; }

        public virtual Candidate? Candidate { get; set; } = null!;

        public virtual Exam? Exam { get; set; } = null!;
    }
}
