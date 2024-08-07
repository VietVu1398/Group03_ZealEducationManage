using System;
using System.Collections.Generic;
using ZealEducationManager.Entities;
namespace ZealEducationManager.Models.CourseViewModels
{
    public class AddCourseView
    {
        public int CourseId { get; set; }

        public string CourseCode { get; set; } = null!;

        public string CourseName { get; set; } = null!;

        public decimal CourseFee { get; set; }

        public virtual ICollection<Enquiry> Enquiries { get; set; } = new List<Enquiry>();

        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}
