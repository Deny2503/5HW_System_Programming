using System.Text.RegularExpressions;

namespace _5HW_System_Programming
{
    internal class Program
    {
        static async Task Main()
        {
            while (true)
            {
                Console.WriteLine("Виберіть джерело тексту:");
                Console.WriteLine("1 - Зчитати з файлу");
                Console.WriteLine("2 - Ввести текст вручну");
                Console.WriteLine("0 - Вийти");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                string text = "";

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

                string report = await Task.Run(() =>
                    AnalyzeText(text, includeSent, includeWords, includeChar, includeQuest, includeExcl)
                );

                Console.WriteLine("\n=== ЗВІТ ===");
                Console.WriteLine(report);

                Console.Write("Зберегти звіт у файл? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.Write("Введіть шлях до файлу для збереження: ");
                    string savePath = Console.ReadLine();
                    await File.WriteAllTextAsync(savePath, report);
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

        static string AnalyzeText(string text, bool includeSent, bool includeWords,
                                  bool includeChar, bool includeQuest, bool includeExcl)
        {
            int sentCount = Regex.Matches(text, @"[.!?]").Count;
            int questCount = Regex.Matches(text, @"\?").Count;
            int exclCount = Regex.Matches(text, @"!").Count;
            int charCount = text.Length;
            int wordCount = Regex.Matches(text, @"\b\w+\b").Count;

            string report = "";

            if (includeSent)
                report += $"Кількість речень: {sentCount}\n";
            if (includeWords)
                report += $"Кількість слів: {wordCount}\n";
            if (includeChar)
                report += $"Кількість символів: {charCount}\n";
            if (includeQuest)
                report += $"Кількість питальних речень: {questCount}\n";
            if (includeExcl)
                report += $"Кількість окличних речень: {exclCount}\n";

            return report;
        }

    }
}
