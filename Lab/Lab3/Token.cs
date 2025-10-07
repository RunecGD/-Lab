using System.Xml.Serialization;

namespace Lab3;


[Serializable]
[XmlInclude(typeof(Word))]
[XmlInclude(typeof(Punctuation))]
public abstract class Token
{
    [XmlIgnore]
    public abstract string Text { get; }
}