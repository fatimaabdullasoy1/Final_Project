using System;
using System.Collections.Generic;
using System.Linq;

using Homewrok_final.Data;
using Homewrok_final.Models;
using Homewrok_final.Services;
using Homewrok_final.UI;

namespace Homewrok_final
{
    class Program
    {
        static FileRepository<User> userRepository = new FileRepository<User>("users.json");
        static FileRepository<Doctor> doctorRepository = new FileRepository<Doctor>("doctors.json");
        static FileRepository<Appointment> appointmentRepository = new FileRepository<Appointment>("appointments.json");

        static INotificationService notificationService = new ConsoleNotificationService();

        static AuthService authService = new AuthService(userRepository, doctorRepository);
        static AppointmentService appointmentService = new AppointmentService(doctorRepository, appointmentRepository, notificationService);
        static ReceiptService receiptService = new ReceiptService();

        static void Main(string[] args)
        {
            SeedService seedService = new SeedService(doctorRepository, appointmentRepository);
            seedService.SeedIfEmpty();

            Console.Clear();
            MenuHelper.PrintBanner();
            Console.WriteLine();
            MenuHelper.PressAnyKeyToContinue();

            while (true)
            {
                string[] mainOptions = {
                    "Admin Entry",
                    "User Qeydiyyat",
                    "User Login",
                    "Doctor Qeydiyyatı",
                    "Doctor Login",
                    "Çıxış"
                };

                int choice = MenuHelper.ShowMenu(" Main MENU ", mainOptions);

                switch (choice)
                {
                    case 0:
                        AdminMenu();
                        break;
                    case 1:
                        UserRegister();
                        break;
                    case 2:
                        UserLogin();
                        break;
                    case 3:
                        DoctorRegister();
                        break;
                    case 4:
                        DoctorLogin();
                        break;
                    case 5:
                        return;
                }
            }
        }

        //  USER REGISTER / LOGIN 

        static void UserRegister()
        {
            Console.Clear();
            Console.WriteLine(" USER QEYDİYYAT ");

            Console.Write("Ad: ");
            string name = Console.ReadLine();

            Console.Write("Soyad: ");
            string surname = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Telefon: ");
            string phone = Console.ReadLine();

            Console.Write("Şifre: ");
            string password = Console.ReadLine();

            User newUser = authService.RegisterUser(name, surname, email, phone, password);

            if (newUser == null)
            {
                MenuHelper.PrintError("Bu email artıq istifadə olunub!");
            }
            else
            {
                MenuHelper.PrintSuccess("Qeydiyyat uğurludur! İndi giriş edə bilərsiniz.");
            }

            MenuHelper.PressAnyKeyToContinue();
        }

        static void UserLogin()
        {
            Console.Clear();
            Console.WriteLine(" USER LOGİN ");

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Şifre: ");
            string password = Console.ReadLine();

            User user = authService.LoginUser(email, password);

            if (user == null)
            {
                MenuHelper.PrintError("Email və ya şifrə yanlışdır!");
                MenuHelper.PressAnyKeyToContinue();
                return;
            }

            MenuHelper.PrintSuccess($"Xoş gəldiniz, {user.Name} {user.Surname}!");
            MenuHelper.PressAnyKeyToContinue();

            SobeSelectionMenu(user);
        }

        //  SOBE -> DOCTOR -> SLOT SELECTION 

        static void SobeSelectionMenu(User user)
        {
            string[] sobeOptions = { "Pediatriya", "Travmatologiya", "Stomatologiya", "Geri qayıt" };

            int choice = MenuHelper.ShowMenu(" Shobe SEÇİN ", sobeOptions);

            if (choice == 3)
            {
                return;
            }

            Shobe selectedSobe = (Shobe)choice;

            DoctorSelectionMenu(user, selectedSobe);
        }

        static void DoctorSelectionMenu(User user, Shobe sobe)
        {
            List<Doctor> doctors = appointmentService.GetApprovedDoctorsBySobe(sobe);

            if (doctors.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Bu Shobede hazırda tesdiqlenmiş Doctor yoxdur.");
                MenuHelper.PressAnyKeyToContinue();
                return;
            }

            List<string> options = doctors.Select(d => $"{d.Name} {d.Surname} ({d.IsTecrube} il tecrübe)").ToList();
            options.Add("Geri qayıt");

            int choice = MenuHelper.ShowMenu(" Doctor SEÇİN ", options.ToArray());

            if (choice == doctors.Count)
            {
                SobeSelectionMenu(user);
                return;
            }

            Doctor selectedDoctor = doctors[choice];
            SlotSelectionMenu(user, selectedDoctor);
        }

        static void SlotSelectionMenu(User user, Doctor doctor)
        {
            List<Appointment> slots = appointmentService.GetSlotsByDoctor(doctor.ID);
            List<string> options = slots.Select(s =>
                $"{s.Time}  -  {(s.IsReserved ? "REZERV OLUNUB" : "BOŞDUR")}"
            ).ToList();
            options.Add("Geri qayıt");

            int choice = MenuHelper.ShowMenu($" {doctor.Name} {doctor.Surname} - SAAT SEÇİN ", options.ToArray());

            if (choice == slots.Count)
            {
                DoctorSelectionMenu(user, doctor.Sobe);
                return;
            }

            Appointment selectedSlot = slots[choice];
            bool success = appointmentService.ReserveSlot(selectedSlot.Id, user, doctor);

            if (!success)
            {
                Console.Clear();
                MenuHelper.PrintError("Bu vaxt artıq rezerv olunub, zəhmət olmasa başqa bir vaxt seçin.");
                MenuHelper.PressAnyKeyToContinue();
                SlotSelectionMenu(user, doctor); // REKURSİYA
                return;
            }

            string receiptPath = receiptService.GenerateReceipt(user, doctor, selectedSlot.Time);

            Console.Clear();
            MenuHelper.PrintSuccess($"Təşəkkürlər {user.Name} {user.Surname}, siz saat {selectedSlot.Time} də {doctor.Name} həkimin qəbuluna yazıldınız.");
            MenuHelper.PrintInfo($"Çekiniz saxlanıldı: {receiptPath}");
            MenuHelper.PressAnyKeyToContinue();
        }

        //  DOCTOR REGISTER / LOGIN 

        static void DoctorRegister()
        {
            Console.Clear();
            Console.WriteLine(" Doctor QEYDİYYATI ");

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("SurName: ");
            string surname = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Pssword: ");
            string password = Console.ReadLine();

            Console.Write("İş təcrübəsi (il): ");
            int experience;
            bool isValid = int.TryParse(Console.ReadLine(), out experience);

            if (!isValid)
            {
                MenuHelper.PrintError("Yanlış format! Rəqəm daxil edin.");
                MenuHelper.PressAnyKeyToContinue();
                return;
            }

            string[] sobeOptions = { "Pediatriya", "Travmatologiya", "Stomatologiya" };
            int sobeChoice = MenuHelper.ShowMenu("Shobe seçin:", sobeOptions);
            Shobe sobe = (Shobe)sobeChoice;

            Doctor newDoctor = authService.RegisterDoctor(name, surname, email, password, sobe, experience);

            Console.Clear();
            if (newDoctor == null)
            {
                Console.WriteLine("Bu email artıq istifade olunub!");
            }
            else
            {
                Console.WriteLine("Müracietiniz gönderildi. Admin tesdiqinden sonra sisteme daxil ola biler siniz.");
            }

            MenuHelper.PressAnyKeyToContinue();
        }

        static void DoctorLogin()
        {
            Console.Clear();
            Console.WriteLine(" Doctor LOGİN ");

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Şifre: ");
            string password = Console.ReadLine();

            Doctor doctor = authService.LoginDoctor(email, password);

            if (doctor == null)
            {
                MenuHelper.PrintError("Email/şifrə yanlışdır, ya da hesabınız hələ təsdiqlənməyib.");
            }
            else
            {
                MenuHelper.PrintSuccess($"Xoş gəldiniz, Dr. {doctor.Name} {doctor.Surname}!");
            }

            MenuHelper.PressAnyKeyToContinue();
        }

        //  ADMIN 

        static void AdminMenu()
        {
            List<Doctor> allDoctors = doctorRepository.LoadAll();
            List<Doctor> pendingDoctors = allDoctors.Where(d => d.Status == DoctorStatus.Pending).ToList();

            if (pendingDoctors.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Tesdiq gözleyen Doctor yoxdur.");
                MenuHelper.PressAnyKeyToContinue();
                return;
            }

            List<string> options = pendingDoctors.Select(d => $"{d.Name} {d.Surname} - {d.Sobe} ({d.IsTecrube} il)").ToList();
            options.Add("Geri qayıt");

            int choice = MenuHelper.ShowMenu(" TeSDİQ GÖZLeYeN DoctorLeR ", options.ToArray());

            if (choice == pendingDoctors.Count)
            {
                return;
            }

            Doctor selected = pendingDoctors[choice];

            string[] decisionOptions = { "Qebul et", "Redd et" };
            int decision = MenuHelper.ShowMenu($"{selected.Name} {selected.Surname} üçün qerar:", decisionOptions);

            DoctorStatus newStatus = decision == 0 ? DoctorStatus.Approved : DoctorStatus.Rejected;

            allDoctors = doctorRepository.LoadAll();
            Doctor toUpdate = allDoctors.FirstOrDefault(d => d.ID == selected.ID);
            toUpdate.Status = newStatus;
            doctorRepository.SaveAll(allDoctors);

            Console.Clear();
            Console.WriteLine($"{selected.Name} {selected.Surname} statusu: {newStatus}");
            MenuHelper.PressAnyKeyToContinue();

            AdminMenu();
        }
    }
}