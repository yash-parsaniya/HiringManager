using System.ComponentModel.DataAnnotations;

namespace HiringManager.Models
{
    public class ApplicationDetail
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        public string? ApplicationId { get; set; }

        // Navigation properties
        public PersonalDetail? PersonalDetail { get; set; }
        public ICollection<EducationDetail> EducationDetails { get; set; } = new List<EducationDetail>();
        public ICollection<ExperienceDetail> ExperienceDetails { get; set; } = new List<ExperienceDetail>();

        // Tracking fields
        public int StageId { get; set; } = 1;
        public string? SessionId { get; set; }

        // Audit fields
        public string CreatedBy { get; set; } = "System";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool ActiveStatus { get; set; } = true;
        public bool Submitted { get; set; } = false;
    }
}
