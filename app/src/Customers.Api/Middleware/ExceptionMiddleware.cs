using System.Net;
using Customer.ServiceInterface.Responses;
using Customers.Domain.Exceptions;

namespace Customers.Api.Middleware;

public class ExceptionMiddleware
{
    readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        if (exception is NotFoundException)
        {
            //user can correct
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        await context.Response.WriteAsJsonAsync(
            new ExceptionResponse(exception.Message));
    }
}