using System.Text.RegularExpressions;

namespace _5HW_System_Programming
{
    internal class Program
    {
        static async Task Main()
        {
            while (true)
            {
                Console.WriteLine("Виберіть джерело тексту:\n1. Файл\n2. Консоль\n0. Вихід");

                string text = "";
                string choice = Console.ReadLine();
                if (choice == "0") break;
                if (choice == "1")
                {
                    Console.Write("Введіть шлях до текстового файлу: ");
                    string path = Console.ReadLine();

                    if (File.Exists(path))
                    {
                        text = await File.ReadAllTextAsync(path);
                    }
                    else
                    {
                        Console.WriteLine("Файл не знайдено.");
                        continue;
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Введіть текст:");
                    text = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Невірний вибір.");
                    continue;
                }

                Console.WriteLine("Що включити у звіт? Введіть y/n:");

                bool includeSent = Ask("Кількість речень?");
                bool includeWords = Ask("Кількість слів?");
                bool includeChar = Ask("Кількість символів?");
                bool includeQuest = Ask("Кількість питальних речень?");
                bool includeExcl = Ask("Кількість окличних речень?");

                var tasks = new List<Task<string>>();

                if (includeSent)
                    tasks.Add(Task.Run(() => $"Кількість речень: {Regex.Matches(text, @"[.!?]").Count}"));
                if (includeWords)
                    tasks.Add(Task.Run(() => $"Кількість слів: {Regex.Matches(text, @"\b\w+\b").Count}"));
                if (includeChar)
                    tasks.Add(Task.Run(() => $"Кількість символів: {text.Length}"));
                if (includeQuest)
                    tasks.Add(Task.Run(() => $"Кількість питальних речень: {Regex.Matches(text, @"\?").Count}"));
                if (includeExcl)
                    tasks.Add(Task.Run(() => $"Кількість окличних речень: {Regex.Matches(text, @"!").Count}"));

                var results = await Task.WhenAll(tasks);

                Console.WriteLine("\n=== ЗВІТ ===");
                foreach (var line in results)
                    Console.WriteLine(line);

                Console.Write("Зберегти звіт у файл? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.Write("Введіть шлях до файлу для збереження: ");
                    string savePath = Console.ReadLine();
                    await File.WriteAllLinesAsync(savePath, results);
                    Console.WriteLine("Звіт збережено.");
                }

                Console.WriteLine("\nХочете проаналізувати ще один текст? (y/n): ");
                if (Console.ReadLine().ToLower() != "y")
                    break;
            }
        }

        static bool Ask(string message)
        {
            Console.Write(message + " ");
            return Console.ReadLine().Trim().ToLower() == "y";
        }
    }
}
