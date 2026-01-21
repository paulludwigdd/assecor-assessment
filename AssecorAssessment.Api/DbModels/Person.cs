namespace AssecorAssessment.Api.DbModels;

internal class Person
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Lastname { get; init; }
    public required string Zipcode { get; init; }
    public required string City { get; init; }
    public int ColorId { get; init; }
    public Color? Color { get; init; }
}
