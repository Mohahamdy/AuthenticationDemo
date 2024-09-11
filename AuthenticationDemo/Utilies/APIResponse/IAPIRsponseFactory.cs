using System.Net;

namespace AuthenticationDemo.Utilies.APIResponse
{
    public interface IAPIRsponseFactory<T> where T : class
    {
        static abstract APIResponseModel<T> Success(T data, string message, HttpStatusCode statusCode);
        static abstract APIResponseModel<T> Failure(T data, string message, HttpStatusCode statusCode);
    }
}
