namespace LM.Domain.DomainServices
{
    public interface IIdentityService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
