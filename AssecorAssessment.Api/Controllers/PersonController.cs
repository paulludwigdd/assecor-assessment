using AssecorAssessment.Api.Dtos;
using AssecorAssessment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssecorAssessment.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController(IPersonService personService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPersonDto>>> GetAll()
    {
        var persons = await personService.GetAllAsync();
        return Ok(persons);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPersonDto>> GetById(int id)
    {
        var person = await personService.GetByIdAsync(id);
        return Ok(person);
    }

    [HttpGet("color/{color}")]
    public async Task<ActionResult<IEnumerable<GetPersonDto>>> GetByColor(string color)
    {
        var persons = await personService.GetByColorAsync(color);
        return Ok(persons);
    }

    [HttpPost]
    public async Task<ActionResult<GetPersonDto>> Create([FromBody] CreatePersonDto createPersonDto)
    {
        var person = await personService.AddAsync(createPersonDto);
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
    }
}
