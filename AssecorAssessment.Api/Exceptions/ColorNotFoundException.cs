namespace AssecorAssessment.Api.Exceptions;

public class ColorNotFoundException(int id) : Exception($"Color with ID {id} not found")
{
    public int ColorId { get; } = id;
}
