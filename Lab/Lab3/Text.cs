namespace Lab3;

class Text
{
    public List<Sentence> Sentences { get; } = new List<Sentence>();

    public void AddSentence(Sentence sentence)
    {
        Sentences.Add(sentence);
    }

    public override string ToString()
    {
        return string.Join(" ", Sentences.ConvertAll(s => s.Content));
    }
}
