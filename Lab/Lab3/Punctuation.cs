using System.Xml.Serialization;

namespace Lab3;

[Serializable]
public class Punctuation : Token
{
    [XmlText]
    public string Symbol { get; set; }

    public Punctuation() { }
    public Punctuation(string s) { Symbol = s; }

    public override string Text => Symbol;

    public override string ToString() => Symbol;
}