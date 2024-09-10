namespace AuthenticationDemo.Utilies.Token
{
    public class TokenModel
    {
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public int ExpireDuration { get; set; }
    }
}
