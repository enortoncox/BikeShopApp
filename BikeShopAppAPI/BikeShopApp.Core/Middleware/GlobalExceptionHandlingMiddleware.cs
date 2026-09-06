using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace BikeShopApp.Core.Middleware
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        public GlobalExceptionHandlingMiddleware() {}

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var result = JsonSerializer.Serialize(new { title = "Internal Server Error", type = "Internal Server Error", detail = ex.Message, status = (int)HttpStatusCode.InternalServerError });

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(result);
            }
        }
    }
}
