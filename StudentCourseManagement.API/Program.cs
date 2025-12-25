using StudentCourseManagement.Business;
using StudentCourseManagement.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();        // removes built-in console logger

builder.Services.AddBusinessLayer();                // Business Layer Registration 
builder.Services.AddDataLayer(builder.Configuration);  // Data layer DI registration


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



public partial class Program { }