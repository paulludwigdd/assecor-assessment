using AssecorAssessment.Api.DbModels;

namespace AssecorAssessment.Api.Repositories;

internal interface IPersonRepository
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person> GetByIdAsync(int id);
    Task<IEnumerable<Person>> GetByColorAsync(string color);
    Task<Person> AddAsync(Person person);
}

internal interface IPersonRepository<T> : IPersonRepository;
