using BookShelf.Repository;

namespace Bookshelf.Tests;
public class ReadingProgressToRepositoryTest
{
    private readonly MetricsRepository repository;
    private readonly int existingBookId;

    public ReadingProgressToRepositoryTest()
    {
        repository = new MetricsRepository();
        existingBookId = repository.AddBook("existing book title", "existing book author", 400);
    }

    [Fact]
    public void AddingProgressToNotExistingBook_fails()
    {
        Assert.Throws<MetricsRepository.MissingBookException>(() => repository.AddProgressUpdate(2, new DateTime(2021, 1, 1), 10, 400));
    }

    [Fact]
    public void NoProgressAddedToBook_GetProgressReturnsNull()
    {
        Assert.Null(repository.GetProgress(existingBookId));
    }

    [Fact]
    public void AddingProgressWithBookPageCount_addsEntry()
    {
        repository.AddProgressUpdate(existingBookId, new DateTime(2021, 1, 1), 10, 400);
        ReadingProgress? readingProgress = repository.GetProgress(existingBookId);
        Assert.NotNull(readingProgress);
        Assert.NotEmpty(readingProgress.Entries);
        Assert.Single(readingProgress.Entries);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), readingProgress.StartDate);
        Assert.Equal(DateOnly.FromDateTime(new DateTime(2021, 1, 1)), readingProgress.LastEntryDate);
        Assert.Equal(10, readingProgress.CurrentPage);
        Assert.Equal(400, readingProgress.PageCount);
        Assert.Equal(existingBookId, readingProgress.Book.Id);

    }

    [Fact]
    public void AddingTwoReadingProgressForDifferentDays_addsTwoEntries()
    {
        repository.AddProgressUpdate(existingBookId, new DateTime(2021, 1, 1), 10, 400);
        repository.AddProgressUpdate(existingBookId, new DateTime(2021, 1, 2), 20, 400);
        ReadingProgress? existingProgress = repository.GetProgress(existingBookId);
        Assert.NotNull(existingProgress);
        Assert.Equal(2, existingProgress.Entries.Count());
    }

    [Fact]
    public void AddingProgressWithoutBookPageCount_usesBookPageCount()
    {
        repository.AddProgressUpdate(existingBookId, new DateTime(2021, 1, 1), 10);
        var progress = repository.GetProgress(existingBookId);
        Assert.NotNull(progress);
        Assert.Equal(10, progress.CurrentPage);
        Assert.Equal(400, progress.PageCount);
        Assert.Equal(10 / 400M, progress.Progress);
    }

    [Fact]
    public void PageCountShouldComeFromLastEntryOrBookIfEmpty()
    {
        repository.AddProgressUpdate(existingBookId, new DateTime(2021, 1, 1), 10, 100);
        var progress = repository.GetProgress(existingBookId);
        Assert.NotNull(progress);
        Assert.Equal(100, progress.PageCount);
        Assert.Equal(10 / 100M, progress.Progress);
        Assert.Equal(10, progress.CurrentPage);
    }
}
