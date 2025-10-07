using System.Xml.Serialization;

namespace Lab3;

[Serializable]
public class Word : Token
{
    [XmlText]
    public string Value { get; set; }

    public Word() { }
    public Word(string v) { Value = v; }

    public override string Text => Value;

    public override string ToString() => Value;
}