using AssecorAssessment.Api.DbContext;
using AssecorAssessment.Api.DbModels;
using AssecorAssessment.Api.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AssecorAssessment.Api.Repositories;

internal class DatabasePersonRepository(AppDbContext dbContext) : IPersonRepository<DatabasePersonRepository>
{
    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await dbContext.Persons
            .Include(p => p.Color)
            .ToListAsync();
    }

    public async Task<Person> GetByIdAsync(int id)
    {
        var person = await dbContext.Persons
            .Include(p => p.Color)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person is null)
        {
            throw new PersonNotFoundException(id);
        }

        return person;
    }

    public async Task<IEnumerable<Person>> GetByColorAsync(string color)
    {
        return await dbContext.Persons
            .Include(p => p.Color)
            .Where(p => p.Color != null &&
                        p.Color.Name.Equals(color, StringComparison.CurrentCultureIgnoreCase))
            .ToListAsync();
    }

    public async Task<Person> AddAsync(Person person)
    {
        var color = await dbContext.Colors.FindAsync(person.ColorId)
            ?? throw new ColorNotFoundException(person.ColorId);

        try
        {
            dbContext.Persons.Add(person);
            await dbContext.SaveChangesAsync();

            return new Person
            {
                Id = person.Id,
                Name = person.Name,
                Lastname = person.Lastname,
                Zipcode = person.Zipcode,
                City = person.City,
                ColorId = person.ColorId,
                Color = color
            };
        }
        catch (DbUpdateException ex)
        {
            throw new DbUpdateException("Error connecting to database.", ex);
        }
    }
}
