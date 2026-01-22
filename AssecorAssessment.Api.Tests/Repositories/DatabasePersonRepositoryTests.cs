using AssecorAssessment.Api.DbContext;
using AssecorAssessment.Api.DbModels;
using AssecorAssessment.Api.Exceptions;
using AssecorAssessment.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AssecorAssessment.Api.Tests.Repositories;

public class DatabasePersonRepositoryTests : IDisposable
{
    private readonly AppDbContext _dbContext;
    private readonly DatabasePersonRepository _repository;

    public DatabasePersonRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _repository = new DatabasePersonRepository(_dbContext);

        SeedTestData();
    }

    private void SeedTestData()
    {
        var colors = new List<Color>
        {
            new() { Id = 1, Name = "Blau" },
            new() { Id = 2, Name = "Grün" },
            new() { Id = 3, Name = "Violett" }
        };

        var persons = new List<Person>
        {
            new() { Id = 1, Name = "Hans", Lastname = "Müller", Zipcode = "12345", City = "Berlin", ColorId = 1 },
            new() { Id = 2, Name = "Peter", Lastname = "Schmidt", Zipcode = "54321", City = "Hamburg", ColorId = 2 },
            new() { Id = 3, Name = "Anna", Lastname = "Weber", Zipcode = "11111", City = "München", ColorId = 1 }
        };

        _dbContext.Colors.AddRange(colors);
        _dbContext.Persons.AddRange(persons);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPersonsWithColors()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        var persons = result.ToList();
        Assert.Equal(3, persons.Count);
        Assert.All(persons, p => Assert.NotNull(p.Color));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPersonWithColor()
    {
        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.Equal("Hans", result.Name);
        Assert.Equal("Müller", result.Lastname);
        Assert.NotNull(result.Color);
        Assert.Equal("Blau", result.Color.Name);
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
    public async Task AddAsync_SavesAndReturnsPersonWithColor()
    {
        // Arrange
        var newPerson = new Person
        {
            Name = "Max",
            Lastname = "Mustermann",
            Zipcode = "99999",
            City = "Teststadt",
            ColorId = 3
        };

        // Act
        var result = await _repository.AddAsync(newPerson);

        // Assert
        Assert.True(result.Id > 0);
        Assert.Equal("Max", result.Name);
        Assert.NotNull(result.Color);
        Assert.Equal("Violett", result.Color.Name);

        // Verify persisted
        var persisted = await _dbContext.Persons.FindAsync(result.Id);
        Assert.NotNull(persisted);
    }

    [Fact]
    public async Task AddAsync_WithInvalidColorId_ThrowsColorNotFoundException()
    {
        // Arrange
        var newPerson = new Person
        {
            Name = "Test",
            Lastname = "User",
            Zipcode = "00000",
            City = "Nirgendwo",
            ColorId = 999
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ColorNotFoundException>(
            () => _repository.AddAsync(newPerson));

        Assert.Equal(999, exception.ColorId);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
