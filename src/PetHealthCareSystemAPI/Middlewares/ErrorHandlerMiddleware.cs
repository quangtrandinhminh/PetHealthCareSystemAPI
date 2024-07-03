using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Serilog;
using Utility.Constants;
using Utility.Exceptions;
using ILogger = Serilog.ILogger;


namespace PetHealthCareSystemAPI.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger = Log.Logger;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                // Check for specific status codes and handle them
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await HandleUnauthorizedAsync(context);
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    var role = GetRequiredRole(context);
                    await HandleForbiddenAsync(context, role);
                }
            }
            catch (AppException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private string GetRequiredRole(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() is IAuthorizeData authorizeData)
            {
                // Extract roles from Authorize attribute
                var roles = authorizeData.Roles;
                return roles;
            }

            return null; // No specific role required
        }


        private static Task HandleUnauthorizedAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var message = ResponseMessageIdentity.UNAUTHENTICATED + " You need AccessToken to access this resource.";

            var data = new BaseResponseDto(statusCode: StatusCodes.Status401Unauthorized, code: "Unauthorized", message: message);
            var result = JsonConvert.SerializeObject(data, 
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return context.Response.WriteAsync(result);
        }

        private static Task HandleForbiddenAsync(HttpContext context, string requiredRole = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            var message = requiredRole != null 
                    ? ResponseMessageIdentity.USER_NOT_ALLOWED + $" You need '{requiredRole}' role to access this resource." 
                    : "ResponseMessageIdentity.USER_NOT_ALLOWED";

            var data = new BaseResponseDto(statusCode: StatusCodes.Status403Forbidden, code: "Forbidden", message: message);
            var result = JsonConvert.SerializeObject(data, 
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return context.Response.WriteAsync(result);
        }

        private async Task HandleExceptionAsync(HttpContext context, AppException ex)
        {
            var response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = ex switch
            {
                AppException e => e.StatusCode,
                _ => StatusCodes.Status500InternalServerError
            };

            BaseResponseDto data;
            if (ex is AppException error)
                data = new BaseResponseDto(statusCode: response.StatusCode, code: error.Code, message: error.Message);
            else
                data = new BaseResponseDto(statusCode: response.StatusCode, code: ResponseCodeConstants.FAILED, data: ex, message: ex.Message);

            var result = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            await response.WriteAsync(result);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.Error(ex, "An unhandled exception has occurred.");
            var response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = StatusCodes.Status500InternalServerError;

            var data = new BaseResponseDto(statusCode: response.StatusCode, code: ResponseCodeConstants.FAILED, data: ex, message: ex.Message);

            var result = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            await response.WriteAsync(result);
        }
    }
}