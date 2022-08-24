using System.ComponentModel.DataAnnotations;

namespace ContactsApi.Dtos
{
    public class GetContactsDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
    }
}
