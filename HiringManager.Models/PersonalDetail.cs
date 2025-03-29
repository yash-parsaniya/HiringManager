using System.ComponentModel.DataAnnotations;

namespace HiringManager.Models
{
    public class PersonalDetail
    {
        public int Id { get; set; }
        public int ApplicationDetailId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public ApplicationDetail ApplicationDetail { get; set; }
    }
}
