using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Project_API.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<Student>("Students");
odataBuilder.EntitySet<Course>("Courses");
odataBuilder.EntitySet<Category>("Categories");
odataBuilder.EntitySet<Section>("Sections");
odataBuilder.EntitySet<Item>("Items");
odataBuilder.EntitySet<Resource>("Resources");
odataBuilder.EntitySet<Assignment>("Assignments");
odataBuilder.EntitySet<CourseStudent>("CourseStudents");
odataBuilder.EntitySet<StudentSubmission>("StudentSubmissions");
builder.Services.AddControllers()
                .AddOData(options => options.AddRouteComponents("odata", odataBuilder.GetEdmModel())
                                             .EnableQueryFeatures(100))
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<Prn231PrjContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Student", builder =>
    {
        builder.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        builder.RequireAuthenticatedUser();
    });
});
// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "issuer",
            ValidAudience = "audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]))
        };
    });
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

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
