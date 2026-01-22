using AssecorAssessment.Api.DbModels;
using AssecorAssessment.Api.Exceptions;
using static AssecorAssessment.Api.Extensions.CsvPersonParser;

namespace AssecorAssessment.Api.Repositories;

internal class CsvPersonRepository(string csvFilePath) : IPersonRepository<CsvPersonRepository>
{
    private readonly List<Person> _persons = ParseFromFile(csvFilePath);

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        await Task.CompletedTask;
        return _persons;
    }

    public async Task<Person> GetByIdAsync(int id)
    {
        await Task.CompletedTask;
        var person = _persons.FirstOrDefault(p => p.Id == id);
        if (person is null)
        {
            throw new PersonNotFoundException(id);
        }
        return person;
    }

    public async Task<IEnumerable<Person>> GetByColorAsync(string color)
    {
        await Task.CompletedTask;
        var persons = _persons.Where(p =>
            p.Color?.Name.Equals(color, StringComparison.OrdinalIgnoreCase) == true);
        return persons;
    }

    public Task<Person> AddAsync(Person person)
    {
        throw new NotSupportedException("Adding persons to CSV is not supported.");
    }
}
