using Microsoft.AspNetCore.Mvc.Rendering;
using ZealEducationManager.Entities;

namespace ZealEducationManager.Models.ExamsViewModel
{
	public class AddExamModel
	{
		public int ExamId { get; set; }

		public DateOnly ExamDate { get; set; }

		public int CourseId { get; set; }

		public int BatchId { get; set; }

		public IEnumerable<SelectListItem>? CourseCode { get; set;}
		public IEnumerable<SelectListItem>? BatchCode { get; set;}

	}
}
