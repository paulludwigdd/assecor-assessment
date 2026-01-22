using AssecorAssessment.Api.Extensions;

namespace AssecorAssessment.Api.Tests.Extensions;

public class ColorMapperTests
{
    [Theory]
    [InlineData(1, "Blau")]
    [InlineData(2, "Grün")]
    [InlineData(3, "Violett")]
    [InlineData(4, "Rot")]
    [InlineData(5, "Gelb")]
    [InlineData(6, "Türkis")]
    [InlineData(7, "Weiß")]
    public void MapToColor_MapsKnownColorIds(int colorId, string expectedName)
    {
        // Act
        var result = ColorMapper.MapToColor(colorId);

        // Assert
        Assert.Equal(colorId, result.Id);
        Assert.Equal(expectedName, result.Name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    [InlineData(-1)]
    public void MapToColor_ReturnsUnbekanntForUnknownColorIds(int colorId)
    {
        // Act
        var result = ColorMapper.MapToColor(colorId);

        // Assert
        Assert.Equal(colorId, result.Id);
        Assert.Equal("Unbekannt", result.Name);
    }
}
