namespace Lab2;

public class GameManager
{
    private Game game;

    public void StartGame(string filePath, string fileExit)
    {
        var lines = File.ReadAllLines(filePath);

        int totalCells;

        try
        {
            totalCells = Convert.ToInt32(lines[0]);
        }
        catch (FormatException)
        {
            File.AppendAllText(fileExit,
                "Ошибка: первая строка должна содержать положительное число, определяющее длину прямой.\n");
            return;
        }
        catch (OverflowException)
        {
            File.AppendAllText(fileExit, "Ошибка: число слишком велико или мало.\n");
            return;
        }

        if (totalCells <= 0 || totalCells > 10000)
        {
            File.AppendAllText(fileExit,
                "Ошибка: первая строка должна содержать положительное число от 1 до 10000.\n");
            return;
        }

        game = new Game(totalCells, fileExit);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            if (line.StartsWith("P"))
            {
                if (Game.IsCaught())
                {
                    File.AppendAllText(fileExit, "------------------\n\n" +
                                                         "Distance traveled:   Mouse    Cat\n" +
                                                         $"                      {Game.Mouse.Distance}       {Game.Cat.Distance}\n\n" +
                                                         $"Mouse caught at: {Game.Mouse.Position}\n");
                    return;
                }

                Game.PrintStatus(fileExit);
            }
            else
            {
                Game.StepCheck(line);
            }
        }

        File.AppendAllText(fileExit, "------------------\n\n" +
                                             "Distance traveled:   Mouse    Cat\n" +
                                             $"                      {Game.Mouse.Distance}       {Game.Cat.Distance}\n");
        File.AppendAllText(fileExit, "Mouse evaded Cat\n");
    }
}