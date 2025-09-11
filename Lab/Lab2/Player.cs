namespace Lab2;

public class Player
{
    public string Name { get; set; }
    public int Position { get; private set; }
    public int Distance { get; private set; }
    public bool InGame { get; private set; }

    public Player(string name)
    {
        Name = name;
        Position = -1; // Игрок не в игре изначально
        Distance = 0;
        InGame = false;
    }

    public void Move(int moveAmount, int totalCells)
    {
        if (!InGame)
        {
            Position = moveAmount;
            InGame = true;
            return;
        }

        int newPosition = Position + moveAmount;

        // Обработка выхода за пределы
        if (newPosition >= totalCells)
        {
            newPosition %= totalCells;
        }
        else if (newPosition < 0)
        {
            newPosition = totalCells + (newPosition % totalCells);
        }

        Position = newPosition;
    }

    public void setDistance(int number)
    {
        
        Distance+=Math.Abs(number);
    }
}