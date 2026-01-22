using AssecorAssessment.Api.Dtos;

namespace AssecorAssessment.Api.Services;

public interface IPersonService
{
    Task<IEnumerable<GetPersonDto>> GetAllAsync();
    Task<GetPersonDto> GetByIdAsync(int id);
    Task<IEnumerable<GetPersonDto>> GetByColorAsync(string color);
    Task<GetPersonDto> AddAsync(CreatePersonDto createPersonDto);
}
