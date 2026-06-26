using FluentValidation;
using PetCare.Application.Common;
using System.Net;
using System.Text.Json;

namespace PetCare.API.Middleware
{
    public class ExceptionMiddleware
    {
        // Representa el siguiente paso en el pipeline
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        // Invoke sirve para ejecutar eventos 
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                
                await _next(context);
            }
            catch (ValidationException ex)
            {
                // FluentValidation lanzó erroresdevolvemos 400
                var errors = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                await WriteResponse(
                    context,
                    HttpStatusCode.BadRequest,
                    ApiResponse<object>.Failure("Error de validación", errors)
                );
            }
            catch (Exception ex)
            {
                // Cualquier error  devolvemos 500
                await WriteResponse(
                    context,
                    HttpStatusCode.InternalServerError,
                    ApiResponse<object>.Failure("Ocurrió un error inesperado: " + ex.Message)
                );
            }
        }

        //escribe un formato de respuesta estandar y es un tipo JSON
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