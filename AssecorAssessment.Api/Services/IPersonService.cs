using AssecorAssessment.Api.Dtos;

namespace AssecorAssessment.Api.Services;

internal interface IPersonService
{
    Task<IEnumerable<PersonDto>> GetAllAsync();
    Task<PersonDto> GetByIdAsync(int id);
    Task<IEnumerable<PersonDto>> GetByColorAsync(string color);
}
