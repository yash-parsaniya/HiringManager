using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HiringManager.Models
{
    public class ExperienceDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TotalExp { get; set; } = "";

        [StringLength(500)]
        public string? PreviousCompanies { get; set; }

        [StringLength(100)]
        public string? CurrentCompany { get; set; }

        [Required]
        [StringLength(500)]
        public string Skills { get; set; } = "";

        [Required]
        [StringLength(500)]
        public string Roles { get; set; } = "";

        [ForeignKey("ApplicationDetails")]
        public int ApplicationDetailsId { get; set; }
        public ApplicationDetails? ApplicationDetails { get; set; }
    }
}
