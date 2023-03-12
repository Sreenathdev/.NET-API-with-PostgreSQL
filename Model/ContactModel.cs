using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Contact_Manager.Model
{
    public class ContactModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Salutation is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Salutation must be at least 3 characters long")]
        public string Salutation { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First Name must be at least 3 characters long")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Last Name must be at least 3 characters long")]
        public string LastName { get; set; }

        public string DisplayName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationTimestamp { get; set; }

        [JsonIgnore]
        [DataType(DataType.DateTime)]
        public DateTime LastChangeTimestamp { get; set; }

        [JsonIgnore]
        public bool NotifyHasBirthdaySoon { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}