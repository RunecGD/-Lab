namespace Lab2;

public class Player
{
    public string Name { get; set; }
    public int Position { get; private set; }
    public int Distance { get; private set; }
    public PlayerState State { get; private set; }

    public Player(string name)
    {
        Name = name;
        Position = -1;
        Distance = 0;
        State = PlayerState.NotInGame; 
    }

    public void Move(int moveAmount, int totalCells)
    {
        if (State == PlayerState.NotInGame)
        {
            Position = moveAmount;
            State = PlayerState.InGame; 
            return;
        }

        int newPosition = Position + moveAmount;

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
        Distance += Math.Abs(number);
    }
}