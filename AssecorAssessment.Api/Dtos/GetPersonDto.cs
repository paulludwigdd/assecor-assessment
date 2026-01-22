namespace AssecorAssessment.Api.Dtos;

public record GetPersonDto(
    int Id,
    string Name,
    string Lastname,
    string Zipcode,
    string City,
    string Color
);
