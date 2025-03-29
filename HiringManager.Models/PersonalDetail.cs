using System.ComponentModel.DataAnnotations;

namespace HiringManager.Models
{
    public class PersonalDetail
    {
        [Key]
        public int Id { get; set; }

        public int ApplicationDetailId { get; set; }
        public ApplicationDetail? ApplicationDetail { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;
    }
}
