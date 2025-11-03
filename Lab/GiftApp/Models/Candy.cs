using System.Xml.Serialization;

[XmlInclude(typeof(ChocolateCandy))]
[XmlInclude(typeof(HardCandy))]
public abstract class Candy  // Объявляем класс абстрактным
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Weight")]
    public double Weight { get; set; }

    [XmlElement("SugarContent")]
    public double SugarContent { get; set; }

    protected Candy() { }  // Параметрный конструктор без параметров

    protected Candy(string name, double weight, double sugarContent)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Weight = weight;
        SugarContent = sugarContent;
    }

    public abstract string GetCandyInfo();  // Абстрактный метод

    public override string ToString()
    {
        return GetCandyInfo();
    }
}