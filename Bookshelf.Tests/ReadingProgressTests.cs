using BookShelf.Repository;

namespace Bookshelf.Tests;

public class ReadingProgressTests
{
    private const int pageCount = 400;
    private const int bookId = 12384;
    private readonly ReadingProgress testProgress;

    public ReadingProgressTests()
    {
        testProgress = new ReadingProgress(new Book(bookId, "book title", new Author("book author"), pageCount));
    }
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var entryDate = new DateOnly(2023, 10, 1);
        int currentPage = 100;

        // Act
        var entry = new ReadingProgressEntry(entryDate, currentPage, pageCount);

        // Assert
        Assert.Equal(entryDate, entry.EntryDate);
        Assert.Equal(currentPage, entry.Page);
        Assert.Equal(pageCount, entry.PageCount);
    }
    [Fact]
    public void InitialReadingProgress_hasLinkedBookId()
    {
        Assert.Equal(bookId, testProgress.Book.Id);
        Assert.Empty(testProgress.Entries);
        Assert.Null(testProgress.LastEntryDate);
        Assert.Null(testProgress.StartDate);
        Assert.Equal(400, testProgress.PageCount);
        Assert.Equal(0, testProgress.CurrentPage);
    }

    [Fact]
    public void AddReadingProgressEntryWithoutPageCustomization()
    {
        testProgress.AddReadingEntry(new DateTime(2021, 1, 1), 10);
        Assert.Equal(bookId, testProgress.Book.Id);
        Assert.Single(testProgress.Entries);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), testProgress.LastEntryDate);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), testProgress.StartDate);
        Assert.Equal(400, testProgress.PageCount);
        Assert.Equal(10, testProgress.CurrentPage);
        ReadingProgressEntry entry = testProgress.Entries.First();
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), entry.EntryDate);
        Assert.Equal(10, entry.Page);
    }

    [Fact]
    public void AddTwoReadingProgressesEntryWithoutPageCustomization()
    {
        testProgress.AddReadingEntry(new DateTime(2021, 1, 1), 10);
        testProgress.AddReadingEntry(new DateTime(2021, 1, 2), 20);
        Assert.Equal(bookId, testProgress.Book.Id);
        Assert.Equal(2, testProgress.Entries.Count());
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 2)), testProgress.LastEntryDate);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), testProgress.StartDate);
        Assert.Equal(400, testProgress.PageCount);
        Assert.Equal(20, testProgress.CurrentPage);
    }

    [Fact]
    public void AddTwoReadingProgressesEntryWithoutPageCustomization_WithEarlierPage()
    {
        testProgress.AddReadingEntry(new DateTime(2021, 1, 1), 40);
        testProgress.AddReadingEntry(new DateTime(2021, 1, 2), 20);
        Assert.Equal(bookId, testProgress.Book.Id);
        Assert.Equal(2, testProgress.Entries.Count());
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 2)), testProgress.LastEntryDate);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), testProgress.StartDate);
        Assert.Equal(400, testProgress.PageCount);
        Assert.Equal(20, testProgress.CurrentPage);
    }

    [Fact]
    public void AddTwoReadingProgressesInReverseOrder()
    {
        testProgress.AddReadingEntry(new DateTime(2021, 1, 2), 20);
        testProgress.AddReadingEntry(new DateTime(2021, 1, 1), 40);
        Assert.Equal(bookId, testProgress.Book.Id);
        Assert.Equal(2, testProgress.Entries.Count());
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 2)), testProgress.LastEntryDate);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), testProgress.StartDate);
        Assert.Equal(400, testProgress.PageCount);
        Assert.Equal(20, testProgress.CurrentPage);
    }

    [Fact]
    public void AddTwoReadingProgressesWithTheSameDate()
    {
        testProgress.AddReadingEntry(new DateTime(2021, 1, 1), 20);
        testProgress.AddReadingEntry(new DateTime(2021, 1, 1), 40);
        Assert.Equal(bookId, testProgress.Book.Id);
        Assert.Single(testProgress.Entries);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), testProgress.LastEntryDate);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), testProgress.StartDate);
        Assert.Equal(400, testProgress.PageCount);
        Assert.Equal(40, testProgress.CurrentPage);
    }

    [Fact]
    public void AddTwoReadingProgressesWithTheSameDateButLowerPage()
    {
        testProgress.AddReadingEntry(new DateTime(2021, 1, 1), 20);
        testProgress.AddReadingEntry(new DateTime(2021, 1, 1), 10);
        Assert.Equal(bookId, testProgress.Book.Id);
        Assert.Single(testProgress.Entries);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), testProgress.LastEntryDate);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), testProgress.StartDate);
        Assert.Equal(400, testProgress.PageCount);
        Assert.Equal(10, testProgress.CurrentPage);
    }
    [Fact]
    public void PageCountShouldComeFromLastEntryOrBookIfEmpty()
    {
        testProgress.AddReadingEntry(new DateTime(2021, 1, 1), 10);
        Assert.Equal(400, testProgress.PageCount);
        Assert.Equal(10M / pageCount, testProgress.Progress);
        Assert.Equal(10, testProgress.CurrentPage);

        testProgress.AddReadingEntry(new DateTime(2021, 1, 2), 11, 100);
        Assert.Equal(100, testProgress.PageCount);
        Assert.Equal(11 / 100M, testProgress.Progress);
        Assert.Equal(11, testProgress.CurrentPage);
    }
}