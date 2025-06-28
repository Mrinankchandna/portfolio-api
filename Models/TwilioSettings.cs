namespace ContactWithWhatsApp.Models
{
    public class TwilioSettings
    {
        public required string AccountSid { get; set; }
        public required string AuthToken { get; set; }
        public required string FromNumber { get; set; }
        public required string ToNumber { get; set; }
    }
}