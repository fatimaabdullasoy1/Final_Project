using System;
namespace Homewrok_final.Models
{
	public class User
	{
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public User()
		{
		}
	}
}

