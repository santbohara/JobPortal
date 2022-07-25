using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Apply
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string JobId { get; set; }

        [Required]
        public string ApplicantName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public int Email { get; set; }

        [Required]
        public DateTime AppliedDate { get; set; }

        [Required]
        public string BioDataPath { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
