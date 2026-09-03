using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Business.Interfaces;


namespace Business.Services
{
    public class EmailService :IEmailService
    {
        private IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendOtpAsync(string email, string otpCode)
        {
            //Install-Package MailKit
            /*
             
             AuthService.RequestPasswordReset(email)

                        ↓
                    
                    Get user by email
                    
                        ↓
                    
                    Generate OTP
                    
                        ↓
                    
                    Create PasswordResetOtp
                    
                        ↓
                    
                    OtpRepository.AddOtpAsync()
                    
                        ↓
                    
                    UnitOfWork.SaveChangesAsync()
                    
                        ↓
                    
                    EmailService.SendOtpAsync()
             
             
             */

            string subject = "Password OTP Code";
            string body =
        $"""
                Your password reset code is:
                
                {otpCode}
                
                This code will expire in 10 minutes.
                
                If you did not request a password reset, please ignore this email.
                """;
            await SendEmailAsync(email, subject, body);
        }
        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            var email = _config["EmailSettings:Email"]
                ?? throw new InvalidOperationException("Email not configured.");
            var password = _config["EmailSettings:Password"]
                ?? throw new InvalidOperationException("Email password not configured.");
            message.From.Add(new MailboxAddress("Assessment System", email));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(email,
                                         password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }

    }
}
