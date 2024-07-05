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
        private readonly ILogger _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await HandleUnauthorizedAsync(context);
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    var roles = GetRequiredRoles(context);
                    await HandleForbiddenAsync(context, roles);
                }
            }
            catch (AppException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static string GetRequiredRoles(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var authorizeData = endpoint?.Metadata?.GetMetadata<IAuthorizeData>();

            return authorizeData?.Roles;
        }

        private static Task HandleUnauthorizedAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var message = ResponseMessageIdentity.UNAUTHENTICATED + " You need AccessToken to access this resource.";

            var data = new BaseResponseDto(StatusCodes.Status401Unauthorized, "Unauthorized", message);
            var result = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return context.Response.WriteAsync(result);
        }

        private static Task HandleForbiddenAsync(HttpContext context, string requiredRoles = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            var message = !string.IsNullOrEmpty(requiredRoles)
                ? ResponseMessageIdentity.USER_NOT_ALLOWED + $" You need '{requiredRoles}' role to access this resource."
                : ResponseMessageIdentity.USER_NOT_ALLOWED;

            var data = new BaseResponseDto(StatusCodes.Status403Forbidden, "Forbidden", message);
            var result = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return context.Response.WriteAsync(result);
        }

        private static async Task HandleExceptionAsync(HttpContext context, AppException ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = ex.StatusCode;

            var data = new BaseResponseDto(response.StatusCode, ex.Code, ex.Message);
            var result = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            await response.WriteAsync(result);
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = StatusCodes.Status500InternalServerError;

            var data = new BaseResponseDto(response.StatusCode, ResponseCodeConstants.FAILED, ex.Message);
            var result = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            await response.WriteAsync(result);
        }
    }
}
