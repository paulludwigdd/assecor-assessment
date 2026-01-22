namespace AssecorAssessment.Api.Exceptions;

public class PersonNotFoundException(int id) : Exception($"Person with ID {id} not found")
{
    public int PersonId { get; } = id;
}
