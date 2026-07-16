using System;

//Yuxari/ashagi ox duymeleri ile menyu gostermek ucun
namespace Homewrok_final.UI
{
    public static class MenuHelper
    {
        public static int ShowMenu(string title, string[] options)
        {
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine(title);
                Console.WriteLine("(Yuxarı/Aşağı ox düymələri ilə seçin, Enter ilə təsdiqləyin)");
                Console.WriteLine();

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("> " + options[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("  " + options[i]);
                    }
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0) selectedIndex = options.Length - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex > options.Length - 1) selectedIndex = 0;
                }

            } while (key != ConsoleKey.Enter);

            return selectedIndex;
        }

        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Davam etmək üçün istənilən düyməyə basın...");
            Console.ReadKey(true);
        }
    }
}

