namespace ContactsApi.Models
{
    public class Contact
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public User? User { get; set; }
        public YourNumber? YourNumber { get; set; }
    }
}
