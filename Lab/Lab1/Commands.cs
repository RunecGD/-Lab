using System.Text;

namespace Lab1;

public static class Commands{
    
    public static void ProcessCommand(StreamWriter writer, string rawLine, List<GeneticData> geneticData,
        ref int operationIndex)
    {
        var trimmedLine = rawLine.TrimEnd('\r', '\n');
        var commandParts = trimmedLine.Split('\t');
        var operationType = commandParts[0].Trim();

        var operationNumber = operationIndex.ToString().PadLeft(3, '0');
        writer.WriteLine();
        writer.WriteLine(new string('-', 40));
        writer.WriteLine($"{operationNumber}\t{trimmedLine}");

        switch (operationType.ToLower())
        {
            case "search" when commandParts.Length >= 2:
                HandleSearch(writer, commandParts[1], geneticData);
                break;
            case "diff" when commandParts.Length >= 3:
                HandleDiff(writer, commandParts[1], commandParts[2], geneticData);
                break;
            case "mode" when commandParts.Length >= 2:
                HandleMode(writer, commandParts[1], geneticData);
                break;
        }

        operationIndex++;
    }

    static void HandleSearch(StreamWriter writer, string encodedQuery, List<GeneticData> geneticData)
    {
        var decodedQuery = RLDecoding(encodedQuery);
        var foundResults = geneticData
            .Where(g => g.amino_acids.Contains(decodedQuery))
            .Select(g => (g.organism, g.protein))
            .ToList();

        if (foundResults.Count == 0)
        {
            writer.WriteLine("NOT FOUND");
            return;
        }

        foreach (var result in foundResults)
            writer.WriteLine($"{result.organism}\t{result.protein}");
    }

    static void HandleDiff(StreamWriter writer, string proteinName1, string proteinName2, List<GeneticData> geneticData)
    {
        var geneticData1 = geneticData.FirstOrDefault(x => x.protein == proteinName1);
        var geneticData2 = geneticData.FirstOrDefault(x => x.protein == proteinName2);

        var isProtein1Missing =
            geneticData1.Equals(default(GeneticData)) || string.IsNullOrEmpty(geneticData1.protein);
        var isProtein2Missing =
            geneticData2.Equals(default(GeneticData)) || string.IsNullOrEmpty(geneticData2.protein);

        if (isProtein1Missing || isProtein2Missing)
        {
            var missingProteins = new List<string>();
            if (isProtein1Missing)
                missingProteins.Add(proteinName1);
            if (isProtein2Missing)
                missingProteins.Add(proteinName2);

            writer.WriteLine($"amino-acids difference: MISSING: {string.Join(" ", missingProteins)}");
            return;
        }

        var differenceCount = AminoDifference(geneticData1.amino_acids, geneticData2.amino_acids);
        writer.WriteLine($"amino-acids difference: {differenceCount}");
    }

    static void HandleMode(StreamWriter writer, string proteinName, List<GeneticData> geneticData)
    {
        var geneticDataItem = geneticData.FirstOrDefault(x => x.protein == proteinName);

        if (geneticDataItem.Equals(default(GeneticData)) || string.IsNullOrEmpty(geneticDataItem.protein))
        {
            writer.WriteLine($"amino-acid occurs: MISSING: {proteinName}");
            return;
        }

        var (aminoAcid, occurrenceCount) = ModeAminoAcid(geneticDataItem.amino_acids);
        writer.WriteLine($"amino-acid occurs: {aminoAcid} {occurrenceCount}");
    }

    public static List<GeneticData> LoadSequences(string filePath)
    {
        var geneticDataList = new List<GeneticData>();
        foreach (var rawLine in File.ReadLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(rawLine))
                continue;

            var lineParts = rawLine.Split('\t');
            if (lineParts.Length < 3)
                continue;

            var geneticData = new GeneticData
            {
                protein = lineParts[0],
                organism = lineParts[1],
                amino_acids = RLDecoding(lineParts[2])
            };
            geneticDataList.Add(geneticData);
        }

        return geneticDataList;
    }

    private static string RLDecoding(string encodedString)
    {
        if (string.IsNullOrEmpty(encodedString))
            return encodedString;

        var decodedBuilder = new StringBuilder();
        for (var currentIndex = 0; currentIndex < encodedString.Length; currentIndex++)
        {
            var currentChar = encodedString[currentIndex];

            if (!char.IsDigit(currentChar))
            {
                decodedBuilder.Append(currentChar);
                continue;
            }

            var repeatCount = currentChar - '0';
            if (currentIndex + 1 >= encodedString.Length)
            {
                decodedBuilder.Append(currentChar);
                continue;
            }

            var aminoAcid = encodedString[++currentIndex];
            decodedBuilder.Append(aminoAcid, repeatCount);
        }

        return decodedBuilder.ToString();
    }

    static int AminoDifference(string aminoAcids1, string aminoAcids2)
    {
        var maxLength = Math.Max(aminoAcids1.Length, aminoAcids2.Length);
        var differenceCount = 0;

        for (var position = 0; position < maxLength; position++)
        {
            var aminoAcid1 = position < aminoAcids1.Length ? aminoAcids1[position] : '\0';
            var aminoAcid2 = position < aminoAcids2.Length ? aminoAcids2[position] : '\0';

            if (aminoAcid1 != aminoAcid2)
                differenceCount++;
        }

        return differenceCount;
    }

    static (char aminoAcid, int count) ModeAminoAcid(string aminoAcidSequence)
    {
        if (string.IsNullOrEmpty(aminoAcidSequence))
            return ('?', 0);

        var aminoAcidCounts = new Dictionary<char, int>();
        foreach (var aminoAcid in aminoAcidSequence)
        {
            aminoAcidCounts.TryGetValue(aminoAcid, out var currentCount);
            aminoAcidCounts[aminoAcid] = currentCount + 1;
        }

        var maxCount = aminoAcidCounts.Values.Max();
        var mostFrequentAminoAcids = aminoAcidCounts
            .Where(entry => entry.Value == maxCount)
            .Select(entry => entry.Key)
            .ToList();

        mostFrequentAminoAcids.Sort();
        return (mostFrequentAminoAcids[0], maxCount);
    }
}