using System.ComponentModel.DataAnnotations;

namespace ContactWithWhatsApp.Models
{
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Subject { get; set; }
        public required string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? WhatsAppMessageSid { get; set; }
        public string? WhatsAppStatus { get; set; }
    }
}