using AssecorAssessment.Api.Exceptions;
using AssecorAssessment.Api.Repositories;

namespace AssecorAssessment.Api.Tests;

public class PersonRepositoryTests
{
    private readonly PersonRepository _repository;

    public PersonRepositoryTests()
    {
        var testCsvPath = Path.Combine(AppContext.BaseDirectory, "TestData", "test-persons.csv");
        _repository = new PersonRepository(testCsvPath);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPersons()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        var persons = result.ToList();
        Assert.Equal(5, persons.Count);
    }

    [Fact]
    public async Task GetAllAsync_ParsesPersonDataCorrectly()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        var persons = result.ToList();
        var firstPerson = persons[0];

        Assert.Equal(1, firstPerson.Id);
        Assert.Equal("Hans", firstPerson.Name);
        Assert.Equal("Müller", firstPerson.Lastname);
        Assert.Equal("67742", firstPerson.Zipcode);
        Assert.Equal("Lauterecken", firstPerson.City);
        Assert.Equal(1, firstPerson.ColorId);
        Assert.Equal("Blau", firstPerson.Color?.Name);
    }

    [Fact]
    public async Task GetAllAsync_HandlesMultiLineRecords()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        var persons = result.ToList();
        var multiLinePerson = persons[3]; // 4th person multiline

        Assert.Equal("Line", multiLinePerson.Name);
        Assert.Equal("Multi", multiLinePerson.Lastname);
        Assert.Equal("99999", multiLinePerson.Zipcode);
        Assert.Equal("Teststadt", multiLinePerson.City);
        Assert.Equal(3, multiLinePerson.ColorId);
        Assert.Equal("Violett", multiLinePerson.Color?.Name);
    }

    [Fact]
    public async Task GetAllAsync_HandlesUnknownColorId()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        var persons = result.ToList();
        var unknownColorPerson = persons[4]; // 5th person with color ID 99

        Assert.Equal(99, unknownColorPerson.ColorId);
        Assert.Equal("Unbekannt", unknownColorPerson.Color?.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectPerson()
    {
        // Act
        var result = await _repository.GetByIdAsync(2);

        // Assert
        Assert.Equal(2, result.Id);
        Assert.Equal("Peter", result.Name);
        Assert.Equal("Petersen", result.Lastname);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ThrowsPersonNotFoundException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonNotFoundException>(
            () => _repository.GetByIdAsync(999));

        Assert.Equal(999, exception.PersonId);
    }

    [Fact]
    public async Task GetByColorAsync_ReturnsFilteredPersons()
    {
        // Act
        var result = await _repository.GetByColorAsync("Blau");

        // Assert
        var persons = result.ToList();
        Assert.Equal(2, persons.Count);
        Assert.All(persons, p => Assert.Equal("Blau", p.Color?.Name));
    }

    [Fact]
    public async Task GetByColorAsync_IsCaseInsensitive()
    {
        // Act
        var result = await _repository.GetByColorAsync("bLaU");

        // Assert
        var persons = result.ToList();
        Assert.Equal(2, persons.Count);
    }

    [Fact]
    public async Task GetByColorAsync_WhenNoMatch_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.GetByColorAsync("NichtExistent");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ColorMapping_MapsAllColorsCorrectly()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        var persons = result.ToList();

        // Person 1 & 3 have color 1 (Blau)
        Assert.Equal("Blau", persons[0].Color?.Name);
        Assert.Equal("Blau", persons[2].Color?.Name);

        // Person 2 has color 2 (Grün)
        Assert.Equal("Grün", persons[1].Color?.Name);

        // Person 4 has color 3 (Violett)
        Assert.Equal("Violett", persons[3].Color?.Name);
    }
}
