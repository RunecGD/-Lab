using System;
using System.IO;

namespace Lab2;

class Program
{
    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("/home/german/IdeaProjects/projectC#/Lab/Lab2/ChaseData.txt");

        if (!int.TryParse(lines[0], out int totalCells) || totalCells <= 0 || totalCells > 10000)
        {
            Console.WriteLine("Ошибка: первая строка должна содержать положительное число, определяющее длину прямой.");
            return;
        }

        Game game = new Game(totalCells);
        Console.WriteLine("Cat and Mouse\nCat Mouse Distance\n------------------");

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            if (line.StartsWith("P"))
            {
                Game.PrintStatus(); // Печать статуса игры при команде P

                if (Game.IsCaught())
                {
                    Console.WriteLine("------------------\n\n" +
                                      "Distance traveled:   Mouse    Cat\n" +
                                      $"                      {Game.Mouse.Distance}       {Game.Cat.Distance}\n\n" +
                                      $"Mouse caught at: {Game.Mouse.Position}");
                    return; // Выходим из программы, если кот поймал мышь
                }
            }
            else
            {
                Game.StepCheck(line); // Обработка движения кота или мыши
            }
        }

        // Выводим пройденную дистанцию в любом случае, если игра не завершилась
        Console.WriteLine("------------------\n\n" +
                          "Distance traveled:   Mouse    Cat\n" +
                          $"                      {Game.Mouse.Distance}       {Game.Cat.Distance}\n");
        Console.WriteLine("Mouse evaded Cat");
    }
}