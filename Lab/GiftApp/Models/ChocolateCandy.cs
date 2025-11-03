public class ChocolateCandy : Candy
{
    public ChocolateCandy() { }  // Параметрный конструктор без параметров

    public ChocolateCandy(string name, double weight, double sugarContent)
        : base(name, weight, sugarContent) { }

    public override string GetCandyInfo()
    {
        return $"{Name} (Шоколадная, Вес: {Weight}g, Сахар: {SugarContent}g)";
    }
}