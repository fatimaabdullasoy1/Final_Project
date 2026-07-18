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




