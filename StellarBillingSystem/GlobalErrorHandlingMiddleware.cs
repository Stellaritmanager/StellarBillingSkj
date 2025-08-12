using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _logFilePath;

    public GlobalErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
        _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "errorlog.txt");
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // Continue request pipeline
        }
        catch (Exception ex)
        {
            LogErrorToFile(context, ex);

            // Build the correct redirect path
            var redirectPath = $"{context.Request.PathBase}/Home/Error";
            context.Response.Redirect(redirectPath);
        }
    }

    private bool IsAjaxRequest(HttpRequest request)
    {
        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }

    private void LogErrorToFile(HttpContext context, Exception ex)
    {
        var sb = new StringBuilder();

        // Basic Info
        sb.AppendLine("===== ERROR OCCURRED =====");
        sb.AppendLine($"Time: {DateTime.Now}");
        sb.AppendLine($"Server Machine: {Environment.MachineName}");

        // User Info
        var userName = context.User.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous";
        sb.AppendLine($"User: {userName}");

        // Request Info
        sb.AppendLine($"Request Path: {context.Request.Path}");
        sb.AppendLine($"Query String: {context.Request.QueryString}");
        sb.AppendLine($"Client IP: {context.Connection.RemoteIpAddress}");
        sb.AppendLine($"User Agent: {context.Request.Headers["User-Agent"]}");

        // Controller/Action
        var routeData = context.GetRouteData();
        if (routeData != null)
        {
            sb.AppendLine($"Controller: {routeData.Values["controller"]}");
            sb.AppendLine($"Action: {routeData.Values["action"]}");
        }

        // Exception Details
        sb.AppendLine($"Error Message: {ex.Message}");
        sb.AppendLine($"StackTrace: {ex.StackTrace}");

        // Inner Exception Details
        var inner = ex.InnerException;
        while (inner != null)
        {
            sb.AppendLine("--- INNER EXCEPTION ---");
            sb.AppendLine($"Message: {inner.Message}");
            sb.AppendLine($"StackTrace: {inner.StackTrace}");
            inner = inner.InnerException;
        }

        sb.AppendLine("========================");
        sb.AppendLine();


        // Create logs folder if missing
        string logFolder = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }

        // File name with date/time
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string logFileName = Path.Combine(logFolder, $"errorlog_{timestamp}.txt");


        File.WriteAllText(logFileName, sb.ToString());
    }
}
