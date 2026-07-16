using System;
using System.Collections.Generic;
using System.Linq;
using Homewrok_final.Data;
using Homewrok_final.Models;

// Sobe, hekim ve saat sechimi + rezervasiya mentiqi burdadir
namespace Homewrok_final.Services
{
    public class AppointmentService
    {
        //readonly — bu sahəyə yalnız bir dəfə, constructor-da dəyər verilə bilər.
        //Sonra proqram işlədikcə, bu sahəni başqa bir dəyərlə əvəz edə bilməzsən
        private readonly FileRepository<Doctor> _doctorRepository;
        private readonly FileRepository<Appointment> _appointmentRepository;
        private readonly INotificationService _notificationService;

        public AppointmentService(
            FileRepository<Doctor> doctorRepository,
            FileRepository<Appointment> appointmentRepository,
            INotificationService notificationService)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _notificationService = notificationService;
        }

        //secilen sobedeki yalniz approved olan hekimleri qaytarir
        public List<Doctor> GetApprovedDoctorsBySobe(Shobe sobe)
        {
            List<Doctor> doctors = _doctorRepository.LoadAll();
            return doctors.Where(d => d.Sobe == sobe && d.Status == DoctorStatus.Approved).ToList();
        }

        public List<Appointment> GetSlotsByDoctor(Guid doctorId)
        {
            List<Appointment> appointments = _appointmentRepository.LoadAll();
            return appointments.Where(a => a.DoctorId == doctorId).ToList();
        }

        public Doctor GetDoctorById(Guid doctorId)
        {
            List<Doctor> doctors = _doctorRepository.LoadAll();
            return doctors.FirstOrDefault(d => d.ID == doctorId);
        }

        // Rezervasiya edir, uğurlu olarsa bildiriş göndərir. true = uğurlu, false = artıq rezerv olunub
        //Rezervasiya edir, ugurlu olsa bildiris gonderir, dolu saat ushun false qaytarir
        public bool ReserveSlot(Guid appointmentId, User user, Doctor doctor)
        {
            List<Appointment> appointments = _appointmentRepository.LoadAll();
            Appointment target = appointments.FirstOrDefault(a => a.Id == appointmentId);

            if (target == null)
            {
                return false;
            }

            if (target.IsReserved)
            {
                return false; // artıq rezerv olunub, burda rekursiya bas verir
            }

            target.IsReserved = true;
            target.UserId = user.ID;

            _appointmentRepository.SaveAll(appointments);

            // Bildiriş göndər,notify INotificationun icindedi
            _notificationService.Notify(user, doctor, target.Time);

            return true;
        }
    }
}