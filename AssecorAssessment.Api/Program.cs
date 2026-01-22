using AssecorAssessment.Api.DbContext;
using AssecorAssessment.Api.Middleware;
using AssecorAssessment.Api.Repositories;
using AssecorAssessment.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var csvFilePath = builder.Configuration["CsvFilePath"] ?? "sample-input.csv";
builder.Services.AddSingleton<IPersonRepository>(new PersonRepository(csvFilePath));
builder.Services.AddScoped<IPersonService, PersonService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
