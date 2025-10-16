using System.Text;
using System.Xml.Serialization;
using Lab3;

[Serializable]
[XmlRoot("text")]
public class Text
{
    [XmlElement("sentence")]

    public List<Sentence> Sentences { get; set; } = new List<Sentence>();

    public Text() { }

    public void Add(Sentence s) => Sentences.Add(s);

    // 1. записать все предложения в порядке возрастания количества слов
    public void WriteSentencesByWordCount(string path)
    {
        var ordered = Sentences.OrderBy(s => s.WordCount);
        File.WriteAllLines(path, ordered.Select(s => s.ToString()));
    }

    // 2. записать по возрастанию длины предложения (символы)
    public void WriteSentencesByLength(string path)
    {
        var ordered = Sentences.OrderBy(s => s.Length);
        File.WriteAllLines(path, ordered.Select(s => s.ToString()));
    }

    public void WriteWordsOfLengthInQuestions(string path, int length)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var sentence in Sentences)
        {
            if (sentence.IsQuestion())
            {
                foreach (var word in sentence.Words)
                {
                    if (GetUnicodeLength(word.Value) == length)
                        set.Add(word.Value);
                }
            }
        }
        File.WriteAllLines(path, set);
    }

    // 4. Удалить из текста все слова заданной длины, начинающиеся с согласной буквы
    // Возвращаем новый Text (скопия) и записываем в файл результат как текст
    public Text RemoveWordsOfLengthStartingWithConsonant(int length)
    {
        var result = new Text();
        foreach (var sentence in Sentences)
        {
            var newSentence = new Sentence();
            foreach (var token in sentence.Tokens)
            {
                if (token is Word word)
                {
                    if (GetUnicodeLength(word.Value) == length && StartsWithConsonant(word.Value))
                    {
                        continue;
                    }
                    else newSentence.Add(new Word(word.Value));
                }
                else if (token is Punctuation punctuation)
                {
                    newSentence.Add(new Punctuation(punctuation.Symbol));
                }
            }
            result.Add(newSentence);
        }
        return result;
    }

    public void ReplaceWordsInSentence(int sentenceIndex, int wordLength, string replacement)
    {
        if (sentenceIndex < 0 || sentenceIndex >= Sentences.Count)
            throw new ArgumentOutOfRangeException(nameof(sentenceIndex));

        var sentence = Sentences[sentenceIndex];
        for (int i = 0; i < sentence.Tokens.Count; i++)
        {
            if (sentence.Tokens[i] is Word word)
            {
                if (GetUnicodeLength(word.Value) == wordLength)
                {
                    sentence.Tokens[i] = new Word(replacement);
                }
            }
        }
    }

    public Text RemoveStopWords(IEnumerable<string> stopWords)
    {
        var set = new HashSet<string>(stopWords.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()), StringComparer.OrdinalIgnoreCase);
        var result = new Text();
        foreach (var sentence in Sentences)
        {
            var newSentence = new Sentence();
            foreach (var token in sentence.Tokens)
            {
                if (token is Word word)
                {
                    if (!set.Contains(word.Value))
                        newSentence.Add(new Word(word.Value));
                }
                else newSentence.Add(new Punctuation(((Punctuation)token).Symbol));
            }
            result.Add(newSentence);
        }
        return result;
    }

    public void ExportToXml(string path)
    {
        var serializer = new XmlSerializer(typeof(Text));
        using var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, this);
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, Sentences.Select(s => s.ToString()));
    }

    static readonly char[] RussianVowels = "аеёиоуыэюяАЕЁИОУЫЭЮЯ".ToCharArray();
    static readonly char[] EnglishVowels = "aeiouyAEIOUY".ToCharArray();

    static bool StartsWithConsonant(string word)
    {
        if (string.IsNullOrEmpty(word)) return false;
        char c = word[0];
        if (!char.IsLetter(c)) return false;
        if (RussianVowels.Contains(c) || EnglishVowels.Contains(c)) return false;
        return true;
    }

    static int GetUnicodeLength(string sentence)
    {
        return sentence.Length;
    }
    public void WriteWordStatistics(string path)
    {
        var wordMap = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < Sentences.Count; i++)
        {
            var sentence = Sentences[i];
            foreach (var word in sentence.Words)
            {
                string cleaned = word.Value.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(cleaned))
                    continue;

                if (!wordMap.ContainsKey(cleaned))
                    wordMap[cleaned] = new List<int>();

                wordMap[cleaned].Add(i + 1); // добавляем номер предложения
            }
        }

        int maxWordLength = wordMap.Keys.Max(w => w.Length);
        var lines = new List<string>();

        foreach (var kvp in wordMap.OrderBy(k => k.Key))
        {
            string word = kvp.Key;
            int count = kvp.Value.Count; // сколько раз встретилось в тексте
            var sentenceList = kvp.Value.Distinct().OrderBy(x => x);
            string sentenceNums = string.Join(" ", sentenceList);

            int dotCount = Math.Max(1, (maxWordLength + 5) - word.Length);
            string dots = new string('.', dotCount);

            string line = $"{word}{dots}{count}: {sentenceNums}";
            lines.Add(line);
        }

        File.WriteAllLines(path, lines, Encoding.UTF8);
    }


}