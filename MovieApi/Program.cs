using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MovieApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("dev");
builder.Services.AddDbContext<DatabaseContext>(Options =>
{
    Options.UseNpgsql(connectionString);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo()
        {
            Title = "Movies Api", Version = "v1", Description = "A test Api with .netCore 7",
            Contact = new OpenApiContact()
                {Name = "Amr", Url = new Uri("https://github.com/asolyma"), Email = "amr.soli@outlook.com"}
        });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "authorization",
        Type = SecuritySchemeType.Http, Scheme = "Bearer", BearerFormat = "JWT", In = ParameterLocation.Header,Description = "Enter you Token"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(cors => cors.AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();