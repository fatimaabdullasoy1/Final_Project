using System;
using System;
using System.Collections.Generic;
using Homewrok_final.Data;
using Homewrok_final.Models;

namespace Homewrok_final.Services
{
    public class SeedService
    {
        private readonly FileRepository<Doctor> _doctorRepository;
        private readonly FileRepository<Appointment> _appointmentRepository;

        public SeedService(FileRepository<Doctor> doctorRepository, FileRepository<Appointment> appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
        }


        public void SeedIfEmpty()
        {
            List<Doctor> existingDoctors = _doctorRepository.LoadAll();

            if (existingDoctors.Count > 0)
            {
                return;
            }

            List<Doctor> doctors = new List<Doctor>();

            doctors.Add(CreateDoctor("Aygun", "Mammadova", 5, Shobe.Pediatriya));
            doctors.Add(CreateDoctor("Kamran", "Isayev", 8, Shobe.Pediatriya));
            doctors.Add(CreateDoctor("Nigar", "Huseynova", 3, Shobe.Pediatriya));

            doctors.Add(CreateDoctor("Elvin", "Rzayev", 10, Shobe.Travmatologiya));
            doctors.Add(CreateDoctor("Turan", "Guliyev", 6, Shobe.Travmatologiya));

            doctors.Add(CreateDoctor("Leyla", "Aliyeva", 4, Shobe.Stomatologiya));
            doctors.Add(CreateDoctor("Rashad", "Hasanov", 7, Shobe.Stomatologiya));
            doctors.Add(CreateDoctor("Sabina", "Karimova", 2, Shobe.Stomatologiya));
            doctors.Add(CreateDoctor("Tural", "Abbasov", 9, Shobe.Stomatologiya));

            _doctorRepository.SaveAll(doctors);

            List<Appointment> appointments = new List<Appointment>();

            string[] timeSlots = { "09:00-11:00", "12:00-14:00", "15:00-17:00" };

            foreach (Doctor doctor in doctors)
            {
                foreach (string slot in timeSlots)
                {
                    appointments.Add(new Appointment
                    {
                        Id = Guid.NewGuid(),
                        DoctorId = doctor.ID,
                        Time = slot,
                        IsReserved = false,
                        UserId = null,
                        Date = null
                    });
                }
            }

            _appointmentRepository.SaveAll(appointments);
        }

        private Doctor CreateDoctor(string name, string surname, int experience, Shobe shobe)
        {
            return new Doctor
            {
                ID = Guid.NewGuid(),
                Name = name,
                Surname = surname,
                IsTecrube = experience,
                Sobe = shobe,
                Status = DoctorStatus.Approved,
                Email = $"{name.ToLower()}.{surname.ToLower()}@hospital.com",
                Password = "1234"
            };
        }
    }
}

