namespace WebApp1API.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}
