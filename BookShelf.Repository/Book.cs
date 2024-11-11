namespace BookShelf.Repository;

public class Book
{
    public Book(int id, string title, Author author, int pages)
    {
        Title = title;
        Author = [author];
        Pages = pages;
        Id = id;
    }

    public Book(int id, string title, Author[] author, int pages)
    {
        Title = title;
        Author = author;
        Pages = pages;
        Id = id;
    }

    public string Title { get; }
    public Author[] Author { get; }
    public int Pages { get; }
    public int Id { get; init; }
}