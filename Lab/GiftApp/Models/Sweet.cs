namespace Models
{
    public abstract class Sweet
    {
        public string Name { get; }
        public double Weight { get; }

        protected Sweet(string name, double weight)
        {
            Name = name;
            Weight = weight;
        }
    }
}