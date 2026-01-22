using AssecorAssessment.Api.Extensions;

namespace AssecorAssessment.Api.Tests.Extensions;

public class CsvPersonParserTests
{
    [Fact]
    public void ParseFromFile_ParsesPersonDataCorrectly()
    {
        // Arrange
        var testCsvPath = Path.Combine(AppContext.BaseDirectory, "TestData", "test-persons.csv");

        // Act
        var result = CsvPersonParser.ParseFromFile(testCsvPath);

        // Assert
        var firstPerson = result[0];
        Assert.Equal(1, firstPerson.Id);
        Assert.Equal("Hans", firstPerson.Name);
        Assert.Equal("Müller", firstPerson.Lastname);
        Assert.Equal("67742", firstPerson.Zipcode);
        Assert.Equal("Lauterecken", firstPerson.City);
        Assert.Equal(1, firstPerson.ColorId);
    }

    [Fact]
    public void ParseFromFile_HandlesMultiLineRecords()
    {
        // Arrange
        var testCsvPath = Path.Combine(AppContext.BaseDirectory, "TestData", "test-persons.csv");

        // Act
        var result = CsvPersonParser.ParseFromFile(testCsvPath);

        // Assert
        var multiLinePerson = result[3];
        Assert.Equal("Line", multiLinePerson.Name);
        Assert.Equal("Multi", multiLinePerson.Lastname);
        Assert.Equal("99999", multiLinePerson.Zipcode);
        Assert.Equal("Teststadt", multiLinePerson.City);
    }

    [Fact]
    public void ParseFromFile_AssignsCorrectColorObjects()
    {
        // Arrange
        var testCsvPath = Path.Combine(AppContext.BaseDirectory, "TestData", "test-persons.csv");

        // Act
        var result = CsvPersonParser.ParseFromFile(testCsvPath);

        // Assert
        Assert.Equal("Blau", result[0].Color?.Name);
        Assert.Equal("Grün", result[1].Color?.Name);
        Assert.Equal("Violett", result[3].Color?.Name);
        Assert.Equal("Unbekannt", result[4].Color?.Name);
    }

    [Fact]
    public void ParseFromFile_ReturnsCorrectNumberOfPersons()
    {
        // Arrange
        var testCsvPath = Path.Combine(AppContext.BaseDirectory, "TestData", "test-persons.csv");

        // Act
        var result = CsvPersonParser.ParseFromFile(testCsvPath);

        // Assert
        Assert.Equal(5, result.Count);
    }
}
