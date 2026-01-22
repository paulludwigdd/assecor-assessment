namespace AssecorAssessment.Api.Dtos;

public record PersonDto(
    int Id,
    string Name,
    string Lastname,
    string Zipcode,
    string City,
    string Color
);
