using System.Collections.Generic;
using System.Linq;
using Models;

namespace Gift
{
    public class GiftBox
    {
        private List<Candy> _candies = new List<Candy>();

        public void AddCandy(Candy candy)
        {
            _candies.Add(candy);
        }

        public double TotalWeight()
        {
            return _candies.Sum(c => c.Weight);
        }

        public List<Candy> SortBySugarContent()
        {
            return _candies.OrderBy(c => c.SugarContent).ToList();
        }
        public List<Candy> SortByWeight()
        {
            return _candies.OrderBy(c => c.Weight).ToList();
        }
        public List<Candy> SortByName()
        {
            return _candies.OrderBy(c => c.Name).ToList();
        }

        public List<Candy> FindCandiesBySugarRange(double min, double max)
        {
            return _candies.Where(c => c.SugarContent >= min && c.SugarContent <= max).ToList();
        }
    }
}