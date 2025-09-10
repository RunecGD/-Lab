using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

struct GeneticData
{
    public string protein;
    public string organism; 
    public string amino_acids; 
}

class Program
{
    static void Main(string[] args)
    {
        string sequencesFile = "sequences2.txt";
        string commandsFile = "commands2.txt";
        string outputFile = "genedata.txt";

        if (!File.Exists(sequencesFile) || !File.Exists(commandsFile))
        {
            Console.WriteLine($"Не найдены входные файлы '{sequencesFile}' или '{commandsFile}' в текущей папке.");
            return;
        }

        var data = LoadSequences(sequencesFile);

        using (var writer = new StreamWriter(outputFile, false, Encoding.UTF8))
        {
            writer.WriteLine("Ваше имя"); // замените на своё имя если нужно
            writer.WriteLine("Генетический поиск");

            int opIndex = 1;
            foreach (var raw in File.ReadLines(commandsFile))
            {
                if (string.IsNullOrWhiteSpace(raw)) continue;
                var line = raw.TrimEnd('\r','\n');
                var parts = line.Split('\t');
                var op = parts[0].Trim();

                string opnum = opIndex.ToString().PadLeft(3, '0');
                writer.WriteLine();
                writer.WriteLine(opnum + "\t" + line);
                writer.WriteLine(new string('-', 40));

                if (op.Equals("search", StringComparison.OrdinalIgnoreCase))
                {
                    if (parts.Length < 2)
                    {
                        writer.WriteLine("INVALID COMMAND");
                    }
                    else
                    {
                        string queryEncoded = parts[1];
                        string query = RLDecoding(queryEncoded);

                        var found = new List<(string organism, string protein)>();
                        foreach (var g in data)
                        {
                            if (g.amino_acids.Contains(query))
                                found.Add((g.organism, g.protein));
                        }

                        if (found.Count == 0)
                            writer.WriteLine("NOT FOUND");
                        else
                        {
                            foreach (var t in found)
                                writer.WriteLine(t.organism + "\t" + t.protein);
                        }
                    }
                }
                else if (op.Equals("diff", StringComparison.OrdinalIgnoreCase))
                {
                    if (parts.Length < 3)
                    {
                        writer.WriteLine("INVALID COMMAND");
                    }
                    else
                    {
                        string p1 = parts[1];
                        string p2 = parts[2];

                        var g1 = data.FirstOrDefault(x => x.protein == p1);
                        var g2 = data.FirstOrDefault(x => x.protein == p2);

                        bool missing1 = string.IsNullOrEmpty(g1.protein);
                        bool missing2 = string.IsNullOrEmpty(g2.protein);

                        if (missing1 || missing2)
                        {
                            writer.Write("amino-acids difference: ");
                            writer.WriteLine("MISSING:" + (missing1 ? " " + p1 : "") + (missing2 ? " " + p2 : ""));
                        }
                        else
                        {
                            int diff = AminoDifference(g1.amino_acids, g2.amino_acids);
                            writer.WriteLine("amino-acids difference: " + diff);
                        }
                    }
                }
                else if (op.Equals("mode", StringComparison.OrdinalIgnoreCase))
                {
                    if (parts.Length < 2)
                    {
                        writer.WriteLine("INVALID COMMAND");
                    }
                    else
                    {
                        string pname = parts[1];
                        var g = data.FirstOrDefault(x => x.protein == pname);
                        if (string.IsNullOrEmpty(g.protein))
                        {
                            writer.Write("amino-acid occurs: ");
                            writer.WriteLine("MISSING: " + pname);
                        }
                        else
                        {
                            var (aa, count) = ModeAminoAcid(g.amino_acids);
                            writer.WriteLine("amino-acid occurs: " + aa + " " + count);
                        }
                    }
                }
                else
                {
                    writer.WriteLine("UNKNOWN OPERATION: " + op);
                }

                opIndex++;
            }
        }

        Console.WriteLine($"Готово. Результат записан в '{outputFile}'");
    }

    static List<GeneticData> LoadSequences(string path)
    {
        var list = new List<GeneticData>();
        foreach (var raw in File.ReadLines(path))
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;
            var parts = raw.Split('\t');
            if (parts.Length < 3) continue;
            var gd = new GeneticData();
            gd.protein = parts[0];
            gd.organism = parts[1];
            gd.amino_acids = RLDecoding(parts[2]);
            list.Add(gd);
        }
        return list;
    }

    static string RLDecoding(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        var sb = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (char.IsDigit(c))
            {
                int n = c - '0';
                if (i + 1 < s.Length)
                {
                    char a = s[i + 1];
                    for (int k = 0; k < n; k++) sb.Append(a);
                    i++; // skip letter
                }
                else
                {
                    sb.Append(c);
                }
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    static string RLEncoding(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        var sb = new StringBuilder();
        int i = 0;
        while (i < s.Length)
        {
            char c = s[i];
            int j = i + 1;
            while (j < s.Length && s[j] == c && j - i < 9) j++;
            int run = j - i;
            if (run >= 3)
            {
                sb.Append(run);
                sb.Append(c);
            }
            else
            {
                for (int k = 0; k < run; k++) sb.Append(c);
            }
            i = j;
        }
        return sb.ToString();
    }

    static int AminoDifference(string a, string b)
    {
        int max = Math.Max(a.Length, b.Length);
        int diff = 0;
        for (int i = 0; i < max; i++)
        {
            char ca = i < a.Length ? a[i] : '\0';
            char cb = i < b.Length ? b[i] : '\0';
            if (ca != cb) diff++;
        }
        return diff;
    }

    static (char aa, int count) ModeAminoAcid(string s)
    {
        var counts = new Dictionary<char,int>();
        foreach (char c in s)
        {
            if (!counts.ContainsKey(c)) counts[c] = 0;
            counts[c]++;
        }
        if (counts.Count == 0) return ('?', 0);
        int max = counts.Values.Max();
        var candidates = counts.Where(p => p.Value == max).Select(p => p.Key).ToList();
        candidates.Sort();
        return (candidates[0], max);
    }
}
