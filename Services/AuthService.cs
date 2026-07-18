using System;
using System.Collections.Generic;
using System.Linq;
using Homewrok_final.Data;
using Homewrok_final.Models;

namespace Homewrok_final.Services
{
    public class AuthService
    {
        private readonly FileRepository<User> _userRepository;
        private readonly FileRepository<Doctor> _doctorRepository;

        public AuthService(FileRepository<User> userRepository, FileRepository<Doctor> doctorRepository)
        {
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
        }

        public User RegisterUser(string name, string surname, string email, string phone, string password)
        {
            List<User> users = _userRepository.LoadAll();

            bool emailExists = users.Any(u => u.Email == email);
            if (emailExists)
            {
                return null; 
            }

            User newUser = new User
            {
                ID = Guid.NewGuid(),
                Name = name,
                Surname = surname,
                Email = email,
                Phone = phone,
                Password = password
            };

            _userRepository.Add(newUser);
            return newUser;
        }

        public Doctor RegisterDoctor(string name, string surname, string email, string password, Shobe sobe, int isTecrube)
        {
            List<Doctor> doctors = _doctorRepository.LoadAll();

            bool emailExists = doctors.Any(d => d.Email == email);
            if (emailExists)
            {
                return null;
            }

            Doctor newDoctor = new Doctor
            {
                ID = Guid.NewGuid(),
                Name = name,
                Surname = surname,
                Email = email,
                Password = password,
                Sobe = sobe,
                IsTecrube = isTecrube,
                Status = DoctorStatus.Pending 
            };

            _doctorRepository.Add(newDoctor);
            return newDoctor;
        }

        public User LoginUser(string email, string password)
        {
            List<User> users = _userRepository.LoadAll();
            return users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public Doctor LoginDoctor(string email, string password)
        {
            List<Doctor> doctors = _doctorRepository.LoadAll();  
            Doctor doctor = doctors.FirstOrDefault(d => d.Email == email && d.Password == password);

            if (doctor == null)
            {
                return null;
            }

            if (doctor.Status != DoctorStatus.Approved)
            {
                return null;
            }

            return doctor;
        }
    }
}

//FirstOrDefault(...) — bu, LINQ metodudur
//(C#-ın daxili siyahı axtarış aləti).
//Deyir: "siyahıda birinci uyğun gələn elementi tap, tapmasan null qaytar"
//(adi First()-dən fərqi budur ki, tapılmasa xəta vermir, sadəcə null qaytarır).
