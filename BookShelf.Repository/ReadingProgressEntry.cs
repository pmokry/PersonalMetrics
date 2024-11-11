namespace BookShelf.Repository;

public class ReadingProgressEntry(DateOnly dateOnly, int currentPage, int pageCount)
{
    public DateOnly EntryDate { get; } = dateOnly;

    public int Page { get; } = currentPage;

    public int PageCount { get; } = pageCount;
}