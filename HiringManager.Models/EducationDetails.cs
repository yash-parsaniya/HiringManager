using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HiringManager.Models
{
    public class EducationDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string HighestQualification { get; set; } = "";

        [Required]
        [StringLength(200)]
        public string CollegeUniversity { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Stream { get; set; } = "";

        [Required]
        [Range(1900, 2100)]
        public int PassoutYear { get; set; }

        [Required]
        [StringLength(20)]
        public string PointerPercentage { get; set; } = "";

        [ForeignKey("ApplicationDetails")]
        public int ApplicationDetailsId { get; set; }
        public ApplicationDetails? ApplicationDetails { get; set; }
    }
}
