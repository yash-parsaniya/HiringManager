using System;
using System.ComponentModel.DataAnnotations;

namespace HiringManager.Models
{
    public class ApplicationDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string ApplicationId { get; set; } = "";

        public int StageId { get; set; } = 1; // 1: Personal, 2: Education, 3: Experience

        [Required]
        public string SessionId { get; set; } = Guid.NewGuid().ToString();

        public string CreatedBy { get; set; } = "System";

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? UpdatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

        public bool ActiveStatus { get; set; } = true;

        public bool IsSubmitted { get; set; } = false;

        // Navigation properties
        public PersonalDetails? PersonalDetails { get; set; }
        public EducationDetails? EducationDetails { get; set; }
        public ExperienceDetails? ExperienceDetails { get; set; }
    }
}
