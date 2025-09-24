using System;
using System.IO;

namespace Lab2;

public class Game
{
    public static Player Cat { get; private set; }
    public static Player Mouse { get; private set; }
    private static int totalCells;

    public Game(int cells, string fileExit)
    {
        totalCells = cells;
        Cat = new Player("Cat");
        Mouse = new Player("Mouse");
        File.WriteAllText(fileExit, "Cat and Mouse\nCat Mouse Distance\n------------------\n"); // Очищаем файл при старте
    }

    public static void StepCheck(string input)
    {
        var character = input[0];
        int number;

        try
        {
            number = Convert.ToInt32(input.Substring(1).Trim());
        }
        catch (FormatException)
        {
            return; 
        }
        catch (OverflowException)
        {
            return; 
        }

        switch (character)
        {
            case 'C':
            {
                DoMove(Cat, totalCells, number);
                if (Cat.State == PlayerState.InGame)
                {
                    Cat.setDistance(number);
                }

                Cat.Move(number, totalCells);
                break;
            }
            case 'M':
            {
                if (Mouse.State == PlayerState.InGame)
                {
                    Mouse.setDistance(number);
                }
                Mouse.Move(number, totalCells);
                break;
            }
        }
    }

    public static void PrintStatus(string fileExit)
    {
        string catPosition = Cat.State == PlayerState.InGame ? Cat.Position.ToString() : "??";
        string mousePosition = Mouse.State == PlayerState.InGame ? Mouse.Position.ToString() : "??";
        int distance = 0;

        if (Cat.State == PlayerState.NotInGame || Mouse.State == PlayerState.NotInGame)
        {
            File.AppendAllText(fileExit, $"{catPosition}     {mousePosition}      \n");
        }
        else
        {
            distance = Math.Abs(Cat.Position - Mouse.Position);
            File.AppendAllText(fileExit, $"{catPosition}     {mousePosition}      {distance}\n");


        }
    }

    public static bool IsCaught()
    {
        return Cat.Position == Mouse.Position;
    }

    private static void DoMove(Player player, int totalCells, int number)
    {
        if (Cat.State == PlayerState.InGame)
        {
            Cat.setDistance(number);
        }

        Cat.Move(number, totalCells);
    }
}