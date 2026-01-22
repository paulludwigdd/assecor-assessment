using AssecorAssessment.Api.DbModels;
using AssecorAssessment.Api.Dtos;

namespace AssecorAssessment.Api.Extensions;

internal static class Mapper
{
    private static readonly Dictionary<int, string> ColorMapping = new()
    {
        { 1, "Blau" },
        { 2, "Grün" },
        { 3, "Violett" },
        { 4, "Rot" },
        { 5, "Gelb" },
        { 6, "Türkis" },
        { 7, "Weiß" }
    };

    internal static Color MapToColor(int colorId)
    {
        var colorName = ColorMapping.GetValueOrDefault(colorId, "Unbekannt");
        return new Color { Id = colorId, Name = colorName };
    }

    internal static GetPersonDto MapToDto(Person person)
    {
        return new GetPersonDto(
            person.Id,
            person.Name,
            person.Lastname,
            person.Zipcode,
            person.City,
            person.Color?.Name ?? "Unbekannt"
        );
    }
}
