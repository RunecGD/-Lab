// See https://aka.ms/new-console-template for more information
namespace Lab3;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        string filePath = "/home/german/IdeaProjects/projectC#/Lab/Lab3/Text.txt";
        Text parsedText = ParseFile(filePath);

        Console.WriteLine("Парсированный текст:");
        Console.WriteLine(parsedText);

        // Вызов новых функций
        PrintSentencesByWordCount(parsedText);
        PrintSentencesByLength(parsedText);
        FindWordsInQuestions(parsedText, 4); // Пример длины слова
        RemoveWordsByLength(parsedText, 3); // Пример длины слова
    }

    static Text ParseFile(string filePath)
    {
        var result = new Text();

        try
        {
            string fileContent = File.ReadAllText(filePath);
            string pattern = @"(\w+|[.,!?;:])";

            List<string> tokens = new List<string>();
            foreach (Match match in Regex.Matches(fileContent, pattern))
            {
                tokens.Add(match.Value);
            }

            string sentence = string.Empty;
            foreach (var token in tokens)
            {
                sentence += token + " ";
                if (token.EndsWith(".") || token.EndsWith("!") || token.EndsWith("?"))
                {
                    result.AddSentence(new Sentence(sentence.Trim()));
                    sentence = string.Empty;
                }
            }

            if (!string.IsNullOrWhiteSpace(sentence))
            {
                result.AddSentence(new Sentence(sentence.Trim()));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
        }

        return result;
    }

    // Функция 1: Вывести все предложения в порядке возрастания количества слов
    static void PrintSentencesByWordCount(Text text)
    {
        var sortedSentences = text.Sentences.OrderBy(s => s.Content.Split(' ').Length);
        Console.WriteLine("\nПредложения по возрастанию количества слов:");
        foreach (var sentence in sortedSentences)
        {
            Console.WriteLine(sentence.Content);
        }
    }

    // Функция 2: Вывести все предложения в порядке возрастания длины предложения
    static void PrintSentencesByLength(Text text)
    {
        var sortedSentences = text.Sentences.OrderBy(s => s.Content.Length);
        Console.WriteLine("\nПредложения по возрастанию длины:");
        foreach (var sentence in sortedSentences)
        {
            Console.WriteLine(sentence.Content);
        }
    }

    // Функция 3: Найти слова заданной длины в вопросительных предложениях
    static void FindWordsInQuestions(Text text, int wordLength)
    {
        var wordsSet = new HashSet<string>();
        foreach (var sentence in text.Sentences)
        {
            if (sentence.Content.EndsWith("?"))
            {
                var words = sentence.Content.Split(' ');
                foreach (var word in words)
                {
                    if (word.Length == wordLength)
                    {
                        wordsSet.Add(word);
                    }
                }
            }
        }

        Console.WriteLine($"\nУникальные слова длины {wordLength} в вопросительных предложениях:");
        foreach (var word in wordsSet)
        {
            Console.WriteLine(word);
        }
    }

    // Функция 4: Удалить из текста все слова заданной длины, начинающиеся с согласной буквы
    static void RemoveWordsByLength(Text text, int wordLength)
    {
        foreach (var sentence in text.Sentences)
        {
            var words = sentence.Content.Split(' ')
                .Where(word => !(word.Length == wordLength && "бвгджзклмнпрстфхцчшщБВГДЖЗКЛМНПРСЕФХЦЧШЩ".Contains(word[0])))
                .ToList();
            sentence.Content = string.Join(" ", words);
        }

        Console.WriteLine($"\nТекст после удаления слов длины {wordLength}, начинающихся с согласной буквы:");
        Console.WriteLine(text);
    }
}