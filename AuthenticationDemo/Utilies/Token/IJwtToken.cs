using AuthenticationDemo.Models;

namespace AuthenticationDemo.Utilies.Token
{
    public interface IJwtToken
    {
        Task<string> Generate(User _user);
    }
}
