using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Gift;

namespace Services
{
    public class GiftService
    {
        private List<Candy> _availableCandies;
        private List<GiftBox> _createdGifts;

        public GiftService()
        {
            _availableCandies = LoadCandies();
            _createdGifts = LoadGifts();
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\nГлавное меню:");
                Console.WriteLine("1. Создать подарок");
                Console.WriteLine("2. Посмотреть доступные сладости");
                Console.WriteLine("3. Добавить сладость");
                Console.WriteLine("4. Удалить сладость");
                Console.WriteLine("5. Посмотреть созданные подарки");
                Console.WriteLine("6. Сохранить сладости");
                Console.WriteLine("7. Сохранить подарки");
                Console.WriteLine("8. Выйти");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateGift();
                        break;
                    case "2":
                        ViewAvailableCandies();
                        break;
                    case "3":
                        AddCandy();
                        break;
                    case "4":
                        RemoveCandy();
                        break;
                    case "5":
                        ViewCreatedGifts();
                        break;
                    case "6":
                        SaveCandies();
                        break;
                    case "7":
                        SaveGifts();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Недопустимый выбор. Пожалуйста, попробуйте снова.");
                        break;
                }
            }
        }

        private void CreateGift()
        {
            var giftBox = new GiftBox();

            Console.WriteLine("Доступные конфеты:");
            ViewAvailableCandies();

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

            _createdGifts.Add(giftBox);
            Console.WriteLine($"Подарок создан с общим весом: {giftBox.TotalWeight()}g");
        }

        private void ViewAvailableCandies()
        {
            Console.WriteLine("Доступные сладости:");
            if (_availableCandies.Count == 0)
            {
                Console.WriteLine("Нет доступных сладостей");
                return;
            }
            for (int i = 0; i < _availableCandies.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_availableCandies[i]}");
            }
        }

        private void AddCandy()
        {
            Console.WriteLine("Введите название сладости:");
            var name = Console.ReadLine();

            Console.WriteLine("Введите вес сладости (в граммах):");
            if (!double.TryParse(Console.ReadLine(), out double weight))
            {
                Console.WriteLine("Недопустимый вес.");
                return;
            }

            Console.WriteLine("Введите содержание сахара (в граммах):");
            if (!double.TryParse(Console.ReadLine(), out double sugarContent))
            {
                Console.WriteLine("Недопустимое содержание сахара.");
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название сладости не может быть пустым.");
                return;
            }

            Console.WriteLine("Выберите тип сладости (1 - шоколадная конфета, 2 - твердая конфета):");
            var type = Console.ReadLine();

            Candy newCandy;
            switch (type)
            {
                case "1":
                    newCandy = new ChocolateCandy(name, weight, sugarContent);
                    break;
                case "2":
                    newCandy = new HardCandy(name, weight, sugarContent);
                    break;
                default:
                    Console.WriteLine("Недопустимый тип сладости.");
                    return;
            }

            _availableCandies.Add(newCandy);
            Console.WriteLine("Сладость добавлена.");
        }

        private void RemoveCandy()
        {
            Console.WriteLine("Введите номер сладости для удаления:");
            ViewAvailableCandies();

            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= _availableCandies.Count)
            {
                _availableCandies.RemoveAt(index - 1);
                Console.WriteLine("Сладость удалена.");
            }
            else
            {
                Console.WriteLine("Недопустимый номер сладости.");
            }
        }

        private void ViewCreatedGifts()
        {
            Console.WriteLine("Созданные подарки:");
            if (_createdGifts.Count == 0)
            {
                Console.WriteLine("Ни одного подарка не было создано");
                return;
            }
            for (int i = 0; i < _createdGifts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Подарок с весом { _createdGifts[i].TotalWeight()}g");
            }

            Console.WriteLine("Выберите подарок для дальнейших действий (введите номер):");
            if (int.TryParse(Console.ReadLine(), out int giftIndex) && giftIndex > 0 && giftIndex <= _createdGifts.Count)
            {
                ApplySortingAndSugarLimit(_createdGifts[giftIndex - 1]);
            }
            else
            {
                Console.WriteLine("Недопустимый номер подарка.");
            }
        }

        private void ApplySortingAndSugarLimit(GiftBox giftBox)
        {
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
                if (foundCandies.Count == 0)
                {
                    Console.WriteLine("Нет сладостей с сахаром в заданном диапазоне.");
                }
                else
                {
                    Console.WriteLine("Конфеты с содержанием сахара в указанном диапазоне:");
                    foreach (var candy in foundCandies)
                    {
                        Console.WriteLine(candy);
                    }
                }
            }
            else
            {
                Console.WriteLine("Недопустимый ввод диапазона.");
            }
        }

        private void SaveCandies()
        {
            SerializeToXml("candies.xml", _availableCandies);
            Console.WriteLine("Сладости сохранены в файл candies.xml.");
        }

        private List<Candy> LoadCandies()
        {
            return DeserializeFromXml<List<Candy>>("candies.xml") ?? new List<Candy>();
        }

        private List<GiftBox> LoadGifts()
        {
            return DeserializeFromXml<List<GiftBox>>("gifts.xml") ?? new List<GiftBox>();
        }

        private void SaveGifts()
        {
            SerializeToXml("gifts.xml", _createdGifts);
            Console.WriteLine("Подарки сохранены в файл gifts.xml.");
        }

        private void SerializeToXml<T>(string fileName, T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new StreamWriter(fileName);
            serializer.Serialize(writer, data);
        }

        private T DeserializeFromXml<T>(string fileName)
        {
            if (!File.Exists(fileName)) return default;

            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StreamReader(fileName);
            return (T)serializer.Deserialize(reader);
        }
    }
}