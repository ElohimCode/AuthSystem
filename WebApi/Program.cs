using WebApi;
using Persistence;
using Infrastructure;
using Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// To allow client apps to connect with the api
builder.Services.AddCors(o =>
    o.AddPolicy("AuthClient", builder =>
    {
        builder
        .AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }
    ));

builder.Services.AddControllers();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentitySettings();
builder.Services.AddIdentityServices();
builder.Services.AddApplicationServices();
//builder.Services.AddRepositoryService();
builder.Services.AddPersistenceDependencies();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.SeedDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AuthClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
