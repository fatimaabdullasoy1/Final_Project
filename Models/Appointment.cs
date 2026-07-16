using System;
namespace Homewrok_final.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; } //hekime aid id
        public string Time { get; set; } //saat araligi
        public bool IsReserved { get; set; } = false; //default false di
        public Guid? UserId { get; set; } // bunlar nulabledir cunki
        public DateTime? Date { get; set; }
    }
}

//appointment objecti program baslayan kimi seedservice vasitesi ile yaradilir


//"C#-da Guid, DateTime, int, bool kimi tiplər struct-dır
//— yəni onlara null verə bilmərik, adətən mütləq bir dəyər
//olmalıdır. Amma bəzi sahələr məntiqi olaraq 'hələ dəyəri
//yoxdur' vəziyyətində ola bilər — bizim vəziyyətimizdə, rezerv
//olunmamış bir appointment-in kim tərəfindən rezerv olunduğunu
//bilə bilmərik, çünki hələ heç kim etməyib. Bu halda ? işarəsi
//əlavə edərək (Guid?, DateTime?) həmin sahəni nullable edirik —
//yəni ya real dəyər saxlaya bilər, ya da null."
