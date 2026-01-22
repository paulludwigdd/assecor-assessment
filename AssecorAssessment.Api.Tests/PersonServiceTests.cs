using AssecorAssessment.Api.DbModels;
using AssecorAssessment.Api.Exceptions;
using AssecorAssessment.Api.Repositories;
using AssecorAssessment.Api.Services;
using NSubstitute;

namespace AssecorAssessment.Api.Tests;

public class PersonServiceTests
{
    private readonly IPersonRepository _repository;
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _repository = Substitute.For<IPersonRepository>();
        _service = new PersonService(_repository);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPersonsAsDtos()
    {
        // Arrange
        var persons = new List<Person>
        {
            new() { Id = 1, Name = "Hans", Lastname = "Müller", Zipcode = "67742", City = "Lauterecken", ColorId = 1, Color = new Color { Id = 1, Name = "Blau" } },
            new() { Id = 2, Name = "Peter", Lastname = "Petersen", Zipcode = "18439", City = "Stralsund", ColorId = 2, Color = new Color { Id = 2, Name = "Grün" } }
        };
        _repository.GetAllAsync().Returns(persons);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        var dtos = result.ToList();
        Assert.Equal(2, dtos.Count);
        Assert.Equal("Hans", dtos[0].Name);
        Assert.Equal("Müller", dtos[0].Lastname);
        Assert.Equal("Blau", dtos[0].Color);
        Assert.Equal("Peter", dtos[1].Name);
        Assert.Equal("Grün", dtos[1].Color);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPersonDto()
    {
        // Arrange
        var person = new Person
        {
            Id = 1,
            Name = "Hans",
            Lastname = "Müller",
            Zipcode = "67742",
            City = "Lauterecken",
            ColorId = 1,
            Color = new Color { Id = 1, Name = "Blau" }
        };
        _repository.GetByIdAsync(1).Returns(person);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("Hans", result.Name);
        Assert.Equal("Müller", result.Lastname);
        Assert.Equal("67742", result.Zipcode);
        Assert.Equal("Lauterecken", result.City);
        Assert.Equal("Blau", result.Color);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ThrowsPersonNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(999).Returns<Person>(_ => throw new PersonNotFoundException(999));

        // Act & Assert
        await Assert.ThrowsAsync<PersonNotFoundException>(() => _service.GetByIdAsync(999));
    }

    [Fact]
    public async Task GetByColorAsync_ReturnsFilteredPersons()
    {
        // Arrange
        var persons = new List<Person>
        {
            new() { Id = 1, Name = "Hans", Lastname = "Müller", Zipcode = "67742", City = "Lauterecken", ColorId = 1, Color = new Color { Id = 1, Name = "Blau" } },
            new() { Id = 2, Name = "Klaus", Lastname = "Klaussen", Zipcode = "43246", City = "Hierach", ColorId = 1, Color = new Color { Id = 1, Name = "Blau" } }
        };
        _repository.GetByColorAsync("Blau").Returns(persons);

        // Act
        var result = await _service.GetByColorAsync("Blau");

        // Assert
        var dtos = result.ToList();
        Assert.Equal(2, dtos.Count);
        Assert.All(dtos, dto => Assert.Equal("Blau", dto.Color));
    }

    [Fact]
    public async Task GetByColorAsync_WhenNoMatch_ReturnsEmptyList()
    {
        // Arrange
        _repository.GetByColorAsync("Unbekannt").Returns(new List<Person>());

        // Act
        var result = await _service.GetByColorAsync("Unbekannt");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task MapToDto_WhenColorIsNull_ReturnsUnbekannt()
    {
        // Arrange
        var person = new Person
        {
            Id = 1,
            Name = "Test",
            Lastname = "Person",
            Zipcode = "12345",
            City = "Stadt",
            ColorId = 99,
            Color = null
        };
        _repository.GetByIdAsync(1).Returns(person);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Equal("Unbekannt", result.Color);
    }
}
