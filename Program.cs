using Microsoft.EntityFrameworkCore;
using Neosoft_puja_backend.BAL.Interfaces;
using Neosoft_puja_backend.BAL.Services;
using Neosoft_puja_backend.DAL;
using Neosoft_puja_backend.DAL.Interface;
using Neosoft_puja_backend.DAL.Repository;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Automapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
builder.Services.AddDbContext<NeosoftDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaulConnection")));
// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IEmployeeService,EmployeeService>();
builder.Services.AddTransient<IEmployeeRepo,EmployeeRepo>();
builder.Services.AddTransient<ICityService, CityService>();
builder.Services.AddTransient<ICityRepo, CityRepo>();
builder.Services.AddTransient<ICountryRepo, CountryRepo>();
builder.Services.AddTransient<ICountryService, CountryService>();
builder.Services.AddTransient<IStateRepo, StateRepo>();
builder.Services.AddTransient<IStateService, StateService>();
builder.Services.AddTransient<IGenderRepo, GenderRepo>();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();
// Apply CORS Policy
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
