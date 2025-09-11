using System;

namespace Lab2;

public class Game
{
    public static Player Cat { get; private set; }
    public static Player Mouse { get; private set; }
    private static int totalCells;
    public static bool noFirstCat, noFirstMouse;

    public Game(int cells)
    {
        totalCells = cells;
        Cat = new Player("Cat");
        Mouse = new Player("Mouse");
    }

    public static void StepCheck(string input)
    {
        char character = input[0];
        if (int.TryParse(input.Substring(1).Trim(), out int number))
        {
            if (character == 'C')
            {
                if (noFirstCat)
                {
                    Cat.setDistance(number);
                }

                noFirstCat = true;
                Cat.Move(number, totalCells);
            }
            else if (character == 'M')
            {
                if (noFirstMouse)
                {
                    Mouse.setDistance(number);

                }
                Mouse.Move(number, totalCells);
                noFirstMouse = true;
            }
        }
    }

    public static void PrintStatus()
    {
        string catPosition = Cat.InGame ? Cat.Position.ToString() : "??";
        string mousePosition = Mouse.InGame ? Mouse.Position.ToString() : "??";
        int distance = Math.Abs(Cat.Position - Mouse.Position);
        Console.WriteLine($"{catPosition}     {mousePosition}      {distance}");
    }

    public static bool IsCaught()
    {
        return Cat.Position == Mouse.Position;
    }
}