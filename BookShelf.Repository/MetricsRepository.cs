namespace BookShelf.Repository;

public class MetricsRepository : IMetricsRepository
{
    private readonly IList<ReadingProgress> _progress = [];
    private readonly IList<Book> _books = [];
    private readonly IList<Author> _authors = [];

    public void AddAuthor(string firstName, string middleName, string lastName)
    {
        _authors.Add(new Author(firstName, middleName, lastName));
    }

    public void AddAuthor(string firstName, string lastName)
    {
        _authors.Add(new Author(firstName, lastName));
    }

    public void AddAuthor(string authorFullName)
    {
        if (_authors.Any(a => a.FullName == authorFullName))
            return;
        _authors.Add(new Author(authorFullName));
    }

    public int AddBook(string title, string authorFullName, int pages)
    {
        if (_books.Any(b => b.Title == title))
        {
            throw new DuplicateBookException();
        }
        int addedBookId = _books.Count + 1;

        if (authorFullName.Contains('&'))
        {
            string[] authors = authorFullName.Split('&');
            foreach (string author in authors)
            {
                AddAuthor(author.Trim());
            }
            _books.Add(new Book(addedBookId, title, authors.Select(a => _authors.First(author => author.FullName == a.Trim())).ToArray(), pages));
        }
        else
        {

            Author? author = _authors.FirstOrDefault(a => a.FullName == authorFullName);
            if (author == null)
            {
                author = new Author(authorFullName);
                _authors.Add(author);
            }
            _books.Add(new Book(addedBookId, title, author, pages));
        }
        return addedBookId;
    }

    public void AddProgressUpdate(int bookId, DateTime entryDate, int currentPage)
    {
        Book? existingBook = _books.FirstOrDefault(b => b.Id == bookId) ?? throw new MissingBookException();
        AddProgressUpdate(bookId, entryDate, currentPage, existingBook.Pages);
    }

    public void AddProgressUpdate(int bookId, DateTime entryDate, int currentPage, int bookPageCount)
    {
        Book connectedBook = _books.FirstOrDefault(b => b.Id == bookId) ?? throw new MissingBookException();
        ReadingProgress? existingProgress = _progress.FirstOrDefault(p => p.Book.Id == bookId);
        if (existingProgress == null)
        {
            existingProgress = new ReadingProgress(connectedBook);
            _progress.Add(existingProgress);
        }
        existingProgress.AddReadingEntry(entryDate, currentPage, bookPageCount);
    }

    public IEnumerable<Author> GetAllAuthors()
    {
        return _authors;
    }

    public IEnumerable<Book> GetAllBooks()
    {
        return _books;
    }

    public IEnumerable<ReadingProgress> GetAllReadingProgress() {
        return _progress;
    }

    public ReadingProgress? GetProgress(int bookId)
    {
        return _progress.FirstOrDefault(p => p.Book.Id == bookId);
    }

    public class MissingBookException : Exception
    {
    }

    public class DuplicateBookException : Exception
    {
    }
}