using System;

namespace Homewrok_final.Models
{
	public class Doctor
	{
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int IsTecrube { get; set; }
        public Shobe Sobe { get; set; }
        public DoctorStatus Status { get; set; } = DoctorStatus.Pending;
        public string Email { get; set; }
        public string Password { get; set; }

        public Doctor()
		{
		}
	}
}

