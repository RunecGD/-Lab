using System;
using System.Collections.Generic;
using Gift;
using Models;

namespace Services
{
    public class GiftService
    {
        private List<Candy> _availableCandies = new List<Candy>
        {
            new ChocolateCandy("Mars", 50, 30),
            new HardCandy("Lollipop", 20, 10),
            new ChocolateCandy("Snickers", 45, 25),
            new HardCandy("Candy Cane", 15, 5)
        };

        public void CreateGift()
        {
            var giftBox = new GiftBox();

            Console.WriteLine("Доступные конфеты:");
            for (int i = 0; i < _availableCandies.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_availableCandies[i]}");
            }

            Console.WriteLine("Выберите конфеты для подарка (введите номера через запятую):");
            var input = Console.ReadLine();
            var indices = input.Split(',');

            foreach (var index in indices)
            {
                if (int.TryParse(index.Trim(), out int idx) && idx > 0 && idx <= _availableCandies.Count)
                {
                    giftBox.AddCandy(_availableCandies[idx - 1]);
                }
                else
                {
                    Console.WriteLine($"Недопустимый номер конфеты: {index}");
                }
            }

            Console.WriteLine($"Общий вес подарка: {giftBox.TotalWeight()}g");

            Console.WriteLine("Выберите параметр сортировки (1 - по содержанию сахара, 2 - по весу, 3 - по названию):");
            var sortOption = Console.ReadLine();

            List<Candy> sortedCandies;

            switch (sortOption)
            {
                case "1":
                    sortedCandies = giftBox.SortBySugarContent();
                    Console.WriteLine("Конфеты отсортированы по содержанию сахара:");
                    break;
                case "2":
                    sortedCandies = giftBox.SortByWeight();
                    Console.WriteLine("Конфеты отсортированы по весу:");
                    break;
                case "3":
                    sortedCandies = giftBox.SortByName();
                    Console.WriteLine("Конфеты отсортированы по названию:");
                    break;
                default:
                    Console.WriteLine("Недопустимый выбор сортировки.");
                    return;
            }

            foreach (var candy in sortedCandies)
            {
                Console.WriteLine(candy);
            }

            Console.WriteLine("Введите диапазон содержания сахара (min и max, через пробел):");
            var rangeInput = Console.ReadLine().Split(' ');
            if (rangeInput.Length == 2 &&
                double.TryParse(rangeInput[0], out double min) &&
                double.TryParse(rangeInput[1], out double max))
            {
                var foundCandies = giftBox.FindCandiesBySugarRange(min, max);
                Console.WriteLine("Конфеты с содержанием сахара в указанном диапазоне:");
                foreach (var candy in foundCandies)
                {
                    Console.WriteLine(candy);
                }
            }
            else
            {
                Console.WriteLine("Недопустимый ввод диапазона.");
            }
        }
    }
}