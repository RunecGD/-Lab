using System.Text;
using Lab1;
using Lab2;

class Program
{
    static void Main(string[] args)
    {
        const string sequencesFileName = "sequences2.txt";
        const string commandsFileName = "commands2.txt";
        const string outputFileName = "genedata.txt";

        if (!File.Exists(sequencesFileName) || !File.Exists(commandsFileName))
        {
            Console.WriteLine(
                $"Не найдены входные файлы '{sequencesFileName}' или '{commandsFileName}' в текущей папке.");
            return;
        }

        List<GeneticData> geneticDataCollection = Commands.LoadSequences(sequencesFileName);

        using var outputWriter = new StreamWriter(outputFileName, false, Encoding.UTF8);
        outputWriter.WriteLine("Herman Runec");
        outputWriter.WriteLine("Genetic search");

        int operationIndex = 1;
        foreach (var rawLine in File.ReadLines(commandsFileName))
        {
            if (string.IsNullOrWhiteSpace(rawLine))
                continue;

            Commands.ProcessCommand(outputWriter, rawLine, geneticDataCollection, ref operationIndex);
        }

        Console.WriteLine($"Готово. Результат записан в '{outputFileName}'");
    }

}
