using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Homewrok_final.Data
{
    public class FileRepository<T>
    {
        private readonly string _filePath;

        public FileRepository(string fileName)
        {
            string folder = "AppData";
            Directory.CreateDirectory(folder);
            _filePath = Path.Combine(folder, fileName);
        }

        public List<T> LoadAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            try
            {
                string json;

                using (StreamReader reader = new StreamReader(_filePath))
                {
                    json = reader.ReadToEnd();
                }

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<T>();
                }

                return JsonSerializer.Deserialize<List<T>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[XƏTA] Fayl oxunarkən problem yarandı: {ex.Message}");
                return new List<T>();
            }
        }

        public void SaveAll(List<T> items)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(items, options);

                using (StreamWriter writer = new StreamWriter(_filePath))
                {
                    writer.Write(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[XƏTA] Fayla yazarkən problem yarandı: {ex.Message}");
            }
        }

        public void Add(T item)
        {
            List<T> items = LoadAll();
            items.Add(item);
            SaveAll(items);
        }
    }
}





//Ayri-ayri "DoctorFileHandler", "UserFileHandler", "AppointmentFileHandler" yazmaq
//evezine (3 defe tekrarlanan kod), bir generic class yazilir ki, her uchu ushun
//ishlesin, chunki hamisina eyni emeliyyatlar lazimdir: siyahini yuklemek, siyahini
//saxlamaq, bir dene elave etmek, yenilemek.

//Niye generic (<T>)?
//Generic olmasaydi, eyni yukleme/saxlama mentiqini 3 defe ayrica yazmali olardiq
//— biri User ushun, biri Doctor ushun, biri Appointment ushun — halbuki mentiq eynidir.
//<T> ile bunu bir defe yaziriq ve tekrar istifade ediriq.