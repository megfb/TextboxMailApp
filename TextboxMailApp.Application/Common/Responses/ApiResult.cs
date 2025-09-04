using System.Net;

namespace TextboxMailApp.Application.Common.Responses
{
    public class ApiResult<T>
    {
        public T? Data { get; set; }
        public List<string>? ErrorMessage { get; set; }
        public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count() == 0;
        public bool IsFail => !IsSuccess;
        public HttpStatusCode Status { get; set; }

        public static ApiResult<T> Success(T data, HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ApiResult<T> { Data = data, Status = status };
        }
        public static ApiResult<T> Fail(List<string> errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ApiResult<T>
            {
                ErrorMessage = errorMessage,
                Status = status
            };
        }
        public static ApiResult<T> Fail(string errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ApiResult<T>
            {
                ErrorMessage = new List<string> { errorMessage },
                Status = status
            };
        }
    }
}
