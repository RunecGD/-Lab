using System;
using System.IO;

namespace Lab2
{
    public class GameManager
    {
        private Game game;

        public void StartGame(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            if (!int.TryParse(lines[0], out int totalCells) || totalCells <= 0 || totalCells > 10000)
            {
                Console.WriteLine("Ошибка: первая строка должна содержать положительное число, определяющее длину прямой.");
                return;
            }

            game = new Game(totalCells);
            Console.WriteLine("Cat and Mouse\nCat Mouse Distance\n------------------");

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.StartsWith("P"))
                {
                    if (Game.IsCaught())
                    {
                        Console.WriteLine("------------------\n\n" +
                                          "Distance traveled:   Mouse    Cat\n" +
                                          $"                      {Game.Mouse.Distance}       {Game.Cat.Distance}\n\n" +
                                          $"Mouse caught at: {Game.Mouse.Position}");
                        return; 
                    }
                    Game.PrintStatus(); 
                }
                else
                {
                    Game.StepCheck(line); 
                }
            }

            Console.WriteLine("------------------\n\n" +
                              "Distance traveled:   Mouse    Cat\n" +
                              $"                      {Game.Mouse.Distance}       {Game.Cat.Distance}\n");
            Console.WriteLine("Mouse evaded Cat");
        }
    }
}