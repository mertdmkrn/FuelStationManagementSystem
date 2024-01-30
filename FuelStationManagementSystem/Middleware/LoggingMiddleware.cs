using FuelStationManagementSystem.Model;
using Serilog;
using System.Text;
using System.Text.Json;

namespace FuelStationManagementSystem.Middleware
{
    public class LoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var requestBody = await FormatRequest(context.Request);
                Log.Information($"Request - Path: {context.Request.Path}, Method: {context.Request.Method}, Body: {requestBody}");

                // Response'ı logla
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await next(context);

                    responseBody.Seek(0, SeekOrigin.Begin);
                    var response = await new StreamReader(responseBody).ReadToEndAsync();
                    Log.Information($"Response - Path: {context.Request.Path}, Method: {context.Request.Method}, Body: {response}");

                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            request.Body = body;

            return $"{requestBody}";
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
