using System.Text;
using System.Xml.Serialization;
using Lab3;

[Serializable]
public class Sentence
{
    [XmlElement("word", typeof(Word))]
    [XmlElement("punctuation", typeof(Punctuation))]
    public List<Token> Tokens { get; set; } = new List<Token>();

    public Sentence() { }

    public void Add(Token t) => Tokens.Add(t);
    [XmlIgnore]
    public IEnumerable<Word> Words => Tokens.OfType<Word>();
    [XmlIgnore]
    public int WordCount => Words.Count();
    
    [XmlIgnore]
    public int Length => ToString().Length;
    
    [return: XmlIgnore]
    public bool IsQuestion()
    {
        for (int i = Tokens.Count - 1; i >= 0; i--)
        {
            var t = Tokens[i];
            if (t is Punctuation)
            {
                if (((Punctuation)t).Symbol.Contains("?")) return true;
                if (((Punctuation)t).Symbol.Trim().Length > 0 && ".!".Contains(((Punctuation)t).Symbol.Trim()[0])) return false;
            }
            else if (t is Word)
            {
                return false;
            }
        }
        return false;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var t in Tokens)
        {
            if (t is Word)
            {
                if (sb.Length > 0)
                {
                    char last = sb[sb.Length - 1];
                    if (!char.IsWhiteSpace(last) && last != '(' && last != '«' && last != '"' && last != '—')
                        sb.Append(' ');
                }
                sb.Append(t.Text);
            }
            else if (t is Punctuation)
            {
                sb.Append(t.Text);
            }
        }
        return sb.ToString().Trim();
    }
}