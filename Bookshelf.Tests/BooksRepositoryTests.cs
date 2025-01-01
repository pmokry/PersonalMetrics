using BookShelf.Repository;

namespace Bookshelf.Tests;

public class BooksRepositoryTests
{
    private readonly MetricsRepository repository;

    public BooksRepositoryTests()
    {
        repository = new MetricsRepository();
    }

    [Fact]
    public void InitialRepositoryBookList_IsEmpty()
    {
        Assert.Empty(repository.GetAllBooks());
    }

    [Fact]
    public void AddingBook()
    {
        repository.AddBook("book title", "book author", 400);
        Assert.Single(repository.GetAllBooks());
    }

    [Fact]
    public void AddedBook_HasCorrectTitle()
    {
        repository.AddBook("book title", "book author", 400);
        Assert.Equal("book title", repository.GetAllBooks().First().Title);
    }

    [Fact]
    public void AddedBook_HasIdAssigned()
    {
        repository.AddBook("book title", "book author", 400);
        Book addedBook = repository.GetAllBooks().First();
        Assert.Equal(1, addedBook.Id);
        Assert.Equal("book title", addedBook.Title);
        Assert.Equal("book", addedBook.Author.First().FirstName);
        Assert.Equal("author", addedBook.Author.First().LastName);
    }

    [Fact]
    public void AddingDifferentBook()
    {
        repository.AddBook("book title", "book author", 400);
        repository.AddBook("book title 2", "book author 2", 300);
        Assert.Equal(2, repository.GetAllBooks().Count());
    }

    [Fact]
    public void AddingDuplicateBook_ThrowsException()
    {
        repository.AddBook("book title", "book author", 400);
        Assert.Throws<MetricsRepository.DuplicateBookException>(() => repository.AddBook("book title", "book author", 300));
    }

    [Fact]
    public void AddAuthor()
    {
        repository.AddAuthor("First Name1", "Last Name1");
        Assert.Single(repository.GetAllAuthors());
        Author author = repository.GetAllAuthors().Last();
        Assert.Equal("Last Name1, First Name1", author.FullName);
        Assert.Equal("First Name1", author.FirstName);
        Assert.Null(author.MiddleName);
        Assert.Equal("Last Name1", author.LastName);

        repository.AddAuthor("First-Name2", "Last-Name2");
        Assert.Equal(2, repository.GetAllAuthors().Count());
        author = repository.GetAllAuthors().Last();
        Assert.Equal("Last-Name2, First-Name2", author.FullName);
        Assert.Equal("First-Name2", author.FirstName);
        Assert.Null(author.MiddleName);
        Assert.Equal("Last-Name2", author.LastName);

        repository.AddAuthor("First-Name3 Last-Name3");
        Assert.Equal(3, repository.GetAllAuthors().Count());
        author = repository.GetAllAuthors().Last();
        Assert.Equal("Last-Name3, First-Name3", author.FullName);
        Assert.Equal("First-Name3", author.FirstName);
        Assert.Null(author.MiddleName);
        Assert.Equal("Last-Name3", author.LastName);

        repository.AddAuthor("Last-Name4, First-Name4");
        Assert.Equal(4, repository.GetAllAuthors().Count());
        author = repository.GetAllAuthors().Last();
        Assert.Equal("Last-Name4, First-Name4", author.FullName);
        Assert.Equal("First-Name4", author.FirstName);
        Assert.Null(author.MiddleName);
        Assert.Equal("Last-Name4", author.LastName);

        repository.AddAuthor("First Name5", "Middle Name5", "Last Name5");
        Assert.Equal(5, repository.GetAllAuthors().Count());
        author = repository.GetAllAuthors().Last();
        Assert.Equal("Last Name5, First Name5 Middle Name5", author.FullName);
        Assert.Equal("First Name5", author.FirstName);
        Assert.Equal("Middle Name5", author.MiddleName);
        Assert.Equal("Last Name5", author.LastName);

        repository.AddAuthor("First-Name6 Middle-Name6 Last-Name6");
        Assert.Equal(6, repository.GetAllAuthors().Count());
        author = repository.GetAllAuthors().Last();
        Assert.Equal("Last-Name6, First-Name6 Middle-Name6", author.FullName);
        Assert.Equal("First-Name6", author.FirstName);
        Assert.Equal("Middle-Name6", author.MiddleName);
        Assert.Equal("Last-Name6", author.LastName);

        repository.AddAuthor("Last-Name7, First-Name7 Middle-Name7");
        Assert.Equal(7, repository.GetAllAuthors().Count());
        author = repository.GetAllAuthors().Last();
        Assert.Equal("Last-Name7, First-Name7 Middle-Name7", author.FullName);
        Assert.Equal("First-Name7", author.FirstName);
        Assert.Equal("Middle-Name7", author.MiddleName);
        Assert.Equal("Last-Name7", author.LastName);
    }

    [Fact]
    public void UnsupportedAuthorFullName_exceptionThrown()
    {
        Assert.Throws<Author.UnsupportedAuthorFullNameFormat>(() => repository.AddAuthor("LastName, FirstName MiddleName, tt"));
        Assert.Throws<Author.UnsupportedAuthorFullNameFormat>(() => repository.AddAuthor("LastName, FirstName MiddleName tt"));
        Assert.Throws<Author.UnsupportedAuthorFullNameFormat>(() => repository.AddAuthor("FirstName MiddleName tt LastName"));
        Assert.Throws<Author.UnsupportedAuthorFullNameFormat>(() => repository.AddAuthor("LastName"));
    }

    [Fact]
    public void AddingBookWithNewAuthor_CreatesAuthorEntity()
    {
        repository.AddBook("book title", "Name Surname", 400);
        Assert.Single(repository.GetAllAuthors());
        Author author = repository.GetAllAuthors().Last();
        Assert.Equal("Surname, Name", author.FullName);
        Assert.Equal("Name", author.FirstName);
        Assert.Null(author.MiddleName);
        Assert.Equal("Surname", author.LastName);
        Assert.Equal(repository.GetAllBooks().First().Author.First(), author);
    }

    [Fact]
    public void AddingExistingAuthorWithDifferentStyle_DoesNotAddNewAuthor()
    {
        repository.AddAuthor("First Name1", "Last Name1");
        Assert.Single(repository.GetAllAuthors());
        repository.AddAuthor("Last Name1, First Name1");
        Assert.Single(repository.GetAllAuthors());
    }

    [Fact]
    public void AddingBookForWithExistingAuthor_DoesNotAddNewAuthor()
    {
        repository.AddAuthor("First Name1", "Last Name1");
        Assert.Single(repository.GetAllAuthors());
        Assert.Empty(repository.GetAllBooks());
        repository.AddBook("book title", "Last Name1, First Name1", 400);
        Assert.Single(repository.GetAllAuthors());
    }

    [Fact]
    public void AddingProgressToNotExistingBook_fails()
    {
        Assert.Throws<MetricsRepository.MissingBookException>(() => repository.AddProgressUpdate(2, new DateTime(2021, 1, 1), 10, 400));
    }

    [Fact]
    public void NoProgressAddedToBook_GetProgressReturnsNull()
    {
        Assert.Null(repository.GetProgress(1));
    }

    [Fact]
    public void BookCanHaveTwoAuthors()
    {
        repository.AddAuthor("First Name1", "Last Name1");
        repository.AddAuthor("First Name2", "Last Name2");
        repository.AddBook("book title", "Last Name1, First Name1 & Last Name2, First Name2", 400);
        Assert.Equal(2, repository.GetAllAuthors().Count());
        Assert.Single(repository.GetAllBooks());
    }

    [Fact]
    public void BookCanHaveThreeAuthors()
    {
        repository.AddAuthor("First Name1", "Last Name1");
        repository.AddAuthor("First Name2", "Last Name2");
        repository.AddAuthor("First Name3", "Last Name3");
        repository.AddBook("book title", "Last Name1, First Name1 & Last Name2, First Name2 & Last Name3, First Name3", 400);
        Assert.Equal(3, repository.GetAllAuthors().Count());
        Assert.Single(repository.GetAllBooks());
    }

    [Fact]
    public void OnlyMissingAuthorsAreAdded()
    {
        repository.AddAuthor("First Name1", "Last Name1");
        repository.AddBook("book title", "Last Name1, First Name1 & Last Name2, First Name2", 400);
        Assert.Equal(2, repository.GetAllAuthors().Count());
        Assert.Single(repository.GetAllBooks());
    }
}
