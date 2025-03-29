using System.ComponentModel.DataAnnotations;

namespace HiringManager.Models
{
    public class ApplicationDetail
    {
        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public int StageId { get; set; } = 1;
        public string SessionId { get; set; }
        public string CreatedBy { get; set; } = "System";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool ActiveStatus { get; set; } = true;
        public bool IsSubmitted { get; set; } = false;

        // Navigation
        public PersonalDetail PersonalDetail { get; set; }
        public EducationDetail EducationDetail { get; set; }
        public ExperienceDetail ExperienceDetail { get; set; }
    }
}
