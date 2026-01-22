using AssecorAssessment.Api.Controllers;
using AssecorAssessment.Api.Dtos;
using AssecorAssessment.Api.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace AssecorAssessment.Api.Tests;

public class PersonsControllerTests
{
    private readonly IPersonService _service;
    private readonly PersonsController _controller;

    public PersonsControllerTests()
    {
        _service = Substitute.For<IPersonService>();
        _controller = new PersonsController(_service);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithPersons()
    {
        // Arrange
        var persons = new List<PersonDto>
        {
            new(1, "Hans", "Müller", "67742", "Lauterecken", "Blau"),
            new(2, "Peter", "Petersen", "18439", "Stralsund", "Grün")
        };
        _service.GetAllAsync().Returns(persons);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPersons = Assert.IsAssignableFrom<IEnumerable<PersonDto>>(okResult.Value);
        Assert.Equal(2, returnedPersons.Count());
    }

    [Fact]
    public async Task GetById_ReturnsOkWithPerson()
    {
        // Arrange
        var person = new PersonDto(1, "Hans", "Müller", "67742", "Lauterecken", "Blau");
        _service.GetByIdAsync(1).Returns(person);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPerson = Assert.IsType<PersonDto>(okResult.Value);
        Assert.Equal(1, returnedPerson.Id);
        Assert.Equal("Hans", returnedPerson.Name);
    }

    [Fact]
    public async Task GetByColor_ReturnsOkWithFilteredPersons()
    {
        // Arrange
        var persons = new List<PersonDto>
        {
            new(1, "Hans", "Müller", "67742", "Lauterecken", "Blau"),
            new(8, "Bertram", "Bart", "12313", "Wasweißich", "Blau")
        };
        _service.GetByColorAsync("Blau").Returns(persons);

        // Act
        var result = await _controller.GetByColor("Blau");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPersons = Assert.IsAssignableFrom<IEnumerable<PersonDto>>(okResult.Value);
        Assert.Equal(2, returnedPersons.Count());
        Assert.All(returnedPersons, p => Assert.Equal("Blau", p.Color));
    }

    [Fact]
    public async Task GetByColor_WhenNoMatch_ReturnsOkWithEmptyList()
    {
        // Arrange
        _service.GetByColorAsync("Unbekannt").Returns(new List<PersonDto>());

        // Act
        var result = await _controller.GetByColor("Unbekannt");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPersons = Assert.IsAssignableFrom<IEnumerable<PersonDto>>(okResult.Value);
        Assert.Empty(returnedPersons);
    }
}
