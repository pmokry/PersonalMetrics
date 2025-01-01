using BookShelf.Repository;

namespace Bookshelf.Tests;

public class MetricsImporterTests
{
    private const string TEST_DATA_DIR = "importTestData";
    private readonly MetricsImporter importer;

    public MetricsImporterTests()
    {
        importer = new MetricsImporter();
    }

    [Fact()]
    public void OpenValidFile()
    {
        Assert.Empty(importer.Headers);
        string testFile = @$"{TEST_DATA_DIR}/Metryki-2024.csv";
        importer.LoadFile(testFile);
        Assert.NotEmpty(importer.Headers);
        Assert.Equal(@"Procent", importer.Headers[0]);
        Assert.Equal(@"Tytu?", importer.Headers[1]);  // TODO: Add polish characters
        Assert.Equal(@"Autor/Autorzy", importer.Headers[2]);
        Assert.Equal(@"Liczba stron", importer.Headers[3]);
        Assert.Equal(@"Strona", importer.Headers[4]);
        Assert.Equal(@"Data zako?czenia", importer.Headers[5]);  // TODO: Add polish characters
        Assert.Equal(@"Kategoria", importer.Headers[6]);
        Assert.Equal(@"Progress", importer.Headers[7]);
        Assert.Equal(@"Planowany progress w stronach", importer.Headers[8]);
        Assert.Equal("31 gru 23", importer.Headers[9]);
        Assert.Equal("31 gru 24", importer.Headers[new Index(3, true)]);
        Assert.NotNull(importer.Headers.FirstOrDefault("31 mar 24"));
        Assert.NotNull(importer.Headers.FirstOrDefault("29 lut 24"));
        Assert.NotNull(importer.Headers.FirstOrDefault("31 sie 24"));
        Assert.NotNull(importer.Headers.FirstOrDefault("31 paź 24"));
        Assert.Null(importer.Headers.Find(item => item == "31 kwi 24"));
        Assert.Null(importer.Headers.Find(item => item == "31 cze 24"));
        Assert.Null(importer.Headers.Find(item => item == "30 lut 24"));
        Assert.Null(importer.Headers.Find(item => item == "31 lis 24"));
    }

}
