using Homewrok_final.Models;

namespace Homewrok_final.Services
{
    public interface INotificationService
    {
        void Notify(User user, Doctor doctor, string time);
    }
}

