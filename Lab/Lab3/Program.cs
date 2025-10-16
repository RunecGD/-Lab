using System.Text;
using Lab3;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("Выберите язык текста для парсинга:");
        Console.WriteLine("1 - Русский");
        Console.WriteLine("2 - English");
        Console.Write("Ваш выбор: ");
        string? choice = Console.ReadLine();

        string inputPath=null;
        switch (choice)
        {
            case "1":
                inputPath = "input_ru.txt";
                if (!File.Exists(inputPath))
                {
                    File.WriteAllText(inputPath,
                        "Пример: Это первое предложение. Как тебя зовут? Это третий тест, с запятой!");
                    Console.WriteLine($"Файл {inputPath} не найден, создан пример.");
                }
                break;
            case "2":
                inputPath = "input_en.txt";
                if (!File.Exists(inputPath))
                {
                    File.WriteAllText(inputPath,
                        "Example: This is the first sentence. How are you? This is the third test, with a comma!");
                    Console.WriteLine($"File {inputPath} not found, created a sample.");
                }
                break;
            default:
                Console.WriteLine("Не верный ввод");
                return;
        }   

        string textContent = File.ReadAllText(inputPath, Encoding.UTF8);

        var parser = new TextParser(textContent);
        var text = parser.Parse();
        var text1 = text;
        text1.WriteSentencesByWordCount("output_by_wordcount.txt");
        var text2 = text;
        text2.WriteSentencesByLength("output_by_length.txt");
        var text3 = text;
        Console.Write("Введите длину для записи: ");
        int len = Convert.ToInt32(Console.ReadLine());
        text3.WriteWordsOfLengthInQuestions("output_question_words_len4.txt", len);
        
        Console.Write("Введите длину слов, которые нужно удалить (начинающиеся с согласной): ");
        var text4 = text;
        if (int.TryParse(Console.ReadLine(), out int removeLength))
        {
            var removed = text4.RemoveWordsOfLengthStartingWithConsonant(removeLength);
            File.WriteAllText($"output_removed_len{removeLength}_consonant.txt", removed.ToString(), Encoding.UTF8);
            Console.WriteLine($"Удалены слова длины {removeLength}, начинающиеся с согласной → output_removed_len{removeLength}_consonant.txt");
        }
        
        Console.Write("Введите номер предложения (начиная с 1), где нужно заменить слова: ");
        var text5 = text;
        var text6 = text;
        if (int.TryParse(Console.ReadLine(), out int sentenceNum))
        {
            Console.Write("Введите длину слов для замены: ");
            if (int.TryParse(Console.ReadLine(), out int replaceLength))
            {
                Console.Write("Введите строку для замены: ");
                string? replacement = Console.ReadLine();
                if (!string.IsNullOrEmpty(replacement))
                {
                    try
                    {
                        text5.ReplaceWordsInSentence(sentenceNum - 1, replaceLength, replacement);
                        File.WriteAllText($"output_replace_sentence{sentenceNum}_len{replaceLength}.txt",
                            text5.Sentences[sentenceNum - 1].ToString(), Encoding.UTF8);
                        Console.WriteLine($"Слова длины {replaceLength} заменены в предложении {sentenceNum} → output_replace_sentence{sentenceNum}_len{replaceLength}.txt");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при замене: " + ex.Message);
                    }
                }
            }
        }
        
        var stopWords = new List<string>();
        if (choice == "1" && File.Exists("stopwords_ru.txt"))
            stopWords.AddRange(File.ReadAllLines("stopwords_ru.txt", Encoding.UTF8));
        if (choice == "2" && File.Exists("stopwords_en.txt"))
            stopWords.AddRange(File.ReadAllLines("stopwords_en.txt", Encoding.UTF8));
        
        if (stopWords.Any())
        {
            var noStops = text6.RemoveStopWords(stopWords);
            File.WriteAllText("output_no_stopwords.txt", noStops.ToString(), Encoding.UTF8);
        }
        
        text.ExportToXml("output_text.xml");
        text.WriteWordStatistics("output_word_statistics.txt");
        Console.WriteLine("Записана статистика слов → output_word_statistics.txt");

        Console.WriteLine("Обработка завершена.");
    }
    
}
