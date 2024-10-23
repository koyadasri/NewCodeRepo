namespace DataModels.Models
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}
