using System.ComponentModel.DataAnnotations;

namespace HiringManager.Models
{
    public class ExperienceDetail
    {
        public int Id { get; set; }
        public int ApplicationDetailId { get; set; }
        public decimal TotalExperience { get; set; }
        public string PreviousCompanies { get; set; } // JSON array
        public string CurrentCompany { get; set; }
        public string Skills { get; set; } // JSON array
        public string Roles { get; set; } // JSON array
        public ApplicationDetail ApplicationDetail { get; set; }
    }
}
