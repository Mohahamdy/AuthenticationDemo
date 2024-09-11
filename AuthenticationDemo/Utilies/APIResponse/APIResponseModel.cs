
using System.Net;

namespace AuthenticationDemo.Utilies.APIResponse
{
    public class APIResponseModel<T> where T : class 
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
