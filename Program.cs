using Microsoft.EntityFrameworkCore;
using StudentAPIBusinessLayer.Context;
using StudentAPIBusinessLayer.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Registering the DbContext
builder.Services.AddDbContext<EntitiesDBContext>(options => 
    options.UseSqlServer(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionString").Value));

// Registering the repository
builder.Services.AddScoped<StudentRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();

    