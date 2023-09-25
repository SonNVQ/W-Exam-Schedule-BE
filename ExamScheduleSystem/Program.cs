using ExamScheduleSystem;
using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IExamScheduleRepository, ExamScheduleRepository>();
builder.Services.AddScoped<IExamSlotRepository, ExamSlotRepository>();
builder.Services.AddScoped<IMajorRepository, MajorRepository>();
builder.Services.AddScoped<IProctoringRepository, ProctoringRepository>();
builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<IStudentListRepository, StudentListRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ExamScheduleSystem"));
});

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable middleware to serve generated Swagger as a JSON endpoint.
// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON end point.
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
}
     );
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");

    // custom CSS
    //c.InjectStylesheet("/swagger-ui/custom.css");
});

app.Use(async (ctx, next) =>
{
    await next();
    if (ctx.Response.StatusCode == 204)
    {
        ctx.Response.ContentLength = 0;
    }
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
