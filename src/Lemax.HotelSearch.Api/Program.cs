using FluentValidation;
using FluentValidation.AspNetCore;
using Lemax.HotelSearch.Api.DependencyInjection;
using Lemax.HotelSearch.Api.Logging;
using Lemax.HotelSearch.Api.Middleware;
using Lemax.HotelSearch.Api.Models;
using Lemax.HotelSearch.Api.Validation.Request;
using Lemax.HotelSearch.Infrastructure.DependencyInjection;
using Lemax.HotelSearch.Service.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddLocalFileLogger(Path.Combine(builder.Environment.ContentRootPath, "logs", "application-log.txt"));

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value!.Errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Invalid value." : e.ErrorMessage).ToArray());

        var logDetails = new LogDetails
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Message = "Input validation failed.",
            Path = context.HttpContext.Request.Path,
            TraceId = context.HttpContext.TraceIdentifier,
            TimestampUtc = DateTimeOffset.UtcNow,
            ValidationErrors = errors
        };

        return new BadRequestObjectResult(logDetails);
    };
});

builder.Services.AddHealthChecks();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateHotelHttpRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiMappings();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

/// <summary>
/// Represents program type.
/// </summary>
public partial class Program;
