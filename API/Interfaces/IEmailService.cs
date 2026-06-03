using API.Services;

namespace API.Interfaces {

    public class EmailResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
    
    public interface IEmailService
    {
        Task<EmailResult> SendEmailAsync(string recipientName, string recipientEmail, string verificationCode);
    }
}