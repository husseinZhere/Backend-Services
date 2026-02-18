using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using PulseX.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace PulseX.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string userName, string otp)
        {
            // 1. اطبع الكود فوراً في الـ Terminal (ده اللي هتاخده كوبي)
            Console.WriteLine("\n" + new string('*', 52));
            Console.WriteLine("📧 PULSEX DEBUG: PASSWORD RESET OTP");
            Console.WriteLine($"TO: {toEmail}");
            Console.WriteLine($"🔑 VERIFICATION CODE: {otp}");
            Console.WriteLine(new string('*', 52) + "\n");

            try
            {
                var smtpHost = _configuration["Email:SmtpHost"];
                var smtpUsername = _configuration["Email:Username"];

                // 2. تأكد إن الإعدادات موجودة قبل ما تحاول تبعت
                if (!string.IsNullOrEmpty(smtpHost) && !string.IsNullOrEmpty(smtpUsername))
                {
                    var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
                    var smtpPassword = _configuration["Email:Password"];
                    var fromEmail = _configuration["Email:FromEmail"];
                    var fromName = _configuration["Email:FromName"] ?? "PulseX";

                    using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                    {
                        Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                        EnableSsl = true
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail!, fromName),
                        Subject = "Password Reset - PulseX",
                        Body = GenerateEmailBody(userName, otp),
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Actual email sent to {toEmail}");
                }
                else
                {
                    _logger.LogWarning("Email sending skipped: SMTP settings are missing in appsettings.json");
                }
            }
            catch (Exception ex)
            {
                // 3. التعديل الجوهري: لو فشل الإرسال، اطبع الـ Error في الـ Log بس
                // ومترميش Exception عشان السواجر يرجع 200 Success وتعرف تكمل تيست بالـ OTP اللي في الـ Console
                _logger.LogError($"Email delivery failed, but OTP was generated: {ex.Message}");
            }
        }

        private string GenerateEmailBody(string userName, string otp)
        {
            return $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: auto; border: 1px solid #ddd; border-radius: 10px; padding: 20px;'>
                    <h2 style='color: #333CF5;'>PulseX Password Reset</h2>
                    <p>Hello {userName},</p>
                    <p>Your verification code is:</p>
                    <div style='background: #F5F6FF; padding: 20px; text-align: center; font-size: 32px; font-weight: bold; color: #333CF5; letter-spacing: 5px;'>
                        {otp}
                    </div>
                    <p>This code expires in 15 minutes.</p>
                </div>
            </body>
            </html>";
        }
    }
}