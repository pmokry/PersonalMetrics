namespace BookShelf.Repository
{
    public class ReadingProgress(Book book)
    {
        private readonly IList<ReadingProgressEntry> _entries = [];

        public Book Book { get; init; } = book ?? throw new ArgumentNullException(nameof(book));

        public int CurrentPage => LastEntry?.Page ?? 0;

        public IEnumerable<ReadingProgressEntry> Entries => _entries;
        public DateOnly? StartDate => FirstEntry?.EntryDate;
        public DateOnly? LastEntryDate => LastEntry?.EntryDate;
        public int PageCount => LastEntry?.PageCount ?? Book.Pages;

        public void AddReadingEntry(DateTime dateTime, int currentPage, int? pageCount = null)
        {
            DateOnly entryDate = DateOnly.FromDateTime(dateTime);
            ReadingProgressEntry? existingEntry = _entries.FirstOrDefault(entry => entry.EntryDate == entryDate);
            if (existingEntry != null)
            {
                _entries.Remove(existingEntry);
            }
            _entries.Add(new ReadingProgressEntry(DateOnly.FromDateTime(dateTime), currentPage, pageCount ?? Book.Pages));
        }

        private ReadingProgressEntry? FirstEntry
        {
            get
            {
                return _entries.OrderBy(entry => entry.EntryDate).FirstOrDefault();
            }
        }

        private ReadingProgressEntry? LastEntry
        {
            get
            {
                return _entries.OrderByDescending(entry => entry.EntryDate).FirstOrDefault();
            }
        }

        public decimal Progress => LastEntry == null ? 0 : (decimal)CurrentPage / PageCount;
    }
}