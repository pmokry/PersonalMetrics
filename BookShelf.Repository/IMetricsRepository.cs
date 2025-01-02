namespace BookShelf.Repository;

internal interface IMetricsRepository
{
    int AddBook(string title, string author, int pageCount);
    void AddProgressUpdate(int bookId, DateTime entryDate, int currentPage);
    void AddProgressUpdate(int bookId, DateTime entryDate, int currentPage, int pageCount);
    IEnumerable<Book> GetAllBooks();
    IEnumerable<Author> GetAllAuthors();
    ReadingProgress? GetProgress(int bookId);
}