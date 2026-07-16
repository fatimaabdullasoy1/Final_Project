using Homewrok_final.Models;

//Bildiris gondermek ucun umumi interface — email ve ya console versiyasi ola biler
namespace Homewrok_final.Services
{
    public interface INotificationService
    {
        void Notify(User user, Doctor doctor, string time);
    }
}

