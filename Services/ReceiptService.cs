using System;
using System.IO;
using Homewrok_final.Models;

namespace Homewrok_final.Services
{
    public class ReceiptService
    {
        private readonly string _folderPath;

        public ReceiptService()
        {
            _folderPath = Path.Combine("AppData", "Receipts");
            Directory.CreateDirectory(_folderPath);
        }

        public string GenerateReceipt(User user, Doctor doctor, string time)
        {
            string fileName = $"receipt_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString().Substring(0, 6)}.txt";
            string fullPath = Path.Combine(_folderPath, fileName);

            string content =
                "========== RECEIPT RECEIPT ==========" + Environment.NewLine +
                $"Tarix: {DateTime.Now:dd.MM.yyyy HH:mm}" + Environment.NewLine +
                "-----------------------------------" + Environment.NewLine +
                $"Xəstə: {user.Name} {user.Surname}" + Environment.NewLine +
                $"Email: {user.Email}" + Environment.NewLine +
                $"Telefon: {user.Phone}" + Environment.NewLine +
                "-----------------------------------" + Environment.NewLine +
                $"Həkim: Dr. {doctor.Name} {doctor.Surname}" + Environment.NewLine +
                $"Şöbə: {doctor.Sobe}" + Environment.NewLine +
                $"Qəbul saatı: {time}" + Environment.NewLine +
                "===================================" + Environment.NewLine +
                "Təşəkkürlər, sağlam qalın!";

            File.WriteAllText(fullPath, content);

            return fullPath;
        }
    }
}
