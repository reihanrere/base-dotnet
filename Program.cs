
using BaseDotnet.Core.DbContext;
using BaseDotnet.Core.Helpers;
using BaseDotnet.Core.Services;
using Serilog;
using Serilog.Events;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/error.log", restrictedToMinimumLevel: LogEventLevel.Warning)
    .WriteTo.File("logs/all-daily_.log", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Debug()
    .CreateBootstrapLogger();
builder.Host.UseSerilog();

// Configure database
builder.Services.AddDbContext<BaseDotnetDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyConnection"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Base Dotnet API", Version = "v1" });
    c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer"
    });
    c.OperationFilter<HeaderFilter>();
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddHttpContextAccessor();

// Configure Services
ServiceInjector.Inject(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseMiddleware<JwtMiddleware>();

app.Run();
