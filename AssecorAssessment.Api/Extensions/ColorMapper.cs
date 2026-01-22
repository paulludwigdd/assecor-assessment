using AssecorAssessment.Api.DbModels;

namespace AssecorAssessment.Api.Extensions;

internal static class ColorMapper
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

    public static Color MapToColor(int colorId)
    {
        var colorName = ColorMapping.GetValueOrDefault(colorId, "Unbekannt");
        return new Color { Id = colorId, Name = colorName };
    }
}
