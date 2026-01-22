using AssecorAssessment.Api.DbModels;
using AssecorAssessment.Api.Dtos;
using AssecorAssessment.Api.Repositories;
using static AssecorAssessment.Api.Extensions.Mapper;

namespace AssecorAssessment.Api.Services;

internal class PersonService(
    IPersonRepository<CsvPersonRepository> csvRepository,
    IPersonRepository<DatabasePersonRepository> dbRepository,
    IConfiguration configuration) : IPersonService
{
    private IPersonRepository Repository =>
        configuration["DataSource"] == "db" ? dbRepository : csvRepository;

    public async Task<IEnumerable<GetPersonDto>> GetAllAsync()
    {
        var persons = await Repository.GetAllAsync();
        return persons.Select(MapToDto);
    }

    public async Task<GetPersonDto> GetByIdAsync(int id)
    {
        var person = await Repository.GetByIdAsync(id);
        return MapToDto(person);
    }

    public async Task<IEnumerable<GetPersonDto>> GetByColorAsync(string color)
    {
        var persons = await Repository.GetByColorAsync(color);
        return persons.Select(MapToDto);
    }

    public async Task<GetPersonDto> AddAsync(CreatePersonDto createPersonDto)
    {
        var person = new Person
        {
            Name = createPersonDto.Name,
            Lastname = createPersonDto.Lastname,
            Zipcode = createPersonDto.Zipcode,
            City = createPersonDto.City,
            ColorId = createPersonDto.ColorId
        };

        var createdPerson = await Repository.AddAsync(person);
        return MapToDto(createdPerson);
    }
}
