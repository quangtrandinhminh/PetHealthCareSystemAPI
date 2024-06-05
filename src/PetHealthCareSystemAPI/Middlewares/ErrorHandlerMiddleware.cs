using BusinessObject.DTO;
using Newtonsoft.Json;
using Utility.Constants;
using Utility.Exceptions;


namespace PetHealthCareSystemAPI.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
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
        }
    }
}