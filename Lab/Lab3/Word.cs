using System.Xml.Serialization;

namespace Lab3;

[Serializable]
public class Word : Token
{
    [XmlText] public string Value { get; set; }

    public Word()
    {
    }

    public Word(string value)
    {
        Value = value;
    }

    public override string Text
        => Value;

    public override string ToString()
        => Value;
}