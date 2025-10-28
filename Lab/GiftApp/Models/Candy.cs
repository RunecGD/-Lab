namespace Models
{
    public abstract class Candy
    {
        public string Name { get; }
        public double Weight { get; }
        public double SugarContent { get; }

        protected Candy(string name, double weight, double sugarContent)
        {
            Name = name;
            Weight = weight;
            SugarContent = sugarContent;
        }

        public override string ToString()
        {
            return $"{Name} (Weight: {Weight}g, Sugar: {SugarContent}g)";
        }
    }
}