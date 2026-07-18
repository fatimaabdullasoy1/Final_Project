using System;
namespace Homewrok_final.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; } 
        public string Time { get; set; }
        public bool IsReserved { get; set; } = false; 
        public Guid? UserId { get; set; } 
        public DateTime? Date { get; set; }
    }
}


