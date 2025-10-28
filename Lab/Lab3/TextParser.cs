using System.Text;

namespace Lab3;

public class TextParser
{
    private readonly string input;
    private static readonly HashSet<char> PunctChars = new HashSet<char> {
        '.', ',', ';', ':', '?', '!', '—', '-', '(', ')', '[', ']', '{', '}', '«', '»', '"', '\'', '…'
    };

    private static readonly HashSet<char> SentenceTerminators = new HashSet<char> { '.', '?', '!' };

    public TextParser(string text)
    {
        input = text ?? string.Empty;
    }

    public Text Parse()
    {
        var text = new Text();
        var sb = new StringBuilder();
        var currentSentence = new Sentence();

        int i = 0;
        while (i < input.Length)
        {
            char ch = input[i];

            if (char.IsWhiteSpace(ch))
            {
                if (sb.Length > 0)
                {
                    var token = new Word(sb.ToString());
                    currentSentence.Add(token);
                    sb.Clear();
                }
                i++;
                continue;
            } 

            if (PunctChars.Contains(ch))
            {
                if (sb.Length > 0)
                {
                    currentSentence.Add(new Word(sb.ToString()));
                    sb.Clear();
                }

                var pSb = new StringBuilder();
                while (i < input.Length && PunctChars.Contains(input[i]))
                {
                    pSb.Append(input[i]);
                    i++;
                }
                var punct = new Punctuation(pSb.ToString());
                currentSentence.Add(punct);

                if (pSb.ToString().Any(c => SentenceTerminators.Contains(c)))
                {
                    text.Add(currentSentence);
                    currentSentence = new Sentence();
                }

                continue;
            }

            sb.Append(ch);
            i++;
        }

        if (sb.Length > 0)
        {
            currentSentence.Add(new Word(sb.ToString()));
            sb.Clear();
        }
        if (currentSentence.Tokens.Count > 0)
            text.Add(currentSentence);

        return text;
    }
}