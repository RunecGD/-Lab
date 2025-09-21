using System;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.StartGame("/home/german/IdeaProjects/projectC#/Lab/Lab2/ChaseData.txt");
        }
    }
}