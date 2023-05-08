using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.Services.CountryServices;
using MonarchsAPI_Net6.Services.DynastyServices;
using MonarchsAPI_Net6.Services.MonarchServices;
using MonarchsAPI_Net6.Services.RatingServices;
using MonarchsAPI_Net6.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IRatingServices, RatingServices>();
builder.Services.AddScoped<IMonarchServices, MonarchServices>();
builder.Services.AddScoped<IDynastyServices, DynastyServices>();
builder.Services.AddScoped<ICountryServices, CountryServices>();
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), options =>
    {
        options.CommandTimeout(120);
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

app.UseCors(builder => builder
       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowAnyOrigin()
    );

app.UseAuthorization();

app.MapControllers();

app.Run();
