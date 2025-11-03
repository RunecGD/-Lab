public class HardCandy : Candy
{
    public HardCandy() { }  // Параметрный конструктор без параметров

    public HardCandy(string name, double weight, double sugarContent)
        : base(name, weight, sugarContent) { }

    public override string GetCandyInfo()
    {
        return $"{Name} (Твердая, Вес: {Weight}g, Сахар: {SugarContent}g)";
    }
}