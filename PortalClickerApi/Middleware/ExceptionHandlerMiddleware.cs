using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using PortalClickerApi.Exceptions;

namespace PortalClickerApi.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException e)
            {
                await Handle(context, 400, new[] { e.Message });
            }
            catch (ValidationException e)
            {
                await Handle(context, 400, e.Errors.Select(x => x.ErrorMessage));
            }
        }

        private async Task Handle(HttpContext context, int statusCode, IEnumerable<string> errors)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(errors));
            }
        }
    }
}
