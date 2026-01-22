using AssecorAssessment.Api.Dtos;
using AssecorAssessment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssecorAssessment.Api.Controllers;

[ApiController]
[Route("[controller]")]
internal class PersonsController(IPersonService personService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetAll()
    {
        var persons = await personService.GetAllAsync();
        return Ok(persons);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonDto>> GetById(int id)
    {
        var person = await personService.GetByIdAsync(id);
        return Ok(person);
    }

    [HttpGet("color/{color}")]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetByColor(string color)
    {
        var persons = await personService.GetByColorAsync(color);
        return Ok(persons);
    }
}
