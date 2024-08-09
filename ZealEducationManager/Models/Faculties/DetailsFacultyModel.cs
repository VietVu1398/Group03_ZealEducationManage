using ZealEducationManager.Entities;

namespace ZealEducationManager.Models.Faculties
{
    public class DetailsFacultyModel
    {

        public int FacultyId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ContactInfo { get; set; }

        public string FacultyCode { get; set; } = null!;

        public virtual ICollection<FacultyBatch> FacultyBatches { get; set; } = new List<FacultyBatch>();
    }
}
