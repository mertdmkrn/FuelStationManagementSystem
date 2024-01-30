using FuelStationManagementSystem.Model;
using Serilog;
using System.Text.Json;

namespace FuelStationManagementSystem.Middleware
{
    public class LoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                Log.Information($"Request - Path: {context.Request.Path}, Method: {context.Request.Method}, Body: {context.Request.Body}");
                await next(context);
                Log.Information($"Response - Path: {context.Request.Path}, Method: {context.Request.Method}, Body: {context.Response.Body}");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.HasError = true;
            response.Message = $"StatusCode : {context.Response.StatusCode} | Message : {ex.Message}";
            response.Data = ex.StackTrace;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
