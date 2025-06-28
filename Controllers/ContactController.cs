using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ContactWithWhatsApp.Models;
using ContactWithWhatsApp.Data;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ContactWithWhatsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly TwilioSettings _twilioSettings;
        private readonly ContactDbContext _context;

        public ContactController(IOptions<TwilioSettings> twilioSettings, ContactDbContext context)
        {
            _twilioSettings = twilioSettings.Value;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm([FromBody] ContactFormDto form)
        {
            var contactMessage = new ContactMessage
            {
                Name = form.Name,
                Email = form.Email,
                Subject = form.Subject,
                Message = form.Message
            };

            try
            {
                // Test database connection first
                Console.WriteLine("Testing database connection...");
                await _context.Database.CanConnectAsync();
                Console.WriteLine("Database connection successful!");
                
                TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

                var messageBody = $"New Contact Form Submission:\n\nName: {form.Name}\nEmail: {form.Email}\nSubject: {form.Subject}\nMessage: {form.Message}";

                var message = await MessageResource.CreateAsync(
                    body: messageBody,
                    from: new Twilio.Types.PhoneNumber(_twilioSettings.FromNumber),
                    to: new Twilio.Types.PhoneNumber(_twilioSettings.ToNumber)
                );

                contactMessage.WhatsAppMessageSid = message.Sid;
                contactMessage.WhatsAppStatus = message.Status.ToString();
                
                Console.WriteLine("Saving to database...");
                _context.ContactMessages.Add(contactMessage);
                await _context.SaveChangesAsync();
                Console.WriteLine("Saved to database successfully!");
                
                return Ok(new { 
                    success = true, 
                    message = "Form submitted, saved to database, and WhatsApp sent successfully.",
                    messageSid = message.Sid,
                    status = message.Status.ToString()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                try 
                {
                    contactMessage.WhatsAppStatus = "Failed";
                    _context.ContactMessages.Add(contactMessage);
                    await _context.SaveChangesAsync();
                    return StatusCode(500, new { success = false, message = "Form saved to database but WhatsApp failed.", error = ex.Message });
                }
                catch (Exception dbEx)
                {
                    Console.WriteLine($"Database error: {dbEx.Message}");
                    return StatusCode(500, new { success = false, message = "Both WhatsApp and database operations failed.", error = ex.Message, dbError = dbEx.Message });
                }
            }
        }
    }
}