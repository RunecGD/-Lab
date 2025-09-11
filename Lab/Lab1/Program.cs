using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lab1;

class Program
{
    static void Main(string[] args)
    {
        const string sequencesFileName = "sequences2.txt";
        const string commandsFileName = "commands2.txt";
        const string outputFileName = "genedata.txt";

        if (!File.Exists(sequencesFileName) || !File.Exists(commandsFileName))
        {
            Console.WriteLine($"Не найдены входные файлы '{sequencesFileName}' или '{commandsFileName}' в текущей папке.");
            return;
        }

        List<GeneticData> geneticDataCollection = LoadSequences(sequencesFileName);

        using (var outputWriter = new StreamWriter(outputFileName, false, Encoding.UTF8))
        {
            outputWriter.WriteLine("Ваше имя"); // замените на своё имя если нужно
            outputWriter.WriteLine("Генетический поиск");

            int operationIndex = 1;
            foreach (var rawLine in File.ReadLines(commandsFileName))
            {
                if (string.IsNullOrWhiteSpace(rawLine)) continue;
                
                var trimmedLine = rawLine.TrimEnd('\r','\n');
                var commandParts = trimmedLine.Split('\t');
                var operationType = commandParts[0].Trim();

                string operationNumber = operationIndex.ToString().PadLeft(3, '0');
                outputWriter.WriteLine();
                outputWriter.WriteLine(operationNumber + "\t" + trimmedLine);
                outputWriter.WriteLine(new string('-', 40));

                if (operationType.Equals("search", StringComparison.OrdinalIgnoreCase))
                {
                    if (commandParts.Length < 2)
                    {
                        outputWriter.WriteLine("INVALID COMMAND");
                    }
                    else
                    {
                        string encodedQuery = commandParts[1];
                        string decodedQuery = RLDecoding(encodedQuery);

                        var foundResults = new List<(string organism, string protein)>();
                        foreach (var geneticData in geneticDataCollection)
                        {
                            if (geneticData.amino_acids.Contains(decodedQuery))
                                foundResults.Add((geneticData.organism, geneticData.protein));
                        }

                        if (foundResults.Count == 0)
                            outputWriter.WriteLine("NOT FOUND");
                        else
                        {
                            foreach (var result in foundResults)
                                outputWriter.WriteLine(result.organism + "\t" + result.protein);
                        }
                    }
                }
                else if (operationType.Equals("diff", StringComparison.OrdinalIgnoreCase))
                {
                    if (commandParts.Length < 3)
                    {
                        outputWriter.WriteLine("INVALID COMMAND");
                    }
                    else
                    {
                        string proteinName1 = commandParts[1];
                        string proteinName2 = commandParts[2];

                        var geneticData1 = geneticDataCollection.FirstOrDefault(x => x.protein == proteinName1);
                        var geneticData2 = geneticDataCollection.FirstOrDefault(x => x.protein == proteinName2);

                        bool isProtein1Missing = string.IsNullOrEmpty(geneticData1.protein);
                        bool isProtein2Missing = string.IsNullOrEmpty(geneticData2.protein);

                        if (isProtein1Missing || isProtein2Missing)
                        {
                            outputWriter.Write("amino-acids difference: ");
                            outputWriter.WriteLine("MISSING:" + (isProtein1Missing ? " " + proteinName1 : "") + (isProtein2Missing ? " " + proteinName2 : ""));
                        }
                        else
                        {
                            int differenceCount = AminoDifference(geneticData1.amino_acids, geneticData2.amino_acids);
                            outputWriter.WriteLine("amino-acids difference: " + differenceCount);
                        }
                    }
                }
                else if (operationType.Equals("mode", StringComparison.OrdinalIgnoreCase))
                {
                    if (commandParts.Length < 2)
                    {
                        outputWriter.WriteLine("INVALID COMMAND");
                    }
                    else
                    {
                        string proteinName = commandParts[1];
                        var geneticData = geneticDataCollection.FirstOrDefault(x => x.protein == proteinName);
                        if (string.IsNullOrEmpty(geneticData.protein))
                        {
                            outputWriter.Write("amino-acid occurs: ");
                            outputWriter.WriteLine("MISSING: " + proteinName);
                        }
                        else
                        {
                            var (aminoAcid, occurrenceCount) = ModeAminoAcid(geneticData.amino_acids);
                            outputWriter.WriteLine("amino-acid occurs: " + aminoAcid + " " + occurrenceCount);
                        }
                    }
                }
                else
                {
                    outputWriter.WriteLine("UNKNOWN OPERATION: " + operationType);
                }

                operationIndex++;
            }
        }

        Console.WriteLine($"Готово. Результат записан в '{outputFileName}'");
    }

    static List<GeneticData> LoadSequences(string filePath)
    {
        var geneticDataList = new List<GeneticData>();
        foreach (var rawLine in File.ReadLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(rawLine)) continue;
            var lineParts = rawLine.Split('\t');
            if (lineParts.Length < 3) continue;
            
            var geneticData = new GeneticData();
            geneticData.protein = lineParts[0];
            geneticData.organism = lineParts[1];
            geneticData.amino_acids = RLDecoding(lineParts[2]);
            geneticDataList.Add(geneticData);
        }
        return geneticDataList;
    }

    static string RLDecoding(string encodedString)
    {
        if (string.IsNullOrEmpty(encodedString)) return encodedString;
        var decodedBuilder = new StringBuilder();
        for (int currentIndex = 0; currentIndex < encodedString.Length; currentIndex++)
        {
            char currentChar = encodedString[currentIndex];
            if (char.IsDigit(currentChar))
            {
                int repeatCount = currentChar - '0';
                if (currentIndex + 1 < encodedString.Length)
                {
                    char aminoAcid = encodedString[currentIndex + 1];
                    for (int repetition = 0; repetition < repeatCount; repetition++) 
                        decodedBuilder.Append(aminoAcid);
                    currentIndex++; // пропускаем букву
                }
                else
                {
                    decodedBuilder.Append(currentChar);
                }
            }
            else
            {
                decodedBuilder.Append(currentChar);
            }
        }
        return decodedBuilder.ToString();
    }

    static string RLEncoding(string decodedString)
    {
        if (string.IsNullOrEmpty(decodedString)) return decodedString;
        var encodedBuilder = new StringBuilder();
        int currentIndex = 0;
        while (currentIndex < decodedString.Length)
        {
            char currentAminoAcid = decodedString[currentIndex];
            int nextIndex = currentIndex + 1;
            while (nextIndex < decodedString.Length && decodedString[nextIndex] == currentAminoAcid && nextIndex - currentIndex < 9) 
                nextIndex++;
            
            int runLength = nextIndex - currentIndex;
            if (runLength >= 3)
            {
                encodedBuilder.Append(runLength);
                encodedBuilder.Append(currentAminoAcid);
            }
            else
            {
                for (int repetition = 0; repetition < runLength; repetition++) 
                    encodedBuilder.Append(currentAminoAcid);
            }
            currentIndex = nextIndex;
        }
        return encodedBuilder.ToString();
    }

    static int AminoDifference(string aminoAcids1, string aminoAcids2)
    {
        int maxLength = Math.Max(aminoAcids1.Length, aminoAcids2.Length);
        int differenceCount = 0;
        for (int position = 0; position < maxLength; position++)
        {
            char aminoAcid1 = position < aminoAcids1.Length ? aminoAcids1[position] : '\0';
            char aminoAcid2 = position < aminoAcids2.Length ? aminoAcids2[position] : '\0';
            if (aminoAcid1 != aminoAcid2) differenceCount++;
        }
        return differenceCount;
    }

    static (char aminoAcid, int count) ModeAminoAcid(string aminoAcidSequence)
    {
        var aminoAcidCounts = new Dictionary<char, int>();
        foreach (char aminoAcid in aminoAcidSequence)
        {
            if (!aminoAcidCounts.ContainsKey(aminoAcid)) aminoAcidCounts[aminoAcid] = 0;
            aminoAcidCounts[aminoAcid]++;
        }
        
        if (aminoAcidCounts.Count == 0) return ('?', 0);
        
        int maxCount = aminoAcidCounts.Values.Max();
        var mostFrequentAminoAcids = aminoAcidCounts
            .Where(entry => entry.Value == maxCount)
            .Select(entry => entry.Key)
            .ToList();
            
        mostFrequentAminoAcids.Sort();
        return (mostFrequentAminoAcids[0], maxCount);
    }
}