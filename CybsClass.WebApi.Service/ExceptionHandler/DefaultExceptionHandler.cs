using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CybsClass.WebApi.Service.ExceptionHandlers;

public class DefaultExceptionHandler : IExceptionHandler
{

    private readonly ILogger<DefaultExceptionHandler> _logger;
    public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext http, Exception ex, CancellationToken ct)
    {
        _logger.LogError(ex, "Unhandled exception at {Path}", http.Request.Path);
        http.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await http.Response.WriteAsJsonAsync(new { error = "unexpected_error" }, ct);
        return true; // swallow so the pipeline doesn’t rethrow
    }

}