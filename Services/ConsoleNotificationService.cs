using System;
using Homewrok_final.Models;
using static System.Net.Mime.MediaTypeNames;

//Test ucun, ekrana bildiris yazir (real email evezine)
namespace Homewrok_final.Services
{
    public class ConsoleNotificationService : INotificationService
    {
        public void Notify(User user, Doctor doctor, string time)
        {
            Console.WriteLine();
            Console.WriteLine("---- BİLDİRİŞ (E-MAIL SIMULYASIYASI) ----");
            Console.WriteLine($"Kimə: {user.Email}");
            Console.WriteLine($"Mövzu: Randevu təsdiqi");
            Console.WriteLine($"Mətn: Hörmətli {user.Name} {user.Surname}, siz saat {time} tarixində Dr. {doctor.Name} {doctor.Surname} həkiminin qəbuluna uğurla yazıldınız.");
            Console.WriteLine("-------------------------------------------");
        }
    }
}
