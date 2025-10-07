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
        foreach (var s in Sentences)
        {
            if (s.IsQuestion())
            {
                foreach (var w in s.Words)
                {
                    if (GetUnicodeLength(w.Value) == length)
                        set.Add(w.Value);
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
        foreach (var s in Sentences)
        {
            var ns = new Sentence();
            foreach (var t in s.Tokens)
            {
                if (t is Word w)
                {
                    if (GetUnicodeLength(w.Value) == length && StartsWithConsonant(w.Value))
                    {
                        continue;
                    }
                    else ns.Add(new Word(w.Value));
                }
                else if (t is Punctuation p)
                {
                    ns.Add(new Punctuation(p.Symbol));
                }
            }
            result.Add(ns);
        }
        return result;
    }

    public void ReplaceWordsInSentence(int sentenceIndex, int wordLength, string replacement)
    {
        if (sentenceIndex < 0 || sentenceIndex >= Sentences.Count)
            throw new ArgumentOutOfRangeException(nameof(sentenceIndex));

        var s = Sentences[sentenceIndex];
        for (int i = 0; i < s.Tokens.Count; i++)
        {
            if (s.Tokens[i] is Word w)
            {
                if (GetUnicodeLength(w.Value) == wordLength)
                {
                    s.Tokens[i] = new Word(replacement);
                }
            }
        }
    }

    public Text RemoveStopWords(IEnumerable<string> stopWords)
    {
        var set = new HashSet<string>(stopWords.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()), StringComparer.OrdinalIgnoreCase);
        var result = new Text();
        foreach (var s in Sentences)
        {
            var ns = new Sentence();
            foreach (var t in s.Tokens)
            {
                if (t is Word w)
                {
                    if (!set.Contains(w.Value))
                        ns.Add(new Word(w.Value));
                }
                else ns.Add(new Punctuation(((Punctuation)t).Symbol));
            }
            result.Add(ns);
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

    static int GetUnicodeLength(string s)
    {
        return s.Length;
    }
}