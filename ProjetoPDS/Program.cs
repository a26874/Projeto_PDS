using ProjetoPDS.Classes;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Aqui é a conexão do sql
var connectionSql = builder.Configuration.GetConnectionString("LigacaoSql");
builder.Services.AddDbContext<dataBase>(options => options.UseSqlServer(connectionSql));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Para conseguir enviar dados.
app.UseCors(builder => builder
    .WithOrigins("http://127.0.0.1:3000") 
    .AllowAnyHeader()
    .AllowAnyMethod());


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
