namespace AssecorAssessment.Api.Dtos;

public record CreatePersonDto(
    string Name,
    string Lastname,
    string Zipcode,
    string City,
    int ColorId
);
