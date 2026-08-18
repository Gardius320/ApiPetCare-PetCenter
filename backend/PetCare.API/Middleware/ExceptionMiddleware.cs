using FluentValidation;
using PetCare.Application.Common;
using System.Net;
using System.Text.Json;

namespace PetCare.API.Middleware
{
    public class ExceptionMiddleware
    {        
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }       
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {

                await _next(context);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                await WriteResponse(
                    context,
                    HttpStatusCode.BadRequest,
                    ApiResponse<object>.Failure("Error de validación", errors)
                );
            }

            catch (UnauthorizedAccessException ex) 
            {
                await WriteResponse(
                    context,
                    HttpStatusCode.Unauthorized,
                    ApiResponse<object>.Failure("Acceso no autorizado: " + ex.Message)
                );
            }
            catch (Exception ex)
            {
                await WriteResponse(
                    context,
                    HttpStatusCode.InternalServerError,
                    ApiResponse<object>.Failure("Ocurrió un error inesperado: " + ex.Message)
                );
            }
        }
        
        private async Task WriteResponse<T>(
            HttpContext context,
            HttpStatusCode statusCode,
            ApiResponse<T> response)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}