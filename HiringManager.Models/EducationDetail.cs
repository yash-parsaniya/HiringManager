using System.ComponentModel.DataAnnotations;

namespace HiringManager.Models
{
    public class EducationDetail
    {
        public int Id { get; set; }
        public int ApplicationDetailId { get; set; }
        public string HighestQualification { get; set; }
        public string CollegeUniversity { get; set; }
        public string Stream { get; set; }
        public int PassoutYear { get; set; }
        public decimal PointerPercentage { get; set; }
        public ApplicationDetail ApplicationDetail { get; set; }
    }

}
