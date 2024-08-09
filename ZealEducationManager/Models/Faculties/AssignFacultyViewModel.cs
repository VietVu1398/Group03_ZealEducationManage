using Microsoft.AspNetCore.Mvc.Rendering;
using ZealEducationManager.Entities;

namespace ZealEducationManager.Models.Faculties
{
    public class AssignFacultyViewModel
    {
        public int FacultyID { get; set; }
        public IEnumerable<SelectListItem>? Batches { get; set; }
        public int? SelectedBatch { get; set; }
    }
}
