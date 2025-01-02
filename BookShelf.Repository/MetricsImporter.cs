

namespace BookShelf.Repository;

public class MetricsImporter
{
    private readonly IMetricsRepository _temporaryRepository = new MetricsRepository();
    private readonly List<string> _headers;
    private const string BOOK_TITLE_HEADER = "Tytu?";
    private const string AUTHOR_HEADER = "Autor/Autorzy";
    private const string PAGES_HEADER = "Liczba stron";
    public MetricsImporter()
    {
        _headers = new List<string>();
    }

    public List<string> Headers { get { return _headers; } }

    public IEnumerable<Book> Books => _temporaryRepository.GetAllBooks();
    public IEnumerable<Author> Authors => _temporaryRepository.GetAllAuthors();

    public void LoadFile(string testFile)
    {
        using StreamReader metricsReader = new StreamReader(testFile);
        _headers.AddRange(metricsReader.ReadLine()!.Split(';').Where(x => !string.IsNullOrEmpty(x)));
        int titleIndex = _headers.IndexOf(BOOK_TITLE_HEADER);
        int authorIndex = _headers.IndexOf(AUTHOR_HEADER);
        int pagesIndex = _headers.IndexOf(PAGES_HEADER);
        while (!metricsReader.EndOfStream)
        {
            string[] line = metricsReader.ReadLine()!.Split(';');
            _temporaryRepository.AddBook(line[titleIndex].Trim(), line[authorIndex], int.Parse(line[pagesIndex]));
            //.Split(',').ToList();
        }
    }
}
