using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Project_API.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<Student>("Students");
odataBuilder.EntitySet<Course>("Courses");
odataBuilder.EntitySet<Category>("Categorys");
odataBuilder.EntitySet<Section>("Sections");
odataBuilder.EntitySet<Item>("Items");
odataBuilder.EntitySet<Resource>("Resources");
odataBuilder.EntitySet<Assignment>("Assignments");
odataBuilder.EntityType<CourseStudent>();
odataBuilder.EntityType<StudentSubmission>();
builder.Services.AddControllers()
                .AddOData(options => options.AddRouteComponents("odata", odataBuilder.GetEdmModel())
                                             .EnableQueryFeatures(100))
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<Prn231PrjContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://localhost:7236","http://localhost:5186").AllowAnyHeader().AllowAnyMethod();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
