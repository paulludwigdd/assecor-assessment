using AssecorAssessment.Api.Exceptions;
using AssecorAssessment.Api.Repositories;

namespace AssecorAssessment.Api.Tests.Repositories;

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
        Assert.Equal(5, result.Count());
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
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByColorAsync_WhenNoMatch_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.GetByColorAsync("NichtExistent");

        // Assert
        Assert.Empty(result);
    }
}
