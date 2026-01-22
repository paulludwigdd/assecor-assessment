using System.Text;
using AssecorAssessment.Api.DbModels;
using static AssecorAssessment.Api.Extensions.ColorMapper;

namespace AssecorAssessment.Api.Extensions;

internal static class CsvPersonParser
{
    public static List<Person> ParseFromFile(string filePath)
    {
        var content = File.ReadAllText(filePath);
        return ParseContent(content);
    }

    private static List<Person> ParseContent(string content)
    {
        var persons = new List<Person>();
        var normalizedContent = NormalizeContent(content);
        var lines = normalizedContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        var id = 1;
        foreach (var line in lines)
        {
            var person = ParseLine(line.Trim(), id);
            if (person is not null)
            {
                persons.Add(person);
                id++;
            }
        }

        return persons;
    }

    private static string NormalizeContent(string content)
    {
        content = content.Replace("\r\n", "\n").Replace("\r", "\n");

        var lines = content.Split('\n');
        var normalizedLines = new List<string>();
        var currentLine = new StringBuilder();

        foreach (var line in lines)
        {
            currentLine.Append(line);

            var commaCount = currentLine.ToString().Count(c => c == ',');
            if (commaCount >= 3)
            {
                normalizedLines.Add(currentLine.ToString().Trim());
                currentLine.Clear();
            }
        }

        return string.Join("\n", normalizedLines);
    }

    private static Person? ParseLine(string line, int id)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;

        var parts = line.Split(',');
        if (parts.Length < 4)
            return null;

        var lastname = parts[0].Trim();
        var name = parts[1].Trim();

        var addressPart = parts[2].Trim();
        var firstSpaceIndex = addressPart.IndexOf(' ');

        string zipcode;
        string city;

        if (firstSpaceIndex > 0)
        {
            zipcode = addressPart[..firstSpaceIndex].Trim();
            city = addressPart[(firstSpaceIndex + 1)..].Trim();
        }
        else
        {
            zipcode = addressPart;
            city = "";
        }

        if (!int.TryParse(parts[3].Trim(), out var colorId))
            return null;

        return new Person
        {
            Id = id,
            Name = name,
            Lastname = lastname,
            Zipcode = zipcode,
            City = city,
            ColorId = colorId,
            Color = MapToColor(colorId)
        };
    }
}
