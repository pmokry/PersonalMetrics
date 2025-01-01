
namespace BookShelf.Repository;

public class MetricsImporter
{
    private readonly List<string> _headers;
    public MetricsImporter()
    {
        _headers = new List<string>();
    }

    public List<string> Headers { get { return _headers; } }

    public void LoadFile(string testFile)
    {
        using StreamReader metricsReader = new StreamReader(testFile);
        _headers.AddRange(metricsReader.ReadLine()!.Split(';'));
        while (!metricsReader.EndOfStream)
        {
            string? line = metricsReader.ReadLine();

            //.Split(',').ToList();
        }
    }
}
