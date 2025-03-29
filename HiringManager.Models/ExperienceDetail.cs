using System.ComponentModel.DataAnnotations;

namespace HiringManager.Models
{
    public class ExperienceDetail
    {
        [Key]
        public int Id { get; set; }

        public int ApplicationDetailId { get; set; }
        public ApplicationDetail? ApplicationDetail { get; set; }

        [Range(0, 50)]
        public decimal TotalExperience { get; set; }

        [StringLength(500)]
        public string PreviousCompanies { get; set; } = string.Empty;

        [StringLength(100)]
        public string CurrentCompany { get; set; } = string.Empty;

        [StringLength(500)]
        public string Skills { get; set; } = string.Empty;

        [StringLength(500)]
        public string Roles { get; set; } = string.Empty;
    }
}
