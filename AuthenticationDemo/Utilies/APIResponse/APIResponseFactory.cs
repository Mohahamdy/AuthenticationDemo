using System.Net;

namespace AuthenticationDemo.Utilies.APIResponse
{
    public class APIResponseFactory<T> : IAPIRsponseFactory<T> where T : class
    {
        public static APIResponseModel<T> Success(T data, string message, HttpStatusCode statusCode)
        {
            return new APIResponseModel<T> { Data = data, Message = message, StatusCode = statusCode, IsSuccess = true };
        }

        public static APIResponseModel<T> Failure(T data, string message, HttpStatusCode statusCode)
        {
            return new APIResponseModel<T> { Data = data, Message = message, StatusCode = statusCode, IsSuccess = false };
        }

    }
}
