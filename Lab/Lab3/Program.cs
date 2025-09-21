// See https://aka.ms/new-console-template for more information
namespace Lab3;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
    class Program
    {
        static void Main()
        {
            string filePath = "/home/german/IdeaProjects/projectC#/Lab/Lab3/Text.txt";
            Text parsedText = ParseFile(filePath);
        
            Console.WriteLine("Парсированный текст:");
            Console.WriteLine(parsedText);
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
    }