using AssecorAssessment.Api.DbModels;
using AssecorAssessment.Api.Dtos;
using AssecorAssessment.Api.Repositories;

namespace AssecorAssessment.Api.Services;

internal class PersonService(IPersonRepository personRepository) : IPersonService
{
    public async Task<IEnumerable<PersonDto>> GetAllAsync()
    {
        var persons = await personRepository.GetAllAsync();
        return persons.Select(MapToDto);
    }

    public async Task<PersonDto> GetByIdAsync(int id)
    {
        var person = await personRepository.GetByIdAsync(id);
        return MapToDto(person);
    }

    public async Task<IEnumerable<PersonDto>> GetByColorAsync(string color)
    {
        var persons = await personRepository.GetByColorAsync(color);
        return persons.Select(MapToDto);
    }

    private static PersonDto MapToDto(Person person)
    {
        return new PersonDto(
            person.Id,
            person.Name,
            person.Lastname,
            person.Zipcode,
            person.City,
            person.Color?.Name ?? "Unbekannt"
        );
    }
}
