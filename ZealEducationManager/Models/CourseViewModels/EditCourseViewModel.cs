using System.ComponentModel.DataAnnotations;
using ZealEducationManager.Entities;

namespace ZealEducationManager.Models.CourseViewModels
{
    public class EditCourseViewModel
    {
        public int CourseId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The Course Code has max length 20 characters")]
        public string CourseCode { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "The Course Code has max length 100 characters")]
        public string CourseName { get; set; } = null!;

        [Required]
        [Range(0, 100000, ErrorMessage = "Course Fee must be between 0 and 100000")]
        public decimal CourseFee { get; set; }

        public virtual ICollection<Enquiry> Enquiries { get; set; } = new List<Enquiry>();

        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}
