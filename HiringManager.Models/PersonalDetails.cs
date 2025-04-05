using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HiringManager.Models
{
    public class PersonalDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = "";

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = "";

        [ForeignKey("ApplicationDetails")]
        public int ApplicationDetailsId { get; set; }
        public ApplicationDetails? ApplicationDetails { get; set; }
    }
}
