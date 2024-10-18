using System.Net;
using System.Net.Mail;

namespace Dating.Email
{
    public class EmailService
    {
        private readonly string _emailFrom = "vaicatrau@gmail.com";
        private readonly string _password = "qtkfwremyoizqnmm";

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_emailFrom, _password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailFrom, "Dating"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            smtpClient.SendCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    Console.WriteLine("Gửi email thất bại: " + e.Error.ToString());
                }
                else
                {
                    Console.WriteLine("Email đã được gửi thành công!");
                }
                smtpClient.Dispose();
                mailMessage.Dispose();
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
