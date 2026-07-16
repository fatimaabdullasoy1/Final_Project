using System;
using System.Net;
using System.Net.Mail;
using Homewrok_final.Models;

//Real email gondermek ucun (SMTP ile) — hazirda istifade olunmur, Console versiyasi aktivdir
namespace Homewrok_final.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public EmailNotificationService(string senderEmail, string senderPassword)
        {
            _senderEmail = senderEmail;
            _senderPassword = senderPassword;
        }

        public void Notify(User user, Doctor doctor, string time)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_senderEmail, _senderPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail, "Xəstəxana Sistemi"),
                    Subject = "Randevu Təsdiqi",
                    Body = $"Hörmətli {user.Name} {user.Surname},\n\n" +
                           $"Siz saat {time} tarixində Dr. {doctor.Name} {doctor.Surname} həkiminin qəbuluna uğurla yazıldınız.\n\n" +
                           $"Sağlam qalın!",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(user.Email);

                smtpClient.Send(mailMessage);

                Console.WriteLine($"[Email göndərildi: {user.Email}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Email göndərilə bilmədi: {ex.Message}]");
            }
        }
    }
}

