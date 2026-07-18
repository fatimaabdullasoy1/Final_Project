using System;
using Homewrok_final.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Homewrok_final.Services
{
    public class ConsoleNotificationService : INotificationService
    {
        public void Notify(User user, Doctor doctor, string time)
        {
            Console.WriteLine();
            Console.WriteLine("---- Notification (E-MAIL SIMULYASIYASI) ----");
            Console.WriteLine($"To whom: {user.Email}");
            Console.WriteLine($"Topic: Randevu təsdiqi");
            Console.WriteLine($"Text: Hörmətli {user.Name} {user.Surname}, siz saat {time} tarixində Dr. {doctor.Name} {doctor.Surname} həkiminin qəbuluna uğurla yazıldınız.");
            Console.WriteLine("-------------------------------------------");
        }
    }
}
