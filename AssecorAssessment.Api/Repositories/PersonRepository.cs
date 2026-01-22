using System.Text;
using AssecorAssessment.Api.DbModels;
using AssecorAssessment.Api.Exceptions;

namespace AssecorAssessment.Api.Repositories;

internal class PersonRepository(string csvFilePath) : IPersonRepository
{
    private readonly List<Person> _persons = LoadPersonsFromCsv(csvFilePath);
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

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        await Task.CompletedTask;
        return _persons;
    }

    public async Task<Person> GetByIdAsync(int id)
    {
        await Task.CompletedTask;
        var person = _persons.FirstOrDefault(p => p.Id == id);
        if (person is null)
        {
            throw new PersonNotFoundException(id);
        }
        return person;
    }

    public async Task<IEnumerable<Person>> GetByColorAsync(string color)
    {
        await Task.CompletedTask;
        var persons = _persons.Where(p =>
            p.Color?.Name.Equals(color, StringComparison.OrdinalIgnoreCase) == true);
        return persons;
    }

    private static List<Person> LoadPersonsFromCsv(string filePath)
    {
        var persons = new List<Person>();
        var content = File.ReadAllText(filePath);

        // Normalize line endings and handle multi-line records
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
        // Replace \r\n with \n, then handle lines that don't have 4 comma-separated values
        content = content.Replace("\r\n", "\n").Replace("\r", "\n");

        var lines = content.Split('\n');
        var normalizedLines = new List<string>();
        var currentLine = new StringBuilder();

        foreach (var line in lines)
        {
            currentLine.Append(line);

            // Count commas to check if we have a complete record (should have 3 commas)
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

        // PLZ and City are in parts[2], need to split by first space
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

        // Color ID is in parts[3]
        if (!int.TryParse(parts[3].Trim(), out var colorId))
            return null;

        var colorName = ColorMapping.GetValueOrDefault(colorId, "Unbekannt");

        return new Person
        {
            Id = id,
            Name = name,
            Lastname = lastname,
            Zipcode = zipcode,
            City = city,
            ColorId = colorId,
            Color = new Color { Id = colorId, Name = colorName }
        };
    }
}
