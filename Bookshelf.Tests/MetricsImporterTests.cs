using BookShelf.Repository;

namespace Bookshelf.Tests;

public class MetricsImporterTests
{
    private readonly MetricsImporter importer;

    public MetricsImporterTests()
    {
        importer = new MetricsImporter();
    }

    [Fact()]
    public void OpenFile()
    {
        string testFile = "Metryki_2016-07.csv";
        importer.LoadFile(testFile);
    }

}
