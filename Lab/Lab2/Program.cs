using System;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.StartGame("ChaseData.txt", "PursuitLog.txt");
            gameManager.StartGame("ChaseData1.txt", "PursuitLog1.txt");
            gameManager.StartGame("ChaseData2.txt", "PursuitLog2.txt");
        }
    }
}