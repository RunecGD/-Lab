namespace BusinessLayer;

public class Patient
{
    public int Id { get; set; }
    public Recommendations recommendations { get; set; }
    public EMR emr { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Department department { get; set; }
}