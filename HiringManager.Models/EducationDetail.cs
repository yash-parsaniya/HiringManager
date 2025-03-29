using System.ComponentModel.DataAnnotations;

namespace HiringManager.Models
{
    public class EducationDetail
    {
        [Key]
        public int Id { get; set; }

        public int ApplicationDetailId { get; set; }
        public ApplicationDetail? ApplicationDetail { get; set; }

        [Required]
        [StringLength(100)]
        public string HighestQualification { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string CollegeUniversity { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Stream { get; set; } = string.Empty;

        [Range(1900, 2100)]
        public int PassoutYear { get; set; }

        [Range(0, 100)]
        public decimal PointerPercentage { get; set; }
    }
}
