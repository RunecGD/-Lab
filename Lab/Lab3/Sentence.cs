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
            var token = Tokens[i];
            if (token is Punctuation)
            {
                if (((Punctuation)token).Symbol.Contains("?")) return true;
                if (((Punctuation)token).Symbol.Trim().Length > 0 && ".!".Contains(((Punctuation)token).Symbol.Trim()[0])) return false;
            }
            else if (token is Word)
            {
                return false;
            }
        }
        return false;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var token in Tokens)
        {
            if (token is Word)
            {
                if (sb.Length > 0)
                {
                    char last = sb[sb.Length - 1];
                    if (!char.IsWhiteSpace(last) && last != '(' && last != '«' && last != '"' && last != '—')
                        sb.Append(' ');
                }
                sb.Append(token.Text);
            }
            else if (token is Punctuation)
            {
                sb.Append(token.Text);
            }
        }
        return sb.ToString().Trim();
    }
}